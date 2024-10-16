//using CatDOTS.VoxelWorld;
//using CatFramework;
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

//namespace CatDOTS.Assets.Scripts.CatDOTS.VoxelWorld.Components.VoxelMap
//{
//    public struct VoxelWorldMap : IComponentData, IRecorder
//    {
//        public struct BigChunkSlice
//        {
//            public readonly int3 BigChunkIndex;
//            public readonly int SliceIndex;
//            NativeSlice<Voxel> Voxels;
//            public readonly NativeSlice<Voxel> VoxelMap => Voxels;
//            public Voxel this[int index]
//            {
//                readonly get { return Voxels[index]; }
//                set { Voxels[index] = value; }
//            }
//            public readonly int TotalLength => Voxels.Length;
//            public readonly int SamllChunkLength => Voxels.Length / Settings.WorldHeightInChunk;
//            public readonly int Batch => Settings.WorldHeightInChunk;
//            public readonly int3 BigChunkPosInt => BigChunkIndex * Settings.SmallChunkSize;
//            public readonly float3 BigChunkPos => BigChunkIndex * Settings.SmallChunkSize;
//            public BigChunkSlice(int sliceIndex, int3 bigChunkIndex, NativeSlice<Voxel> voxels)
//            {
//                SliceIndex = sliceIndex;
//                BigChunkIndex = bigChunkIndex;
//                Voxels = voxels;
//            }
//            public static BigChunkSlice Empty => new BigChunkSlice(int.MinValue, int.MinValue, default);
//        }
//        public readonly struct BigChunkSliceReadOnly
//        {
//            public readonly int3 BigChunkIndex;
//            readonly NativeSlice<Voxel> Voxels;
//            public readonly int TotalLength => Voxels.Length;
//            public readonly int SamllChunkLength => Settings.VoxelCapacityInSmallChunk;
//            public readonly Voxel this[int index]
//                => Voxels[index];
//            public readonly int3 BigChunkPosInt => BigChunkIndex * Settings.SmallChunkSize;
//            public BigChunkSliceReadOnly(int3 bigChunkIndex, NativeSlice<Voxel> voxels)
//            {
//                BigChunkIndex = bigChunkIndex;
//                Voxels = voxels;
//            }
//        }

//        NativeArray<Voxel> Voxels;
//        NativeList<int> pool;
//        NativeArray<int> map;// 规定-1则为这个区不存在
//        readonly int loadDistanceInChunk;
//        int2 mapOriginalInChunk;
//        public void Clear()
//        {
//            ResetPool();
//        }
//        public void Dispose()
//        {
//            Voxels.Dispose();
//            pool.Dispose();
//            map.Dispose();
//        }
//        public VoxelWorldMap(int bigChunkCount, int loadDistanceInChunk)
//        {
//            Voxels = new NativeArray<Voxel>(bigChunkCount * Settings.VoxelCapacityInBigChunk, Allocator.Persistent);
//            pool = new NativeList<int>(bigChunkCount, Allocator.Persistent);
//            map = new NativeArray<int>(loadDistanceInChunk * loadDistanceInChunk, Allocator.Persistent);
//            this.loadDistanceInChunk = loadDistanceInChunk;
//            mapOriginalInChunk = 0;
//            ResetPool();

