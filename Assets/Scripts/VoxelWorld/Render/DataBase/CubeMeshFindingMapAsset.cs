using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [Obsolete]
    public struct CubeMeshFindingMapAsset
    {
        public BlobArray<float3> CubeVerts;
        public BlobArray<int3> CubeFaceForwardVoxelPos;
        public BlobArray<int> CubeFaceVertexIndex;
        public BlobArray<float2> CubeFaceUVs;
        public static BlobAssetReference<CubeMeshFindingMapAsset> Create()
        {
            BlobBuilder builder = new BlobBuilder(Allocator.Temp);
            ref CubeMeshFindingMapAsset voxelDataMap = ref builder.ConstructRoot<CubeMeshFindingMapAsset>();

            // 8个顶点分为 前面四个和后面四个，按顺时针
            BlobBuilderArray<float3> cubeVerts = builder.Allocate(ref voxelDataMap.CubeVerts, 8);
            cubeVerts[0] = new float3(0.0f, 0.0f, 0.0f); // 0 0
            cubeVerts[1] = new float3(0.0f, 1.0f, 0.0f); // 1 向上   y+1
            cubeVerts[2] = new float3(1.0f, 1.0f, 0.0f); // 2 再向右 x+1 
            cubeVerts[3] = new float3(1.0f, 0.0f, 0.0f); // 3 向下   y-1

            cubeVerts[4] = new float3(0.0f, 0.0f, 1.0f); // 4 0 进深 z+1
            cubeVerts[5] = new float3(0.0f, 1.0f, 1.0f); // 5 向上   y+1
            cubeVerts[6] = new float3(1.0f, 1.0f, 1.0f); // 6 再向右 x+1
            cubeVerts[7] = new float3(1.0f, 0.0f, 1.0f); // 7 向下   y-1
            // 面的三角形顶点顺序
            // 1/4  5
            // 0   2/3
            // 顺序添加0，1，3，3，1，2索引的顶点

            // 数组里是每个面要加入的顶点在顶点数组的索引
            BlobBuilderArray<int> cubeFaceVertexIndex = builder.Allocate(ref voxelDataMap.CubeFaceVertexIndex, 24);
            // 按面朝向，顺时针
            // 遍历每个面需要加的4个顶点索引
            // 前面
            cubeFaceVertexIndex[0] = 0;
            cubeFaceVertexIndex[1] = 1;
            cubeFaceVertexIndex[2] = 2;
            cubeFaceVertexIndex[3] = 3;
            // 背面
            cubeFaceVertexIndex[4] = 7;
            cubeFaceVertexIndex[5] = 6;
            cubeFaceVertexIndex[6] = 5;
            cubeFaceVertexIndex[7] = 4;
            // 顶面
            cubeFaceVertexIndex[8] = 1;
            cubeFaceVertexIndex[9] = 5;
            cubeFaceVertexIndex[10] = 6;
            cubeFaceVertexIndex[11] = 2;
            // 底面
            cubeFaceVertexIndex[12] = 4;
            cubeFaceVertexIndex[13] = 0;
            cubeFaceVertexIndex[14] = 3;
            cubeFaceVertexIndex[15] = 7;
            // 左面
            cubeFaceVertexIndex[16] = 4;
            cubeFaceVertexIndex[17] = 5;
            cubeFaceVertexIndex[18] = 1;
            cubeFaceVertexIndex[19] = 0;
            // 右面
            cubeFaceVertexIndex[20] = 3;
            cubeFaceVertexIndex[21] = 2;
            cubeFaceVertexIndex[22] = 6;
            cubeFaceVertexIndex[23] = 7;

            BlobBuilderArray<float2> cubeFaceUVs = builder.Allocate(ref voxelDataMap.CubeFaceUVs, 4);
            cubeFaceUVs[0] = new float2(0.0f, 0.0f); // 0
            cubeFaceUVs[1] = new float2(0.0f, 1.0f); // 向上y+1
            cubeFaceUVs[2] = new float2(1.0f, 1.0f); // 向右x+1
            cubeFaceUVs[3] = new float2(1.0f, 0.0f); // 向下y-1

            BlobBuilderArray<int3> cubeFaceForwardVoxelPos = builder.Allocate(ref voxelDataMap.CubeFaceForwardVoxelPos, 6);
            // 以视角顺着Z轴为方向
            cubeFaceForwardVoxelPos[0] = new int3(0, 0, -1); // 前面//方块面向视角的面为正面，相对于Z轴-1
            cubeFaceForwardVoxelPos[1] = new int3(0, 0, 1);  // 背面//方块与视角同方向的为背面，顺着Z轴+1
            cubeFaceForwardVoxelPos[2] = new int3(0, 1, 0);  // 顶面
            cubeFaceForwardVoxelPos[3] = new int3(0, -1, 0); // 底面
            cubeFaceForwardVoxelPos[4] = new int3(-1, 0, 0); // 左面
            cubeFaceForwardVoxelPos[5] = new int3(1, 0, 0);  // 右面

            BlobAssetReference<CubeMeshFindingMapAsset> result = builder.CreateBlobAssetReference<CubeMeshFindingMapAsset>(Allocator.Persistent);
            builder.Dispose();
            return result;
        }
    }
}
