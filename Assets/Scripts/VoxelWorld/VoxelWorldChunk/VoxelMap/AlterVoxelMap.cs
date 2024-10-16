//using System.Runtime.CompilerServices;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    public struct AlterVoxelMap
//    {
//        NativeHashSet<int3> dirtyChunkSet;
//        NativeArray<Voxel> voxels;
//        NativeHashMap<int3, int> chunkIndexToSliceMap;
//        public AlterVoxelMap(NativeHashSet<int3> dirtyChunkSet, NativeArray<Voxel> voxels, NativeHashMap<int3, int> chunkIndexToSliceMap)
//        {
//            this.dirtyChunkSet = dirtyChunkSet;
//            this.chunkIndexToSliceMap = chunkIndexToSliceMap;
//            this.voxels = voxels;
//        }
//        public readonly bool Contains(int3 bigChunkIndex)
//        {
//            return chunkIndexToSliceMap.ContainsKey(bigChunkIndex);
//        }
//        #region 引用区块数据
//        public void TrySetVoxel(in Voxel voxel, in int3 voxelIndexInWorld)
//        {
//            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//            if (chunkIndexToSliceMap.TryGetValue(bigChunkIndex, out int sliceIndex))
//            {
//                int3 voxelIndexInBigChunk = VoxelMath.VoxelIndexInBigChunk(bigChunkIndex, voxelIndexInWorld);
//                voxels[VoxelMath.LocalVoxelArrayIndexInBigChunk(voxelIndexInBigChunk) + sliceIndex] = voxel;
//                DirtyNearSmallChunk(in voxelIndexInBigChunk, in bigChunkIndex);
//            }
//        }
//        void DirtyNearSmallChunk(in int3 voxelIndexInBigChunk, in int3 bigChunkIndex)
//        {
//            const int smallChunkLimitIndex = Settings.SmallChunkSize - 1;

//            int3 voxelIndexInSmallChunk
//                = new int3(voxelIndexInBigChunk.x, voxelIndexInBigChunk.y % Settings.SmallChunkSize, voxelIndexInBigChunk.z);

//            int3 smallChunkIndex
//                    = new int3(bigChunkIndex.x, voxelIndexInBigChunk.y / Settings.SmallChunkSize, bigChunkIndex.z);

//            dirtyChunkSet.Add(smallChunkIndex);
//            // 不要删除
//            // 不能分开使用math.any(voxelIndexInSmallChunk == 0)和math.any(voxelIndexInSmallChunk == (Settings.SmallChunkSize - 1))
//            // 因为可能有处于背面（+Z）又同时处于左面的（-X）的
//            // 前面 // 除了上下两个以外,其他临近的区块Y索引是一样的
//            if (voxelIndexInSmallChunk.z == smallChunkLimitIndex)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z + 1));
//            }
//            // 后面
//            if (voxelIndexInSmallChunk.z == 0)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z - 1));
//            }
           

//            // 顶面 // 这个Y必须最最终索引小一,顶上才有区块
//            if (voxelIndexInSmallChunk.y == smallChunkLimitIndex && smallChunkIndex.y < Settings.WorldHeightInChunk - 1)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y + 1, smallChunkIndex.z));
//            }
//            // 下边 // 自身区块索引大于0,则下方才能有小区块
//            if (voxelIndexInSmallChunk.y == 0 && smallChunkIndex.y > 0)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x, smallChunkIndex.y - 1, smallChunkIndex.z));
//            }

//            // 右面
//            if (voxelIndexInSmallChunk.x == smallChunkLimitIndex)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x + 1, smallChunkIndex.y, smallChunkIndex.z));
//            }
//            // 左边
//            if (voxelIndexInSmallChunk.x == 0)
//            {
//                dirtyChunkSet.Add(new int3(smallChunkIndex.x - 1, smallChunkIndex.y, smallChunkIndex.z));
//            }
//        }
//        #endregion
//    }
//}
