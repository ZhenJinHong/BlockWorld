using Unity.Collections;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class FlatFillerBuilder : IWorldFillerDefinition
    {
        public string Name { get; set; }
        public Voxel[] LayerVoxels;

        public IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            NativeArray<Voxel> voxels = new NativeArray<Voxel>(LayerVoxels.Length, Allocator.Persistent);
            voxels.CopyFrom(LayerVoxels);
            return new FlatFiller(voxels)
            {
                Name = Name,
            };
        }
    }
}