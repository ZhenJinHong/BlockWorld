using CatFramework;
using CatFramework.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.Analytics;

namespace CatDOTS.VoxelWorld
{
    // TODO 外圈未显示的网格体不应该更新,可以先把网格体加入活动池里并标记Dirty,如果需要显示的时候是Dirty的,再更新下网格
    [UpdateInGroup(typeof(InitializedSystemMiaoGroup), OrderLast = true)]
    //[UpdateInGroup(typeof(LateUpdateVWSystemGroup), OrderLast = true)]
    public partial class BuildChunkMeshSystem : SystemBase, ISystemMiao
    {
        public string Name => "区块网格系统";
        public void GetInfo(StringBuilder stringBuilder)
        {
            meshManaged?.GetInfo(stringBuilder);
        }
        //EntityCommandBuffer CreateEntityCommandBuffer()
        //{
        //    return SystemAPI.GetSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        //}
        EntityQuery voxelWorldQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAll<VoxelWorldTag, VoxelWorldDataBaseManaged, VoxelWorldMap, Archival, WaitingRebuildSmallChunkMeshQueue, VoxelWorldObserver>();
            voxelWorldQuery = builder.Build(this);

            RequireForUpdate(voxelWorldQuery);
        }
        // 外部提供
        VoxelWorldDataBaseManaged dataBaseManaged;
        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            Entity voxelWorldTagEntity = voxelWorldQuery.GetSingletonEntity();
            dataBaseManaged = EntityManager.GetComponentObject<VoxelWorldDataBaseManaged>(voxelWorldTagEntity);

            #region 不需要重新赋值的部分
            meshManaged ??= new VoxelWorldMeshManaged(dataBaseManaged);
            #endregion
        }
        VoxelWorldMeshManaged meshManaged;
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            meshManaged.Clear();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            meshManaged?.Dispose();
        }

        protected override void OnUpdate()
        {
            // 每帧都交换
            // ToAdd要转移到ToComplete;
            meshManaged.Update();

            var voxelWorldEntity = voxelWorldQuery.GetSingletonEntity();
            WaitingRebuildSmallChunkMeshQueue waitingRebuildSmallChunkMeshQueue = EntityManager.GetComponentData<WaitingRebuildSmallChunkMeshQueue>(voxelWorldEntity);

            //if (waitingRebuildSmallChunkMeshQueue.HasWaiting)
            //{

            //}移动时,边缘进入内围,但此时的边缘的边缘数据还未存在
            VoxelWorldObserver voxelWorldObserver = EntityManager.GetComponentData<VoxelWorldObserver>(voxelWorldEntity);

            int2 worldCenterChunkIndex = voxelWorldObserver.WorldCenterChunkIndex;
            meshManaged.AddRender(waitingRebuildSmallChunkMeshQueue.Enumerator(), worldCenterChunkIndex);
            if (meshManaged.HasWaiting)
            {
                VoxelWorldMap.ReadOnly voxelWorldMap = voxelWorldQuery.GetSingleton<VoxelWorldMap>().AsReadOnly();
                Dependency = meshManaged.GetJob(voxelWorldMap, dataBaseManaged.VoxelWorldSetting.LoadBigChunkPerFrame * Settings.WorldHeightInChunk, Dependency);
            }

            meshManaged.CompleteJob();
        }
    }
    // 需要被容器调用则意味着不可能用结构体
    public class NativeMeshDataContainerPool : ForcedPool<MeshDataContainer>
    {
        public NativeMeshDataContainerPool(int capacity, int maxCapacity) : base(capacity, maxCapacity) { }
        protected override MeshDataContainer CreateElement()
        {
            return new MeshDataContainer();
        }
    }
    public class MeshDataContainer : IPoolItem, IDisposable
    {
        public class Block
        {
            public readonly NativeList<float3> vertices;
            public readonly NativeList<float4> uvs;
            public readonly NativeList<ushort> triangles;
            public readonly NativeList<float3> normals;
            // 第一个网格也有，即0
            public readonly NativeList<SubMeshRange> subMeshRanges;

            public readonly NativeList<FaceDataCommand> opaqueFaces;
            public readonly NativeList<FaceDataCommand> transparentFaces;
            public bool NonEmpty => vertices.Length != 0;
            public Block()
            {
                int capacity = 8192;
                vertices = new NativeList<float3>(capacity, Allocator.Persistent);
                uvs = new NativeList<float4>(capacity, Allocator.Persistent);
                triangles = new NativeList<ushort>(capacity * 3, Allocator.Persistent);
                normals = new NativeList<float3>(capacity, Allocator.Persistent);
                subMeshRanges = new NativeList<SubMeshRange>(Allocator.Persistent);
                int faceCapacity = 2048;
                opaqueFaces = new NativeList<FaceDataCommand>(faceCapacity, Allocator.Persistent);
                transparentFaces = new NativeList<FaceDataCommand>(faceCapacity, Allocator.Persistent);
            }
            public void Clear()
            {
                vertices.Clear();
                uvs.Clear();
                triangles.Clear();
                normals.Clear();
                subMeshRanges.Clear();
                opaqueFaces.Clear();
                transparentFaces.Clear();

            }
            public void Dispose()
            {
                vertices.Dispose();
                uvs.Dispose();
                triangles.Dispose();
                normals.Dispose();
                subMeshRanges.Dispose();
                opaqueFaces.Dispose();
                transparentFaces.Dispose();
            }
        }
        public class PointMeshData
        {
            public readonly NativeList<float3> verts;
            public readonly NativeList<ushort> indexs;
            public bool NonEmpty => verts.Length != 0;
            public PointMeshData(int capacity = 512)
            {
                verts = new NativeList<float3>(capacity, Allocator.Persistent);
                indexs = new NativeList<ushort>(capacity, Allocator.Persistent);
            }
            public void Clear()
            {
                verts.Clear();
                indexs.Clear();
            }
            public void Dispose()
            {
                verts.Dispose();
                indexs.Dispose();
            }
        }
        public readonly NativeReference<AABB> aabb;
        public readonly Block block;
        public readonly PointMeshData grass;
        public readonly PointMeshData water;
        public readonly PointMeshData fire;
        public bool NonEmpty => block.NonEmpty || grass.NonEmpty || water.NonEmpty || fire.NonEmpty;
        public void Dispose()
        {
            aabb.Dispose();
            block.Dispose();
            grass.Dispose();
            water.Dispose();
            fire.Dispose();
        }
        public MeshDataContainer()
        {
            aabb = new NativeReference<AABB>(Allocator.Persistent);
            block = new Block();
            grass = new PointMeshData();
            water = new PointMeshData();
            fire = new PointMeshData();
        }
        public void Clear()
        {
            block.Clear();
            grass.Clear();
            water.Clear();
            fire.Clear();
        }
        public void Reset()
        {
            Clear();
        }
    }
}
