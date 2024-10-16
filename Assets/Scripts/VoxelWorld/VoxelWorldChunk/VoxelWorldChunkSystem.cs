using CatFramework;
using CatFramework.Tools;
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
    // 职责
    // 处理大区块生成与写入
    // 处理其他修改区块的系统写入体素后产生的dirtyData,产生更新网格,碰撞体等命令
    [UpdateInGroup(typeof(InitializedSystemMiaoGroup))]
    public partial class VoxelWorldChunkSystem : SystemBase, ISystemMiao
    {
        class CommonData
        {
            public void GetInfo(StringBuilder stringBuilder)
            {
                VoxelChunkMapManaged.GetInfo(stringBuilder);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("大区块数据容器池 : ");
                stringBuilder.AppendLine(BigChunkDataContainerPool.ToString());
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("帧循环数据 : ");
                FrameLoopsData.GetInfo(stringBuilder);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"待重构网格的大区块数 : {NewlyLoadedBigChunkList.Count}");
            }
            public VoxelWorldDataBaseManaged.IVoxelWorldSetting VoxelWorldSetting => DataBaseManaged.VoxelWorldSetting;
            public IWorldGenerator WorldGenerator => DataBaseManaged.VoxelWorldGameArchivalData.WorldGenerator;
            public VoxelWorldDataBaseManaged DataBaseManaged { get; private set; }

            // 池

            public VoxelWorldMapManaged VoxelChunkMapManaged { get; private set; }
            public FrameLoopsData FrameLoopsData { get; private set; }
            public BigChunkDataContainerPool BigChunkDataContainerPool { get; private set; }
            public List<int3> NewlyLoadedBigChunkList { get; private set; }
            public CommonData(VoxelWorldDataBaseManaged dataBaseManaged)
            {
                this.DataBaseManaged = dataBaseManaged;

                VoxelChunkMapManaged = new VoxelWorldMapManaged(VoxelWorldSetting);
                FrameLoopsData = new FrameLoopsData(VoxelWorldSetting);

                BigChunkDataContainerPool = new BigChunkDataContainerPool(2, VoxelWorldSetting.LoadBigChunkPerFrame * 2);
                NewlyLoadedBigChunkList = new List<int3>(128);
            }
            public void Update()
            {
                VoxelChunkMapManaged.Update();
                FrameLoopsData.Update();
            }
            public void Reset()
            {
                VoxelChunkMapManaged.Reset();
                FrameLoopsData.Reset();

                BigChunkDataContainerPool.ForcedRepaid();
                NewlyLoadedBigChunkList.Clear();
            }
            public void Dispose()
            {
                VoxelChunkMapManaged.Dispose();
                FrameLoopsData.Dispose();
                BigChunkDataContainerPool.Dispose();
                NewlyLoadedBigChunkList = null;
            }
        }
        class FrameLoopsData
        {
            public List<BigChunkDataContainer> ToComplete { get; private set; }
            public List<BigChunkDataContainer> ToAdd { get; private set; }
            public WaitingRebuildSmallChunkMeshQueueData WaitingRebuildSmallChunkMeshQueueData { get; private set; }
            public JustLoadBigChunkListData JustLoadBigChunkListData { get; private set; }
            // 前面执行过行为并变动的小区块,这帧检查是否因变动后需要继续变动
            public NativeList<int3> SmallChunkVariationList { get; private set; }
            public void GetInfo(StringBuilder stringBuilder)
            {
                stringBuilder.AppendLine("大区块数据生成工作:");
                stringBuilder.AppendLine($"工作ToAdd : {ToAdd.Count}");
                stringBuilder.AppendLine($"工作ToComplete : {ToComplete.Count}");
                stringBuilder.AppendLine($"待重构小区块网格数量 : {WaitingRebuildSmallChunkMeshQueueData.WaitingCount}");
            }
            public FrameLoopsData(VoxelWorldDataBaseManaged.IVoxelWorldSetting voxelWorldSetting)
            {
                ToComplete = new List<BigChunkDataContainer>();
                ToAdd = new List<BigChunkDataContainer>();
                WaitingRebuildSmallChunkMeshQueueData = new WaitingRebuildSmallChunkMeshQueueData(voxelWorldSetting.LoadBigChunkPerFrame * Settings.WorldHeightInChunk);
                JustLoadBigChunkListData = new JustLoadBigChunkListData(voxelWorldSetting.LoadBigChunkPerFrame);
                SmallChunkVariationList = new NativeList<int3>(4, Allocator.Persistent);
            }
            public void Update()
            {
                List<BigChunkDataContainer> temp = ToComplete;
                ToComplete = ToAdd;
                ToAdd = temp;

                WaitingRebuildSmallChunkMeshQueueData.Clear();
                JustLoadBigChunkListData.Clear();
                SmallChunkVariationList.Clear();
            }
            public void Reset()
            {
                ToComplete.Clear();
                ToAdd.Clear();

                WaitingRebuildSmallChunkMeshQueueData.Clear();
                JustLoadBigChunkListData.Clear();
                SmallChunkVariationList.Clear();
            }
            public void Dispose()
            {
                ToComplete.Clear();
                ToAdd.Clear();

                WaitingRebuildSmallChunkMeshQueueData.Dispose();
                JustLoadBigChunkListData.Dispose();
                SmallChunkVariationList.Dispose();
            }
        }
        public string Name => "区块系统";
        public void GetInfo(StringBuilder stringBuilder)
        {
            commonData?.GetInfo(stringBuilder);
        }
        //EntityCommandBuffer CreateEntityCommandBuffer()
        //{
        //    return SystemAPI.GetSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        //}

        EntityQuery voxelWorldQuery;
        EntityQuery selfRWQuery;
        protected override void OnCreate()
        {
            base.OnCreate();

            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            // 因为托管对象只能在主线程处理，所以就算对托管对象进行写入操作，也不会影响后续系统的读写；
            builder.Reset();
            builder.WithAll<VoxelWorldTag, VoxelWorldDataBaseManaged, Archival, VoxelWorldObserver>();
            voxelWorldQuery = builder.Build(this);

            builder.Reset();
            builder.WithAllRW<VoxelWorldMap, WaitingRebuildSmallChunkMeshQueue>().WithAllRW<JustLoadedBigChunkList, SmallChunkVariationList>();
            selfRWQuery = builder.Build(this);

            RequireForUpdate(voxelWorldQuery);
        }
        // 外部提供
        VoxelWorldDataBaseManaged dataBaseManaged;
        Archival Archival => dataBaseManaged.VoxelWorldGameArchivalData.Archival;
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            Entity voxelWorldTagEntity = voxelWorldQuery.GetSingletonEntity();

            dataBaseManaged = EntityManager.GetComponentObject<VoxelWorldDataBaseManaged>(voxelWorldTagEntity);

            if (commonData == null)
            {
                commonData = new CommonData(dataBaseManaged);
                EntityManager.AddComponentData<VoxelWorldMap>(voxelWorldTagEntity, VoxelWorldMap);
                EntityManager.AddComponentData<WaitingRebuildSmallChunkMeshQueue>(voxelWorldTagEntity, WaitingUpdateMeshQueue);
                EntityManager.AddComponentData<JustLoadedBigChunkList>(voxelWorldTagEntity, JustLoadedBigChunkList);
                EntityManager.AddComponentData<SmallChunkVariationList>(voxelWorldTagEntity, SmallChunkVariationList);
            }
            ref Archival.Data archivalData = ref Archival.ArchiveDataRef.Value;
            VoxelWorldMapManaged.CheckBounds(archivalData.InitialWorldCenterChunkIndex);
        }
        // 内部
        CommonData commonData;
        VoxelWorldMapManaged VoxelWorldMapManaged
            => commonData.VoxelChunkMapManaged;
        VoxelWorldMap VoxelWorldMap
            => VoxelWorldMapManaged.VoxelChunkMap;
        WaitingRebuildSmallChunkMeshQueue WaitingUpdateMeshQueue
            => commonData.FrameLoopsData.WaitingRebuildSmallChunkMeshQueueData.WaitingUpdateMeshQueue;
        JustLoadedBigChunkList JustLoadedBigChunkList => commonData.FrameLoopsData.JustLoadBigChunkListData.JustLoadedBigChunkList;
        SmallChunkVariationList SmallChunkVariationList => new SmallChunkVariationList(commonData.FrameLoopsData.SmallChunkVariationList);
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            commonData.Reset();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            commonData?.Dispose();
        }
        // 大区块加载-> 加入大区块Dirty列表->更新网格
        // 小区块修改-> 获取指定数量->更新网格
        protected override void OnUpdate()
        {
            Dependency.Complete();// 为了Complete不会导致生成工作提前完成，这个放在前面

            commonData.Update();

            BigChunkUpdate(commonData);

            #region 对于体素图的修改工作,放在这里,在新区块写入与区块卸载后
            #endregion
            if (!JustLoadedBigChunkList.Has)// 加载大区块过程中,暂不更新?
                SmallChunkUpdate(commonData);

            // 实际上应该不用，只要查询中标明对chunkmap的读写，就可以确保依赖关系正确，不会有提前读取chunkmap的情况
            Entity self = selfRWQuery.GetSingletonEntity();
            EntityManager.SetComponentData<VoxelWorldMap>(self, VoxelWorldMap);
            EntityManager.SetComponentData<WaitingRebuildSmallChunkMeshQueue>(self, WaitingUpdateMeshQueue);
            EntityManager.SetComponentData<JustLoadedBigChunkList>(self, JustLoadedBigChunkList);
            EntityManager.SetComponentData<SmallChunkVariationList>(self, SmallChunkVariationList);
        }
        #region 大区块加载与卸载
        void BigChunkUpdate(CommonData commonData)
        {
            var VoxelWorldMapManaged = commonData.VoxelChunkMapManaged;
            int waitforGenerateCount = VoxelWorldMapManaged.WaitforGenerateCount(commonData.VoxelWorldSetting.LoadBigChunkPerFrame);
            if (waitforGenerateCount > 0)
            {
                ChunkGenerate(waitforGenerateCount, commonData);
            }
            else if (waitforGenerateCount == 0)
            {
                var NewlyLoadedBigChunkList = commonData.NewlyLoadedBigChunkList;
                if (NewlyLoadedBigChunkList.Count != 0)
                {
                    var VoxelWorldSetting = commonData.DataBaseManaged.VoxelWorldSetting;
                    var waitingRebuildSmallChunkMeshQueueData = commonData.FrameLoopsData.WaitingRebuildSmallChunkMeshQueueData;
                    var justLoadBigChunkListData = commonData.FrameLoopsData.JustLoadBigChunkListData;
                    int loadPerFrame = VoxelWorldSetting.LoadBigChunkPerFrame;
                    while (loadPerFrame > 0 && NewlyLoadedBigChunkList.Count != 0)
                    {
                        loadPerFrame--;
                        int3 bigChunkIndex = NewlyLoadedBigChunkList[^1];
                        NewlyLoadedBigChunkList.RemoveAt(NewlyLoadedBigChunkList.Count - 1);
                        waitingRebuildSmallChunkMeshQueueData.AddBigChunk(bigChunkIndex);
                        justLoadBigChunkListData.Add(bigChunkIndex);
                    }
                }
                else if (VoxelWorldMapManaged.NoWaiting)
                {
                    int2 worldCenterChunkIndex = EntityManager.GetComponentData<VoxelWorldObserver>(voxelWorldQuery.GetSingletonEntity()).WorldCenterChunkIndex;
                    VoxelWorldMapManaged.CheckBounds(worldCenterChunkIndex);
                }
            }

            CompleteLastJob(commonData);// 此处会写入体素图,所以要注意的调用Job如果也写入,应在这之后调用job

            RepaidChunk(commonData);
        }
        void ChunkGenerate(int generateCount, CommonData commonData)
        {
            BigChunkDataContainerPool bigChunkDataContainerPool = commonData.BigChunkDataContainerPool;
            var toAdd = commonData.FrameLoopsData.ToAdd;

            if (toAdd.Count == 0)
            {
                IWorldGenerator worldGenerator = commonData.WorldGenerator;
                while (generateCount > 0)
                {
                    generateCount--;
                    if (commonData.VoxelChunkMapManaged.TryGetWaitForGenerateBigChunk(out int3 bigChunkIndex))
                    {
                        BigChunkDataContainer bigChunkDataContainer = bigChunkDataContainerPool.Get();
                        bigChunkDataContainer.BigChunkIndex = bigChunkIndex;

                        JobHandle buildHeightMapJobHandle = worldGenerator.ScheduleGenerateBigChunkMap(bigChunkDataContainer, default);
                        JobHandle fillingVoxelChunkJobHandle = worldGenerator.ScheduleFillBigChunk(bigChunkDataContainer, buildHeightMapJobHandle);
                        // 每个大区块保留一个作业句柄
                        bigChunkDataContainer.SaveJob(fillingVoxelChunkJobHandle);
                        toAdd.Add(bigChunkDataContainer);
                    }
                    else
                    {
                        if (ConsoleCat.Enable)
                        {
                            ConsoleCat.LogWarning("读取等待生成区块,提前结束了生成数量");
                        }
                        break;
                    }
                }
            }
            else
            {
                if (ConsoleCat.Enable)
                {
                    ConsoleCat.LogWarning($"区块的生成工作列表未完成工作");
                }
            }
        }
        void CompleteLastJob(CommonData commonData)
        {
            var toComplete = commonData.FrameLoopsData.ToComplete;
            if (toComplete.Count != 0)
            {
                VoxelWorldMapManaged voxelWorldMapManaged = commonData.VoxelChunkMapManaged;
                List<int3> NewlyLoadedBigChunkList = commonData.NewlyLoadedBigChunkList;
                BigChunkDataContainerPool bigChunkDataContainerPool = commonData.BigChunkDataContainerPool;
                for (int i = 0; i < toComplete.Count; i++)
                {
                    var bigChunkDataContainer = toComplete[i];
                    int3 bigChunkIndex = bigChunkDataContainer.BigChunkIndex;
                    bigChunkDataContainer.Complete();
                    voxelWorldMapManaged.CopyDataToChunk(bigChunkDataContainer);
                    bigChunkDataContainerPool.Repaid(bigChunkDataContainer);

                    NewlyLoadedBigChunkList.Add(bigChunkIndex);
                }
                toComplete.Clear();
            }
        }
        void RepaidChunk(CommonData commonData)
        {
            VoxelWorldMapManaged voxelWorldMapManaged = commonData.VoxelChunkMapManaged;
            int waitforUnloadCount = voxelWorldMapManaged.WaitforUnloadCount(commonData.VoxelWorldSetting.LoadBigChunkPerFrame);
            while (waitforUnloadCount > 0 && voxelWorldMapManaged.TryGetWaitForUnLoadChunk(out _))
            {
                waitforUnloadCount--;
                // TODO
            }
        }
        #endregion
        #region 小区块修改后的网格脏处理
        void SmallChunkUpdate(CommonData commonData)
        {
            var VoxelWorldMapManaged = commonData.VoxelChunkMapManaged;
            var voxelWorldMapData = VoxelWorldMapManaged.VoxelWorldMapData;
            if (voxelWorldMapData.DirtySmallChunkCount != 0)
            {
                var VoxelWorldSetting = commonData.DataBaseManaged.VoxelWorldSetting;
                // 如果在当前阶段派发修改体素的工作,可能会导致这个dirtychunkset无法读取?
                int rebuildMeshPerFrame = /*VoxelWorldSetting.LoadBigChunkPerFrame * Settings.WorldHeightInChunk*/VoxelWorldSetting.UpdateMeshPerFrame;

                new ProcessedVoxelWorldMapDirtyJob()
                {
                    ProcessCount = rebuildMeshPerFrame,
                    VoxelWorldMapData = voxelWorldMapData,
                    WaitingRebuildSmallChunkMeshQueueData = commonData.FrameLoopsData.WaitingRebuildSmallChunkMeshQueueData,
                    SmallChunkVariationList = commonData.FrameLoopsData.SmallChunkVariationList,
                }.Run();
            }
        }
        #endregion
    }
    public class BigChunkDataContainer : IPoolItem, IBigChunkMapContainer
    {
        private int3 bigChunkIndex;
        public int3 BigChunkIndex
        {
            get => bigChunkIndex;
            set => bigChunkIndex = value;
        }
        public int3 BigChunkPosInt => bigChunkIndex * Settings.SmallChunkSize;
        public NativeArray<byte> HeightMap { get; private set; }
        public NativeArray<float> RegionNoiseMap { get; private set; }
        public NativeArray<Voxel> Voxels { get; private set; }
        public BigChunkDataContainer()
        {
            HeightMap = new NativeArray<byte>(Settings.VoxelCountInFloor, Allocator.Persistent);
            RegionNoiseMap = new NativeArray<float>(Settings.VoxelCountInFloor, Allocator.Persistent);
            Voxels = new NativeArray<Voxel>(Settings.VoxelCapacityInBigChunk, Allocator.Persistent);
        }
        JobHandle jobHandle;
        public void SaveJob(JobHandle jobHandle)
        {
            if (!this.jobHandle.IsCompleted)
            {
#if UNITY_EDITOR
                Debug.LogError("工作未完成");
#endif
                this.jobHandle.Complete();
            }
            this.jobHandle = jobHandle;
        }
        public void Complete()
        {
            jobHandle.Complete();
        }
        public void Dispose()
        {
            Complete();
            HeightMap.Dispose();
            RegionNoiseMap.Dispose();
            Voxels.Dispose();
        }

        public void Reset()
        {
            Complete();
            BigChunkIndex = int.MaxValue;
        }
    }
    public class BigChunkDataContainerPool : ForcedPool<BigChunkDataContainer>
    {
        public BigChunkDataContainerPool(int initCapacity, int maxCapacity) : base(initCapacity, maxCapacity)
        {
        }

        protected override BigChunkDataContainer CreateElement()
        {
            return new BigChunkDataContainer();
        }
    }
}
