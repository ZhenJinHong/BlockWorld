//using CatFramework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    public struct BigChunkData
//    {
//        public bool IsDirty;
//    }
//    public class BigChunkMapData
//    {
//        NativeHashSet<int3> DirtyBigChunks;
//        NativeHashSet<int3> ActiveBigChunks;
//        NativeList<int3> WaitingGenerates;
//        NativeList<int3> WaitingUnloads;

//        public bool NoWaiting => WaitingGenerates.Length == 0 && WaitingUnloads.Length == 0;
//        readonly VoxelWorldDataBaseManaged.IVoxelWorldSetting voxelWorldSetting;
//        public VoxelWorldMapData VoxelWorldMapData { get; private set; }
//        int2 playerChunkIndex;
//        int maxPreloadingDistance;
//        public void Reset()
//        {

//        }
//        public void Dispose()
//        {

//        }
//        public BigChunkMapData()
//        {
//            int initCapacity = 64;
//            ActiveBigChunks = new NativeHashSet<int3>(initCapacity, Allocator.Persistent);
//        }
//        public void Update()
//        {
//            maxPreloadingDistance = voxelWorldSetting.MaxPreloadingDistance;
//        }
//        public bool CheckOutOfPreloadingDistance(int2 bigChunkIndex)
//        {
//            return VoxelMath.OutOfDistance(bigChunkIndex, playerChunkIndex, maxPreloadingDistance);
//        }
//        public void CheckBounds(int2 playerChunkIndex)
//        {
//            if (math.any(playerChunkIndex != this.playerChunkIndex))
//            {
//                this.playerChunkIndex = playerChunkIndex;
//                new CheckWorldBoundsJob()
//                {
//                    ActiveBigChunks = ActiveBigChunks,
//                    WaitingGenerates = WaitingGenerates,
//                    WaitingUnloads = WaitingUnloads,
//                    HalfSimulationRange = voxelWorldSetting.HalfSimulationRange,
//                    MaxPreloadingDistance = voxelWorldSetting.MaxPreloadingDistance,
//                    PlayerChunkIndex = playerChunkIndex,
//                }.Run();
//            }
//        }
//        /// <summary>
//        /// 当剩余的区块切片不足时,待生成的以切片数量为数
//        /// </summary>
//        /// <returns></returns>
//        public int WaitforGenerateCount()
//        {
//            // 移动速度过快时会导致一次加入的等待生成过多,提示切片不足,这种情况下会等待卸载完成为止才有余量
//            return VoxelWorldMapData.CheckMeetNeed(WaitingGenerates.Length) ? WaitingGenerates.Length : VoxelWorldMapData.Rest;
//        }
//        public void GetWaitForGenerateBigChunk(int count, NativeList<int3> waitingGenerate)
//        {
//            while (WaitingGenerates.Length != 0 && count != 0)
//            {
//                count--;
//                int3 bigChunkIndex = WaitingGenerates[^1];
//                WaitingGenerates.RemoveAt(WaitingGenerates.Length - 1);
//                if (!CheckOutOfPreloadingDistance(bigChunkIndex.As2D()))
//                {
//                    waitingGenerate.Add(bigChunkIndex);
//                }
//            }
//        }
//        public void CopyDataToChunk(BigChunkDataContainer bigChunkDataContainer)
//        {
//            if (!CheckOutOfPreloadingDistance(bigChunkDataContainer.BigChunkIndex.As2D()))
//            {
//                int3 bigChunkIndex = bigChunkDataContainer.BigChunkIndex;
//                if (ActiveBigChunks.Contains(bigChunkIndex))
//                {
//                    if (ConsoleCat.Enable)
//                        ConsoleCat.LogWarning($"要重新生成的区块存在于活动区块中{bigChunkIndex}");
//                }
//                else
//                {
//                    if (VoxelWorldMapData.CheckHasOneSlice())
//                    {
//                        var bigChunkSlice = VoxelWorldMapData.AllocatedSlice(bigChunkIndex);
//                        ActiveBigChunks.Add(bigChunkIndex);// 并加入已渲染池中；
//                        for (int i = 0; i < bigChunkDataContainer.Count; i++)
//                        {
//                            var smallChunkData = bigChunkDataContainer[i];
//                            var smallChunkSlice = VoxelWorldMap.SplitBigChunkSlice(i, bigChunkSlice.VoxelMap);
//                            smallChunkSlice.CopyFrom(smallChunkData.Voxels);
//                        }
//                    }
//                    else
//                    {
//                        if (ConsoleCat.Enable)
//                        {
//                            ConsoleCat.LogError("缺少区块切片!");
//                        }
//                    }
//                }
//            }
//        }
//    }
//    [DisableAutoCreation]
//    public partial class ChunkSystem : SystemBase
//    {
//        class DoubleBuildChunkDataJobList : IDisposable
//        {
//            public BigChunkDataContainerPool ChunkDataContainerPool { get; private set; }
//            public BuildChunkDataJobList ToComplete { get; private set; }
//            public BuildChunkDataJobList ToAdd { get; private set; }
//            public void GetInfo(StringBuilder stringBuilder)
//            {
//                stringBuilder.AppendLine("大区块临时数据容器池:" + ChunkDataContainerPool.PoolTotalCapacity);
//            }
//            public DoubleBuildChunkDataJobList()
//            {
//                ChunkDataContainerPool = new BigChunkDataContainerPool(2, 32);
//                ToComplete = new BuildChunkDataJobList();
//                ToAdd = new BuildChunkDataJobList();
//            }
//            public void Update()
//            {
//                var temp = ToComplete;
//                ToComplete = ToAdd;
//                ToAdd = temp;
//            }
//            public void Reset()
//            {
//                ChunkDataContainerPool.ForcedRepaid();
//                ToComplete.Clear();
//                ToAdd.Clear();
//            }
//            public void Dispose()
//            {
//                ChunkDataContainerPool.Dispose();
//                ToComplete.Clear();
//                ToAdd.Clear();
//            }
//        }
//        class CommonData
//        {
//            public IWorldGenerator WorldGenerator => dataBaseManaged.VoxelWorldGameArchivalData.WorldGenerator;
//            public VoxelWorldDataBaseManaged dataBaseManaged;
//            public DoubleBuildChunkDataJobList doubleBuildChunkDataJobList;
//            public VoxelWorldMapManaged voxelChunkMapManaged;
//            public Queue<int3> dirtySmallChunkQueueForChunkGenerate;
//            public void GetInfo(StringBuilder stringBuilder)
//            {
//                voxelChunkMapManaged.GetInfo(stringBuilder);
//                stringBuilder.AppendLine("---");
//                doubleBuildChunkDataJobList.GetInfo(stringBuilder);
//            }
//            public CommonData(VoxelWorldDataBaseManaged dataBaseManaged)
//            {
//                this.dataBaseManaged = dataBaseManaged;
//                doubleBuildChunkDataJobList = new DoubleBuildChunkDataJobList();
//                voxelChunkMapManaged = new VoxelWorldMapManaged(dataBaseManaged.VoxelWorldSetting);
//                dirtySmallChunkQueueForChunkGenerate = new Queue<int3>();
//            }
//            public void Update()
//            {
//                doubleBuildChunkDataJobList.Update();
//                voxelChunkMapManaged.UpdateInitializedData();
//            }
//            public void Reset()
//            {
//                doubleBuildChunkDataJobList.Reset();
//                voxelChunkMapManaged.Clear();
//                dirtySmallChunkQueueForChunkGenerate.Clear();
//            }
//            public void Dispose()
//            {
//                voxelChunkMapManaged.Dispose();
//                doubleBuildChunkDataJobList.Dispose();
//            }
//        }
//        EntityQuery voxelWorldQuery;
//        EntityQuery selfRWQuery;
//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

