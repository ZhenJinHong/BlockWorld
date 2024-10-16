using CatFramework;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using static CatDOTS.VoxelWorld.VoxelWorldMap;

namespace CatDOTS.VoxelWorld
{
    //public struct BigChunkMap
    //{
    //    public NativeHashMap<int3, bool> ActiveBigChunkMap;
    //    public NativeHashMap<int3, int> BigChunkIndexToSliceIndexMap;
    //    public BigChunkMap(int initialCapacity)
    //    {
    //        ActiveBigChunkMap = new NativeHashMap<int3, bool>(initialCapacity, Allocator.Persistent);
    //        BigChunkIndexToSliceIndexMap = new NativeHashMap<int3, int>(initialCapacity, Allocator.Persistent);
    //    }
    //    public bool ContainsKey(int3 bigChunkIndex)
    //    {
    //        bool2 contains = new bool2(ActiveBigChunkMap.ContainsKey(bigChunkIndex), BigChunkIndexToSliceIndexMap.ContainsKey(bigChunkIndex));
    //        if (!math.all(contains))
    //            throw new System.Exception($"切片图与活动图不同步 ! ! !");
    //        return math.all(contains);
    //    }
    //    public void Add(int3 bigChunkIndex, int sliceIndex)
    //    {
    //        ActiveBigChunkMap.Add(bigChunkIndex, false);
    //        BigChunkIndexToSliceIndexMap.Add(bigChunkIndex, sliceIndex);
    //    }
    //    public bool Remove(int3 bigChunkIndex)
    //    {
    //        bool2 remove = new bool2(ActiveBigChunkMap.Remove(bigChunkIndex), BigChunkIndexToSliceIndexMap.Remove(bigChunkIndex));
    //        if (!math.all(remove))
    //            throw new System.Exception($"切片图与活动图不同步 ! ! !");
    //        return math.any(remove);
    //    }
    //    public void SetDirty(int3 bigChunkIndex)
    //        => ActiveBigChunkMap[bigChunkIndex] = true;
    //    public void ClearDirty(int3 bigChunkIndex)
    //        => ActiveBigChunkMap[bigChunkIndex] = false;
    //    public bool TryGetValue(int3 bigChunkIndex, out bool isDirty)
    //        => ActiveBigChunkMap.TryGetValue(bigChunkIndex, out isDirty);
    //    public bool TryGetValue(int3 bigChunkIndex, out int sliceIndex)
    //        => BigChunkIndexToSliceIndexMap.TryGetValue(bigChunkIndex, out sliceIndex);
    //}
    public struct VoxelWorldMapData
    {
        public NativeArray<Voxel> Voxels;

        public NativeHashMap<int3, bool> ActiveBigChunkMap;
        public NativeHashSet<int3> DirtySmallChunkSet;

        public NativeHashMap<int3, int> BigChunkIndexToSliceIndexMap;
        public NativeList<int> Pool;
        public void GetInfo(StringBuilder builder)
        {
            builder.AppendLine($"大区块图池子总量 : {BigChunkIndexToSliceIndexMap.Count + Pool.Length};");
            builder.AppendLine($"活动的大区块区块数量 : {ActiveBigChunkMap.Count};");
            builder.AppendLine($"活动的大区块区块转切片数量 : {BigChunkIndexToSliceIndexMap.Count};");
            builder.AppendLine($"大区块切片池余量 : {Pool.Length};");

            builder.AppendLine($"脏小区块数量 : {DirtySmallChunkSet.Count};");
        }
        public int Capacity => BigChunkIndexToSliceIndexMap.Count + Pool.Length;
        public int ActiveCount
        {
            get
            {
                if (BigChunkIndexToSliceIndexMap.Count != ActiveBigChunkMap.Count)
                    throw new System.Exception();
                return BigChunkIndexToSliceIndexMap.Count;
            }
        }

