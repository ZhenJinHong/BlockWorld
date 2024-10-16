using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public struct NatureWorldFilling
    {
        public BlobArray<Voxel> Blocks;
        public BlobArray<Voxel> UnderWaterBlocks;
        public BlobArray<Voxel> SurfaceBlocks;
        public BlobArray<Voxel> Grasss;
        public BlobArray<Voxel> Flowers;

        public float FlowerRangeOffset;
        public float FlowerRangeScale;
        public float FlowerThreshold;
        public float WaterHeight;
    }
}