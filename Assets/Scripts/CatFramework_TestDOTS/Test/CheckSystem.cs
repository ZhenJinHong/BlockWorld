using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UI;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.VFX;

namespace CatFramework_TestDOTS
{
    [DisableAutoCreation]
    [BurstCompile]
    internal partial struct CheckSystem : ISystem
    {
        EntityQuery visualEffectQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
            entityQueryBuilder
                .WithAll<VisualEffect>();

            visualEffectQuery = entityQueryBuilder.Build(ref state);

            state.RequireForUpdate(visualEffectQuery);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //Debug.Log($"视觉效果数量：{visualEffectQuery.CalculateEntityCount()}");
            //var ecsSingleton= SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            //var ecs = ecsSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            //var job = new FallCheckJob
            //{
            //    writer = ecs.AsParallelWriter(),
            //};
            //job.ScheduleParallel();
            //if (SystemAPI.TryGetSingleton<AssetCollection>(out AssetCollection collection))
            //{
            //    Debug.Log("找到集合");
            //}
            //EntityArchetype
            //state.EntityManager.Instantiate(new Entity ());
            //state.EntityManager.CreateArchetype();
            //state.EntityManager.CreateEntity(state.EntityManager.GetChunk(new Entity()).Archetype);
            //state.EntityManager.SetComponentData<>
            //state.EntityManager.SetArchetype;
            //state.EntityManager.GetChunk;
            //SystemAPI.;
            //EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            //entityCommandBuffer.Instantiate();
            //entityCommandBuffer.DestroyEntity();
            //entityCommandBuffer.CreateEntity();
            //entityCommandBuffer.Playback();
            //SystemAPI.GetSingleton<PhysicsWorldSingleton>().CastRay(new RaycastInput(),out Unity.Physics.RaycastHit hit);
            //ComponentLookup<Bedrock> componentLookup=state.GetComponentLookup<Bedrock>();
            //componentLookup.
            //var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            ////collisionWorld.CheckSphere(0f, new Unity.Mathematics.Random(math.asuint(SystemAPI.Time.DeltaTime)).NextFloat(0.01f, 10f), new CollisionFilter() { BelongsTo = ~0u, CollidesWith = ~0u, GroupIndex = 0 });
            //Unity.Mathematics.Random random = new Unity.Mathematics.Random(math.asuint(SystemAPI.Time.DeltaTime));

            //bool result = collisionWorld.CheckBox(random.NextFloat(1f, 100f), quaternion.identity, random.NextFloat3(0f, 10f), new CollisionFilter() { BelongsTo = ~0u, CollidesWith = ~0u });
            //Debug.Log($"{result}");
        }
    }
    [BurstCompile]
    struct UpdateChunkJob : IJob
    {
        [BurstCompile]
        public void Execute()
        {

        }
    }
    //[WithAll(typeof(PhysicsCollider))]
    //[BurstCompile]
    //partial struct FallCheckJob : IJobEntity
    //{
    //    public EntityCommandBuffer.ParallelWriter writer;
    //    void Execute(Entity entity,[ChunkIndexInQuery] int chunkIndex,ref WorldTransform worldTransform)
    //    {
    //        if (worldTransform.Position.y < -100f)
    //        {
    //            //writer.DestroyEntity(chunkIndex, entity);
    //        }
    //    }
    //}
}
