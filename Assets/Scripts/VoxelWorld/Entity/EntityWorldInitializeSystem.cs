//using CatDOTS;
//using System;
//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Collections.LowLevel.Unsafe;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Physics;
//using UnityEngine;

//namespace CatFrameworkDOTS.VoxelWorld
//{
//    [UpdateInGroup(typeof(LateUpdateSystemMiaoGroup))]
//    [BurstCompile]
//    public partial struct EntityWorldInitializeSystem : ISystem
//    {
//        [BurstCompile]
//        public void OnCreate(ref SystemState state)
//        {
//        }
//        [BurstCompile]
//        public void OnDestroy(ref SystemState state)
//        {
//        }
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//            state.Dependency = new PhysicssC().Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//        }
//    }
//    [BurstCompile]
//    public struct PhysicssC : ICollisionEventsJob
//    {
//        public void Execute(CollisionEvent collisionEvent)
//        {
//            Debug.Log($"{collisionEvent.ColliderKeyA}");
//        }

//        public void Execute(TriggerEvent triggerEvent)
//        {
//            Debug.Log($"{triggerEvent.ColliderKeyA}");
//        }
//    }
//}
