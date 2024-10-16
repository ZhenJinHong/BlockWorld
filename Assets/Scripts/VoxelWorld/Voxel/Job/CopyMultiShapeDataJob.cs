using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct CopyMultiShapeDataJob : IJob// 如果交由网格构建工作处理旋转,则各方向不同形状的不再支持
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
            int baseIndexIndex = allIndexs.Length;
            int baseVertexIndex = allverts.Length;

            for (int i = 0; i < 有序临时面数据数组.Length; i++)
            {
                VoxelFaceData voxelFaceData = 有序临时面数据数组[i];
                voxelFaceData.IndexStartIndex += baseIndexIndex;
                voxelFaceData.IndexEnd += baseIndexIndex;
                voxelFaceData.VertexStartIndex += baseVertexIndex;
                voxelFaceData.VertexEnd += baseVertexIndex;
                allfaceDatas.Add(voxelFaceData);
            }
            allIndexs.AddRange(有序临时索引数组.AsArray());
            for (int i = 0; i < 有序临时顶点数组.Length; i++)
            {
                allverts.Add(有序临时顶点数组[i] + 0.5f);
            }
            alluvs.AddRange(有序临时UV数组.AsArray());
            allnormals.AddRange(有序临时法向数组.AsArray());
        }
    }
}
