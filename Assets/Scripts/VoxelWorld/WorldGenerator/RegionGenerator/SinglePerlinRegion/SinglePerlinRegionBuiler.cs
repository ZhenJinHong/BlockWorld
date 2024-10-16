using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class SinglePerlinRegionBuiler : IWorldRegionDefinition
    {
        public float2 Scale = new float2(0.0008f, 0.001f);
        public float2 Offset = new float2(1f, 1000f);
        public IWorldRegionGenarator Create(uint seed, float baseHeight)
        {
            Unity.Mathematics.Random random = new(seed);

            return new SinglePerlinRegion()
            {
                Scale = random.NextFloat(Scale.x, Scale.y),
                Offset = random.NextFloat(Offset.x, Offset.y),
            };
        }
    }
}