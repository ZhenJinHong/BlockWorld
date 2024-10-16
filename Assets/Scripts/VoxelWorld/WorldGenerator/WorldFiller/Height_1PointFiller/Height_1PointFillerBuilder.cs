using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [Serializable]
    public class Height_1PointFillerBuilder : IWorldFillerDefinition
    {
        public string Name { get; set; }
        public Voxel Toput;
        public float2 RangeScale = new float2(0.03f, 0.04f);
        public float2 PutScale = new float2(1.0022f, 1.9954f);
        public float2 RangeThreshold = new float2(0.0f, 0.1f);
        public float2 PutThreshold = new float2(0.75f, 0.8f);
        public IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);


            return new Height_1PointFiller()
            {
                Name = Name,
                Toput = Toput,
                RangeScale = random.NextFloat(RangeScale.x, RangeScale.y),
                PutScale = random.NextFloat(PutScale.x, PutScale.y),
                RangeThreshold = random.NextFloat(RangeThreshold.x, RangeThreshold.y),
                PutThreshold=random.NextFloat(PutThreshold.x,PutThreshold.y),
            };
        }
    }
}
