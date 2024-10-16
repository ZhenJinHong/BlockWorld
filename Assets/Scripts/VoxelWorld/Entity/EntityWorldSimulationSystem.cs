//using System;
//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;

//namespace CatFrameworkDOTS.VoxelWorld
//{
//    //[DisableAutoCreation]
//    [UpdateInGroup(typeof(EntityWorldSimulationSystemGroup))]
//    [UpdateAfter(typeof(EntityWorldInitializeSystem))]
//    [BurstCompile]
//    public partial struct EntityWorldSimulationSystem : ISystem
//    {
//        EntityQuery voxelEntityQuery;
//        EntityQuery voxelWorldQuery;
//        EntityQuery voxelDataBaseQuery;
//        [BurstCompile]
//        public void OnCreate(ref SystemState state)
//        {
//            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
//            builder.WithAllRW<EntitySimulationResult, EntityVelocity>()
//                .WithAllRW<LocalTransform>()
//                .WithAll<EntityGravityFactor>();

//            voxelEntityQuery = builder.Build(ref state);

//            builder.Reset();
//            builder.WithAll<VoxelWorldTag, ChunkMap>();
//            voxelWorldQuery = builder.Build(ref state);

//            builder.Reset();
//            builder.WithAll<VoxelWorldDataBase>();
//            voxelDataBaseQuery = builder.Build(ref state);

//            state.RequireForUpdate(voxelEntityQuery);
//            state.RequireForUpdate(voxelWorldQuery);
//            state.RequireForUpdate(voxelDataBaseQuery);
//        }
//        [BurstCompile]
//        public void OnDestroy(ref SystemState state)
//        {
//        }
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//            state.Dependency.Complete();
//            //Entity player = voxelEntityQuery.GetSingletonEntity();
//            //RefRW<EntitySimulationResult> result = voxelEntityQuery.GetSingletonRW<EntitySimulationResult>();
//            //RefRW<EntityVelocity> velocity = voxelEntityQuery.GetSingletonRW<EntityVelocity>();
//            //RefRW<LocalTransform> localTransform = voxelEntityQuery.GetSingletonRW<LocalTransform>();
//            //EntityGravityFactor entityGravityFactor = voxelEntityQuery.GetSingleton<EntityGravityFactor>();
//            //ChunkMap chunkMap = state.EntityManager.GetComponentData<ChunkMap>(voxelWorldQuery.GetSingletonEntity());
//            //VoxelWorldDataBase voxelWorldDataBase = state.EntityManager.GetComponentData<VoxelWorldDataBase>(voxelDataBaseQuery.GetSingletonEntity());
//            //ref VoxelTypeAsset voxelTypeAsset = ref voxelWorldDataBase.VoxelTypeDataBase.Value;
//            //ref BlobArray<VoxelType> voxelTypes = ref voxelTypeAsset.VoxelTypes;

//            //float radius = 0.2f;
//            //float3 velocityTemp = velocity.ValueRO.Value;
//            //EntitySimulationResult entitySimulationResult = result.ValueRO;
//            //float gravityFactor = entityGravityFactor.Value;
//            //float verticalVelocity = velocityTemp.y;
//            //float3 pos = localTransform.ValueRO.Position;
//            //if (verticalVelocity < 0.01f)
//            //{
//            //    if ((
//            //    chunkMap.CheckForVoxelSolid(new float3(pos.x - radius, pos.y + verticalVelocity, pos.z - radius), ref voxelTypes) ||
//            //    chunkMap.CheckForVoxelSolid(new float3(pos.x + radius, pos.y + verticalVelocity, pos.z - radius), ref voxelTypes) ||
//            //    chunkMap.CheckForVoxelSolid(new float3(pos.x + radius, pos.y + verticalVelocity, pos.z + radius), ref voxelTypes) ||
//            //    chunkMap.CheckForVoxelSolid(new float3(pos.x - radius, pos.y + verticalVelocity, pos.z + radius), ref voxelTypes)
//            //    ))
//            //    {
//            //        entitySimulationResult.State = EntitySimulationState.OnGround;
//            //        verticalVelocity = 0f;
//            //    }
//            //    else
//            //    {
//            //        entitySimulationResult.State = EntitySimulationState.Falling;
//            //    }
//            //}
//            //if (entitySimulationResult.State == EntitySimulationState.Falling)
//            //{
//            //    verticalVelocity += gravityFactor * SystemAPI.Time.DeltaTime * -9.8f;
//            //    if (verticalVelocity < -9.8f)
//            //        verticalVelocity = -9.8f;
//            //}
//            //result.ValueRW = entitySimulationResult;
//            //velocity.ValueRW.Value.y = verticalVelocity;
//            //float3 distance = velocity.ValueRW.Value * SystemAPI.Time.DeltaTime;
//            //localTransform.ValueRW.Position += distance;
//            //new EntitySimulationJob()
//            //{
//            //    DeltaTime = SystemAPI.Time.DeltaTime,
//            //    //ChunkMap = state.EntityManager.GetComponentData<ChunkMap>(voxelWorldQuery.GetSingletonEntity()),
//            //}.Run(voxelEntityQuery);

//        }
//    }
//    [BurstCompile]
//    public partial struct EntitySimulationJob : IJobEntity
//    {
//        //[ReadOnly] public ChunkMap ChunkMap;
//        public float DeltaTime;
//        public void Execute(ref EntitySimulationResult result, ref EntityVelocity velocity, ref LocalTransform localTransform, in EntityGravityFactor gravityFactor)
//        {
//            // 修改垂直速度不应该把原有速度完全去除
//            if (math.abs(gravityFactor.Value) < 0.001f)
//            {
//                //velocity.Value.y = 0.0f;
//            }
//            else
//            {
//                // 如果不在地板上时
//                velocity.Value.y += gravityFactor.Value * DeltaTime * -9.8f;
//                if (velocity.Value.y < -9.8f)
//                    velocity.Value.y = -9.8f;
//            }

//            float3 distance = velocity.Value * DeltaTime;
//            localTransform.Position += distance;
//        }
//    }
//}
