using System;
using UnityEngine;

namespace CatFramework.CatMath
{
    public static class Noise
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">采样位置</param>
        /// <param name="offset">偏移图的起始采样区域，对于相同种子，偏移值应当是一致</param>
        /// <param name="scale">缩放采样精细度，越大采样区域越小便越精细</param>
        /// <returns></returns>
        public static float GetPerlinNoise(Vector2 position, float offset, float scale)
        {
            return Mathf.PerlinNoise((position.x) / scale + offset, position.y / scale + offset);
        }
        public static float Get3DPerlinNoise(Vector3 position, float offset, float scale)
        {
            float x = (position.x + offset) * scale;
            float y = (position.y + offset) * scale;
            float z = (position.z + offset) * scale;
            float AB = Mathf.PerlinNoise(x, y);
            float BC = Mathf.PerlinNoise(y, z);
            float AC = Mathf.PerlinNoise(x, z);
            float BA = Mathf.PerlinNoise(y, x);
            float CB = Mathf.PerlinNoise(z, y);
            float CA = Mathf.PerlinNoise(z, x);
            return (AB + BC + AC + BA + CB + CA) / 6f;
        }
    }
}
