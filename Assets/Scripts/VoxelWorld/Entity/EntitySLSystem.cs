using CatFramework;
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
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public struct UnloadEntityChunk : IComponentData
    {
        public int3 BigChunkIndex;
    }
    //public struct SaveEntityData
    //{
    //    public NativeArray<NativeArray<byte>.ReadOnly>.ReadOnly bytes;
    //}
    //public class EntityLoad
    //{
    //    public JobHandle Schdule(EntityCommandBuffer ECB, JobHandle dependOn)
    //    {
    //        return dependOn;
    //    }
    //}
    // 将实体的实例化交给一个系统统一处理,
    // 预案一: 发布一个卸载组件,列出全部需要卸载的
    // 然后多个系统读取,遍历该系统对应实体的全部实体,查询对应区域,这里可能得集合套集合了,或者分摊到多帧,每帧一个,然后读取的组件数据转byte[] 交给保存系统(当前系统)一起保存,或者自行保存?
    [DisableAutoCreation]
    [UpdateInGroup(typeof(UpdateSystemMiaoGroup))]
    public partial class EntitySLSystem : SystemBase, ISystemMiao
    {
        class CommonData
        {
            public void GetInfo(StringBuilder stringBuilder)
            {
                stringBuilder.AppendLine($"已加载的实体区块 : {loadedBigChunkSet.Count}");
            }
            VoxelWorldDataBaseManaged dataBaseManaged;
            IWorldGenerator WorldGenerator => dataBaseManaged.VoxelWorldGameArchivalData.WorldGenerator;
            VoxelWorldDataBaseManaged.IVoxelWorldSetting VoxelWorldSetting => dataBaseManaged.VoxelWorldSetting;

            public NativeHashSet<int3> loadedBigChunkSet;
            public NativeList<int3> waitUnloads;
            public CommonData(VoxelWorldDataBaseManaged dataBaseManaged)
            {
                this.dataBaseManaged = dataBaseManaged;
                loadedBigChunkSet = new NativeHashSet<int3>(128, Allocator.Persistent);
                waitUnloads = new NativeList<int3>(32, Allocator.Persistent);
            }
            public void Update()
            {

            }
            public void Clear()
            {
                loadedBigChunkSet.Clear();
                waitUnloads.Clear();
            }
            public void Dispose()
            {
                loadedBigChunkSet.Dispose();
                waitUnloads.Dispose();
            }
        }
        EntityCommandBufferSystem ecbSystem;
        EntityCommandBufferSystem ECBSystem
        {
            get { ecbSystem ??= World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>(); return ecbSystem; }
        }
        public string Name => "实体SL系统";
        public void GetInfo(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"实体数量 : {voxelWorldEntityQuery.CalculateEntityCount()}");
            stringBuilder.AppendLine($"世界中心区块索引 : {worldCenterIndex}");
            commonData?.GetInfo(stringBuilder);
        }
        EntityQuery voxelWorldQuery;
        EntityQuery voxelWorldEntityQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAll<VoxelWorldTag, VoxelWorldDataBaseManaged, VoxelWorldMap, JustLoadedBigChunkList, VoxelWorldObserver>();

            voxelWorldQuery = builder.Build(this);

            builder.Reset();
            builder.WithAll<VoxelWorldEntity, LocalTransform>();
            voxelWorldEntityQuery = builder.Build(this);

            RequireForUpdate(voxelWorldQuery);
        }
        VoxelWorldDataBaseManaged dataBaseManaged;
        IWorldGenerator WorldGenerator => dataBaseManaged.VoxelWorldGameArchivalData.WorldGenerator;
        VoxelWorldDataBaseManaged.IVoxelWorldSetting VoxelWorldSetting => dataBaseManaged.VoxelWorldSetting;
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            Entity voxelWorldTagEntity = voxelWorldQuery.GetSingletonEntity();

            dataBaseManaged = EntityManager.GetComponentObject<VoxelWorldDataBaseManaged>(voxelWorldTagEntity);
            worldCenterIndex = int.MaxValue;
            if (commonData == null)
            {
                commonData = new CommonData(dataBaseManaged);
            }
        }
        CommonData commonData;
        int2 worldCenterIndex;
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            commonData.Clear();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            commonData?.Dispose();
        }
        protected override void OnUpdate()
        {
            var waitUnloads = commonData.waitUnloads;
            var loadedBigChunkSet = commonData.loadedBigChunkSet;
            if (waitUnloads.Length == 0)
            {
                VoxelWorldObserver voxelWorldObserver = voxelWorldQuery.GetSingleton<VoxelWorldObserver>();
                if (math.any(voxelWorldObserver.WorldCenterChunkIndex != worldCenterIndex))
                {
                    this.worldCenterIndex = voxelWorldObserver.WorldCenterChunkIndex;
                    new CheckEntityWorldBoundsJob()
                    {
                        ActiveBigChunks = loadedBigChunkSet.AsReadOnly(),
                        WaitingUnloads = waitUnloads,
                        PlayerChunkIndex = worldCenterIndex,
                        UnloadingDistance = VoxelWorldSetting.UnloadingDistance,
                    }.Run();
                }
            }
            else if (waitUnloads.Length != 0)
            {
                var enumerator = waitUnloads.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    loadedBigChunkSet.Remove(enumerator.Current);
                }
                waitUnloads.Clear();
                Dependency = new UnLoadEntityJob()
                {
                    ECB = ECBSystem.CreateCommandBuffer(),
                    WorldCenterInVoxel = worldCenterIndex * Settings.SmallChunkSize,
                    UnloadDistanceInVoxel = VoxelWorldSetting.UnloadingDistance * Settings.SmallChunkSize,
                }.Schedule(voxelWorldEntityQuery, Dependency);
            }
            JustLoadedBigChunkList justLoadedBigChunkList = voxelWorldQuery.GetSingleton<JustLoadedBigChunkList>();
            if (justLoadedBigChunkList.Has)
            {
                VoxelWorldMap.ReadOnly voxelWorldMap = voxelWorldQuery.GetSingleton<VoxelWorldMap>().AsReadOnly();
                var enumerator = justLoadedBigChunkList.Enumerator();
                while (enumerator.MoveNext())
                {
                    int3 bigChunkIndex = enumerator.Current;
                    if (!loadedBigChunkSet.Contains(bigChunkIndex))
                    {
                        loadedBigChunkSet.Add(bigChunkIndex);
                        if (voxelWorldMap.TryGetReadOnlySlice(bigChunkIndex, out var sliceReadOnly))
                        {
                            Dependency = WorldGenerator.ScheduleFillEntity(sliceReadOnly, ECBSystem, Dependency);
                        }
                        else
                        {
                            //Debug.LogWarning($"需要生成的实体区块无对应体素区块 : {bigChunkIndex}");
                            if (ConsoleCat.Enable) ConsoleCat.LogWarning($"需要生成的实体区块无对应体素区块 : {bigChunkIndex}");
                        }
                    }
                    else
                    {
                        //Debug.LogWarning($"需要生成的实体区块已经存在于集合中 : {bigChunkIndex}");
                        if (ConsoleCat.Enable) ConsoleCat.LogWarning($"需要生成的实体区块已经存在于集合中 : {bigChunkIndex}");
                    }
                }
                enumerator.Dispose();
            }
        }
    }
    [BurstCompile]
    public partial struct UnLoadEntityJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public float2 WorldCenterInVoxel;
        public float UnloadDistanceInVoxel;
        public void Execute(in Entity entity, in LocalTransform transform)
        {
            if (math.any(math.abs(transform.Position.xz - WorldCenterInVoxel) > UnloadDistanceInVoxel))
            {
                ECB.DestroyEntity(entity);
            }
        }
    }
}
