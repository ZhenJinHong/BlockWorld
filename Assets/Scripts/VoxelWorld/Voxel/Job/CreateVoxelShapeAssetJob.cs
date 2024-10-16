using Unity.Burst;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 角部的旋转是从左上角开始,顺时针
    /// </summary>
    [BurstCompile]
    public struct CreateVoxelShapeAssetJob : IJob
    {
        public NativeList<VoxelFaceData> 有序临时面数据数组;
        public NativeList<float3> 有序临时顶点数组;
        public NativeList<ushort> 有序临时索引数组;
        public NativeList<float2> 有序临时UV数组;
        public NativeList<float3> 有序临时法向数组;

        public NativeList<VoxelFaceData> allfaceDatas;
        public NativeList<float3> allverts;
        public NativeList<ushort> allIndexs;
        public NativeList<float2> alluvs;
        public NativeList<float3> allnormals;
        public void Execute()
        {
            ExecuteAddRotateVertex();
        }
        void ExecuteAddRotateVertex()
        {
            NativeList<VoxelFaceData> 临时7面数据数组 = new NativeList<VoxelFaceData>(Allocator.Temp);

            for (int i = 0; i < 有序临时面数据数组.Length; i += VoxelFaceData.FaceCountInSingleShape)
            {
                GetTargetShapeData(ref 临时7面数据数组, i);
                RotateShapeAndAdd(ref 临时7面数据数组);
                临时7面数据数组.Clear();
                GetTargetShapeData(ref 临时7面数据数组, i);
                FilpShapeData(ref 临时7面数据数组);
                RotateShapeAndAdd(ref 临时7面数据数组);
                临时7面数据数组.Clear();
            }
        }
        
        void GetTargetShapeData(ref NativeList<VoxelFaceData> 临时7面数据数组, int 有序临时面数据起始索引)
        {
            for (int faceDataIndex = 0; faceDataIndex < VoxelFaceData.FaceCountInSingleShape; faceDataIndex++)
            {
                VoxelFaceData voxelFaceData = 有序临时面数据数组[faceDataIndex + 有序临时面数据起始索引];
                临时7面数据数组.Add(voxelFaceData);
            }
        }
        #region 仅加入一次面数据的方式,已注销,如果交由网格构建工作处理旋转,则各方向不同形状的不再支持
        //void ExecuteNotAddRotateVertex()
        //{
        //    NativeList<VoxelFaceData> 临时7面数据数组 = new NativeList<VoxelFaceData>(Allocator.Temp);

        //    for (int i = 0; i < 有序临时面数据数组.Length; i += VoxelFaceData.FaceCountInSingleShape)
        //    {
        //        GetTargetShapeData(ref 临时7面数据数组, i);
        //        RotateShapeRect(ref 临时7面数据数组);
        //        临时7面数据数组.Clear();
        //        GetTargetShapeData(ref 临时7面数据数组, i);
        //        FilpShapeRect(ref 临时7面数据数组);
        //        RotateShapeRect(ref 临时7面数据数组);
        //        临时7面数据数组.Clear();
        //    }
        //}
        //void RotateShapeRect(ref NativeList<VoxelFaceData> 临时7面数据数组)
        //{
        //    // 按照7个面的数据,仅添加一次各面数据
        //    for (int faceDataIndex = 0; faceDataIndex < VoxelFaceData.FaceCountInSingleShape; faceDataIndex++)
        //    {
        //        VoxelFaceData voxelFaceData = 临时7面数据数组[faceDataIndex];
        //        int indexStartIndex = voxelFaceData.IndexStartIndex;
        //        int indexEnd = voxelFaceData.IndexEnd;
        //        int vertexStartIndex = voxelFaceData.VertexStartIndex;
        //        int vertexEnd = voxelFaceData.VertexEnd;
        //        for (; indexStartIndex < indexEnd; indexStartIndex++)
        //        {
        //            allIndexs.Add(有序临时索引数组[indexStartIndex]);
        //        }
        //        for (; vertexStartIndex < vertexEnd; vertexStartIndex++)
        //        {
        //            allverts.Add(有序临时顶点数组[vertexStartIndex]);
        //            alluvs.Add(有序临时UV数组[vertexStartIndex]);
        //            allnormals.Add(有序临时法向数组[vertexStartIndex]);
        //        }
        //    }
        //    float loop = 0f;
        //    do
        //    {
        //        for (int faceDataIndex = 0; faceDataIndex < VoxelFaceData.FaceCountInSingleShape; faceDataIndex++)
        //        {
        //            allfaceDatas.Add(临时7面数据数组[faceDataIndex]);
        //        }
        //        // 每个形状旋转,将面数据转动,提供给下个循环读取对应分段的顶点等数据
        //        VoxelFaceData 前面 = 临时7面数据数组[0];
        //        VoxelFaceData 背面 = 临时7面数据数组[1];
        //        VoxelFaceData 上面 = 临时7面数据数组[2];
        //        VoxelFaceData 下面 = 临时7面数据数组[3];
        //        VoxelFaceData 右面 = 临时7面数据数组[4];
        //        VoxelFaceData 左面 = 临时7面数据数组[5];

        //        前面.FaceRect = Rotate((ulong)前面.FaceRect, RotateAxis.Y);
        //        背面.FaceRect = Rotate((ulong)背面.FaceRect, RotateAxis.Y);
        //        上面.FaceRect = Rotate((ulong)上面.FaceRect, RotateAxis.水平绕自身中心90度);
        //        下面.FaceRect = Rotate((ulong)下面.FaceRect, RotateAxis.水平绕自身中心90度);
        //        //右面.FaceRect = 左面.FaceRect;
        //        //左面.FaceRect = 右面.FaceRect;// 旋转后仍在第一象限,保持不变
        //        // 要注意的是前面先旋转了矩形,之后这里才重新赋值,如果先赋值后旋转,旋转的操作就不一样了
        //        VoxelFaceData temp = 前面;
        //        前面 = 左面;// 左面赋值到前面
        //        左面 = 背面;// 背面赋值到左面
        //        背面 = 右面;// 右面赋值到背面
        //        右面 = temp;// 前面赋值到右面

        //        临时7面数据数组[0] = 前面;
        //        临时7面数据数组[1] = 背面;
        //        临时7面数据数组[2] = 上面;
        //        临时7面数据数组[3] = 下面;
        //        临时7面数据数组[4] = 右面;
        //        临时7面数据数组[5] = 左面;

        //        loop++;
        //    }
        //    while (loop != 4);
        //}
        //void FilpShapeRect(ref NativeList<VoxelFaceData> 临时7面数据数组)
        //{
        //    VoxelFaceData 前面 = 临时7面数据数组[0];
        //    VoxelFaceData 背面 = 临时7面数据数组[1];
        //    VoxelFaceData 上面 = 临时7面数据数组[2];
        //    VoxelFaceData 下面 = 临时7面数据数组[3];
        //    VoxelFaceData 右面 = 临时7面数据数组[4];
        //    VoxelFaceData 左面 = 临时7面数据数组[5];
        //    前面.FaceRect = Rotate((ulong)前面.FaceRect, RotateAxis.水平绕自身中心180度);
        //    背面.FaceRect = Rotate((ulong)背面.FaceRect, RotateAxis.水平绕自身中心180度);
        //    上面.FaceRect = Rotate((ulong)上面.FaceRect, RotateAxis.Y);
        //    下面.FaceRect = Rotate((ulong)下面.FaceRect, RotateAxis.Y);
        //    右面.FaceRect = Rotate((ulong)右面.FaceRect, RotateAxis.X);
        //    左面.FaceRect = Rotate((ulong)左面.FaceRect, RotateAxis.X);
        //    临时7面数据数组[0] = 前面;
        //    临时7面数据数组[1] = 背面;
        //    临时7面数据数组[2] = 下面;
        //    临时7面数据数组[3] = 上面;
        //    临时7面数据数组[4] = 左面;
        //    临时7面数据数组[5] = 右面;
        //}
        #endregion
        void RotateShapeAndAdd(ref NativeList<VoxelFaceData> 临时7面数据数组)
        {
            float angle = 0f;
            int loop = 0;
            do
            {
                float4x4 matrix = float4x4.RotateY(math.radians(angle));
                for (int faceDataIndex = 0; faceDataIndex < VoxelFaceData.FaceCountInSingleShape; faceDataIndex++)
                {
                    VoxelFaceData voxelFaceData = 临时7面数据数组[faceDataIndex];
                    int indexStartIndex = voxelFaceData.IndexStartIndex;
                    int indexEnd = voxelFaceData.IndexEnd;
                    int vertexStartIndex = voxelFaceData.VertexStartIndex;
                    int vertexEnd = voxelFaceData.VertexEnd;
                    // 新的面数组应该指向总数组了
                    VoxelFaceData newVoxelFaceData = new VoxelFaceData()
                    {
                        IndexStartIndex = allIndexs.Length,
                        VertexStartIndex = allverts.Length,
                        FaceRect = voxelFaceData.FaceRect,
                    };
                    for (; indexStartIndex < indexEnd; indexStartIndex++)
                    {
                        allIndexs.Add(有序临时索引数组[indexStartIndex]);
                    }
                    for (; vertexStartIndex < vertexEnd; vertexStartIndex++)
                    {
                        allverts.Add(math.rotate(matrix, 有序临时顶点数组[vertexStartIndex]) + 0.5f);
                        alluvs.Add(有序临时UV数组[vertexStartIndex]);
                        allnormals.Add(math.rotate(matrix, 有序临时法向数组[vertexStartIndex]));
                    }
                    newVoxelFaceData.IndexEnd = allIndexs.Length;
                    newVoxelFaceData.VertexEnd = allverts.Length;
                    allfaceDatas.Add(newVoxelFaceData);
                }
                // 每个形状旋转,将面数据转动
                VoxelFaceData 前面 = 临时7面数据数组[0];
                VoxelFaceData 背面 = 临时7面数据数组[1];
                VoxelFaceData 上面 = 临时7面数据数组[2];
                VoxelFaceData 下面 = 临时7面数据数组[3];
                VoxelFaceData 右面 = 临时7面数据数组[4];
                VoxelFaceData 左面 = 临时7面数据数组[5];

                前面.FaceRect = Rotate((ulong)前面.FaceRect, RotateAxis.Y);
                背面.FaceRect = Rotate((ulong)背面.FaceRect, RotateAxis.Y);
                上面.FaceRect = Rotate((ulong)上面.FaceRect, RotateAxis.水平绕自身中心90度);
                下面.FaceRect = Rotate((ulong)下面.FaceRect, RotateAxis.水平绕自身中心90度);
                //右面.FaceRect = 左面.FaceRect;
                //左面.FaceRect = 右面.FaceRect;// 旋转后仍在第一象限,保持不变
                // 要注意的是前面先旋转了矩形,之后这里才重新赋值,如果先赋值后旋转,旋转的操作就不一样了
                VoxelFaceData temp = 前面;
                前面 = 左面;// 左面赋值到前面
                左面 = 背面;// 背面赋值到左面
                背面 = 右面;// 右面赋值到背面
                右面 = temp;// 前面赋值到右面

                临时7面数据数组[0] = 前面;
                临时7面数据数组[1] = 背面;
                临时7面数据数组[2] = 上面;
                临时7面数据数组[3] = 下面;
                临时7面数据数组[4] = 右面;
                临时7面数据数组[5] = 左面;

                angle += 90f;
                loop++;
            }
            while (loop < 4);
        }
        void FilpShapeData(ref NativeList<VoxelFaceData> 临时7面数据数组)
        {
            VoxelFaceData 前面 = 临时7面数据数组[0];
            VoxelFaceData 背面 = 临时7面数据数组[1];
            VoxelFaceData 上面 = 临时7面数据数组[2];
            VoxelFaceData 下面 = 临时7面数据数组[3];
            VoxelFaceData 右面 = 临时7面数据数组[4];
            VoxelFaceData 左面 = 临时7面数据数组[5];
            前面.FaceRect = Rotate((ulong)前面.FaceRect, RotateAxis.水平绕自身中心180度);
            背面.FaceRect = Rotate((ulong)背面.FaceRect, RotateAxis.水平绕自身中心180度);
            上面.FaceRect = Rotate((ulong)上面.FaceRect, RotateAxis.Y);
            下面.FaceRect = Rotate((ulong)下面.FaceRect, RotateAxis.Y);
            右面.FaceRect = Rotate((ulong)右面.FaceRect, RotateAxis.X);
            左面.FaceRect = Rotate((ulong)左面.FaceRect, RotateAxis.X);
            临时7面数据数组[0] = 前面;
            临时7面数据数组[1] = 背面;
            临时7面数据数组[2] = 下面;
            临时7面数据数组[3] = 上面;
            临时7面数据数组[4] = 左面;
            临时7面数据数组[5] = 右面;

            float4x4 martix = float4x4.RotateZ(math.radians(180f));
            for (int faceDataindex = 0; faceDataindex < VoxelFaceData.FaceCountInSingleShape; faceDataindex++)
            {
                VoxelFaceData temp = 临时7面数据数组[faceDataindex];
                int vertexStartIndex = temp.VertexStartIndex;
                int vertexEnd = temp.VertexEnd;
                for (; vertexStartIndex < vertexEnd; vertexStartIndex++)
                {
                    有序临时顶点数组[vertexStartIndex] = math.rotate(martix, 有序临时顶点数组[vertexStartIndex]);
                    有序临时法向数组[vertexStartIndex] = math.rotate(martix, 有序临时法向数组[vertexStartIndex]);
                }
            }
        }
        static FaceRect Rotate(ulong faceRect, RotateAxis rotateAxis)
        {
            // 水平面 从Y看向-Y，1从原点开始算，
            // 垂直面 从X看向-X，1从原点开始算，从Z看向-Z，1从原点开始算
            // 
            const int n = 8;
            ulong newFaceRect = 0;
            //  ((faceRect & (1ul << oriOff)) >> oriOff) 取到目标位的值
            switch (rotateAxis)
            {
                case RotateAxis.X:
                    for (int i = 0; i < n; i++)
                    {
                        int total = i * n;
                        for (int j = 0; j < n; j++)
                        {
                            int oriOff = (total + j);
                            newFaceRect |= ((faceRect & (1ul << oriOff)) >> oriOff) << (56 - total + j);
                        }
                    }
                    break;
                case RotateAxis.Y:
                    for (int i = 0; i < n; i++)
                    {
                        int total = i * n;
                        for (int j = 0; j < n; j++)
                        {
                            int oriOff = (total + j);
                            newFaceRect |= ((faceRect & (1ul << oriOff)) >> oriOff) << (total - j + 7);
                        }
                    }
                    break;
                case RotateAxis.水平绕自身中心90度:
                    for (int i = 0; i < n; i++)
                    {
                        int total = i * n;
                        for (int j = 0; j < n; j++)
                        {
                            int oriOff = (total + j);
                            newFaceRect |= ((faceRect & (1ul << oriOff)) >> oriOff) << ((n - j - 1) * n + i);
                        }
                    }
                    break;
                case RotateAxis.水平绕自身中心180度:
                    for (int i = 0; i < 64; i++)
                    {
                        //  << (63 - i) 这个是翻转后的新位移规律
                        newFaceRect |= ((faceRect & (1ul << i)) >> i) << (63 - i);
                    }
                    break;
            }
            return (FaceRect)newFaceRect;
        }
    }
}
