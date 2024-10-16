using CatFramework;
using CatFramework.Tools;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class WorldGeneratorBuilder : IWorldDefinition
    {
        public string Name { get; set; }
        public float2 BaseHeight = new float2(90f, 130f);
        public float2 TopographicHeightDiff = new float2(40f, 50f);
        public IWorldRegionDefinition WorldRegionDefinition;
        public ITerrainDefinition[] TerrainDefinitions;
        public IWorldFillerDefinition[] WorldFillerDefinitions;
        public IEntityFillerDefinition[] EntityFillerDefinitions;
        public WorldGeneratorBuilder() { }
        public IWorldGenerator Create(uint seed, VoxelWorldDataBaseManaged dataBase)
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);
            float baseHeight = random.NextFloat(BaseHeight.x, BaseHeight.y);
            float topographicHeightDiff = random.NextFloat(TopographicHeightDiff.x, TopographicHeightDiff.y);

            // 世界地势
            WorldTopography worldTopography = new WorldTopography()
            {
                BaseHeight = baseHeight,
                TopographicHeightDiff = topographicHeightDiff,
            };
            // 区域
            IWorldRegionGenarator worldRegionGenarator = 
                WorldRegionDefinition == null ? null : WorldRegionDefinition.Create(random.NextUInt(), baseHeight);
            // 地形
            ITerrainGenerator[] terrainGenerators = new ITerrainGenerator[TerrainDefinitions.SafeLength()];
            for (int i = 0; i < terrainGenerators.Length; i++)
            {
                seed = random.NextUInt();
                terrainGenerators[i] = TerrainDefinitions[i].Create(seed, baseHeight);
            }
            // 体素填充
            IWorldFiller[] worldFillerGenarators = new IWorldFiller[WorldFillerDefinitions.SafeLength()];
            for (int i = 0; i < worldFillerGenarators.Length; i++)
            {
                seed = random.NextUInt();
                worldFillerGenarators[i] = WorldFillerDefinitions[i].Create(seed, baseHeight, dataBase);
            }
            // 实体填充
            IEntityFiller[] entityFillers = new IEntityFiller[EntityFillerDefinitions.SafeLength()];
            for (int i = 0; i < entityFillers.Length; i++)
            {
                seed = random.NextUInt();
                entityFillers[i] = EntityFillerDefinitions[i].Create(seed, dataBase);
            }
            
            return new WorldGenerator()
            {
                WorldTopography = worldTopography,
                WorldRegionGenarator = worldRegionGenarator,
                TerrainGenerators = terrainGenerators,
                WorldFillerGenarators = worldFillerGenarators,
                EntityFillers = entityFillers,
            };
        }
    }
}