        public int DirtySmallChunkCount => DirtySmallChunkSet.Count;
        public VoxelWorldMap GetVoxelWorldMap()
        {
            return new VoxelWorldMap(Voxels, BigChunkIndexToSliceIndexMap, DirtySmallChunkSet);
        }
        public void Clear()
        {
            ActiveBigChunkMap.Clear();
            DirtySmallChunkSet.Clear();

            BigChunkIndexToSliceIndexMap.Clear();
            ResetPool();
        }
        public void Dispose()
        {
            Voxels.Dispose();

            ActiveBigChunkMap.Dispose();
            DirtySmallChunkSet.Dispose();

            BigChunkIndexToSliceIndexMap.Dispose();
            Pool.Dispose();
        }
        public VoxelWorldMapData(int poolCapacity)
        {
            Voxels = new NativeArray<Voxel>(poolCapacity * Settings.VoxelCapacityInBigChunk, Allocator.Persistent);

            ActiveBigChunkMap = new NativeHashMap<int3, bool>(poolCapacity, Allocator.Persistent);
            DirtySmallChunkSet = new NativeHashSet<int3>(64, Allocator.Persistent);

            BigChunkIndexToSliceIndexMap = new NativeHashMap<int3, int>(poolCapacity, Allocator.Persistent);
            Pool = new NativeList<int>(poolCapacity, Allocator.Persistent);
            ResetPool();

            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log($"整个体素图的体素数量：{Voxels.Length}；切片池的量：{Pool.Length}；");
            }
        }
        void ResetPool()
        {
            Pool.Clear();
            int bigChunkCount = Voxels.Length / Settings.VoxelCapacityInBigChunk;
            for (int i = 0; i < bigChunkCount; i++)
            {
                Pool.Add(i * Settings.VoxelCapacityInBigChunk);
            }
        }
        public bool CheckHasOneSlice()
            => Pool.Length != 0;
        /// <summary>
        /// 获取可以实际可用切片数最大至最大需要数量
        /// </summary>
        public int GetAvailableNumber(int maxNeedCount)
        {
            if (ConsoleCat.Enable && maxNeedCount > Pool.Length)
                ConsoleCat.LogWarning($"区块池容量不足，当前需求：{maxNeedCount}；切片池余量：{Pool.Length}；已派出切片量：{BigChunkIndexToSliceIndexMap.Count}");
            return math.min(maxNeedCount, Pool.Length);
        }
        public bool Contains(int3 bigChunkIndex)
        {
            return ActiveBigChunkMap.ContainsKey(bigChunkIndex);
        }
        public void SetBigChunkDataDirty(int3 bigChunkIndex)
            => ActiveBigChunkMap[bigChunkIndex] = true;
        public void ClearBigChunkDataDirty(int3 bigChunkIndex)
            => ActiveBigChunkMap[bigChunkIndex] = false;
        public bool TryGetValue(int3 bigChunkIndex, out bool isDirty)
            => ActiveBigChunkMap.TryGetValue(bigChunkIndex, out isDirty);
        public bool TryGetValue(int3 bigChunkIndex, out int sliceIndex)
            => BigChunkIndexToSliceIndexMap.TryGetValue(bigChunkIndex, out sliceIndex);
        public BigChunkSlice GetBigChunkSlice(int3 bigChunkIndex)
        {
            int sliceIndex = BigChunkIndexToSliceIndexMap[bigChunkIndex];
            NativeSlice<Voxel> slice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
            return new BigChunkSlice(sliceIndex, bigChunkIndex, slice);
        }
        public BigChunkSlice Add(int3 bigChunkIndex)
        {
            if (bigChunkIndex.y != 0)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogError($"大区块索引越界：{bigChunkIndex}");
                return default;
            }
            if (BigChunkIndexToSliceIndexMap.TryGetValue(bigChunkIndex, out int sliceIndex))
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning($"传来的需要分配数组的区块索引已经分配过：{bigChunkIndex}");
            }
            else
            {
                sliceIndex = Pool[^1];
                Pool.RemoveAt(Pool.Length - 1);
                ActiveBigChunkMap.Add(bigChunkIndex, false);
                BigChunkIndexToSliceIndexMap.Add(bigChunkIndex, sliceIndex);
            }
            NativeSlice<Voxel> slice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
            return new BigChunkSlice(sliceIndex, bigChunkIndex, slice);
        }
        public void Remove(int3 bigChunkIndex)
        {
            if (BigChunkIndexToSliceIndexMap.TryGetValue(bigChunkIndex, out int sliceIndex))
            {
                Pool.Add(sliceIndex);
                BigChunkIndexToSliceIndexMap.Remove(bigChunkIndex);
                if (!ActiveBigChunkMap.Remove(bigChunkIndex))
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning($"切片图与活动图不同步 ! ! !");
                }
            }
            else if (ConsoleCat.Enable)
                ConsoleCat.LogWarning($"要解除分配的：{sliceIndex}切片并不存在于已借出的分配池中");
        }
        /// <summary>
        /// 返回脏小区块索引列表,上限为processCount
        /// </summary>
        public NativeArray<int3> ProcessedChunkDirty(int processCount)
        {
            processCount = math.min(DirtySmallChunkSet.Count, processCount);
            NativeArray<int3> needRebuidMeshs = new NativeArray<int3>(processCount, Allocator.Temp);

            var dirtySmallChunkEnumerator = DirtySmallChunkSet.GetEnumerator();
            while (dirtySmallChunkEnumerator.MoveNext() && processCount > 0)
            {
                processCount--;
                int3 smallChunkIndex = dirtySmallChunkEnumerator.Current;
                needRebuidMeshs[processCount] = smallChunkIndex;
                int3 bigChunkIndex = new int3(smallChunkIndex.x, 0, smallChunkIndex.z);
                // 大区块数据标记脏,需要在卸载时保存
                if (TryGetValue(bigChunkIndex, out bool isDirtyData) && !isDirtyData)
                    SetBigChunkDataDirty(bigChunkIndex);
            }
            dirtySmallChunkEnumerator.Dispose();
            #region 将已经标记完成脏的小区块从集合移除
            var dirtyMeshEnumerator = needRebuidMeshs.GetEnumerator();
            while (dirtyMeshEnumerator.MoveNext())
            {
                DirtySmallChunkSet.Remove(dirtyMeshEnumerator.Current);
            }
            dirtyMeshEnumerator.Dispose();
            #endregion

            return needRebuidMeshs;
        }
    }
}