//            if (ConsoleCatD.Enable)
//            {
//                ConsoleCatD.Log($"整个体素图的体素数量：{Voxels.Length}；切片池的量：{pool.Length}；体素图长度为：{map.Length}；卸载距离为：{this.loadDistanceInChunk}；");
//            }
//        }
//        void ResetPool()
//        {
//            pool.Clear();
//            int bigChunkCount = Voxels.Length / Settings.VoxelCapacityInBigChunk;
//            for (int i = 0; i < bigChunkCount; i++)
//            {
//                pool.Add(i * Settings.VoxelCapacityInBigChunk);
//            }
//            for (int i = 0; i < map.Length; i++)
//            {
//                map[i] = -1;
//            }
//        }
//        public void UpdateMap(int2 playerChunkIndex, Dictionary<int3, BigChunkManaged> activeBigChunkMap)
//        {
//            // 玩家0，0，原点-4，-4
//            mapOriginalInChunk = playerChunkIndex - /*4*/loadDistanceInChunk / 2;// 重置原点
//            int x = 0; int z = 0;
//            for (int i = 0; i < map.Length; i++)
//            {
//                int3 bigChunkIndex = new int3(mapOriginalInChunk.x + x, 0, mapOriginalInChunk.y + z);
//                // x,z从0开始,
//                map[VoxelMath.D2IndexToIndex(x, z, loadDistanceInChunk)] =
//                    activeBigChunkMap.TryGetValue(bigChunkIndex, out BigChunkManaged bigChunk)
//                    ? bigChunk.Slice.SliceIndex : -1;
//                z++;
//                if (z == loadDistanceInChunk)
//                {
//                    z = 0;
//                    x++;
//                }
//            }
//        }
//        public readonly bool CheckHasOne()
//        {
//            return pool.Length != 0;
//        }
//        public readonly bool CheckMeetNeed(int needChunkCount)
//        {
//            if (ConsoleCatD.Enable && needChunkCount > pool.Length)
//            {
//                ConsoleCatD.LogWarning($"区块池容量不足，当前需求：{needChunkCount}；切片池余量：{pool.Length}；");
//            }
//            return needChunkCount <= pool.Length;
//        }
//        public readonly BigChunkSlice AllocatedSlice(int3 bigChunkIndex)
//        {
//            if (ConsoleCatD.Enable && bigChunkIndex.y != 0)
//            {
//                ConsoleCatD.LogWarning($"大区块索引越界：{bigChunkIndex}");
//            }
//            int sliceIndex = pool[^1];
//            pool.RemoveAt(pool.Length - 1);
//            NativeSlice<Voxel> slice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
//            return new BigChunkSlice(sliceIndex, bigChunkIndex, slice);
//        }
//        public void DeallocatedSlice(ref BigChunkSlice slice)
//        {
//            //if (ClearTargetSliceIndex(slice.BigChunkIndex.As2D()))
//            //{
//            //    pool.Add(slice.SliceIndex);
//            //    slice = BigChunkSlice.Empty;
//            //}
//            //else if (ConsoleCat.Enable)
//            //{
//            //    ConsoleCat.LogWarning($"大区块：{slice.SliceIndex}，重复解除分配");
//            //}
//            pool.Add(slice.SliceIndex);
//            slice = BigChunkSlice.Empty;
//        }
//        public bool RefSlice(int3 bigChunkIndex, out NativeSlice<Voxel> bigChunkSlice)
//        {
//#if UNITY_EDITOR
//            if (bigChunkIndex.y != 0)
//            {
//                Debug.LogWarning($"大区块索引越界：{bigChunkIndex}");
//            }
//#endif
//            if (TryGetSliceIndex(bigChunkIndex, out int sliceIndex))
//            {
//                bigChunkSlice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
//                return true;
//            }
//            bigChunkSlice = default;
//            return false;
//        }
//        public readonly bool TryGetReadOnlySlice(int3 bigChunkIndex, out BigChunkSliceReadOnly sliceReadOnly)
//        {
//#if UNITY_EDITOR
//            if (bigChunkIndex.y != 0)
//            {
//                Debug.LogWarning($"大区块索引越界：{bigChunkIndex}");
//            }
//#endif
//            if (TryGetSliceIndex(bigChunkIndex, out int sliceIndex))
//            {
//                NativeSlice<Voxel> slice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
//                sliceReadOnly = new BigChunkSliceReadOnly(bigChunkIndex, slice);
//                return true;
//            }
//            sliceReadOnly = default;
//            return false;
//        }
//        public readonly bool ContainsKey(int3 bigChunkIndex)
//        {
//            return GetSliceIndex(bigChunkIndex.As2D()) != -1;// 除了判断索引是否在范围内，还需要判断这个区块在图里是否已存在
//        }
//        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//        //bool ClearTargetSliceIndex(int2 bigChunkIndex)// 使用2D索引，这个方法并不检查Y方向的合法性
//        //{
//        //    int2 relativeIndex = bigChunkIndex - mapOriginalInChunk;// 此处减去的原点，得到已是相对索引位置
//        //                                                            // 首先是否在范围内，如果在范围内则计算后的索引一定在地图的索引内
//        //    if (VoxelMath.D2IndexInRange(relativeIndex, loadDistanceInChunk) && map[VoxelMath.D2IndexToIndex(relativeIndex, loadDistanceInChunk)] != -1)
//        //    {
//        //        map[VoxelMath.D2IndexToIndex(relativeIndex, loadDistanceInChunk)] = -1;
//        //        return true;
//        //    }
//        //    return false;
//        //}

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly int GetSliceIndex(int2 bigChunkIndex)// 使用2D索引，这个方法并不检查Y方向的合法性
//        {
//            // 原点-4，-4，区块0，0，相对4，4
//            int2 relativeIndex = bigChunkIndex - mapOriginalInChunk;// 此处减去的原点，得到已是相对索引位置
//                                                                    // 首先是否在范围内，如果在范围内则计算后的索引一定在地图的索引内
//            return VoxelMath.D2IndexInRange(relativeIndex, loadDistanceInChunk) ? map[VoxelMath.D2IndexToIndex(relativeIndex, loadDistanceInChunk)] : -1;
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly bool TryGetSliceIndex(int3 bigChunkIndex, out int sliceIndex)
//        {
//            sliceIndex = bigChunkIndex.y == 0 ? GetSliceIndex(bigChunkIndex.As2D()) : -1;
//            return sliceIndex != -1;
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        readonly Voxel ReadVoxel(int3 bigChunkIndex, int3 voxelIndexInWorld, int sliceIndex)
//        {
//            return Voxels[VoxelMath.VoxelArrayIndexInBigChunk(bigChunkIndex, voxelIndexInWorld) + sliceIndex];
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public void SetVoxel(Voxel voxel, int3 bigChunkIndex, int3 voxelIndexInWorld, int sliceIndex)
//        {
//            Voxels[VoxelMath.VoxelArrayIndexInBigChunk(bigChunkIndex, voxelIndexInWorld) + sliceIndex] = voxel;
//        }
//        #region 体素写入的方法
//        public void SetVoxel(in int3 voxelIndexInWorld, Voxel voxel)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            if (TryGetSliceIndex(bigChunkIndex, out int sliceIndex))
//            {
//                SetVoxel(voxel, bigChunkIndex, voxelIndexInWorld, sliceIndex);
//            }
//        }
//        #endregion
//        #region 体素读取的方法
//        /// <summary>
//        /// 一般不检查，直接设置体素，不会存在放置到了不存在区块的情况
//        /// </summary>
//        public readonly bool CheckCanPutVoxel(in int3 voxelIndexInWorld)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            // 存在的区块，并且空体素，才可以放置体素
//            return TryGetSliceIndex(bigChunkIndex, out int sliceIndex) 
//                && ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex).VoxelTypeIndex == VoxelType.EmptyVoxelID;

