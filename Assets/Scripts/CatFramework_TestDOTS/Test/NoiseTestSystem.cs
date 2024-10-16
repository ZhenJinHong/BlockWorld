using System.Collections;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace CatFramework_TestDOTS
{
    [BurstCompile, DisableAutoCreation]
    public partial struct NoiseTestSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}