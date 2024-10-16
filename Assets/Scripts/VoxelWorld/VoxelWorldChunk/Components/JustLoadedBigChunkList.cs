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
    public readonly struct JustLoadedBigChunkList : IComponentData
    {
        readonly NativeList<int3> bigChunks;
        public bool Has => bigChunks.Length != 0;
        public JustLoadedBigChunkList(NativeList<int3> bigChunks) => this.bigChunks = bigChunks;
        public NativeArray<int3>.ReadOnly ReadOnly() => bigChunks.AsParallelReader();
        public NativeArray<int3>.Enumerator Enumerator() => bigChunks.GetEnumerator();
    }
    public struct JustLoadBigChunkListData : IDisposable
    {
        NativeList<int3> bigChunks;
        public void Clear()
        {
            bigChunks.Clear();
        }
        public void Dispose()
        {
            bigChunks.Dispose();
        }
        public JustLoadedBigChunkList JustLoadedBigChunkList => new JustLoadedBigChunkList(bigChunks);
        public JustLoadBigChunkListData(int initialCapacity)
        {
            bigChunks = new NativeList<int3>(initialCapacity, Allocator.Persistent);
        }
        public void Add(int3 bigChunkIndex)
        {
            bigChunks.Add(bigChunkIndex);
        }
    }
}
