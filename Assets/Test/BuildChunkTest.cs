//using CatDOTS.VoxelWorld;
//using CatDOTS;
//using System.Collections;
//using Unity.Entities;
//using UnityEngine;
//using System;

//namespace Assets.Test
//{
//    [Obsolete]
//    public class BuildChunkTest : MonoBehaviour
//    {
//        //[SerializeField] DataBaseDefinition dataBaseDefinition;
//        //[SerializeField] long seed = 239672397;
//        //VoxelWorldDataBaseManaged voxelWorldDataBaseManaged;
//        //ChunkMapManaged chunkMapManaged;
//        //Entity entity;
//        //public void Create()
//        //{
//        //    if (entity == Entity.Null)
//        //    {
//        //        ECSEntry.IsActive = true;
//        //        voxelWorldDataBaseManaged = new VoxelWorldDataBaseManaged(dataBaseDefinition);
//        //        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

//        //        entity = entityManager.CreateSingleton<VoxelWorldTag>(new VoxelWorldTag()
//        //        {
//        //            Seed = seed,
//        //        });
//        //        chunkMapManaged = new ChunkMapManaged();
//        //        entityManager.AddComponentObject(entity, voxelWorldDataBaseManaged);
//        //        entityManager.AddComponentObject(entity, chunkMapManaged);
//        //        entityManager.AddComponentData<ChunkMap>(entity, chunkMapManaged.ChunkMap);
//        //        entityManager.AddComponentObject(entity, new WaitForBuildMeshChunkQueue());
//        //    }
//        //}
//        //private void OnDestroy()
//        //{
//        //    ECSEntry.IsActive = false;
//        //    // 组件是被自动释放的
//        //    //voxelWorldDataBaseManaged?.Dispose();
//        //    //chunkMapManaged?.Dispose();
//        //}
//        //private void Start()
//        //{

//        //}
//        //public void Build()
//        //{

//        //}
//    }
//}