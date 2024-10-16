//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Transforms;

//namespace CatDOTS.VoxelWorld
//{
//    public struct BioEntityMap : IComponentData
//    {

//    }
//    [DisableAutoCreation]
//    [UpdateInGroup(typeof(InitializedVWSystemGroup))]
//    [BurstCompile]
//    public partial struct BuildBioEntityTreeSystem : ISystem
//    {
//        EntityQuery entityQuery;
//        [BurstCompile]
//        public void OnCreate(ref SystemState state)
//        {
//            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
//            builder.WithAll<BioEntity, BeingsData, LocalTransform>();

//            entityQuery = builder.Build(ref state);

//            Entity entity = state.EntityManager.CreateSingleton<BioEntityMap>(new BioEntityMap()
//            {

//            });

//        }
//        [BurstCompile]
//        public void OnDestroy(ref SystemState state)
//        {
//        }
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//        }
//    }
//    [BurstCompile]
//    partial struct InitBioEntityJob : IJobEntity
//    {
//        public void Execute(in LocalTransform localTransform)
//        {

//        }
//    }
//}
