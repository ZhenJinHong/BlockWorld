using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    //public class TerrainRandom
    //{
    //    Unity.Mathematics.Random random;
    //    public TerrainRandom(uint seed)
    //    {
    //        random = new Unity.Mathematics.Random(seed);
    //    }
    //    public void NextFloat(float min, float max) => random.NextFloat(min, max);
    //}
    //public static class TerrainUtility
    //{
    //    public static ITerrainGenerator[] GetTerrainGenerators(ITerrainDefinition[] terrainDefinitions, uint seed)
    //    {
    //        ITerrainGenerator[] terrainGenerators = new ITerrainGenerator[terrainDefinitions.Length];
    //        for (int i = 0; i < terrainDefinitions.Length; i++)
    //        {
    //            terrainGenerators[i] = terrainDefinitions[i].GetTerrainGenerator(seed);
    //        }
    //        return terrainGenerators;
    //    }
    //}
    public static class TerrainMaths
    {
        ///// <summary>
        ///// p 为0~1 越靠近一,越接近边界
        ///// </summary>
        //public static bool Range(float bottom, float rangeWidth, float rangeValue, out float p)
        //{
        //    bool inRange = bottom + rangeWidth > rangeValue && bottom <= rangeValue;

        //    // (limit - rangeValue) / rangeWidth 求得当前rangevalue占分段的比例,0~1;
        //    // * 2f - 1f 用以翻倍,并将中间值算为0f,
        //    // 然后abs 对折,则越靠近边界端值1f或-1f的,应越接近rangevalue,越靠近中的越近自身高度
        //    p = inRange ? math.abs((bottom + rangeWidth - rangeValue) / rangeWidth * 2f - 1f) : default;
        //    return inRange;
        //}
        /// <summary>
        /// p 为0~1 越靠近一,越接近边界
        /// </summary>
        public static bool Range(float2 range, float rangeValue, out float p)
        {
            bool inRange = range.y > rangeValue && range.x <= rangeValue;

            // (limit - rangeValue) / rangeWidth 求得当前rangevalue占分段的比例,0~1;
            // * 2f - 1f 用以翻倍,并将中间值算为0f,
            // 然后abs 对折,则越靠近边界端值1f或-1f的,应越接近rangevalue,越靠近中的越近自身高度
            if (inRange)
            {
                // (limit - rangeValue) / rangeWidth 求得当前rangevalue占分段的比例,0~1;
                // * 2f - 1f 用以翻倍,并将中间值算为0f,
                // 然后abs 对折,则越靠近边界端值1f或-1f的,应越接近rangevalue,越靠近中的越近自身高度
                p = RangePercent(range, rangeValue);
            }
            else
            {
                // (limit - rangeValue) / rangeWidth 求得当前rangevalue占分段的比例,0~1;
                // * 2f - 1f 用以翻倍,并将中间值算为0f,
                // 然后abs 对折,则越靠近边界端值1f或-1f的,应越接近rangevalue,越靠近中的越近自身高度
                p = default;
            }
            //p = inRange ? math.pow((math.abs((range.y - rangeValue) / (range.y - range.x) * 2f - 1f) * -1f + 1f), 3f) : default;
            return inRange;
        }
        ///// <summary>
        ///// p 为0~1 越靠近一,越接近边界
        ///// </summary>
        //public static bool Range(float2 range, float rangeValue, float transition, out float p)
        //{
        //    bool inRange = range.y > rangeValue && range.x <= rangeValue;

        //    // (limit - rangeValue) / rangeWidth 求得当前rangevalue占分段的比例,0~1;
        //    // * 2f - 1f 用以翻倍,并将中间值算为0f,
        //    // 然后abs 对折,则越靠近边界端值1f或-1f的,应越接近rangevalue,越靠近中的越近自身高度
        //    p = inRange ? math.clamp(math.abs((range.y - rangeValue) / (range.y - range.x) * -2f + 1f) * 10f, 0f, 1f) : default;
        //    return inRange;
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RangePercent(float2 range, float rangeValue)
        {
            float v = math.abs((range.y - rangeValue) / (range.y - range.x) * 2f - 1f);
            return v * v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float noise, float rangeValue, float p)
        {
            return math.lerp(noise + rangeValue, rangeValue, p);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Height(WorldTopography worldTopography, float noise, float terrainHeight, float rangeValue, float p)
        {
            float rangeHeight = rangeValue * worldTopography.TopographicHeightDiff;
            //return ClampHeight(math.lerp((noise + rangeValue) * terrainHeight, rangeValue * worldTopography.TopographicHeightDiff, p) + worldTopography.BaseHeight);
            return ClampHeight(math.lerp(noise * terrainHeight + rangeHeight, rangeHeight, p) + worldTopography.BaseHeight);
        }
        /// <summary>
        /// 最终高度2f~234f
        /// </summary>
        public static byte ClampHeight(float height)
        {
            return (byte)math.clamp(height, 2f, 234f);// 避免除0出现
        }
        /// <param name="persistance">(1f, 1.3f)</param>
        /// <param name="frequency">(0.04f, 0.06f)</param>
        /// <returns></returns>
        public static float OctavePerlin(float2 position, float persistance, float frequency)
        {
            float total = 0f;
            float amplitude = 1f;
            float amplitudeSum = 0;
            for (int i = 0; i < 3; i++)
            {
                total += noise.cnoise(position * frequency) * amplitude;
                amplitudeSum += amplitude;
                amplitude *= persistance;
                frequency *= 0.5f;
            }
            return total / amplitudeSum;
        }
    }
}
