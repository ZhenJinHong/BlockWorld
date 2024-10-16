using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct LayeredWorldFillerJob : IJobParallelForBatch
    {
        public BlobAssetReference<LayeredWorldFilling> WorldFilling;
        [ReadOnly] public NativeArray<byte> HeightMap;
        [ReadOnly] public NativeArray<float> RangeNoiseMap;
        public NativeArray<Voxel> Voxels;
        public int3 BigChunkPos;
        public void Execute(int startIndex, int count)
        {
            // startIndex 0,4096....
            int y = startIndex / Settings.VoxelCapacityInSmallChunk * Settings.SmallChunkSize;
            int x = 0, z = 0;
            int end = startIndex + count;
            for (; startIndex < end; startIndex++)
            {
                int mapIndex = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                Voxels[startIndex] = GetVoxelID(new int3(x, y, z) + BigChunkPos, HeightMap[mapIndex], RangeNoiseMap[mapIndex], ref WorldFilling.Value);
                z++;
                if (z == Settings.SmallChunkSize)
                {
                    z = 0;
                    x++;
                    if (x == Settings.SmallChunkSize)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }
        private Voxel GetVoxelID(int3 voxelIndexInWorld, byte height, float rangeValue, ref LayeredWorldFilling filling)
        {
            Voxel voxel = Voxel.Empty;
            int y = voxelIndexInWorld.y;

            if (y > height)
            {
                if (y == height + 1)
                {
                    if (y > filling.WaterHeight)
                    {
                        float2 flowerRangeOffset = filling.FlowerRangeOffset;
                        float flower = Noise.GetSNoise(voxelIndexInWorld.xz + flowerRangeOffset, filling.FlowerRangeScale);
                        if (flower > filling.FlowerThreshold)
                        {
                            voxel = WorldMaths.GetVoxelByPercent(ref filling.Flowers, voxelIndexInWorld.xz, filling.BorderWidth, filling.BorderScale, WorldMaths.RemapToZeroOne(flower));
                        }
                        else
                        {
                            voxel = WorldMaths.GetVoxelByPercent(ref filling.Grasss, voxelIndexInWorld.xz, filling.BorderWidth, filling.BorderScale, WorldMaths.RemapToZeroOne(rangeValue));
                        }
                    }
                }
            }
            else if (y < height - 3)
            {
                voxel = WorldMaths.GetVoxelByPercent(ref filling.Blocks, y / (float)height);
            }
            else
            {
                voxel = WorldMaths.GetVoxelByPercent(ref filling.SurfaceBlocks, voxelIndexInWorld.xz, filling.BorderWidth, filling.BorderScale, WorldMaths.RemapToZeroOne(rangeValue));
            }

            if (y < filling.WaterHeight)
            {
                voxel.VoxelMaterial |= VoxelMaterial.Water;
            }
            return voxel;
        }
    }
}
