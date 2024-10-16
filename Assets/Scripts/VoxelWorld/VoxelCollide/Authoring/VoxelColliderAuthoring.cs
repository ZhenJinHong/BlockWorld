using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public class VoxelColliderAuthoring : MonoBehaviour
    {
        class VoxelColliderBaker : Baker<VoxelColliderAuthoring>
        {
            public override void Bake(VoxelColliderAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PhysicsCollider>(entity);
                AddSharedComponent<PhysicsWorldIndex>(entity, new PhysicsWorldIndex()
                {
                    Value = 0,
                });
                AddComponent<VoxelCollider>(entity, new VoxelCollider()
                {
                    ShapeIndex = ushort.MaxValue,
                });
                AddComponent<Prefab>(entity);
            }
        }
    }
    public struct VoxelCollider : IComponentData
    {
        public ushort ShapeIndex;
        public byte ShapeDirection;
        public bool solid;
    }
}