using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class MessyTerrainBuilder : ITerrainDefinition
    {
        public string Name { get; set; }
        public float2 Scale = new float2(0.02f, 0.08f);
        public float2 TerrainHeight = new float2(10f, 20f);
        public float2 Range = new float2(-1, 1f);
        public MessyTerrainBuilder()
        {
        }
        public ITerrainGenerator Create(uint seed, float baseHeight)
        {
            Random random = new Random(seed);
            return new MessyTerrain(new MessySeed()
            {
                Scale = random.NextFloat(0.02f, 0.08f),
                TerrainHeight = random.NextFloat(10f, 20f),
                Range = Range,
            });
        }
    }
}
