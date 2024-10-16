using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public static class VectorCompress
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CompressByte3ToFloat(int x, int y, int z)
        {
            return x + (y << 8) + (z << 16);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Compress31X4ToFloat(int x, int y, int z, int w)
        {
            return x + (y << 5) + (z << 10) + (w << 15);
        }
    }
    // 关键的注释
    // 体素运算以左下角为原点运算，将体素当做一个点来计算（渲染包围盒除外）
    // 定义
    // 未标明ArrayIndex的都为维度索引
    // 对于坐标和索引的需要标明是InWorld还InLocal
    // 坐标和索引都存在世界或本地类型
    // 区块索引除外，区块索引是除以区块大小之后的，属于世界索引
    // 所有的坐标都是浮点值，所有的索引都是整型，比如float3 VoxelCoordInWorld  int3 VoxelIndexInWorld
    public static class VoxelMath
    {
        public static Voxel VoxelMask(VoxelMaskTag voxelMaskTag)
        {
            //ulong mask = 0ul;
            //if ((voxelMaskTag & VoxelMaskTag.ID) == VoxelMaskTag.ID)
            //{
            //    mask |= (ulong)ushort.MaxValue << 48;
            //}
            //if ((voxelMaskTag & VoxelMaskTag.ShapeIndex) == VoxelMaskTag.ShapeIndex)
            //{
            //    mask |= (ulong)ushort.MaxValue << 32;
            //}
            //if ((voxelMaskTag & VoxelMaskTag.Material) == VoxelMaskTag.Material)
            //{
            //    mask |= (ulong)byte.MaxValue << 24;
            //}
            //if ((voxelMaskTag & VoxelMaskTag.Energy) == VoxelMaskTag.Energy)
            //{
            //    mask |= 0b1111ul << 20;
            //}
            //if ((voxelMaskTag & VoxelMaskTag.ShapeDiretion) == VoxelMaskTag.ShapeDiretion)
            //{
            //    mask |= 0b1111ul << 16;
            //}
            //// 0b11111111 << 8
            //// 0b11111111 << 0
            //return mask;
            Voxel mask = 0ul;
            if ((voxelMaskTag & VoxelMaskTag.ID) == VoxelMaskTag.ID)
            {
                mask.VoxelTypeIndex = ushort.MaxValue;
            }
            if ((voxelMaskTag & VoxelMaskTag.ShapeIndex) == VoxelMaskTag.ShapeIndex)
            {
                mask.ShapeIndex = ushort.MaxValue;
            }
            if ((voxelMaskTag & VoxelMaskTag.Material) == VoxelMaskTag.Material)
            {
                mask.VoxelMaterial = (VoxelMaterial)byte.MaxValue;
            }
            if ((voxelMaskTag & VoxelMaskTag.Energy) == VoxelMaskTag.Energy)
            {
                mask.Energy = byte.MaxValue;
            }
            if ((voxelMaskTag & VoxelMaskTag.ShapeDiretion) == VoxelMaskTag.ShapeDiretion)
            {
                mask.ShapeDirection = byte.MaxValue;
            }
            // 0b11111111 << 8
            // 0b11111111 << 0
            return mask;
        }
        public unsafe static int TypeSize<T>() where T : unmanaged
        {
            return sizeof(T);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 CeilToInt3(Vector3 vector3)
        {
            return new int3(math.ceil(vector3));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 As2D(this int3 int3)
        {
            return new int2(int3.x, int3.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 As2D(this float3 int3)
        {
            return new float2(int3.x, int3.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemapValue01(float value, float outputMin, float outputMax)
        {
            return outputMin + value * (outputMax - outputMin);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VoxelInBigChunkRange(int3 voxelIndexInBigChunk)
        {
            return math.all(voxelIndexInBigChunk > -1) // 首先必须不为负数
                && voxelIndexInBigChunk.y < Settings.WorldHeightInVoxel// 高度不能超过体素世界高度
                && math.all(new int2(voxelIndexInBigChunk.x, voxelIndexInBigChunk.z) < Settings.SmallChunkSize);// 长宽不能超过区块大小
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VoxelInSamllChunkRange(int3 voxelIndexInBigChunk)
        {
            return math.all(voxelIndexInBigChunk > -1) // 首先必须不为负数
                && math.all(voxelIndexInBigChunk < Settings.SmallChunkSize);// 长宽不能超过区块大小
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SmallChunkIndexInRange(int yIndex)
        {
            return yIndex > -1 && yIndex < Settings.WorldHeightInChunk;
        }

        /// <summary>
        /// 玩家位置变更的使用round
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 PlayerChunkIndex(float3 worldCoord)
        {
            return new int2(math.round(new float2(worldCoord.x, worldCoord.z) / Settings.SmallChunkSize));
        }
        public static void PositionToVoxelIndexInWorldAndBigChunkIndex(float3 pos, out int3 voxelIndexInWorld, out int3 bigChunkIndex)
        {
            voxelIndexInWorld = new int3(math.floor(pos));
            bigChunkIndex = BigChunkIndexByWorldCoord(voxelIndexInWorld);
        }
        #region 索引逆向
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 SmallChunkIndexToWorldCoord(int3 smallChunkIndex)
        {
            return smallChunkIndex << Settings.SmallChunkSizeDisplacement;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 BigChunkIndexToWorldCoord(int3 bigChunkIndex)
        {
            return bigChunkIndex << Settings.SmallChunkSizeDisplacement;
        }
        /// <summary>
        /// 要求该体素在区块内，按照Y层，X列，Z个
        /// </summary>
        public static int3 InverseLocalVoxelArrayIndexInSmallChunk(int voxelIndexInSmallChunk)
        {
            int y = (voxelIndexInSmallChunk >> Settings.VoxelCountInFloorDisplacement);// 层数
            int x = (voxelIndexInSmallChunk - (y << Settings.VoxelCountInFloorDisplacement)) >> Settings.SmallChunkSizeDisplacement;// 列数
            int z = voxelIndexInSmallChunk - (y << Settings.VoxelCountInFloorDisplacement) - (x << Settings.SmallChunkSizeDisplacement);// 余个
            return new int3(x, y, z);
        }
        #endregion
        #region 区块索引获取
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 BigChunkIndexByWorldCoord(int3 worldCoord)
        {
            return new int3(
                worldCoord.x >> Settings.SmallChunkSizeDisplacement,
                worldCoord.y >> Settings.WorldHeightInVoxelDisplacement,
                worldCoord.z >> Settings.SmallChunkSizeDisplacement);
        }
        // 本质为除以32
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 SmallChunkIndexByWorldCoord(int3 voxelIndexInWorld)
        {
            return voxelIndexInWorld >> Settings.SmallChunkSizeDisplacement;
        }
        #endregion
        #region 三维体素索引
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 PositionToVoxelIndexInWorld(float3 pos)
        {
            return new int3(math.floor(pos));
        }
        /// <summary>
        /// 将体素世界索引转化为指定区块本地索引
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 VoxelIndexInBigChunk(int3 voxelIndexInWorld)
        {
            return new int3(
                voxelIndexInWorld.x & Settings.TruncToSmallChunkSize,
                voxelIndexInWorld.y & Settings.TruncToBigChunkHeight,
                voxelIndexInWorld.z & Settings.TruncToSmallChunkSize);
        }
        /// <summary>
        /// 将体素世界索引转化为指定区块本地索引,实际为截断为32
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 VoxelIndexInSmallChunk(int3 voxelIndexInWorld)
        {
            return voxelIndexInWorld & Settings.TruncToSmallChunkSize;
        }
        #endregion
        #region 数组体素索引
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LocalVoxelArrayIndexInBigChunk(int3 voxelIndexInBigChunk)
        {
            return LocalVoxelArrayIndexInBigChunk(voxelIndexInBigChunk.x, voxelIndexInBigChunk.y, voxelIndexInBigChunk.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LocalVoxelArrayIndexInBigChunk(int x, int y, int z)
        {
            return (y << Settings.VoxelCountInFloorDisplacement) + (x << Settings.SmallChunkSizeDisplacement) + z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LocalVoxelArrayIndexInSmallChunk(int3 voxelIndexInSmallChunk)
        {
            return LocalVoxelArrayIndexInSmallChunk(voxelIndexInSmallChunk.x, voxelIndexInSmallChunk.y, voxelIndexInSmallChunk.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LocalVoxelArrayIndexInSmallChunk(int x, int y, int z)
        {
            return (y << Settings.VoxelCountInFloorDisplacement) + (x << Settings.SmallChunkSizeDisplacement) + z;
        }
        /// <summary>
        /// 将体素世界索引转化为指定区块本地数组索引
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int VoxelArrayIndexInBigChunk(int3 voxelIndexInWorld)
        {
            return LocalVoxelArrayIndexInBigChunk(VoxelIndexInBigChunk(voxelIndexInWorld));
        }
        #endregion
        /// <summary>
        /// X列，Z个
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int D2IndexToIndex(int x, int z, int width)
        {
            return x * width + z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SmallChunkWidthD2IndexToIndex(int x, int z)
        {
            return (x << Settings.SmallChunkSizeDisplacement) + z;
        }
        /// <summary>
        /// X列，Z个
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int D2IndexToIndex(int2 index, int width)
        {
            return index.x * width + index.y;
        }
        /// <summary>
        /// 判断索引是否在0到length-1内
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool D2IndexInRange(int2 index, int length)
        {
            //return math.all(index > -1) && math.all(index < length);
            return math.all(new bool4(index > -1, index < length));
        }
        /// <summary>
        /// 按照Y层，X列，Z个
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int D3IndexToIndex(int3 index, int countInFloor, int rowWidth)
        {
            return index.y * countInFloor + index.x * rowWidth + index.z;
        }
        /// <summary>
        /// 判断是否大于距离,不包含距离,这意味着边界这一圈也将算在范围内
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OutOfDistance(int2 bigChunkIndex, int2 center, int distance)
        {
            return math.any(math.abs(bigChunkIndex - center) > distance);
        }
        /// <summary>
        /// 包含距离
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WithInDistance(int2 bigChunkIndex, int2 center, int distance)
        {
            return math.all(math.abs(bigChunkIndex - center) <= distance);
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static float D2DitanceToRadius(int distance)
        //{
        //    return 1.41f * distance;
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool InRadius(int2 bigChunkIndex, int2 center, int radius)
        //{
        //    return math.lengthsq(math.abs(bigChunkIndex - center)) < math.lengthsq(radius);
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool InRadiusSq(int2 bigChunkIndex, int2 center, int radiussq)
        //{
        //    return math.lengthsq(math.abs(bigChunkIndex - center)) < radiussq;
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool InRadiusSq(int2 ditance, int radiussq)
        //{
        //    return math.lengthsq(ditance) < radiussq;
        //}
    }
}
