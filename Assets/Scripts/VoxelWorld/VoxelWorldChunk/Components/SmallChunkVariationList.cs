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
    // 小区块有变更列表
    public struct SmallChunkVariationList : IComponentData
    {
        NativeList<int3> smallchunkIndexs;
        public bool Has => smallchunkIndexs.Length != 0;
        public SmallChunkVariationList(NativeList<int3> indexs)
        {
            this.smallchunkIndexs = indexs;
        }
        public NativeArray<int3>.Enumerator Enumerator() => smallchunkIndexs.GetEnumerator();
    }
    //public struct SmallChunkVariationListData
    //{
    //    public NativeList<int3> smallChunkIndexs;
    //    public void Clear() { }
    //}
}
