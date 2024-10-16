using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct Height_1PointFillerJob : IJob
    {
        public Voxel Toput;
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
            for (int z = 0; z < Settings.SmallChunkSize; z++)
            {
                for (int x = 0; x < Settings.SmallChunkSize; x++)
                {
                    float2 pos = new float2(x, z) + BigChunkPos.As2D();
                    if (Noise.GetPerlinNoise(pos * RangeScale) > RangeThreshold && Noise.GetPerlinNoise(pos * PutScale) > PutThreShold)
                    {
                        int index = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                        int voxelIndex = VoxelMath.LocalVoxelArrayIndexInBigChunk(x, HeightMap[index] + 1, z);
                        Voxels[voxelIndex] = Toput;
                    }
                }
            }
        }
    }
}
