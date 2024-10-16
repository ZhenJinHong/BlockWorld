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
    public struct OctavePerlinTerrainJob : IJob
    {
        public WorldTopography WorldTopography;
        public OctavePerlinSeed Seed;
        public float2 BigChunkPos;
        public NativeArray<byte> HeightMap;
        [ReadOnly] public NativeArray<float> RegionMap;
        public void Execute()
        {
            BigChunkPos += Seed.Offset;
            for (int x = 0; x < Settings.SmallChunkSize; x++)
            {
                for (int z = 0; z < Settings.SmallChunkSize; z++)
                {
                    int index = VoxelMath.SmallChunkWidthD2IndexToIndex(x, z);
                    float rangeValue = RegionMap[index];
                    if (TerrainMaths.Range(Seed.Range, rangeValue, out float p))
                    {
                        float noise = TerrainMaths.OctavePerlin(new float2(x, z) + BigChunkPos, Seed.Persistance, Seed.Frequency);
                        //noise = TerrainMaths.Lerp(noise, rangeValue, p);
                        HeightMap[index] = TerrainMaths.Height(WorldTopography, noise, Seed.TerrainHeight, rangeValue, p);
                    }
                }
            }
        }
    }
}
