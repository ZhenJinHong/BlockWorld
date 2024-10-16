using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct EntityVelocity : IComponentData
    {
        public float3 Value;
    }
}
