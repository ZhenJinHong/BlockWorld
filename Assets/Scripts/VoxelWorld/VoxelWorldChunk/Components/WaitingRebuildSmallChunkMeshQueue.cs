using CatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct WaitingRebuildSmallChunkMeshQueue : IComponentData
    {
        NativeList<int3> SmallChunkIndexs;
        public bool HasWaiting => SmallChunkIndexs.Length != 0;
        public WaitingRebuildSmallChunkMeshQueue(NativeList<int3> smallchunkIndexs)
        {
            SmallChunkIndexs = smallchunkIndexs;
        }
        public NativeArray<int3>.ReadOnly ReadOnly()
        {
            return SmallChunkIndexs.AsParallelReader();
        }
        public NativeArray<int3>.Enumerator Enumerator()
        {
            return SmallChunkIndexs.GetEnumerator();
        }
    }
    public struct WaitingRebuildSmallChunkMeshQueueData
    {
        NativeList<int3> SmallChunkIndexs;
        NativeHashSet<int3> CheckMap;
        public int WaitingCount => SmallChunkIndexs.Length;
        public WaitingRebuildSmallChunkMeshQueue WaitingUpdateMeshQueue => new WaitingRebuildSmallChunkMeshQueue(SmallChunkIndexs);
        public void Clear()
        {
            SmallChunkIndexs.Clear();
            CheckMap.Clear();
        }
        public void Dispose()
        {
            SmallChunkIndexs.Dispose();
            CheckMap.Dispose();
        }
        public WaitingRebuildSmallChunkMeshQueueData(int initialCapacity)
        {
            SmallChunkIndexs = new NativeList<int3>(initialCapacity, Allocator.Persistent);
            CheckMap = new NativeHashSet<int3>(initialCapacity, Allocator.Persistent);
        }
        //public void AddBigChunk(List<int3> waitingRebuildMeshBigChunks, int count)
        //{
        //    while (count > 0 && waitingRebuildMeshBigChunks.Count > 0)
        //    {
        //        count--;
        //        int3 bigChunkIndex = waitingRebuildMeshBigChunks[^1];
        //        AddBigChunk(bigChunkIndex);
        //        waitingRebuildMeshBigChunks.RemoveAt(waitingRebuildMeshBigChunks.Count - 1);
        //    }
        //}
        public void AddBigChunk(int3 waitingRebuildMeshBigChunk)
        {
            for (int y = 0; y < Settings.WorldHeightInChunk; y++)
            {
                AddSmallChunk(new int3(waitingRebuildMeshBigChunk.x, y, waitingRebuildMeshBigChunk.z));
            }
        }
        public void AddSmallChunk(int3 smallChunkIndex)
        {
            if (!CheckMap.Contains(smallChunkIndex))
            {
                CheckMap.Add(smallChunkIndex);
                SmallChunkIndexs.Add(smallChunkIndex);
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Debug.LogWarning($"重复登记需要更新网格 : {smallChunkIndex}");
            }
#endif
        }
    }
}
