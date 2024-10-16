using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatFramework_TestDOTS.Assets.Scripts.CatFramework_TestDOTS.Test
{
    public class VoxelColliderTestAuthoring : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] int width = 9;
        class VBaker : Baker<VoxelColliderTestAuthoring>
        {
            public override void Bake(VoxelColliderTestAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<VoxelTestTag>(entity, new VoxelTestTag()
                {
                    Entity = GetEntity(authoring.target, TransformUsageFlags.Dynamic),
                    width = authoring.width,
                });
            }
        }
    }
    public struct VoxelTestTag : IComponentData
    {
        public Entity Entity;
        public int width;
    }
}