//        }
//        /// <summary>
//        /// 构件网格时使用的
//        /// </summary>
//        public readonly Voxel ReadVoxelOrBarrier(in int3 voxelIndexInWorld)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            return TryGetSliceIndex(bigChunkIndex, out int sliceIndex)// 在范围内？
//                ? ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex)// 则读取体素
//                : Voxel.Barrier;// 否则该面不绘制
//        }
//        public readonly bool CheckVoxelNeedCollide(in int3 voxelIndexInWorld, ref BlobArray<VoxelType> voxelTypes)
//        {
//            return CheckVoxelCollideSolid(in voxelIndexInWorld, ref voxelTypes)// 首先自身需要有碰撞
//                                                                               // 然后需要有任意面不需要碰撞
//                && (!CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x + 1, voxelIndexInWorld.y, voxelIndexInWorld.z), ref voxelTypes) ||
//                    !CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x - 1, voxelIndexInWorld.y, voxelIndexInWorld.z), ref voxelTypes) ||
//                    !CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y + 1, voxelIndexInWorld.z), ref voxelTypes) ||
//                    !CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y - 1, voxelIndexInWorld.z), ref voxelTypes) ||
//                    !CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y, voxelIndexInWorld.z + 1), ref voxelTypes) ||
//                    !CheckVoxelNeedCollide(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y, voxelIndexInWorld.z - 1), ref voxelTypes));
//        }
//        /// <summary>
//        /// 对于范围外的算做固体
//        /// </summary>
//        readonly bool CheckVoxelCollideSolid(in int3 voxelIndexInWorld, ref BlobArray<VoxelType> voxelTypes)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);

//            return !TryGetSliceIndex(bigChunkIndex, out int sliceIndex)// 没获取到则为溢出图外，需要碰撞
//                || voxelTypes[ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex).VoxelTypeIndex].Solid;// 否则获取到的情况下，返回体素是否固体
//        }
//        /// <summary>
//        /// 对于范围之外的为非固体
//        /// </summary>
//        public readonly bool CheckVoxelNonSolid(in int3 voxelIndexInWorld, ref BlobArray<VoxelType> voxelTypes)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            return !TryGetSliceIndex(bigChunkIndex, out int sliceIndex)// 没获取到则为溢出图外，非固体
//                || !voxelTypes[ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex).VoxelTypeIndex].Solid;// 否则获取到了，返回体素！是否固体
//        }
//        public readonly bool CheckVoxelSolid(in int3 voxelIndexInWorld, ref BlobArray<VoxelType> voxelTypes)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            return TryGetSliceIndex(bigChunkIndex, out int sliceIndex) && voxelTypes[ReadVoxel(bigChunkIndex, voxelIndexInWorld, sliceIndex).VoxelTypeIndex].Solid;
//        }
//        #endregion
//    }
//}
