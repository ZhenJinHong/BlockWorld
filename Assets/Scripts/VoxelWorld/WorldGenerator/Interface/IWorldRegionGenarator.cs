using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public interface IWorldRegionGenarator : IDisposable
    {
        JobHandle ScheduleRegionJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn);
    }
}
