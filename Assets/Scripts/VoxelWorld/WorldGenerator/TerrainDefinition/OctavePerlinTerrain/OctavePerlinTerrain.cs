using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class OctavePerlinTerrain : ITerrainGenerator
    {
        OctavePerlinSeed Seed;
        public OctavePerlinTerrain(OctavePerlinSeed seed)
        {
            Seed = seed;
        }
        public void Dispose()
        {
        }

        public JobHandle ScheduleGenerateHeightMap(IBigChunkMapContainer bigChunkMapContainer, WorldTopography worldTopography, JobHandle dependsOn)
        {
            return new OctavePerlinTerrainJob()
            {
                WorldTopography = worldTopography,
                Seed = Seed,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt.As2D(),

                HeightMap = bigChunkMapContainer.HeightMap,
                RegionMap = bigChunkMapContainer.RegionNoiseMap,

            }.Schedule(dependsOn);
        }

    }
}
