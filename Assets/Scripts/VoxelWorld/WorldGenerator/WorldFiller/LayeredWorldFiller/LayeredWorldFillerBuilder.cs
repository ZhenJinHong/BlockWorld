using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class LayeredWorldFillerBuilder : IWorldFillerDefinition
    {
        public string Name { get; set; }
        public float2 FlowerRangeOffset = new float2(1f, 1234f);
        public float2 FlowerRangeScale = new float2(1f, 2f);
        public float2 FlowerThreshold = new float2(-0.3f, 0.5f);
        public float2 SeaLevelOffset = new float2(-5f, 5f);
        public float BorderWidth = 0.03f;
        public float BorderScale = 0.05f;
        public Voxel[] Blocks;
        public Voxel[] SurfaceBlocks;
        public Voxel[] Grasss;
        public Voxel[] Flowers;
        public IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            return Create(seed, baseHeight);
        }
        public IWorldFiller Create(uint seed, float baseHeight)
        {
            Unity.Mathematics.Random random = new Random(seed);

            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            ref LayeredWorldFilling filler = ref blobBuilder.ConstructRoot<LayeredWorldFilling>();
            filler.FlowerRangeOffset = random.NextFloat(FlowerRangeOffset.x, FlowerRangeOffset.y);
            filler.FlowerRangeScale = random.NextFloat(FlowerRangeScale.x, FlowerRangeScale.y);
            filler.FlowerThreshold = random.NextFloat(FlowerThreshold.x, FlowerThreshold.y);
            filler.WaterHeight = baseHeight + random.NextFloat(SeaLevelOffset.x, SeaLevelOffset.y);
            filler.BorderWidth = BorderWidth;
            filler.BorderScale = BorderScale;
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Blocks, Blocks);
            //BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.UnderWaterBlocks, UnderWaterBlocks);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.SurfaceBlocks, SurfaceBlocks);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Grasss, Grasss);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Flowers, Flowers);

            var Filling = blobBuilder.CreateBlobAssetReference<LayeredWorldFilling>(Allocator.Persistent);
            blobBuilder.Dispose();

            return new LayeredWorldFiller(Filling) { Name = Name };
        }
    }
}
