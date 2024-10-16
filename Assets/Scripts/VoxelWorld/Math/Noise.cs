using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatDOTS.VoxelWorld
{
    public static class Noise
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static float RemapToZeroOne(float v)
        //{
        //    return (v + 1f) * 0.5f;
        //}
        // cnoise的范围是-1到1
        //public static float GetPerlinNoise(float2 position, float offset, float scale)
        //{
        //    return noise.cnoise((position + offset) * scale);// 偏移需要被乘以缩放,以免因为较大的偏移值+缩放后的位置导致缩放值精度丢失
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPerlinNoise(float2 position, float scale)
        {
            return noise.cnoise(position * scale);// 偏移需要被乘以缩放,以免因为较大的偏移值+缩放后的位置导致缩放值精度丢失
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPerlinNoise(float2 position)
        {
            return noise.cnoise(position);// 偏移需要被乘以缩放,以免因为较大的偏移值+缩放后的位置导致缩放值精度丢失
        }
        //public static float GetPerlinNoise(float2 position, float2 offset, float scale)
        //{
        //    return noise.cnoise((position + offset) * scale);// 偏移需要被乘以缩放,以免因为较大的偏移值+缩放后的位置导致缩放值精度丢失
        //}
        //public static float GetSNoise(float2 position, float offset, float scale)
        //{
        //    return noise.snoise((position + offset) * scale);
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSNoise(float2 position, float scale)
        {
            return noise.snoise(position * scale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSNoise(float2 position)
        {
            return noise.snoise(position);
        }
        //public static float GetSNoise(float2 position, float2 offset, float scale)
        //{
        //    return noise.snoise((position + offset) * scale);
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Get3DPerlinNoise(float3 position, float scale)
        {
            return noise.cnoise(position * scale);
        }
        //public static float OctavePerlin(float2 position, float offset, float scale, int octave, float persistance)
        //{
        //    position.x *= scale;
        //    position.y *= scale;
        //    position.x += scale;
        //    position.y += scale;
        //    float total = 0;
        //    float frequency = 1;
        //    float amplitude = 1;
        //    float amplitudeSum = 0;
        //    for (int i = 0; i < octave; i++)
        //    {
        //        total += GetPerlinNoise(position, offset, frequency) * amplitude;
        //        amplitudeSum += amplitude;
        //        amplitude *= persistance;
        //        frequency *= 2;
        //    }
        //    return total / amplitudeSum;
        //}
        //public static float OctavePerlin(float2 position, float offset, float scale, float persistance)
        //{
        //    position.x *= scale;
        //    position.y *= scale;
        //    position.x += scale;
        //    position.y += scale;
        //    float total = 0;
        //    float frequency = 1;
        //    float amplitude = 1;
        //    float amplitudeSum = 0;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        total += GetPerlinNoise(position, offset, frequency) * amplitude;
        //        amplitudeSum += amplitude;
        //        amplitude *= persistance;
        //        frequency *= 2;
        //    }
        //    return total / amplitudeSum;
        //}
        //public static float OctavePerlin(float2 position, float offset, float scale, int octave)
        //{
        //    position.x *= scale;
        //    position.y *= scale;
        //    position.x += scale;
        //    position.y += scale;
        //    float total = 0;
        //    for (int i = 0; i < octave; i++)
        //    {
        //        total += GetPerlinNoise(position, offset, scale);
        //        scale *= 50;
        //    }
        //    return total / octave;
        //}
        //public static float MultiPerlin(float2 position, float offset, float scale, int octave, float persistance)
        //{
        //    float n0 = GetPerlinNoise(position, offset, scale);
        //    float n1 = math.lerp(GetPerlinNoise(position, offset, scale * 0.01f), GetPerlinNoise(position, offset, scale * 0.1f), n0);
        //    float n2;
        //    //total += GetPerlinNoise(position, offset + scale * 1000f, scale * 0.06f);
        //    //total /= 2f;
        //    //return Get3DPerlinNoise(new float3(position, persistance), offset, scale) > total ? total : GetPerlinNoise(position, offset, scale);
        //    //return math.lerp(Get3DPerlinNoise(new float3(position, octave), offset, scale), total, total);

        //    position.x *= scale;
        //    position.y *= scale;
        //    position.x += scale;
        //    position.y += scale;
        //    float total = 0;
        //    float frequency = 1;
        //    float amplitude = 1;
        //    float amplitudeSum = 0;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        total += GetPerlinNoise(position, offset, frequency) * amplitude;
        //        amplitudeSum += amplitude;
        //        amplitude *= persistance;
        //        frequency *= 2;
        //    }
        //    n2 = math.lerp(total / amplitudeSum, n1, n0);
        //    return (n0 + n1 + n2) * 0.33f;
        //}
    }
}
