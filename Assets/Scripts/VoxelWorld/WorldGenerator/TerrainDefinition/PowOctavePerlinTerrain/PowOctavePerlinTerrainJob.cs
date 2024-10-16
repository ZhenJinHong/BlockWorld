using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct PowOctavePerlinTerrainJob : IJob
    {
        public PowOctavePerlinSeed Seed;
        public WorldTopography WorldTopography;

        [ReadOnly] public NativeArray<float> RegionMap;
        public NativeArray<byte> HeightMap;
        public float2 BigChunkPos;
        public void Execute()
        {
            BigChunkPos += Seed.Offset;
            for (int z = 0; z < Settings.SmallChunkSize; z++)
            {
                for (int x = 0; x < Settings.SmallChunkSize; x++)
                {
                    int index = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                    float rangeValue = RegionMap[index];
                    if (TerrainMaths.Range(Seed.Range, rangeValue, out float p))
                    {
                        float noise = TerrainMaths.OctavePerlin(new float2(x, z) + BigChunkPos, Seed.persistance, Seed.frequency);

                        //noise = TerrainMaths.Lerp(noise, rangeValue, p);
                        noise = Pow(noise * Seed.persistance);
                        static float Pow(float v)
                        {
                            return v * v * (v < 0f ? -1f : 1f);
                        }
                        HeightMap[index] = TerrainMaths.Height(WorldTopography, noise, Seed.TerrainHeight, rangeValue, p);
                    }
                }
            }
        }
    }
}