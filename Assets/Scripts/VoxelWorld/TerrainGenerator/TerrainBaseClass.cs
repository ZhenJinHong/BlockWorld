//using System.Collections;
//using System.Collections.Generic;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    public interface ITerrainDataContainer
//    {
//        //NativeArray<bool> VoxelPlaceholder { get; }
//        NativeArray<Voxel> BigChunkSlice { get; }
//        NativeArray<byte> HeightMap { get; }
//        int3 BigChunkIndex { get; }
//        int3 BigChunkPosInt { get; }
//    }
//    public abstract class TerrainBaseClass : ScriptableObject
//    {
//        public abstract void Initialize(VoxelWorldDataBaseManaged manager);
//        public abstract JobHandle ScheduleGenerateHeightMapJob(BlobAssetReference<SeedData> seedData, BigChunkDataContainer bigChunkDataContainer);
//        public abstract JobHandle ScheduleGenerateVoxelJob(BlobAssetReference<SeedData> seedData, NativeArray<byte> HeightMap, SmallChunkDataContainer smallChunkDataContainer, JobHandle dependOn);
//    }
//}