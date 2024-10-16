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
    public struct MessyTerrainJob : IJob
    {
        public WorldTopography WorldTopography;
        public MessySeed Seed;
        public float2 BigChunkPos;
        public NativeArray<byte> HeightMap;
        [ReadOnly] public NativeArray<float> RegionMap;
        public void Execute()
        {
            float2 bigChunkPos = new float2(BigChunkPos.x, BigChunkPos.y);
            for (int x = 0; x < Settings.SmallChunkSize; x++)
            {
                for (int z = 0; z < Settings.SmallChunkSize; z++)
                {
                    int index = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                    float rangeValue = RegionMap[index];
                    if (TerrainMaths.Range(Seed.Range, rangeValue, out float p))
                    {
                        float2 pos = new float2(x, z) + bigChunkPos;
                        float noise = Noise.GetSNoise(pos, Seed.Scale * 0.1f);
                        noise += Noise.GetPerlinNoise(pos, Seed.Scale);
                        //noise = math.lerp(noise, rangeValue, p);
                        //HeightMap[index] = TerrainMaths.ClampHeight((noise + rangeValue) * Seed.TerrainHeight + BaseHeight);
                        //noise = TerrainMaths.Lerp(noise, rangeValue, p);
                        HeightMap[index] = TerrainMaths.Height(WorldTopography, noise, Seed.TerrainHeight, rangeValue, p);
                    }
                }
            }
        }
    }
}
