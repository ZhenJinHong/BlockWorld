using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public abstract class BaseEntityFillerDefinition : ScriptableObject, IEntityFillerDefinition
    {
        public string Name => name;
        public abstract IEntityFiller Create(uint seed, VoxelWorldDataBaseManaged dataBase);
    }
}
