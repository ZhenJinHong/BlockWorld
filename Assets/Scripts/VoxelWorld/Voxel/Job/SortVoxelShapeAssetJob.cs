using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct TempVoxelShapeData
    {
        public int IndexStartIndex;
        public int IndexEnd;
        /// <summary>
        /// 整个形状的顶点分段开始
        /// </summary>
        public int VertexStartIndex;
        public int VertexEnd;
        public FaceRect FrontRect;
        public FaceRect BackRect;
        public FaceRect TopRect;
        public FaceRect BottomRect;
        public FaceRect RightRect;
        public FaceRect LeftRect;
    }
    #region 形状生成与数据类
    [BurstCompile]
    public struct SortVoxelShapeAssetJob : IJob
    {
        public NativeList<TempVoxelShapeData> shapesTempForJob;
        public NativeList<float3> vertsTempForJob;
        public NativeList<int> trianglesTempForJob;
        public NativeList<float2> uvsTempForJob;
        public NativeList<float3> normalsTempForJob;

        public NativeList<VoxelFaceData> 有序临时面数据数组;
        public NativeList<float3> 有序临时顶点数组;
        public NativeList<ushort> 有序临时索引数组;
        public NativeList<float2> 有序临时UV数组;
        public NativeList<float3> 有序临时法向数组;

        public NativeHashMap<int, ushort> 旧新顶点索引查找图;

        const float minThreshold = -0.48f;
        const float maxThreshold = 0.48f;
        public void Execute()
        {
            NativeList<int> left = new NativeList<int>(Allocator.Temp);
            NativeList<int> right = new NativeList<int>(Allocator.Temp);
            NativeList<int> top = new NativeList<int>(Allocator.Temp);
            NativeList<int> bottom = new NativeList<int>(Allocator.Temp);
            NativeList<int> front = new NativeList<int>(Allocator.Temp);
            NativeList<int> back = new NativeList<int>(Allocator.Temp);
            NativeList<int> notFit = new NativeList<int>(Allocator.Temp);

            for (int i = 0; i < shapesTempForJob.Length; i++)
            {
                TempVoxelShapeData normal = shapesTempForJob[i];
                // 要注意的是这里计算索引，本身是基于原本网格的
                CalculateShapeData(in normal, ref front, ref back, ref top, ref bottom, ref right, ref left, ref notFit);
            }
        }
        /// <summary>
        /// 计算正向/左上角形状，将网格数据划分各面中
        /// </summary>
        void CalculateShapeData(in TempVoxelShapeData shapeData, ref NativeList<int> front, ref NativeList<int> back, ref NativeList<int> top, ref NativeList<int> bottom, ref NativeList<int> right, ref NativeList<int> left, ref NativeList<int> notFit)
        {
            front.Clear(); back.Clear(); top.Clear(); bottom.Clear(); right.Clear(); left.Clear(); notFit.Clear();
            int baseVertexIndex = shapeData.VertexStartIndex;
            // 将已列入索引数据的三角形索引，重新按照归属面排序
            // start和end指定该形状的网格数据放在了哪一段
            // 接下来就是遍历这段网格的所有的三角形,读取三角形的三个顶点,判断归属面
            for (int startIndex = shapeData.IndexStartIndex; startIndex < shapeData.IndexEnd; startIndex += 3)
            {
                float3 v1 = vertsTempForJob[trianglesTempForJob[startIndex] + baseVertexIndex];
                float3 v2 = vertsTempForJob[trianglesTempForJob[startIndex + 1] + baseVertexIndex];
                float3 v3 = vertsTempForJob[trianglesTempForJob[startIndex + 2] + baseVertexIndex];
                float3 xf = new float3(v1.x, v2.x, v3.x);
                float3 yf = new float3(v1.y, v2.y, v3.y);
                float3 zf = new float3(v1.z, v2.z, v3.z);

                if (math.all(zf > maxThreshold))// 在前面 顺Z轴
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref front, startIndex, baseVertexIndex);
                }
                else if (math.all(zf < minThreshold))// 在背面
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref back, startIndex, baseVertexIndex);
                }
                else if (math.all(yf > maxThreshold))// 在上面
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref top, startIndex, baseVertexIndex);
                }
                else if (math.all(yf < minThreshold))// 在下面
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref bottom, startIndex, baseVertexIndex);
                }
                else if (math.all(xf > maxThreshold))// 在右面
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref right, startIndex, baseVertexIndex);
                }
                else if (math.all(xf < minThreshold))// 在左面
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref left, startIndex, baseVertexIndex);
                }
                else
                {
                    AddTriangleToTempFaceList(in trianglesTempForJob, ref notFit, startIndex, baseVertexIndex);
                }
            }
            // 每个面的三角形索引需要从0开始
            // 三角面索引是不能排序的，每三个排序呢？
            // 这里等于把网格拆成7份
            SortTriangle(in front, in shapeData.FrontRect);
            SortTriangle(in back, in shapeData.BackRect);
            SortTriangle(in top, in shapeData.TopRect);
            SortTriangle(in bottom, in shapeData.BottomRect);
            SortTriangle(in right, in shapeData.RightRect);
            SortTriangle(in left, in shapeData.LeftRect);
            SortTriangle(in notFit, FaceRect.None);
        }
        // 仅仅是将判断完归属面的三个索引加入到对应面的列表里
        static void AddTriangleToTempFaceList(in NativeList<int> ori, ref NativeList<int> target, int start, int baseVertexIndex)
        {
            target.Add(ori[start] + baseVertexIndex);
            target.Add(ori[start + 1] + baseVertexIndex);
            target.Add(ori[start + 2] + baseVertexIndex);
        }
        // 每个面的三角形索引都从0开始
        // start和end则指明了这个面的顶点数据和索引在哪个面
        void SortTriangle(in NativeList<int> faceTriangle, in FaceRect faceRect)
        {
            旧新顶点索引查找图.Clear();
            ushort 新相对顶点索引 = 0;
            VoxelFaceData voxelFaceData = new VoxelFaceData()
            {
                FaceRect = faceRect,
                IndexStartIndex = 有序临时索引数组.Length,
                VertexStartIndex = 有序临时顶点数组.Length,
            };
            for (int i = 0; i < faceTriangle.Length; i++)
            {
                int 旧顶点索引 = faceTriangle[i];
                
                if (!旧新顶点索引查找图.TryGetValue(旧顶点索引, out ushort 已有的新索引))
                { 
                    已有的新索引 = 新相对顶点索引;
                    有序临时顶点数组.Add(vertsTempForJob[旧顶点索引]);
                    有序临时UV数组.Add(uvsTempForJob[旧顶点索引]);
                    有序临时法向数组.Add(normalsTempForJob[旧顶点索引]);
                    旧新顶点索引查找图[旧顶点索引] = 已有的新索引;
                    新相对顶点索引++;
                }
                有序临时索引数组.Add(已有的新索引);
            }
            voxelFaceData.IndexEnd = 有序临时索引数组.Length;
            voxelFaceData.VertexEnd = 有序临时顶点数组.Length;
            有序临时面数据数组.Add(voxelFaceData);
        }
    }
    #endregion
}
