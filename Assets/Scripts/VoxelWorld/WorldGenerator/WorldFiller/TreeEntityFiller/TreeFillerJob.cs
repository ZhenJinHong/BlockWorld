using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct TreeFillerJob : IJob
    {
        public Voxel Trunk;
        public Voxel Leave;
        [ReadOnly] public NativeArray<byte> HeightMap;
        [ReadOnly] public NativeArray<float> RangeNoiseMap;
        public NativeArray<Voxel> Voxels;
        public int3 BigChunkPos;
        public float RangeScale;
        public float PutScale;
        public float RangeThreshold;
        public float PutThreShold;
        public void Execute()
        {
            int maxhalfWidth = 2;
            for (int z = 0; z < Settings.SmallChunkSize; z++)
            {
                for (int x = 0; x < Settings.SmallChunkSize; x++)
                {
                    if (!In(new int2(x, z), maxhalfWidth)) continue;
                    int terrainHeight = HeightMap[VoxelMath.SmallChunkWidthD2IndexToIndex(x, z)];
                    int voxelIndex = VoxelMath.LocalVoxelArrayIndexInBigChunk(x, terrainHeight + 1, z);
                    if (Voxel.Water(Voxels[voxelIndex].VoxelMaterial)) continue;
                    float2 pos = new float2(x, z) + BigChunkPos.As2D();
                    if (terrainHeight < Settings.WorldHeightInVoxel - 10 && Noise.GetPerlinNoise(pos * RangeScale) > RangeThreshold && Noise.GetPerlinNoise(pos * PutScale) > PutThreShold)
                    {
                        // trunk

                        // 
                        int trunkHeight = terrainHeight + (x & 1) + (z & 1) + 5;
                        for (int i = terrainHeight + 1; i < trunkHeight; i++)
                        {
                            Voxels[voxelIndex] = Trunk;
                            voxelIndex += Settings.VoxelCountInFloor;
                        }
                        int2 xz = new int2(x, z);
                        FillRect(trunkHeight - 2, maxhalfWidth, xz, Leave, Voxels);
                        FillRect(trunkHeight - 1, maxhalfWidth, xz, Leave, Voxels);
                        FillCircle(trunkHeight, maxhalfWidth, xz, Leave, Voxels);
                        FillCircle(trunkHeight + 1, maxhalfWidth - 1, xz, Leave, Voxels);
                    }
                }
            }
        }
        static bool In(int2 xz, int treeHalfWidth)
        {
            return math.all(math.bool4(xz - treeHalfWidth >= 0, xz + treeHalfWidth < Settings.SmallChunkSize));
        }
        static void FillVerticalTrunk()
        {

        }
        // 中心加宽小于区块大小,所以遍历的时候可以等于宽度
        static void FillRect(int height, int halfWidth, int2 xz, Voxel leave, NativeArray<Voxel> Voxels)
        {
            // 一层矩形叶
            for (int z = -halfWidth; z <= halfWidth; z++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++)
                {
                    int tempIndex = VoxelMath.LocalVoxelArrayIndexInBigChunk(xz.x + x, height, xz.y + z);
                    if (Voxel.IsAir(Voxels[tempIndex].VoxelTypeIndex))
                        Voxels[tempIndex] = leave;
                }
            }
        }
        static void FillCircle(int height, int halfWidth, int2 xz, Voxel leave, NativeArray<Voxel> Voxels)
        {
            int radiussq = halfWidth * halfWidth;
            for (int z = -halfWidth; z <= halfWidth; z++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++)
                {
                    if (x * x + z * z <= radiussq)
                    {
                        int tempIndex = VoxelMath.LocalVoxelArrayIndexInBigChunk(xz.x + x, height, xz.y + z);
                        if (Voxel.IsAir(Voxels[tempIndex].VoxelTypeIndex))
                            Voxels[tempIndex] = leave;
                    }
                }
            }
        }
    }
}
