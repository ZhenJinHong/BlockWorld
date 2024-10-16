using System;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public interface IEntityFiller : IDisposable
    {
        JobHandle Schedule(VoxelWorldMap.BigChunkSliceReadOnly bigChunkSlice, EntityCommandBuffer ECB, JobHandle dependOn);
    }
}
