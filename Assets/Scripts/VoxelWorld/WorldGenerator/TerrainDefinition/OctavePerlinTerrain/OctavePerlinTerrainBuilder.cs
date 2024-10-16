using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class OctavePerlinTerrainBuilder : ITerrainDefinition
    {
        public string Name { get; set; }
        public float2 Offset = new float2(122f, 2345f);
        public float2 TerrainHeight = new float2(10f, 12f);
        public float2 Persistance = new float2(1f, 1.3f);
        public float2 Frequency = new float2(0.04f, 0.06f);
        public float2 Range = new float2(-1f, 1f);
        public OctavePerlinTerrainBuilder()
        {
        }
        public ITerrainGenerator Create(uint seed, float baseHeight)
        {
            Unity.Mathematics.Random random = new(seed);
            return new OctavePerlinTerrain(new OctavePerlinSeed()
            {
                Offset = random.NextFloat(Offset.x, Offset.y),
                TerrainHeight = random.NextFloat(TerrainHeight.x, TerrainHeight.y),
                Frequency = random.NextFloat(Frequency.x, Frequency.y),
                Persistance = random.NextFloat(Persistance.x, Persistance.y),
                Range = Range,
            });
        }
    }
}
