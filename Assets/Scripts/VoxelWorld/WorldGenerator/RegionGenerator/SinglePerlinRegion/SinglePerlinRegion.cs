using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class SinglePerlinRegion : IWorldRegionGenarator
    {
        public float Scale;
        public float Offset;
        public void Dispose()
        {
        }

        public JobHandle ScheduleRegionJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new SinglePerlinRegionJob()
            {
                Scale = Scale,
                Offset = Offset,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt.As2D(),
                RegionNoiseMap = bigChunkMapContainer.RegionNoiseMap,
            }.Schedule(dependsOn);
        }
        public JobHandle ScheduleRegionJob<Container>(Container bigChunkMapContainer, JobHandle dependsOn)
            where Container : unmanaged, IBigChunkMapContainer
        {
            return new SinglePerlinRegionJob()
            {
                Scale = Scale,
                Offset = Offset,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt.As2D(),
                RegionNoiseMap = bigChunkMapContainer.RegionNoiseMap,
            }.Schedule(dependsOn);
        }
    }
}
