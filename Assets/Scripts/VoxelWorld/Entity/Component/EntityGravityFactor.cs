using System;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public struct EntityGravityFactor : IComponentData
    {
        public float Value;
    }
}
