using CatFramework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelWorldMap : IComponentData
    {
        #region 子类
        public struct BigChunkSlice
        {
            public readonly int3 BigChunkIndex;
            NativeSlice<Voxel> Voxels;
            public readonly int SliceIndex;
            public readonly NativeSlice<Voxel> VoxelMap 
                => Voxels;
            public Voxel this[int index]
            {
                readonly get => Voxels[index];
                set => Voxels[index] = value;
            }
            public readonly int Length
                => Voxels.Length;
            public readonly int3 BigChunkPosInt
                => VoxelMath.BigChunkIndexToWorldCoord(BigChunkIndex);
            public readonly float3 BigChunkPos
                => VoxelMath.BigChunkIndexToWorldCoord(BigChunkIndex);
            public BigChunkSlice(int sliceIndex, int3 bigChunkIndex, NativeSlice<Voxel> voxels)
            {
                SliceIndex = sliceIndex;
                BigChunkIndex = bigChunkIndex;
                Voxels = voxels;
            }
        }
        public readonly struct BigChunkSliceReadOnly
        {
            public readonly int3 BigChunkIndex;
            readonly NativeSlice<Voxel> Voxels;
            public readonly int Length 
                => Voxels.Length;
            public readonly Voxel this[int index]
                => Voxels[index];
            public readonly int3 BigChunkPosInt
                => VoxelMath.BigChunkIndexToWorldCoord(BigChunkIndex);
            public BigChunkSliceReadOnly(int3 bigChunkIndex, NativeSlice<Voxel> voxels)
            {
                BigChunkIndex = bigChunkIndex;
                Voxels = voxels;
            }
            public SmallChunkSliceReadyOnly Split(int smallChunkIndex)
            {
                int3 chunkIndex = new int3(BigChunkIndex.x, smallChunkIndex, BigChunkIndex.z);
                return new SmallChunkSliceReadyOnly(chunkIndex, SplitBigChunkSlice(smallChunkIndex, Voxels));
            }
        }
        public readonly struct SmallChunkSliceReadyOnly
        {
            public readonly int3 ChunkIndex;
            public readonly int3 ChunkPosInt
                => VoxelMath.SmallChunkIndexToWorldCoord(ChunkIndex);
            readonly NativeSlice<Voxel> Voxels;
            public readonly int Length 
                => Voxels.Length;
            public readonly Voxel this[int index]
                => Voxels[index];
            public SmallChunkSliceReadyOnly(int3 chunkIndex, NativeSlice<Voxel> voxels)
            {
                ChunkIndex = chunkIndex;
                Voxels = voxels;
            }
        }
        public readonly struct ReadOnly
        {
            readonly NativeArray<Voxel> Voxels;
            readonly NativeHashMap<int3, int> chunkIndexToSliceMap;
            public Voxel this[int index]
               => Voxels[index];
            public ReadOnly(NativeArray<Voxel> voxels, NativeHashMap<int3, int> chunkIndexToSliceMap)
            {
                Voxels = voxels;
                this.chunkIndexToSliceMap = chunkIndexToSliceMap;
            }
            public int GetSliceIndex(int3 bigChunkIndex)
            {
                return chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex) ? sliceIndex : -1;
            }
            public Voxel GetVoxel(in int3 voxelIndexInWorld, int sliceIndex)
            {
                return Voxels[VoxelMath.VoxelArrayIndexInBigChunk(voxelIndexInWorld) + sliceIndex];
            }
            public Voxel GetVoxelOrBorder(int3 voxelIndexInWorld)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                return bigChunkIndex.y == 0 && chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex)
                    ? Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)]
                    : Voxel.Border;
            }
            public Voxel GetVoxelOrEmpty(int3 voxelIndexInWorld)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                return bigChunkIndex.y == 0 && chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex)
                    ? Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)]
                    : Voxel.Empty;
            }
            public bool CheckVoxelExposed(int3 voxelIndexInWorld)
            {
                return (CheckVoxelNonSolid(new int3(voxelIndexInWorld.x + 1, voxelIndexInWorld.y, voxelIndexInWorld.z)) ||
                        CheckVoxelNonSolid(new int3(voxelIndexInWorld.x - 1, voxelIndexInWorld.y, voxelIndexInWorld.z)) ||
                        CheckVoxelNonSolid(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y + 1, voxelIndexInWorld.z)) ||
                        CheckVoxelNonSolid(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y - 1, voxelIndexInWorld.z)) ||
                        CheckVoxelNonSolid(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y, voxelIndexInWorld.z + 1)) ||
                        CheckVoxelNonSolid(new int3(voxelIndexInWorld.x, voxelIndexInWorld.y, voxelIndexInWorld.z - 1)));
            }
            /// <summary>
            /// 如果体素不在世界中是非固体？
            /// </summary>
            public bool CheckVoxelNonSolid(int3 voxelIndexInWorld)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                // 如果没获取到，返回非固体
                return bigChunkIndex.y != 0
                    || !chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex)
                    || !Voxel.Solid(Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)].VoxelMaterial);
            }
            public bool CheckVoxelSolid(int3 voxelIndexInWorld)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                // 如果获取到了，并且是固体返回固体，否则非固体
                return bigChunkIndex.y == 0 && chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex)
                       && Voxel.Solid(Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)].VoxelMaterial);
            }
            public bool CheckHasVoxel(int3 voxelIndexInWorld)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                // Y值不等于0，永远不可以放置体素
                return bigChunkIndex.y != 0 || (chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex)
                       && Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)].VoxelTypeIndex != VoxelType.EmptyVoxelID);
            }
            public bool TryGetVoxel(int3 voxelIndexInWorld, out Voxel voxel)
            {
                int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
                if (bigChunkIndex.y == 0 && chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex))
                {
                    voxel = Voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)];
                    return true;
                }
                voxel = Voxel.Empty;
                return false;
            }
            public bool Contains(int3 bigChunkIndex) => chunkIndexToSliceMap.ContainsKey(bigChunkIndex);
            public bool HasNearBigChunk(int3 bigChunkIndex)
            {
                return chunkIndexToSliceMap.ContainsKey(new int3(bigChunkIndex.x + 1, bigChunkIndex.y, bigChunkIndex.z)) &&
                    chunkIndexToSliceMap.ContainsKey(new int3(bigChunkIndex.x - 1, bigChunkIndex.y, bigChunkIndex.z)) &&
                    chunkIndexToSliceMap.ContainsKey(new int3(bigChunkIndex.x, bigChunkIndex.y, bigChunkIndex.z + 1)) &&
                    chunkIndexToSliceMap.ContainsKey(new int3(bigChunkIndex.x, bigChunkIndex.y, bigChunkIndex.z - 1));
            }
            public bool TryGetReadOnlySlice(int3 bigChunkIndex, out BigChunkSliceReadOnly sliceReadOnly)
            {
#if UNITY_EDITOR
                if (bigChunkIndex.y != 0)
                {
                    Debug.LogWarning($"大区块索引越界：{bigChunkIndex}");
                }
#endif
                if (chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex))
                {
                    NativeSlice<Voxel> slice = Voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
                    sliceReadOnly = new BigChunkSliceReadOnly(bigChunkIndex, slice);
                    return true;
                }
                sliceReadOnly = default;
                return false;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ConvertedVoxelIndexInTotalArray(int3 voxelIndexInWorld, int sliceIndex)
        {
            return VoxelMath.VoxelArrayIndexInBigChunk(voxelIndexInWorld) + sliceIndex;
        }
        #endregion
        NativeHashSet<int3> dirtySmallChunkSet;

        NativeArray<Voxel> voxels;
        NativeHashMap<int3, int> chunkIndexToSliceMap;
        public readonly int Capacity => voxels.Length / Settings.VoxelCapacityInBigChunk;
        public Voxel this[int index]
            => voxels[index];

        public VoxelWorldMap(NativeArray<Voxel> voxels, NativeHashMap<int3, int> chunkIndexToSliceMap, NativeHashSet<int3> dirtySmallChunkSet)
        {
            this.dirtySmallChunkSet = dirtySmallChunkSet;

            this.voxels = voxels;
            this.chunkIndexToSliceMap = chunkIndexToSliceMap;
        }
        public bool CopySmallChunkData(ref NativeArray<Voxel> voxels, int3 smallChunkIndex)
        {
            if (VoxelMath.SmallChunkIndexInRange(smallChunkIndex.y)
                && RefSliceWithoutDirty(new int3(smallChunkIndex.x, 0, smallChunkIndex.z), out NativeSlice<Voxel> bigChunkSlice))
            {
                var smallChunkSlice = SplitBigChunkSlice(smallChunkIndex.y, bigChunkSlice);
                smallChunkSlice.CopyTo(voxels);
                return true;
            }
            return false;
        }
        public bool RefSmallChunkSliceWithoutDirty(int3 smallChunkIndex, out NativeSlice<Voxel> smallChunkSlice)
        {
            if (VoxelMath.SmallChunkIndexInRange(smallChunkIndex.y)
                && RefSliceWithoutDirty(new int3(smallChunkIndex.x, 0, smallChunkIndex.z), out NativeSlice<Voxel> bigChunkSlice))
            {
                smallChunkSlice = SplitBigChunkSlice(smallChunkIndex.y, bigChunkSlice);
                return true;
            }
            smallChunkSlice = default;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeSlice<Voxel> SplitBigChunkSlice(int smallChunkYIndex, NativeSlice<Voxel> bigChunkSlice)
        {
            int start = smallChunkYIndex * Settings.VoxelCapacityInSmallChunk;
            return bigChunkSlice.Slice(start, Settings.VoxelCapacityInSmallChunk);
        }
        public bool RefSliceWithoutDirty(int3 bigChunkIndex, out NativeSlice<Voxel> bigChunkSlice)
        {
#if UNITY_EDITOR
            if (bigChunkIndex.y != 0)
            {
                Debug.LogWarning($"大区块索引越界：{bigChunkIndex}");
            }
#endif
            if (chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex))
            {
                bigChunkSlice = voxels.Slice(sliceIndex, Settings.VoxelCapacityInBigChunk);
                return true;
            }
            bigChunkSlice = default;
            return false;
        }
        public void SetVoxelWithoutDirty(int voxelIndexInTotalArray, Voxel voxel)
        {
            voxels[voxelIndexInTotalArray] = voxel;
        }
        public bool ContainsKey(int3 bigChunkIndex)
        {
            return chunkIndexToSliceMap.ContainsKey(bigChunkIndex);
        }
        #region 索引获知
        public bool TryGetVoxelIndexInTotalArray(in int3 voxelIndexInWorld, out int indexInTotalArray)
        {
            if (chunkIndexToSliceMap.TryGetValue(VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld), out int sliceIndex))
            {
                indexInTotalArray = ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex);
                return true;
            }
            indexInTotalArray = -1;
            return false;
        }
        public bool TryGetSliceIndexByBigChunkIndex(in int3 bigChunkIndex, out int sliceIndex)
        {
            return chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out sliceIndex);
        }
        #endregion
        #region 读取
        public bool TryGetVoxel(in float3 worldCoord, out Voxel voxel)
        {
            return TryGetVoxel(VoxelMath.PositionToVoxelIndexInWorld(worldCoord), out voxel);
        }
        public bool TryGetVoxel(in int3 voxelIndexInWorld, out Voxel voxel)
        {
            if (TryGetVoxelIndexInTotalArray(in voxelIndexInWorld, out int indexInTotalArray))
            {
                voxel = voxels[indexInTotalArray];
                return true;
            }
            voxel = Voxel.Null;
            return false;
        }
        #endregion
        #region 引用区块数据
        #region 有世界坐标和大区块索引时
        public void SetVoxel(in Voxel voxel, int voxelIndexInTotalArray, in int3 voxelIndexInWorld)
        {
            voxels[voxelIndexInTotalArray] = voxel;
            DirtyNearSmallChunk(in voxelIndexInWorld);
        }
        #endregion
        #region 仅有世界坐标时
        public Voxel TryReplaceVoxel(in Voxel voxel, in float3 worldCoord)
        {
            return TryReplaceVoxel(in voxel, VoxelMath.PositionToVoxelIndexInWorld(worldCoord));
        }
        public Voxel TryReplaceVoxel(in Voxel voxel, in int3 voxelIndexInWorld)
        {
            if (chunkIndexToSliceMap.TryGetValue(VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld), out int sliceIndex))
            {
                int index = ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex);
                Voxel target = voxels[index];
                if (target != voxel)
                {
                    voxels[index] = voxel;
                    DirtyNearSmallChunk(in voxelIndexInWorld);
                }
                return target;
            }
            return voxel;
        }
        public void TrySetVoxel(in Voxel voxel, in float3 worldCoord)
        {
            TrySetVoxel(in voxel, VoxelMath.PositionToVoxelIndexInWorld(worldCoord));
        }
        public void TrySetVoxel(in Voxel voxel, in int3 voxelIndexInWorld)
        {
            if (chunkIndexToSliceMap.TryGetValue(VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld), out int sliceIndex))
            {
                voxels[ConvertedVoxelIndexInTotalArray(voxelIndexInWorld, sliceIndex)] = voxel;
                DirtyNearSmallChunk(in voxelIndexInWorld);
            }
        }
        #endregion
        public void DirtyNearSmallChunk(in int3 voxelIndexInWorld)
        {
            const int smallChunkLimitIndex = Settings.SmallChunkSize - 1;
            int3 smallChunkIndex = VoxelMath.SmallChunkIndexByWorldCoord(voxelIndexInWorld);
            int3 voxelIndexInSmallChunk = VoxelMath.VoxelIndexInSmallChunk(voxelIndexInWorld);

            dirtySmallChunkSet.Add(smallChunkIndex);
            // 不要删除
            // 不能分开使用math.any(voxelIndexInSmallChunk == 0)和math.any(voxelIndexInSmallChunk == (Settings.SmallChunkSize - 1))
            // 因为可能有处于背面（+Z）又同时处于左面的（-X）的
            // 前面 // 除了上下两个以外,其他临近的区块Y索引是一样的
            if (math.any(voxelIndexInSmallChunk == smallChunkLimitIndex) || math.any(voxelIndexInSmallChunk == 0))
            {
                if (voxelIndexInSmallChunk.z == smallChunkLimitIndex)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z + 1));
                }
                // 后面
                if (voxelIndexInSmallChunk.z == 0)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z - 1));
                }


                // 顶面 // 这个Y必须最最终索引小一,顶上才有区块
                if (voxelIndexInSmallChunk.y == smallChunkLimitIndex && smallChunkIndex.y < Settings.WorldHeightInChunk - 1)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y + 1, smallChunkIndex.z));
                }
                // 下边 // 自身区块索引大于0,则下方才能有小区块
                if (voxelIndexInSmallChunk.y == 0 && smallChunkIndex.y > 0)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y - 1, smallChunkIndex.z));
                }

                // 右面
                if (voxelIndexInSmallChunk.x == smallChunkLimitIndex)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x + 1, smallChunkIndex.y, smallChunkIndex.z));
                }
                // 左边
                if (voxelIndexInSmallChunk.x == 0)
                {
                    dirtySmallChunkSet.Add(new int3(smallChunkIndex.x - 1, smallChunkIndex.y, smallChunkIndex.z));
                }
            }
        }
        [Obsolete]
        public void DirtyNearSmallChunk(in int3 voxelIndexInBigChunk, in int3 bigChunkIndex)
        {
            const int smallChunkLimitIndex = Settings.SmallChunkSize - 1;

            int3 voxelIndexInSmallChunk
                = new int3(voxelIndexInBigChunk.x, voxelIndexInBigChunk.y & Settings.TruncToSmallChunkSize, voxelIndexInBigChunk.z);

            int3 smallChunkIndex
                    = new int3(bigChunkIndex.x, voxelIndexInBigChunk.y >> Settings.SmallChunkSizeDisplacement, bigChunkIndex.z);

            dirtySmallChunkSet.Add(smallChunkIndex);
            // 不要删除
            // 不能分开使用math.any(voxelIndexInSmallChunk == 0)和math.any(voxelIndexInSmallChunk == (Settings.SmallChunkSize - 1))
            // 因为可能有处于背面（+Z）又同时处于左面的（-X）的
            // 前面 // 除了上下两个以外,其他临近的区块Y索引是一样的
            if (voxelIndexInSmallChunk.z == smallChunkLimitIndex)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z + 1));
            }
            // 后面
            if (voxelIndexInSmallChunk.z == 0)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z - 1));
            }


            // 顶面 // 这个Y必须最最终索引小一,顶上才有区块
            if (voxelIndexInSmallChunk.y == smallChunkLimitIndex && smallChunkIndex.y < Settings.WorldHeightInChunk - 1)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y + 1, smallChunkIndex.z));
            }
            // 下边 // 自身区块索引大于0,则下方才能有小区块
            if (voxelIndexInSmallChunk.y == 0 && smallChunkIndex.y > 0)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y - 1, smallChunkIndex.z));
            }

            // 右面
            if (voxelIndexInSmallChunk.x == smallChunkLimitIndex)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x + 1, smallChunkIndex.y, smallChunkIndex.z));
            }
            // 左边
            if (voxelIndexInSmallChunk.x == 0)
            {
                dirtySmallChunkSet.Add(new int3(smallChunkIndex.x - 1, smallChunkIndex.y, smallChunkIndex.z));
            }
        }
        #endregion
        public ReadOnly AsReadOnly()
        {
            return new ReadOnly(voxels, chunkIndexToSliceMap);
        }
    }
}
