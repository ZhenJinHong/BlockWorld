//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    public struct VoxelMap : IComponentData
//    {
//        NativeArray<int> SliceIndexMap;// 规定-1则为这个区不存在
//        NativeArray<Voxel> voxels;
//        int loadDistanceInChunk;
//        int2 mapOriginalInChunk;
//        public void Clear()
//        {
//        }
//        public void Dispose()
//        {
//            if (SliceIndexMap.IsCreated)
//                SliceIndexMap.Dispose();
//        }
//        public void Update(int loadDistanceInChunk, int2 centerIndexInChunk, NativeArray<Voxel> voxels, NativeHashMap<int3, int> indexSliceMapping)
//        {
//            if (loadDistanceInChunk != this.loadDistanceInChunk)
//            {
//                this.loadDistanceInChunk = loadDistanceInChunk;
//                if (SliceIndexMap.IsCreated)
//                    SliceIndexMap.Dispose();
//                SliceIndexMap = new NativeArray<int>(loadDistanceInChunk * loadDistanceInChunk, Allocator.Persistent);
//#if UNITY_EDITOR
//                Debug.Log($"体素图已更新，加载距离为：{loadDistanceInChunk}；切片索引图长度为：{SliceIndexMap.Length}");
//#endif
//            }
//            this.voxels = voxels;
//            mapOriginalInChunk = centerIndexInChunk - loadDistanceInChunk / 2;
//            for (int x = 0; x < loadDistanceInChunk; x++)
//            {
//                for (int z = 0; z < loadDistanceInChunk; z++)
//                {
//                    int3 bigChunkIndex = new int3(x + mapOriginalInChunk.x, 0, z + mapOriginalInChunk.y);
//                    SliceIndexMap[VoxelMath.D2IndexToIndex(x, z, loadDistanceInChunk)] =
//                        indexSliceMapping.TryGetValue(bigChunkIndex, out int sliceIndex) ? sliceIndex : -1;
//                }
//            }
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly int GetSliceIndex(int2 bigChunkIndex)// 使用2D索引，这个方法并不检查Y方向的合法性
//        {
//            // 原点-4，-4，区块0，0，相对4，4
//            int2 relativeIndex = bigChunkIndex - mapOriginalInChunk;// 此处减去的原点，得到已是相对索引位置
//                                                                    // 首先是否在范围内，如果在范围内则计算后的索引一定在地图的索引内
//            return VoxelMath.D2IndexInRange(relativeIndex, loadDistanceInChunk) ? SliceIndexMap[VoxelMath.D2IndexToIndex(relativeIndex, loadDistanceInChunk)] : -1;
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly bool TryGetSliceIndex(int3 bigChunkIndex, out int sliceIndex)
//        {
//            sliceIndex = bigChunkIndex.y == 0 ? GetSliceIndex(bigChunkIndex.As2D()) : -1;
//            return sliceIndex != -1;
//        }
//        //public Voxel ReadVoxel(in int3 voxelIndexInWorld)
//        //{
//        //    int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//        //    int2 relativeIndex = bigChunkIndex.As2D() - mapOriginalInChunk;
//        //    return VoxelMath.D2IndexInRange(relativeIndex, loadDistanceInChunk)
//        //        ? voxels[VoxelMath.VoxelArrayIndexInBigChunk(bigChunkIndex, voxelIndexInWorld)/*在区块内的索引*/
//        //        + SliceIndexMap[VoxelMath.D2IndexToIndex(relativeIndex, loadDistanceInChunk)]]/*加上切片偏移*/
//        //        : Voxel.Empty;
//        //}
//        public readonly Voxel ReadVoxel(in int3 voxelIndexInWorld)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            return TryGetSliceIndex(bigChunkIndex, out int sliceIndex)
//                ? ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex)
//                : Voxel.Empty;
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly Voxel ReadVoxel(int3 bigChunkIndex, int3 voxelIndexInWorld, int sliceIndex)
//        {
//            return voxels[VoxelMath.VoxelArrayIndexInBigChunk(bigChunkIndex, voxelIndexInWorld) + sliceIndex];
//        }
//        /// <summary>
//        /// 构件网格时使用的
//        /// </summary>
//        public readonly Voxel ReadVoxelOrBorder(in int3 voxelIndexInWorld)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            return TryGetSliceIndex(bigChunkIndex, out int sliceIndex)// 在范围内？
//                ? ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex)// 则读取体素
//                : Voxel.Border;// 否则该面不绘制
//        }
//    }
//}
