//using CatDOTS.VoxelWorld;
//using CatFramework.CatMath;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Physics;
//using Unity.Transforms;

//namespace CatDOTS
//{
//    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
//    //[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
//    public partial class FinishPlayerDataSystem : SystemBase
//    {
//        public interface IOutPutReceiver
//        {
//            bool EnableInput { get; }
//            bool PutVoxel { get; }
//            Voxel WhatVoxelToPut { get; }
//            float CameraXAngle { set; }
//            float CameraYAngle { set; }
//            float3 Position { set; }
//            quaternion CameraRotation { set; }
//            quaternion PlayerRotation { set; }
//            void PreviewPostion(float3 pos);
//        }
//        EntityQuery playerQuery;
//        EntityQuery inGameQuery;
//        EntityQuery voxelWorldQuery;
//        public IOutPutReceiver outPutReceiver;
//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            EntityQueryBuilder builder = new(Allocator.Temp);
//            builder.WithAll<LocalTransform>()
//                .WithAllRW<PlayerOutputCache>();
//            playerQuery = builder.Build(this);

//            builder.Reset();
//            builder.WithAll<InGame>();
//            inGameQuery = builder.Build(this);

//            builder.Reset();
//            builder.WithAll<VoxelWorldTag, VoxelWorldMap>();
//            voxelWorldQuery = builder.Build(this);

//            RequireForUpdate(playerQuery);
//            RequireForUpdate(inGameQuery);
//            RequireForUpdate(voxelWorldQuery);
//            builder.Dispose();
//        }
//        // TODO 如果有档，初始化的玩家共享信息应当从档里读取
//        protected override void OnStartRunning()
//        {
//            base.OnStartRunning();
//            Entity player = playerQuery.GetSingletonEntity();
//            // 共享信息必须由这个系统添加，以确保信息的及时性
//            EntityManager.AddComponentData<PlayerShareInfo>(player, new PlayerShareInfo()
//            {
//                Position = EntityManager.GetComponentData<LocalTransform>(player).Position,
//            });
//        }
//        protected override void OnStopRunning()
//        {
//            base.OnStopRunning();
//            EntityManager.RemoveComponent<PlayerShareInfo>(playerQuery.GetSingletonEntity());
//        }
//        protected override void OnUpdate()
//        {
//            if (outPutReceiver != null && outPutReceiver.EnableInput)
//            {
//                Entity player = playerQuery.GetSingletonEntity();
//                PlayerOutputCache playerOutputCache = EntityManager.GetComponentData<PlayerOutputCache>(player);
//                LocalTransform localTransform = EntityManager.GetComponentData<LocalTransform>(player);


//                NativeReference<float3> result = new NativeReference<float3>(WorldUpdateAllocator);
//                JobHandle putVoxelJobHandle = new PutVoxelJob()
//                {
//                    CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
//                    Rotation = playerOutputCache.CameraRotation,
//                    Start = localTransform.Position,
//                    Result = result,
//                }.Schedule(Dependency);

//                outPutReceiver.CameraXAngle = playerOutputCache.CameraXAngle;
//                outPutReceiver.CameraYAngle = playerOutputCache.CameraYAngle;
//                outPutReceiver.Position = localTransform.Position;
//                outPutReceiver.CameraRotation = playerOutputCache.CameraRotation;
//                outPutReceiver.PlayerRotation = playerOutputCache.PlayerRotation;
//                PlayerShareInfo playerShareInfo = new PlayerShareInfo
//                {
//                    Position = localTransform.Position
//                };
//                EntityManager.SetComponentData<PlayerShareInfo>(player, playerShareInfo);



//                putVoxelJobHandle.Complete();

//                outPutReceiver.PreviewPostion(result.Value);
//                if (math.all(result.Value != float3.zero))
//                {
//                    if (outPutReceiver.PutVoxel)
//                    {

//                    }
//                }

//            }
//        }
//    }
//    struct PutVoxelJob : IJob
//    {
//        //[ReadOnly] public VoxelWorldMap.ReadOnly VoxelWorld;
//        [ReadOnly] public CollisionWorld CollisionWorld;
//        public quaternion Rotation;
//        public float3 Start;
//        public NativeReference<float3> Result;
//        public void Execute()
//        {
//            float3 dir = math.rotate(Rotation, math.forward());
//            RaycastInput raycastInput = new RaycastInput()
//            {
//                Start = Start,
//                End = dir * 5f,
//                Filter = new CollisionFilter()
//                {
//                    CollidesWith = DOTSLayer.ExcludePlayer,
//                    BelongsTo = DOTSLayer.Player,
//                    GroupIndex = 0,
//                }
//            };
//            if (CollisionWorld.CastRay(raycastInput, out RaycastHit raycastHit))
//            {
//                Result.Value = math.floor(raycastHit.Position + raycastHit.SurfaceNormal * 0.5f);
//            }
//        }
//    }
//}
