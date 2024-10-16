using CatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    // 作用
    // 管理区块的加载与卸载时的切片分配，
    // 计算区块是否需要有区块卸载与加载的，并进入等待队列
    // 然后由系统获取等待加载或卸载的区块，进行加载与卸载
    // 在由系统获取出去之前，分配或接触切片分配
    public class VoxelWorldMapManaged
    {
        public void GetInfo(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("体素世界图信息 : ");
            VoxelWorldMapData.GetInfo(stringBuilder);
            stringBuilder.AppendLine("玩家区块索引 : " + playerChunkIndex);
        }
        public VoxelWorldMapData VoxelWorldMapData { get; private set; }
        NativeList<int3> WaitingGenerates;
        NativeList<int3> WaitingUnloads;

        public bool NoWaiting => WaitingGenerates.Length == 0 && WaitingUnloads.Length == 0;
        readonly VoxelWorldDataBaseManaged.IVoxelWorldSetting voxelWorldSetting;

        public VoxelWorldMap VoxelChunkMap => VoxelWorldMapData.GetVoxelWorldMap();
        int2 playerChunkIndex;
        int unloadingDistance;
        public void Dispose()
        {
            VoxelWorldMapData.Dispose();
            WaitingGenerates.Dispose();
            WaitingUnloads.Dispose();
        }
        public void Reset()
        {
            playerChunkIndex = int.MaxValue;
            VoxelWorldMapData.Clear();
            WaitingGenerates.Clear();
            WaitingGenerates.Clear();
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log("清空托管区块池");
            }
        }
        public VoxelWorldMapManaged(VoxelWorldDataBaseManaged.IVoxelWorldSetting voxelWorldSetting)
        {
            this.voxelWorldSetting = voxelWorldSetting;
            playerChunkIndex = int.MaxValue;
            int bigChunkCount = Settings.BigVoxelChunkMapCapacity(voxelWorldSetting.UnloadingRange, voxelWorldSetting.MaxMemoryMB);

            VoxelWorldMapData = new VoxelWorldMapData(bigChunkCount);

            WaitingGenerates = new NativeList<int3>(Allocator.Persistent);
            WaitingUnloads = new NativeList<int3>(Allocator.Persistent);
        }
        public void CheckBounds(int2 playerChunkIndex)
        {
            if (math.any(playerChunkIndex != this.playerChunkIndex))
            {
                this.playerChunkIndex = playerChunkIndex;
                new CheckWorldBoundsJob()
                {
                    ActiveBigChunks = VoxelWorldMapData.ActiveBigChunkMap.AsReadOnly(),
                    WaitingGenerates = WaitingGenerates,
                    WaitingUnloads = WaitingUnloads,
                    LoadingDistance = voxelWorldSetting.LoadingDistance,
                    UnloadingDistance = voxelWorldSetting.UnloadingDistance,
                    PlayerChunkIndex = playerChunkIndex,
                }.Run();
            }
        }

        public void Update()
        {
            unloadingDistance = voxelWorldSetting.UnloadingDistance;
        }
        public bool CheckOutOfUnloadingDistance(int2 bigChunkIndex)
        {
            return VoxelMath.OutOfDistance(bigChunkIndex, playerChunkIndex, unloadingDistance);
        }
        /// <summary>
        /// 当剩余的区块切片不足时,待生成的以切片数量为数
        /// </summary>
        /// <returns></returns>
        public int WaitforGenerateCount(int maxGenerateCount)
        {
            maxGenerateCount = math.min(WaitingGenerates.Length, maxGenerateCount);
            return VoxelWorldMapData.GetAvailableNumber(maxGenerateCount);
        }
        public int WaitforUnloadCount(int maxUnloadCount)
            => math.min(WaitingUnloads.Length, maxUnloadCount);
        public bool TryGetWaitForGenerateBigChunk(out int3 bigChunkIndex)
        {
            while (WaitingGenerates.Length != 0)
            {
                bigChunkIndex = WaitingGenerates[^1];
                WaitingGenerates.RemoveAt(WaitingGenerates.Length - 1);
                if (!CheckOutOfUnloadingDistance(bigChunkIndex.As2D()))
                {
                    return true;
                }
            }
            bigChunkIndex = default;
            return false;
        }
        // 卸载的时候要检查下是否又重新在活动图里
        public bool TryGetWaitForUnLoadChunk(out int3 bigChunkIndex)
        {
            if (WaitingUnloads.Length != 0)
            {
                bigChunkIndex = WaitingUnloads[^1];
                WaitingUnloads.RemoveAt(WaitingUnloads.Length - 1);
                // 需要在真正卸载前，再次判断是否在卸载距离
                if (CheckOutOfUnloadingDistance(bigChunkIndex.As2D()))
                {
                    // 如果缓存池中找到了要彻底卸载的
                    if (VoxelWorldMapData.TryGetValue(bigChunkIndex, out bool isDirty))
                    {
                        if (isDirty)
                        {

                        }
                        // 后续写保存操作
                        VoxelWorldMapData.Remove(bigChunkIndex);// 解除分配
                        return true;
                    }
                    else
                    {
                        if (ConsoleCat.Enable)
                        {
                            ConsoleCat.LogWarning($"要移除的大区块：{bigChunkIndex}，并不存在于活动池中");
                        }
                    }
                }
            }
            bigChunkIndex = default;
            return false;
        }
        public void CopyDataToChunk(BigChunkDataContainer bigChunkDataContainer)
        {
            if (!CheckOutOfUnloadingDistance(bigChunkDataContainer.BigChunkIndex.As2D()))
            {
                int3 bigChunkIndex = bigChunkDataContainer.BigChunkIndex;
                if (VoxelWorldMapData.Contains(bigChunkIndex))
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning($"要重新生成的区块存在于活动区块中{bigChunkIndex}");
                }
                else
                {
                    if (VoxelWorldMapData.CheckHasOneSlice())
                    {
                        VoxelWorldMap.BigChunkSlice bigChunkSlice = VoxelWorldMapData.Add(bigChunkIndex);
                        bigChunkSlice.VoxelMap.CopyFrom(bigChunkDataContainer.Voxels);
                    }
                    else
                    {
                        if (ConsoleCat.Enable)
                        {
                            ConsoleCat.LogError("缺少区块切片!");
                        }
                    }
                }
            }
        }
    }
}
