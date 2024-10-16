using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public abstract class BaseTerrainDefinition : ScriptableObject, ITerrainDefinition
    {
        public virtual string Name => name;
        public abstract ITerrainGenerator Create(uint seed, float baseHeight);
    }
    public abstract class BaseTerrainDefinition<T> : BaseTerrainDefinition
        where T : ITerrainDefinition, new()
    {
        [SerializeField] protected T builder = new();
        public override ITerrainGenerator Create(uint seed, float baseHeight)
        {
            return builder.Create(seed, baseHeight);
        }
    }
}
