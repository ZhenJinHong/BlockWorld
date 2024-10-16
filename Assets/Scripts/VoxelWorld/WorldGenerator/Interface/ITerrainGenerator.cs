using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public interface ITerrainGenerator : IDisposable
    {
        JobHandle ScheduleGenerateHeightMap(IBigChunkMapContainer bigChunkMapContainer, WorldTopography worldTopography, JobHandle dependsOn);
    }
}