//            // 因为托管对象只能在主线程处理，所以就算对托管对象进行写入操作，也不会影响后续系统的读写；
//            builder.WithAll<VoxelWorldTag, VoxelWorldDataBaseManaged, Archival, VoxelWorldObserver>();
//            voxelWorldQuery = builder.Build(this);

//            builder.Reset();
//            builder.WithAllRW<VoxelWorldMap, WaitForUpdateChunkQueue>();
//            selfRWQuery = builder.Build(this);

//            RequireForUpdate(voxelWorldQuery);
//        }
//        VoxelWorldDataBaseManaged dataBaseManaged;
//        CommonData commonData;
//        VoxelWorldMapManaged VoxelWorldMapManaged => commonData.voxelChunkMapManaged;
//        Archival Archival => dataBaseManaged.VoxelWorldGameArchivalData.Archival;
//        protected override void OnStartRunning()
//        {
//            base.OnStartRunning();
//            Entity voxelWorldTagEntity = voxelWorldQuery.GetSingletonEntity();

//            dataBaseManaged = EntityManager.GetComponentObject<VoxelWorldDataBaseManaged>(voxelWorldTagEntity);
//            ref Archival.Data archivalData = ref Archival.ArchiveDataRef.Value;

//            if (commonData == null)
//            {
//                commonData = new CommonData(dataBaseManaged);
//                EntityManager.AddComponentData<VoxelWorldMap>(voxelWorldTagEntity, VoxelWorldMapManaged.VoxelChunkMap);
//            }
//            VoxelWorldMapManaged.CheckBounds(archivalData.InitialWorldCenterChunkIndex);
//        }
//        protected override void OnUpdate()
//        {

//        }
//    }
//}
