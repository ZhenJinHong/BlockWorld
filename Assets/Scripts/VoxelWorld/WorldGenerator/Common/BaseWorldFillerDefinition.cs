using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public abstract class BaseWorldFillerDefinition : ScriptableObject, IWorldFillerDefinition
    {
        public abstract IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase);
    }
}