using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public static class WorldMaths
    {
        /// <summary>
        /// name="percentage" 0 ~ 1
        /// </summary>
        /// <param name="percentage">0~1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel GetVoxelByPercent(ref BlobArray<Voxel> array, float percentage)
        {
            int index = (int)math.round(percentage * (array.Length - 1));// 若百分比正确,该值不会溢出
            return array[math.clamp(index, 0, array.Length - 1)];
        }
        /// <summary>
        /// name="percentage" 0 ~ 1
        /// </summary>
        /// <param name="percentage">0~1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel GetVoxelByPercent(ref BlobArray<Voxel> array, float2 pos, float percentage)
        {
            int index = (int)math.round(percentage * (array.Length - 1) + noise.cnoise(pos * 0.18f) * 0.05f);

            return array[math.clamp(index, 0, array.Length - 1)];
        }
        /// <summary>
        /// name="percentage" 0 ~ 1
        /// </summary>
        /// <param name="percentage">0~1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel GetVoxelByPercent(ref BlobArray<Voxel> array, float2 pos, float borderWidth, float borderScale, float percentage)
        {
            int indexLimit = array.Length - 1;
            float remapV = percentage * indexLimit;
            int index = (int)math.round(remapV + noise.cnoise(pos * borderScale * (array.Length - remapV)) * borderWidth);

            return array[math.clamp(index, 0, indexLimit)];
        }
        /// <summary>
        /// name="percentage" -1 ~ 1
        /// </summary>
        /// <param name="noise">0~1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel GetVoxelByNoise(ref BlobArray<Voxel> array, float noise)
        {
            return GetVoxelByPercent(ref array, RemapToZeroOne(noise));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemapToZeroOne(float v)
        {
            return (v + 1f) * 0.5f;
        }
    }
}
