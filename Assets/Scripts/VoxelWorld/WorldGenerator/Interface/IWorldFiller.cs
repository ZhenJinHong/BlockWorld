using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public interface IWorldFiller : IDisposable
    {
        string Name { get; set; }

        JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer,  JobHandle dependsOn);
    }
}
