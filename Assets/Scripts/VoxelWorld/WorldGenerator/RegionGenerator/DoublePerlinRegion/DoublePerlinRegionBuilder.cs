using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class DoublePerlinRegionBuilder : IWorldRegionDefinition
    {
        public float2 Scale = new float2(0.0008f, 0.001f);
        public float2 Offset = new float2(1f, 1000f);
        public float2 SecondScale = new float2(0.002f, 0.004f);
        public float2 FirstNoisePercent = new float2(0.2f, 0.4f);
        public string Name { get; set; }
        IWorldRegionGenarator IWorldRegionDefinition.Create(uint seed, float baseHeight)
        {
            Unity.Mathematics.Random random = new(seed);
            return new DoublePerlinRegion()
            {
                Scale = random.NextFloat(Scale.x, Scale.y),
                Offset = random.NextFloat(Offset.x, Offset.y),
                SecondScale = random.NextFloat(SecondScale.x, SecondScale.y),
                FirstNoisePercent = random.NextFloat(FirstNoisePercent.x, FirstNoisePercent.y),
            };
        }
    }
}
