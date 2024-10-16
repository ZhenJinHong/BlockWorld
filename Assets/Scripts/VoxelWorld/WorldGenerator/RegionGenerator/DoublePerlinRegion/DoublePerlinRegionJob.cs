using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct DoublePerlinRegionJob : IJob
    {
        public float Scale;
        public float Offset;
        public float SecondScale;
        public float FirstNoisePercent;
        public float2 BigChunkPos;
        public NativeArray<float> RegionNoiseMap;
        public void Execute()
        {
            BigChunkPos += Offset;
            for (int x = 0; x < Settings.SmallChunkSize; x++)
            {
                for (int z = 0; z < Settings.SmallChunkSize; z++)
                {
                    int index = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                    float2 pos = new float2(x, z) + BigChunkPos;
                    RegionNoiseMap[index] = (Noise.GetPerlinNoise(pos, Scale) * FirstNoisePercent + Noise.GetPerlinNoise(pos, SecondScale) * (1 - FirstNoisePercent));
                }
            }
        }
    }
}
