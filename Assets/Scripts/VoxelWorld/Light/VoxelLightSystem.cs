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
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    //[DisableAutoCreation]
    //[BurstCompile]
    //[UpdateInGroup(typeof(InitializedVWSystemGroup))]
    //public partial struct VoxelLightSystem : ISystem
    //{
    //    //internal struct JobList
    //    //{
    //    //    internal NativeList<VoxelLightJobHandle> voxelLightJobHandles;
    //    //    public JobList(byte w)
    //    //    {
    //    //        voxelLightJobHandles = new NativeList<VoxelLightJobHandle>(Allocator.p);
    //    //    }
    //    //}
    //    internal struct VoxelLightJobHandle
    //    {
    //        internal int3 smallChunkIndex;
    //        internal JobHandle jobHandle;
    //        // 这个应当是临时的?
    //        internal NativeArray<byte> voxelLightMatrix;// 检查这个是否空,就可以判断是否有工作
    //    }
    //    public struct HasRebuildLightQueue : IComponentData
    //    {
    //        const int Limit = 8;
    //        NativeList<int3> smallChunkIndexs;
    //        NativeList<NativeArray<byte>> lightMatrixs;
    //        internal bool Nonfull => lightMatrixs.Length == Limit;
    //        public bool HasRebuild => lightMatrixs.Length != 0;
    //        public HasRebuildLightQueue(byte w)
    //        {
    //            smallChunkIndexs = new NativeList<int3>(Limit, Allocator.Persistent);
    //            lightMatrixs = new NativeList<NativeArray<byte>>(Limit, Allocator.Persistent);
    //        }
    //        internal void Add(int3 smallChunkIndex, NativeArray<byte> lightMatrix)
    //        {
    //            smallChunkIndexs.Add(smallChunkIndex);
    //            lightMatrixs.Add(lightMatrix);
    //        }
    //        internal void Dispose()
    //        {
    //            smallChunkIndexs.Dispose();
    //            lightMatrixs.Dispose();
    //        }
    //    }
    //    public struct AwaitUpdateLightQueue : IComponentData
    //    {
    //        NativeHashSet<int3> Check;
    //        NativeList<int3> SmallChunkIndexs;
    //        internal bool HasAwait => SmallChunkIndexs.Length != 0;
    //        public AwaitUpdateLightQueue(byte w)
    //        {
    //            Check = new NativeHashSet<int3>(Settings.WorldHeightInChunk, Allocator.Persistent);
    //            SmallChunkIndexs = new NativeList<int3>(Settings.WorldHeightInChunk, Allocator.Persistent);
    //        }
    //        public void Add(int3 smallChunkIndex)
    //        {
    //            if (Check.Add(smallChunkIndex))
    //            {
    //                SmallChunkIndexs.Add(smallChunkIndex);
    //            }
    //        }
    //        internal int3 GetWaitForUpdate()
    //        {
    //            int3 smallChunkIndex = SmallChunkIndexs[^1];
    //            SmallChunkIndexs.RemoveAt(SmallChunkIndexs.Length - 1);
    //            Check.Remove(smallChunkIndex);
    //            return smallChunkIndex;
    //        }
    //        internal void Dispose()
    //        {
    //            Check.Dispose();
    //            SmallChunkIndexs.Dispose();
    //        }
    //    }
    //    // 只是用来保留原本计算好的光照,不直接提供出去查询
    //    NativeHashMap<int3, NativeArray<byte>> chunkIndexToLightDataMap;
    //    NativeArray<Voxel> tempCopyChunkVoxels;
    //    NativeArray<byte> tempLightMatrix;
    //    //NativeArray<byte> tempCopyLightMatrix;
    //    AwaitUpdateLightQueue awaitUpdateQueue;
    //    HasRebuildLightQueue hasRebuildLightQueue;
    //    EntityQuery voxelWorldQuery;
    //    EntityQuery selfQueueQuery;
    //    [BurstCompile]
    //    public void OnCreate(ref SystemState state)
    //    {
    //        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
    //        builder.WithAll<VoxelWorldTag, VoxelWorldMap>();
    //        voxelWorldQuery = builder.Build(ref state);

    //        builder.Reset();
    //        builder.WithAllRW<AwaitUpdateLightQueue, HasRebuildLightQueue>();
    //        selfQueueQuery = builder.Build(ref state);

    //        chunkIndexToLightDataMap = new NativeHashMap<int3, NativeArray<byte>>(64, Allocator.Persistent);

    //        tempCopyChunkVoxels = new NativeArray<Voxel>(Settings.VoxelCapacityInSmallChunk, Allocator.Persistent);
    //        tempLightMatrix = new NativeArray<byte>(Settings.VoxelCapacityInSmallChunk, Allocator.Persistent);
    //        //tempCopyLightMatrix = new NativeArray<byte>(Settings.VoxelCapacityInSmallChunk, Allocator.Persistent);

    //        awaitUpdateQueue = new AwaitUpdateLightQueue(2);
    //        hasRebuildLightQueue = new HasRebuildLightQueue(2);

    //        Entity singleton = state.EntityManager.CreateSingleton<AwaitUpdateLightQueue>(awaitUpdateQueue);
    //        state.EntityManager.AddComponentData<HasRebuildLightQueue>(singleton, hasRebuildLightQueue);

    //        state.RequireForUpdate(voxelWorldQuery);
    //    }
    //    [BurstCompile]
    //    public void OnDestroy(ref SystemState state)
    //    {
    //        var enumerator = chunkIndexToLightDataMap.GetEnumerator();
    //        while (enumerator.MoveNext())
    //        {
    //            enumerator.Current.Value.Dispose();
    //        }
    //        chunkIndexToLightDataMap.Dispose();
    //        tempCopyChunkVoxels.Dispose();
    //        tempLightMatrix.Dispose();
    //        //tempCopyLightMatrix.Dispose();
    //        awaitUpdateQueue.Dispose();
    //        hasRebuildLightQueue.Dispose();
    //    }
    //    [BurstCompile]
    //    public void OnUpdate(ref SystemState state)
    //    {
    //        ref AwaitUpdateLightQueue awaitUpdateLightQueue = ref selfQueueQuery.GetSingletonRW<AwaitUpdateLightQueue>().ValueRW;
    //        ref HasRebuildLightQueue hasRebuildLightQueue = ref selfQueueQuery.GetSingletonRW<HasRebuildLightQueue>().ValueRW;
    //        if (awaitUpdateLightQueue.HasAwait && hasRebuildLightQueue.Nonfull)
    //        {
    //            int3 smallChunkIndex = awaitUpdateLightQueue.GetWaitForUpdate();
    //            VoxelWorldMap voxelWorldMap = voxelWorldQuery.GetSingleton<VoxelWorldMap>();
    //            if (voxelWorldMap.CopySmallChunkData(ref tempCopyChunkVoxels, smallChunkIndex))
    //            {
    //                //BuildVoxelLightDataJob buildVoxelLightDataJob = new BuildVoxelLightDataJob();

    //                //int3 front = smallChunkIndex + new int3(0, 0, 1);
    //            }
    //        }
    //    }
    //}
    //[BurstCompile]
    //public struct BuildVoxelLightDataJob : IJob
    //{
    //    //[ReadOnly] public NativeSlice<Voxel> SmallChunkSlice;
    //    [ReadOnly] public VoxelWorldMap VoxelWorldMap;
    //    [ReadOnly] public NativeArray<Voxel> SmallChunkVoxels;
    //    public int3 SmallChunkIndex;
    //    public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
    //    public NativeArray<byte> VoxelLightMatrix;
    //    //public NativeArray<byte> OriginalMatrix;
    //    //public NativeArray<byte> Front;
    //    //public NativeArray<byte> Back;
    //    //public NativeArray<byte> Top;
    //    //public NativeArray<byte> Bottom;
    //    //public NativeArray<byte> Right;
    //    //public NativeArray<byte> Left;
    //    public void Execute()
    //    {
    //        Clear();
    //        ReadBackLight();
    //        ReadLeftLight();
    //        ReadBottomLight();
    //        ReadFrontLight();
    //        ReadRightLight();
    //        ReadTopLight();

    //        ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;

            
    //        //int length = Settings.VoxelCapacityInSmallChunk;
    //        //int x = 0, y = 0, z = 0;
    //        //for (int index = 0; index != length; index++)
    //        //{

    //        //    z++;
    //        //    if (z == Settings.SmallChunkSize)
    //        //    {
    //        //        z = 0;
    //        //        x++;
    //        //        if (x == Settings.SmallChunkSize)
    //        //        {
    //        //            x = 0;
    //        //            y++;
    //        //        }
    //        //    }
    //        //}
    //    }
    //    void Clear()
    //    {
    //        for(int i = 0; i < Settings.VoxelCapacityInSmallChunk; i++)
    //        {
    //            VoxelLightMatrix[i] = default;
    //        }
    //    }
    //    void ReadBackLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.z -= 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> backChunk))
    //        {
    //            for (int x = 0; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (int y = 0; y != Settings.SmallChunkSize; y++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, 0);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, borderIndex);
    //                    VoxelLightMatrix[selfIndex] = backChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //    void ReadLeftLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.x -= 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> leftChunk))
    //        {
    //            for (int y = 0; y != Settings.SmallChunkSize; y++)
    //            {
    //                for (int z = 0; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(0, y, z);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(borderIndex, y, z);
    //                    VoxelLightMatrix[selfIndex] = leftChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //    void ReadBottomLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.y -= 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> bottomChunk))
    //        {
    //            for (int x = 0; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (int z = 0; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, 0, z);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, borderIndex, z);
    //                    VoxelLightMatrix[selfIndex] = bottomChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //    void ReadFrontLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.z += 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> frontChunk))
    //        {
    //            for (int x = 0; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (int y = 0; y != Settings.SmallChunkSize; y++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, borderIndex);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, 0);
    //                    VoxelLightMatrix[selfIndex] = frontChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //    void ReadRightLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.x += 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> rightChunk))
    //        {
    //            for (int y = 0; y != Settings.SmallChunkSize; y++)
    //            {
    //                for (int z = 0; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(borderIndex, y, z);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(0, y, z);
    //                    VoxelLightMatrix[selfIndex] = rightChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //    void ReadTopLight()
    //    {
    //        int borderIndex = Settings.SmallChunkSize - 1;
    //        int3 faceSmallChunkIndex = SmallChunkIndex;
    //        faceSmallChunkIndex.y += 1;
    //        if (VoxelWorldMap.RefSmallChunkSlice(faceSmallChunkIndex, out NativeSlice<Voxel> topChunk))
    //        {
    //            for (int x = 0; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (int z = 0; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, borderIndex, z);
    //                    int faceIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, 0, z);
    //                    VoxelLightMatrix[selfIndex] = topChunk[faceIndex].Data2;
    //                }
    //            }
    //        }
    //    }
    //}
}
