//using Unity.Collections;
//using Unity.Mathematics;
//using UnityEngine;
//using static UnityEditor.Experimental.GraphView.Port;

//namespace CatDOTS.VoxelWorld
//{
//    public class AlterVoxelMapData
//    {
//        NativeArray<int> sliceIndexs;// 规定-1则为这个区不存在
//        NativeArray<int> dirtyChunkDatas;
//        NativeHashMap<int3, int> sliceArrayIndexMap;
//        int preloadingRange;
//        public void Clear()
//        {
//            sliceArrayIndexMap.Clear();
//        }
//        public void Dispose()
//        {
//            sliceIndexs.Dispose();
//            dirtyChunkDatas.Dispose();
//            sliceArrayIndexMap.Dispose();
//        }

//        public AlterVoxelMapData(int preloadingRange)
//        {
//            this.preloadingRange = preloadingRange;
//            int capacity = preloadingRange * preloadingRange;
//            sliceArrayIndexMap = new NativeHashMap<int3, int>(capacity, Allocator.Persistent);
//            sliceIndexs = new NativeArray<int>(capacity, Allocator.Persistent);
//            dirtyChunkDatas = new NativeArray<int>(capacity, Allocator.Persistent);
//        }

//        public void Refresh(int2 centerIndexInChunk, NativeHashMap<int3, int> chunkIndexToSliceMap)
//        {
//            sliceArrayIndexMap.Clear();
//            int2 mapOriginalInChunk = centerIndexInChunk - preloadingRange / 2;
//            for (int x = 0; x < preloadingRange; x++)
//            {
//                for (int z = 0; z < preloadingRange; z++)
//                {
//                    int3 bigChunkIndex = new int3(x + mapOriginalInChunk.x, 0, z + mapOriginalInChunk.y);
//                    int arrayIndex = VoxelMath.D2IndexToIndex(x, z, preloadingRange);
//                    if (chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex))
//                    {
//                        sliceIndexs[arrayIndex] = sliceIndex;
//                        sliceArrayIndexMap.Add(bigChunkIndex, arrayIndex);
//                    }
//                    else
//                    {
//                        sliceIndexs[arrayIndex] = -1;
//                    }
//                }
//            }
//        }
//    }
//}
