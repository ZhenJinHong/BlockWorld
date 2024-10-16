using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public class EntitySimulationAuthoring : MonoBehaviour
    {
        class EntitySimulationBaker : Baker<EntitySimulationAuthoring>
        {
            public override void Bake(EntitySimulationAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EntityVelocity>(entity);
                AddComponent<EntityGravityFactor>(entity, new EntityGravityFactor()
                {
                    Value = 1f,
                });
                AddComponent<EntitySimulationResult>(entity);
            }
        }
    }
}