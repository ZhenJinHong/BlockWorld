using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CatFramework.CatMath
{
    public static partial class MathC
    {
        public static Vector2 As2D(this Vector3 v)
            => new Vector2(v.x, v.z);
        public static Vector2 Abs(Vector2 v)
            => new Vector2(System.Math.Abs(v.x), System.Math.Abs(v.y));
        public static Vector3 Abs(Vector3 v)
            => new Vector3(System.Math.Abs(v.x), System.Math.Abs(v.y), System.Math.Abs(v.z));


        public static Vector2Int As2D(this Vector3Int v)
            => new Vector2Int(v.x, v.z);
        public static Vector2Int Abs(Vector2Int v)
            => new Vector2Int(System.Math.Abs(v.x), System.Math.Abs(v.y));
        public static Vector3Int Abs(Vector3Int v)
            => new Vector3Int(System.Math.Abs(v.x), System.Math.Abs(v.y), System.Math.Abs(v.z));

        public static Vector3 Floor(Vector3 v)
            => new Vector3((float)Math.Floor(v.x), (float)Math.Floor(v.y), (float)Math.Floor(v.z));


        public static bool AllEquals(Vector2 v1, float v2)
            => v1.x == v2 && v1.y == v2;
        public static bool AnyEquals(Vector2 v1, float v2)
           => v1.x == v2 || v1.y == v2;
        public static bool AllGreater(Vector2 v1, float v2)
           => v1.x > v2 && v1.y > v2;
        public static bool AnyGreater(Vector2 v1, float v2)
            => v1.x > v2 || v1.y > v2;
        public static bool AllGreater(Vector3 v1, float v2)
            => v1.x > v2 && v1.y > v2 && v1.z > v2;
        public static bool AnyGreater(Vector3 v1, float v2)
            => v1.x > v2 || v1.y > v2 || v1.z > v2;


        public static bool AllEquals(Vector2Int v1, int v2)
            => v1.x == v2 && v1.y == v2;
        public static bool AnyEquals(Vector2Int v1, int v2)
            => v1.x == v2 || v1.y == v2;
        public static bool AllGreater(Vector2Int v1, int v2)
            => v1.x > v2 && v1.y > v2;
        public static bool AnyGreater(Vector2Int v1, int v2)
            => v1.x > v2 || v1.y > v2;
        public static bool AllGreater(Vector3Int v1, int v2)
            => v1.x > v2 && v1.y > v2 && v1.z > v2;
        public static bool AnyGreater(Vector3Int v1, int v2)
            => v1.x > v2 || v1.y > v2 || v1.z > v2;

        public static Vector2 Clamp(Vector2 v1, Vector2 min, Vector2 max)
            => new Vector2(Clamp(v1.x, min.x, max.x), Clamp(v1.y, min.y, max.y));
        public static Vector3 Clamp(Vector3 v1, Vector3 min, Vector3 max)
            => new Vector3(Clamp(v1.x, min.x, max.x), Clamp(v1.y, min.y, max.y), Clamp(v1.z, min.z, max.z));

        /// <summary>
        /// 检查索引是否在长度之内
        /// </summary>
        public static bool IndexIsInRange(Vector3Int v, int length)
            => (v.x > -1 && v.y > -1 && v.z > -1 && v.x < length && v.y < length && v.z < length);


        public static Vector3 GetBezierPointer(float t, Vector3 start, Vector3 center, Vector3 end)
            => (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
        public static void GatBezier(Vector3[] container, Vector3 startPoint, float height, Vector3 endPoint)
        {
            Vector3 controlPoint = (startPoint + endPoint) * 0.5f;
            controlPoint.y += height;
            for (int i = 0; i < container.Length; i++)
            {
                float t = (i + 0.999f) / container.Length;
                container[i] = GetBezierPointer(t, startPoint, controlPoint, endPoint);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 V3AddV2(Vector3 v3, Vector2 v2)
        {
            v3.x += v2.x;
            v3.z += v2.y;
            return v3;
        }
    }
}
