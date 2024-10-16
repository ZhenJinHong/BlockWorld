namespace CatDOTS.VoxelWorld
{
    public interface ITerrainDefinition
    {
        string Name { get; }
        ITerrainGenerator Create(uint seed, float baseHeight);
    }
}