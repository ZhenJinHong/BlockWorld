using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class MessyTerrain : ITerrainGenerator
    {
        MessySeed Seed;
        public void Dispose()
        {
        }
        public MessyTerrain(MessySeed seed)
        {
            Seed = seed;
        }
        public JobHandle ScheduleGenerateHeightMap(IBigChunkMapContainer bigChunkMapContainer, WorldTopography worldTopography, JobHandle dependsOn)
        {
            return new MessyTerrainJob()
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
