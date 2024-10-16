using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.CatMath
{
    public static partial class MathC
    {
        /// <summary>
        /// 不包括最大值
        /// </summary>
        public static int NextInt(int min, int max) => random.Next(min, max);
        public static long NextLong() => random.Next() * (long)random.Next();
        public static int NextInt() => random.Next();
        public static uint NextUInt()
        {
            int u = random.Next();
            return (u < 0) ? (uint)-u : (uint)u;
        }
        public static uint NextUInt(int min, int max)
        {
            int u = random.Next(min, max);
            return (u < 0) ? (uint)-u : (uint)u;
        }
    }
}
