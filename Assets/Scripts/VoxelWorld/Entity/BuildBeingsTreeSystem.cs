//using System;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    [DisableAutoCreation]
//    [UpdateInGroup(typeof(FixedUpdateVWSystemGroup))]
//    [BurstCompile]
//    public partial struct BuildBeingsTreeSystem : ISystem
//    {
//        EntityQuery beingsQuery;
//        EntityQuery playerQuery;
//        EntityQuery voxelWorldQuery;
//        [BurstCompile]
//        public void OnCreate(ref SystemState state)
//        {
//            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

//            builder.WithAll<BeingsData, LocalTransform>();
//            beingsQuery = builder.Build(ref state);

//            builder.Reset();
//            builder.WithAll<PlayerShareInfo>();
//            playerQuery = builder.Build(ref state);

//            builder.Reset();
//            builder.WithAll<VoxelWorldTag>();
//            voxelWorldQuery = builder.Build(ref state);

//            state.RequireForUpdate(beingsQuery);
//            state.RequireForUpdate(playerQuery);
//            state.RequireForUpdate(voxelWorldQuery);

//            MightPosCheckMap = new NativeHashSet<int3>(1024, Allocator.Persistent);
//            MightPosMap = new NativeParallelMultiHashMap<int3, int3>(1024, Allocator.Persistent);

//            Entity entity = state.EntityManager.CreateSingleton<BeingsMightCollideMap>(new BeingsMightCollideMap()
//            {
//                MightPosCheckMap = MightPosCheckMap,
//                MightPosMap = MightPosMap,
//            });
//        }
//        NativeHashSet<int3> MightPosCheckMap;
//        NativeParallelMultiHashMap<int3, int3> MightPosMap;
//        [BurstCompile]
//        public void OnDestroy(ref SystemState state)
//        {
//            MightPosCheckMap.Dispose();
//            MightPosMap.Dispose();
//        }
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//            PlayerShareInfo playerShareInfo = state.EntityManager.GetComponentData<PlayerShareInfo>(playerQuery.GetSingletonEntity());
//            MightPosCheckMap.Clear();
//            MightPosMap.Clear();
//            JobHandle buildColliderMightPosJob = new BuildColliderMightPosJob()
//            {
//                MightPosCheckMap = MightPosCheckMap,
//                MightPosMap = MightPosMap,
//                PlayerCoord = playerShareInfo.Position,
//            }.Schedule(beingsQuery, state.Dependency);

//            state.Dependency = buildColliderMightPosJob;
//        }
//    }
//    [BurstCompile]
//    partial struct BuildColliderMightPosJob : IJobEntity
//    {
//        public NativeHashSet<int3> MightPosCheckMap;
//        public NativeParallelMultiHashMap<int3, int3> MightPosMap;
//        public float3 PlayerCoord;// 未做处理的坐标
//        public void Execute(in LocalTransform localTransform)
//        {
//            float3 beingsCoord = localTransform.Position;
//            if (math.all(math.abs(beingsCoord - PlayerCoord) < Settings.SmallChunkSize))
//            {

//                int max = 3;
//                int min = -3;
//                int range = max - min;
//                int length = range * range * range;

//                int x = min, y = min, z = min;
//                for (int i = 0; i < length; i++)
//                {
//                    // 可能碰撞体世界位置
//                    float3 colliderPosInWorld = math.floor(new float3(beingsCoord.x + x, beingsCoord.y + y, beingsCoord.z + z));
//                    if (colliderPosInWorld.y > -0.01f && colliderPosInWorld.y < Settings.WorldHeightInVoxel)
//                    {
//                        int3 mightColliderCoord = new int3(colliderPosInWorld);
//                        if (MightPosCheckMap.Add(mightColliderCoord))// 这样加会出现超出9大区块的碰撞体
//                        {
//                            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(colliderPosInWorld);
//#if UNITY_EDITOR
//                            if (bigChunkIndex.y != 0)
//                                Debug.LogWarning($"大区块索引越界：{bigChunkIndex}");
//#endif
//                            MightPosMap.Add(bigChunkIndex, mightColliderCoord);
//                        }
//                    }
//                    z++;
//                    if (z == max)
//                    {
//                        z = min;
//                        x++;
//                        if (x == max)
//                        {
//                            x = min;
//                            y++;
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
