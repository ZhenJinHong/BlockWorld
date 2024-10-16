using Unity.Collections;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public interface ISmallChunkDataContainer
    {
        float3 SmallChunkPos { get; }
        int3 SmallChunkPosInt { get; }
        NativeArray<Voxel> SmallChunkVoxelMap { get; }
    }
}