namespace CatDOTS.VoxelWorld
{
    public interface IEntityFillerDefinition
    {
        string Name { get; }

        IEntityFiller Create(uint seed, VoxelWorldDataBaseManaged dataBase);
    }
}
