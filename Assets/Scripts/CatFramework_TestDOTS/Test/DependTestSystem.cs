using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace CatFramework_TestDOTS
{
    struct DependTest : IComponentData
    {
        public int ID;
    }
    [DisableAutoCreation]
    internal partial class DependTestSystem : SystemBase
    {
        Entity entity1;
        Entity entity2;
        protected override void OnCreate()
        {
            base.OnCreate();
            entity1 = EntityManager.CreateEntity();
            entity2 = EntityManager.CreateEntity();

            EntityManager.AddComponent<DependTest>(entity1);
            EntityManager.AddComponent<DependTest>(entity2);
        }
        protected override void OnUpdate()
        {
            JobHandle _1 = new DependTestJob()
            {
                ECB = SystemAPI.GetSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged),
                entity = entity1,
            }.Schedule(Dependency);
            JobHandle _2 =new DependTestJob()
            {
                ECB = SystemAPI.GetSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged),
                entity = entity1,
            }.Schedule(Dependency);
            Dependency = JobHandle.CombineDependencies(_1, _2);
        }
    }
    [BurstCompile]
    struct DependTestJob : IJob
    {
        public EntityCommandBuffer ECB;
        [ReadOnly] public Entity entity;
        public void Execute()
        {
            ECB.SetComponent<DependTest>(entity, new DependTest()
            {
                ID = 2,
            });
        }
    }
}
