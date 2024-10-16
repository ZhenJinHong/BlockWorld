using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld
{
    [UpdateInGroup(typeof(LateUpdateSystemMiaoGroup))]
    [BurstCompile]
    partial struct VoxelWorldObserverSystem : ISystem, ISystemStartStop
    {
        EntityCommandBuffer CreateECB(ref SystemState state)
        {
            return SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        }
        EntityQuery observerQuery;
        EntityQuery voxelWorldQuery;
        EntityQuery playerQuery;
        NativeReference<VoxelWorldObserver> voxelWorldObserver;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAllRW<VoxelWorldObserver>();
            observerQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<VoxelWorldTag, Archival>();
            voxelWorldQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<FirstPersonPlayer, FirstPersonPlayerOutputCache, LocalTransform>();
            playerQuery = builder.Build(ref state);

            state.RequireForUpdate(voxelWorldQuery);

            builder.Dispose();

            voxelWorldObserver = new NativeReference<VoxelWorldObserver>(Allocator.Persistent);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            voxelWorldObserver.Dispose();
        }
        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            Entity voxelWorldEntity = voxelWorldQuery.GetSingletonEntity();
            Archival archival = state.EntityManager.GetComponentData<Archival>(voxelWorldEntity);
            ref Archival.Data data = ref archival.ArchiveDataRef.Value;
            var ecb = CreateECB(ref state);
            ecb.AddComponent<VoxelWorldObserver>(voxelWorldEntity, new VoxelWorldObserver()
            {
                AngleOriented = quaternion.identity,
                WorldCenterChunkIndex = data.InitialWorldCenterChunkIndex,
            });
        }
        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
            var ecb = CreateECB(ref state);
            ecb.RemoveComponent<VoxelWorldObserver>(observerQuery.GetSingletonEntity());
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!playerQuery.IsEmpty)
            {
                JobHandle readPlayerDataJob = new ReadPlayerDataJob()
                {
                    VoxelWorldObserver = voxelWorldObserver,
                }.Schedule(playerQuery, state.Dependency);

                state.Dependency = new SetObserverDataJob()
                {
                    VoxelWorldObserver = voxelWorldObserver
                }.Schedule(observerQuery, readPlayerDataJob);
            }
        }
    }
    [BurstCompile]
    partial struct ReadPlayerDataJob : IJobEntity
    {
        public NativeReference<VoxelWorldObserver> VoxelWorldObserver;
        public void Execute(in FirstPersonPlayerOutputCache outputCache, in LocalTransform transform)
        {
            VoxelWorldObserver.Value = new VoxelWorldObserver()
            {
                AngleOriented = outputCache.CameraRotation,
                WorldCenterChunkIndex = VoxelMath.PlayerChunkIndex(transform.Position),
            };
        }
    }
    [BurstCompile]
    partial struct SetObserverDataJob : IJobEntity
    {
        public NativeReference<VoxelWorldObserver> VoxelWorldObserver;
        public void Execute(ref VoxelWorldObserver voxelWorldObserver)
        {
            voxelWorldObserver = VoxelWorldObserver.Value;
        }
    }
}
