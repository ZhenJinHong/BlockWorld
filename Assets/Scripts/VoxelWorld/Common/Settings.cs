using CatFramework;
using CatFramework.CatMath;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    //public readonly struct WorldChunkSetting
    //{
    //    public const int WorldRangeLimitInVoxel = 1024;

    //    public readonly int SmallChunkSize;
    //    public readonly int WorldHeightInVoxel;
    //    public readonly int WorldHeightInChunk => WorldHeightInVoxel / SmallChunkSize;
    //    public readonly int WorldRangeLimitInChunk => WorldRangeLimitInVoxel / SmallChunkSize;
    //    public readonly int HalfChunkSize => SmallChunkSize / 2;
    //    /// <summary>
    //    /// 区块每层体素数量
    //    /// </summary>
    //    public readonly int VoxelCountInFloor => SmallChunkSize * SmallChunkSize;
    //    public readonly int VoxelCapacityInBigChunk => VoxelCapacityInSmallChunk * WorldHeightInChunk;
    //    public readonly int VoxelCapacityInSmallChunk => SmallChunkSize * SmallChunkSize * SmallChunkSize;
    //    public readonly int ClampWorldRange(int value)
    //    {
    //        return math.clamp(value, 2, WorldRangeLimitInChunk) / 2 * 2;
    //    }
    //    /// <summary>
    //    /// 小区块高度索引是否在范围内
    //    /// </summary>
    //    public readonly bool ValidChunkHeightIndex(int y)
    //        => y > -1 && y < WorldHeightInChunk;
    //    public WorldChunkSetting(int smallChunkSize, int worldHeightInVoxel)
    //    {
    //        if (worldHeightInVoxel % smallChunkSize != 0)
    //            throw new InvalidOperationException();
    //        SmallChunkSize = smallChunkSize;
    //        WorldHeightInVoxel = worldHeightInVoxel;
    //    }
    //}
    public readonly struct Settings
    {
        public const int SmallChunkSize = 32;
        //public const float InverseSmallChunkSize = 1 / SmallChunkSize;
        public const int WorldHeightInVoxel = 256;
        public const int WorldHeightInVoxelDisplacement = 8;
        public const int SmallChunkSizeDisplacement = 5;
        /// <summary>
        /// 用以位并运算求余32
        /// </summary>
        public const int TruncToSmallChunkSize = 0b11111;
        /// <summary>
        /// 用以位并运算求余256
        /// </summary>
        public const int TruncToBigChunkHeight = 0b11111111;
        public const int VoxelCountInFloorDisplacement = 10;
        //public const float InverseWorldHeightInVoxel = 1 / WorldHeightInVoxel;
        public const int WorldRangeLimitInVoxel = 2048;

        public const int WorldHeightInChunk = WorldHeightInVoxel / SmallChunkSize;
        public const int WorldRangeLimitInChunk = WorldRangeLimitInVoxel / SmallChunkSize;
        public const int HalfChunkSize = SmallChunkSize / 2;

        public const int MinWorldRangeInChunk = 2;
        public static int ClampWorldRange(int value)
        {
            return MathC.Clamp(value, MinWorldRangeInChunk, WorldRangeLimitInChunk) / 2 * 2;
        }
        public static int ClampWorldDistance(int value)
        {
            return MathC.Clamp(value, MinWorldRangeInChunk / 2, WorldRangeLimitInChunk / 2);
        }
        public const int VoxelColliderLimit = 10240;
        /// <summary>
        /// 同时可以渲染的区块上限
        /// </summary>
        const int BigChunkRenderLimit = WorldRangeLimitInChunk * WorldRangeLimitInChunk;
        /// <summary>
        /// 上限并非最终可能使用的，具体容量取决于加载距离
        /// </summary>
        const int BigChunkMapCapacityLimit = BigChunkRenderLimit * 2;
        /// <summary>
        /// 区块每层体素数量
        /// </summary>
        public const int VoxelCountInFloor = SmallChunkSize * SmallChunkSize;

        #region 大区块
        public const int VoxelCapacityInBigChunk = VoxelCapacityInSmallChunk * WorldHeightInChunk;
        public const int VoxelCapacityInSmallChunk = SmallChunkSize * SmallChunkSize * SmallChunkSize;
        // TODO 后面修改增加一个方法按照卸载距离计算需要的内存，再把内存传给当前方法
        /// <summary>
        /// 切片图容量不足时，开始彻底卸载区块
        /// </summary>
        public static int BigVoxelChunkMapCapacity(int preloadingRange, int maxMemoryMB, bool auto = true)
        {
            #region
            // 在区块构建系统中
            // int min = -preloadingDistance / 2;
            // int max = preloadingDistance / 2;// 这里是距离而非索引,导致大了一圈
            #endregion
            int minBigChunkCount = preloadingRange * (preloadingRange/* + 3*/);// 直接大一圈
            int minMemoryMB = BigChunkCapacityToMB(minBigChunkCount);
            if (minMemoryMB > maxMemoryMB && ConsoleCat.Enable)
                ConsoleCat.LogWarning($"最小需要的内存：{minMemoryMB}MB大于允许的最大内存：{maxMemoryMB}MB");
            if (!auto && minMemoryMB < maxMemoryMB)// 如果非自动，则按照指定内存计算
            {
                int maxBigChunkCount = MBToBigChunkCapacity(maxMemoryMB);
                if (maxBigChunkCount / minBigChunkCount > 4)
                {
                    if (ConsoleCat.Enable) ConsoleCat.LogWarning($"设定的允许的最大内存计算的区块数量：{maxBigChunkCount}比以加载距离计算的最小需要的区块数量：{minBigChunkCount}大了4倍以上");
                }
                else
                {
                    minMemoryMB = maxMemoryMB;
                    minBigChunkCount = maxBigChunkCount;
                }
            }
            if (ConsoleCat.Enable) ConsoleCat.Log($"大区块体素图区块数量：{minBigChunkCount}个区块；需要的内存：{minMemoryMB}MB");
            #region // 这个实际上不可能存在超出的情况
            if (minBigChunkCount > BigChunkMapCapacityLimit)
            {
                if (ConsoleCat.Enable)
                {
                    ConsoleCat.LogWarning($"大区块容量超过上限：{BigChunkMapCapacityLimit}，当前为：{minBigChunkCount}");
                }
                minBigChunkCount = BigChunkMapCapacityLimit;
            }
            #endregion
            return minBigChunkCount;
        }
        static int BigChunkCapacityToMB(int capacity)
            => (int)(((ulong)capacity) * VoxelCapacityInBigChunk * Voxel.TypeSize / 1024 / 1024);
        static int MBToBigChunkCapacity(int MB)
            => (int)((ulong)MB * 1024 * 1024 / Voxel.TypeSize / VoxelCapacityInBigChunk);

        #endregion
    }
}
