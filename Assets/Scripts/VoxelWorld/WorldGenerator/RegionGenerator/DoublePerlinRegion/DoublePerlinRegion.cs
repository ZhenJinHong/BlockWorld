using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class DoublePerlinRegion : IWorldRegionGenarator
    {
        public float Scale;
        public float Offset;
        public float SecondScale;
        public float FirstNoisePercent;
        public void Dispose()
        {
        }

        public JobHandle ScheduleRegionJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {

            return new DoublePerlinRegionJob()
            {
                Scale = Scale,
                Offset = Offset,
                SecondScale = SecondScale,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt.As2D(),
                RegionNoiseMap = bigChunkMapContainer.RegionNoiseMap,
            }.Schedule(dependsOn);
        }
    }
}
