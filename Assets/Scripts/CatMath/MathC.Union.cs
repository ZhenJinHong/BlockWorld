using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatFramework.CatMath
{
    // 负数int转Uint是位形式转换，高于int的Uint同样，因此同位数的强制转换应当特别注意
    public static partial class MathC
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct IntFloatUnion
        {
            [FieldOffset(0)]
            public int intValue;
            [FieldOffset(0)]
            public float floatValue;
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct LongDoubleUnion
        {
            [FieldOffset(0)]
            public long longValue;
            [FieldOffset(0)]
            public double doubleValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Asint(float x)
        {
            IntFloatUnion u;
            u.intValue = 0;
            u.floatValue = x;
            return u.intValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Asuint(float x) { return (uint)Asint(x); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asfloat(int x)
        {
            IntFloatUnion u;
            u.floatValue = 0;
            u.intValue = x;

            return u.floatValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Aslong(double x)
        {
            LongDoubleUnion u;
            u.longValue = 0;
            u.doubleValue = x;
            return u.longValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Asdouble(long x)
        {
            LongDoubleUnion u;
            u.doubleValue = 0;
            u.longValue = x;
            return u.doubleValue;
        }

        public static float FoldToFloat(ulong x)
        {
            return (uint)(x >> 32) ^ (uint)x;
        }
        public static float FoldToFloat(long x)
        {
            return (uint)(x >> 32) ^ (uint)x;
        }
        public static uint FoldToUint(long x)
        {
            return (uint)(x >> 32) ^ (uint)x;
        }
        //public static float FoldToRange(ulong x, float min, float max)
        //{
        //    float v = FoldToFloat(x);
        //    v %= max;
        //    return v;
        //}
    }
}
