using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    ///// <summary>
    ///// 比较接近了,但还是负优化
    ///// </summary>
    //public struct NearBigChunkHelper
    //{
    //    int3 original;
    //    NativeArray<int> SliceIndex;
    //    public NearBigChunkHelper(int3 centerBigChunkIndex, VoxelWorldMap.ReadOnly voxelWorldMap)
    //    {
    //        SliceIndex = new NativeArray<int>(9, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
    //        original = centerBigChunkIndex + new int3(-1, 0, -1);
    //        SliceIndex[0] = voxelWorldMap.GetSliceIndex(original + new int3(0, 0, 0));
    //        SliceIndex[1] = voxelWorldMap.GetSliceIndex(original + new int3(1, 0, 0));
    //        SliceIndex[2] = voxelWorldMap.GetSliceIndex(original + new int3(2, 0, 0));
    //        SliceIndex[3] = voxelWorldMap.GetSliceIndex(original + new int3(0, 0, 1));
    //        SliceIndex[4] = voxelWorldMap.GetSliceIndex(original + new int3(1, 0, 1));
    //        SliceIndex[5] = voxelWorldMap.GetSliceIndex(original + new int3(2, 0, 1));
    //        SliceIndex[6] = voxelWorldMap.GetSliceIndex(original + new int3(0, 0, 2));
    //        SliceIndex[7] = voxelWorldMap.GetSliceIndex(original + new int3(1, 0, 2));
    //        SliceIndex[8] = voxelWorldMap.GetSliceIndex(original + new int3(2, 0, 2));
    //    }
    //    public int GetSliceIndex(int3 bigChunkIndex)
    //    {
    //        return bigChunkIndex.y == 0 ? SliceIndex[bigChunkIndex.x - original.x + (bigChunkIndex.z - original.z) * 3] : -1;
    //    }
    //}
    //    /// <summary>
    //    /// 负优化
    //    /// </summary>
    //    [Obsolete]
    //    public struct NearBigChunkHelper
    //    {
    //        //NativeSlice<Voxel> _0_0;
    //        //NativeSlice<Voxel> _1_0;
    //        //NativeSlice<Voxel> _2_0;
    //        //NativeSlice<Voxel> _0_1;
    //        //NativeSlice<Voxel> _1_1;
    //        //NativeSlice<Voxel> _2_1;
    //        //NativeSlice<Voxel> _0_2;
    //        //NativeSlice<Voxel> _1_2;
    //        //NativeSlice<Voxel> _2_2;
    //        int3 original;
    //        Voxel defaultVoxel;
    //        NativeArray<NativeSlice<Voxel>> Chunks;
    //        public NearBigChunkHelper(int3 centerBigChunkIndex, Voxel defaultVoxel, VoxelWorldMap voxelWorldMap)
    //        {
    //#if UNITY_EDITOR
    //            if (centerBigChunkIndex.y != 0)
    //                throw new Exception("Y非0");
    //#endif
    //            Chunks = new NativeArray<NativeSlice<Voxel>>(9, Allocator.Temp);
    //            NativeSlice<Voxel> _0_0;
    //            NativeSlice<Voxel> _1_0;
    //            NativeSlice<Voxel> _2_0;
    //            NativeSlice<Voxel> _0_1;
    //            NativeSlice<Voxel> _1_1;
    //            NativeSlice<Voxel> _2_1;
    //            NativeSlice<Voxel> _0_2;
    //            NativeSlice<Voxel> _1_2;
    //            NativeSlice<Voxel> _2_2;
    //            original = centerBigChunkIndex + new int3(-1, 0, -1);

    //            this.defaultVoxel = defaultVoxel;
    //            voxelWorldMap.RefSlice(original + new int3(0, 0, 0), out _0_0);
    //            voxelWorldMap.RefSlice(original + new int3(1, 0, 0), out _1_0);
    //            voxelWorldMap.RefSlice(original + new int3(2, 0, 0), out _2_0);
    //            voxelWorldMap.RefSlice(original + new int3(0, 0, 1), out _0_1);
    //            voxelWorldMap.RefSlice(original + new int3(1, 0, 1), out _1_1);
    //            voxelWorldMap.RefSlice(original + new int3(2, 0, 1), out _2_1);
    //            voxelWorldMap.RefSlice(original + new int3(0, 0, 2), out _0_2);
    //            voxelWorldMap.RefSlice(original + new int3(1, 0, 2), out _1_2);
    //            voxelWorldMap.RefSlice(original + new int3(2, 0, 2), out _2_2);

    //            Chunks[0] = _0_0;
    //            Chunks[1] = _1_0;
    //            Chunks[2] = _2_0;
    //            Chunks[3] = _0_1;
    //            Chunks[4] = _1_1;
    //            Chunks[5] = _2_1;
    //            Chunks[6] = _0_2;
    //            Chunks[7] = _1_2;
    //            Chunks[8] = _2_2;
    //        }
    //        public Voxel GetVoxel(int3 voxelIndexInWorld)
    //        {
    //            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
    //            int3 relativeIndex = bigChunkIndex - original;
    //            int voxelIndexInBigChunk = VoxelMath.VoxelArrayIndexInBigChunk(bigChunkIndex, voxelIndexInWorld);
    //            // y也判断确保不会超出范围
    //            //return (relativeIndex.x, relativeIndex.y, relativeIndex.z) switch
    //            //{
    //            //    (0, 0, 0) => _0_0.Length != 0 ? _0_0[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (1, 0, 0) => _1_0.Length != 0 ? _1_0[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (2, 0, 0) => _2_0.Length != 0 ? _2_0[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (0, 0, 1) => _0_1.Length != 0 ? _0_1[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (1, 0, 1) => _1_1.Length != 0 ? _1_1[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (2, 0, 1) => _2_1.Length != 0 ? _2_1[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (0, 0, 2) => _0_2.Length != 0 ? _0_2[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (1, 0, 2) => _1_2.Length != 0 ? _1_2[voxelIndexInBigChunk] : defaultVoxel,
    //            //    (2, 0, 2) => _2_2.Length != 0 ? _2_2[voxelIndexInBigChunk] : defaultVoxel,
    //            //    _ => defaultVoxel,
    //            //};
    //            int index = relativeIndex.x + relativeIndex.z * 3;
    //            return Chunks[index].Length != 0 ? Chunks[index][voxelIndexInBigChunk] : defaultVoxel;
    //        }
    //    }
}
