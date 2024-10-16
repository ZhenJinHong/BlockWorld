using Unity.Collections;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public interface IBigChunkMapContainer
    {
        int3 BigChunkPosInt { get; }
        NativeArray<byte> HeightMap { get; }
        NativeArray<float> RegionNoiseMap { get; }
        NativeArray<Voxel> Voxels { get; }
    }
}