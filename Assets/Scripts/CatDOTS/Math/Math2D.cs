using CatFramework.CatMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace CatDOTS
{
    public static class Math2D
    {
        public static bool GetPointInPlane(Ray ray, out float3 point)
        {
            if (new Plane(new float3(0f, 1f, 0f), 0f).Raycast(ray, out float enter))
            {
                point = ray.GetPoint(enter);
                return true;
            }
            point = default;
            return false;
        }
        public static float3 GetPointInPlane(Ray ray)
        {
            return new Plane(new float3(0f, 1f, 0f), 0f).Raycast(ray, out float enter) ? (float3)ray.GetPoint(enter) : float3.zero;
        }
        public static float3 PointAlignGrid(float3 point, float height = 0.5f)
        {
            //point = math.floor(point);
            //point.y = height;
            //return point + new float3(0.5f, 0f, 0.5f);
            return (int3)point + new float3(0.5f, height, 0.5f);// 使用floor需要注意不能floor到-1
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 RandomDirection(uint seed)
        {
            float2 newdir = new Random(seed).NextFloat2Direction();
            return new float3(newdir.x, 0f, newdir.y);
        }
    }
}
