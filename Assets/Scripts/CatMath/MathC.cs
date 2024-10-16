using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace CatFramework.CatMath
{
    public static partial class MathC
    {
        readonly static Regex regex;
        readonly static string empty = "";
        readonly static string numChezck = @"\d+";

        readonly static System.Random random;
        static MathC()
        {
            regex = new Regex(@"[\\/:*?""<>|]");
            random = new System.Random();
        }
        /// <summary>
        /// 检查到不符合的文本，返回true
        /// </summary>
        public static bool CheackTextIsError(string text) => regex.IsMatch(text);
        /// <summary>
        /// 取消不支持的字符
        /// </summary>
        public static string ProcessedText(string text) => regex.Replace(text, empty);
        /// <summary>
        /// "pattern"检测的字符，"replacement"检测到的字符替换目标
        /// </summary>
        /// <param name="text">被检测文本</param>
        /// <param name="pattern">检测的字符</param>
        /// <param name="replacement">检测到的字符替换目标</param>
        /// <returns></returns>
        [Obsolete]
        public static string Replace(string text, string pattern, string replacement) => Regex.Replace(text, pattern, replacement);
        /// <summary>
        /// "pattern"检测的字符，替换为空
        /// </summary>
        /// <param name="text">被检测文本</param>
        /// <param name="pattern">检测的字符</param>
        [Obsolete]
        public static string Replace(string text, string pattern) => Regex.Replace(text, pattern, string.Empty);
        public static bool CheackTextIsNum(string text) => Regex.IsMatch(text, numChezck);

        /// <summary>
        /// next为正负，索引钳制在0到count-1
        /// </summary>
        public static int IndexLoopIClamp(int index, int next, int count)
        {
            index += next;
            if (count == 0)
                throw new Exception("数量不可为0");
            if (index >= count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = count - 1;
            }
            return index;
        }
        /// <summary>
        /// 将已增减过的索引循环在索引范围内
        /// </summary>
        public static int IndexLoopIClamp(int index, int count)
        {
            if (count == 0)
                throw new Exception("数量不可为0");
            if (index >= count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = count - 1;
            }
            return index;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
            => value < min ? min : (value > max ? max : value);
            //=> Math.Clamp(value, min, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Clamp(uint value, uint min, uint max)
           => value < min ? min : (value > max ? max : value);
           //=> Math.Clamp(value, min, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
            => value < min ? min : (value > max ? max : value);
            //=> Math.Clamp(value, min, max);
        /// <summary>
        /// 角度循环在360度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LoopAngleIn360(float angle)
        {
            return angle < -360f ? angle + 360f : (angle > 360f ? angle - 360f : angle);
        }
        /// <summary>
        /// 通过与180比大小将角度循环在360度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LoopAngleIn360By180(float angle)
        {
            return angle < -180 ? angle + 360f : (angle > 180f ? angle - 360f : angle);
        }
    }
}
