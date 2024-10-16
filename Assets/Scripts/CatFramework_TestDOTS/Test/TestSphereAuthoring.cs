using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CatFramework_TestDOTS
{
    internal class TestSphereAuthoring : MonoBehaviour
    {
        public ushort SpawnCount;
        public float3 SpawnCoord;
        class TestSphereBaker : Baker<TestSphereAuthoring>
        {
            public override void Bake(TestSphereAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Prefab>(entity);
                AddComponent<TestSphereSingleton>(entity, new TestSphereSingleton()
                {
                    SpawnCount = authoring.SpawnCount,
                    SpawnCoord = authoring.SpawnCoord,
                });
            }
        }
    }
    internal struct TestSphereSingleton : IComponentData
    {
        public ushort SpawnCount;
        public float3 SpawnCoord;
    }
}
