using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public abstract class BaseWorldRegionDefinition : ScriptableObject, IWorldRegionDefinition
    {
        public abstract IWorldRegionGenarator Create(uint seed, float baseHeight);
    }
    public abstract class BaseWorldRegionDefinition<Builder> : BaseWorldRegionDefinition
        where Builder : IWorldRegionDefinition, new()
    {
        [SerializeField] protected Builder builder = new();
        public override IWorldRegionGenarator Create(uint seed, float baseHeight)
        {
            return builder.Create(seed, baseHeight);
        }
    }
}