using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class NatureWorldFillerBuilder : IWorldFillerDefinition
    {
        public string Name { get; set; }
        public float2 FlowerRangeOffset = new float2(1f, 1234f);
        public float2 FlowerRangeScale = new float2(1f, 2f);
        public float2 FlowerThreshold = new float2(-0.3f, 0.5f);
        public float2 SeaLevelOffset = new float2(-1f, 1f);
        public Voxel[] Blocks;
        public Voxel[] UnderWaterBlocks;
        public Voxel[] SurfaceBlocks;
        public Voxel[] Grasss;
        public Voxel[] Flowers;
        public IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            return Create(seed, baseHeight);
        }
        public IWorldFiller Create(uint seed, float baseHeight)
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);

            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            ref NatureWorldFilling filler = ref blobBuilder.ConstructRoot<NatureWorldFilling>();
            filler.FlowerRangeOffset = random.NextFloat(FlowerRangeOffset.x, FlowerRangeOffset.y);
            filler.FlowerRangeScale = random.NextFloat(FlowerRangeScale.x, FlowerRangeScale.y);
            filler.FlowerThreshold = random.NextFloat(FlowerThreshold.x, FlowerThreshold.y);
            filler.WaterHeight = baseHeight + random.NextFloat(SeaLevelOffset.x, SeaLevelOffset.y);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Blocks, Blocks);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.UnderWaterBlocks, UnderWaterBlocks);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.SurfaceBlocks, SurfaceBlocks);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Grasss, Grasss);
            BlobReferenceExtension.VoxelArrayToBlobArray(blobBuilder, ref filler.Flowers, Flowers);

            var Filler = blobBuilder.CreateBlobAssetReference<NatureWorldFilling>(Allocator.Persistent);
            blobBuilder.Dispose();

            return new NatureWorldFiller(Filler) { Name = Name };
        }
    }
}
