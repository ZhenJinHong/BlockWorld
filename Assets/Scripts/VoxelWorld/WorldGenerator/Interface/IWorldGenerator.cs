using System;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public interface IWorldGenerator : IDisposable
    {
        JobHandle ScheduleGenerateBigChunkMap(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn);
        JobHandle ScheduleFillBigChunk(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn);
        JobHandle ScheduleFillEntity(VoxelWorldMap.BigChunkSliceReadOnly bigChunkSlice, EntityCommandBufferSystem entityCommandBufferSystem, JobHandle dependsOn);
    }
}