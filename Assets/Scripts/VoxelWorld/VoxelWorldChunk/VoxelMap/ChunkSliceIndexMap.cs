using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct ChunkSliceIndexMap : IComponentData
    {
        int2 original;
        int width;
        NativeArray<int> sliceIndex;
        public ChunkSliceIndexMap(int width, NativeArray<int> sliceIndex)
        {
            this.original = 0;
            this.width = width;
            this.sliceIndex = sliceIndex;
        }
        public void Update(int2 original, NativeHashMap<int3, int> slickIndexMap)
        {
            this.original = original;
            for (int i = 0; i < sliceIndex.Length; i++)
            {
                sliceIndex[i] = -1;
            }
            var enumerator = slickIndexMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int2 relaIndex = enumerator.Current.Key.As2D() - original;
                if (VoxelMath.D2IndexInRange(relaIndex, width))
                {
                    int index = VoxelMath.D2IndexToIndex(relaIndex, width);
                    sliceIndex[index] = enumerator.Current.Value;
                }
            }
        }
        public int GetSlickIndex(int2 bigChunkIndex)
        {
            int2 relaIndex = bigChunkIndex - original;
            return VoxelMath.D2IndexInRange(relaIndex, width) ? sliceIndex[VoxelMath.D2IndexToIndex(relaIndex, width)] : -1;
        }
    }
}
