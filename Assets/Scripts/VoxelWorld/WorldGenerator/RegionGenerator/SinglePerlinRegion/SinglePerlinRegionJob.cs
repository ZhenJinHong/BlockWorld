using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct SinglePerlinRegionJob : IJob
    {
        public float Scale;
        public float Offset;
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
                    RegionNoiseMap[index] = Noise.GetPerlinNoise((new float2(x, z) + BigChunkPos), Scale);
                }
            }
        }
    }
}