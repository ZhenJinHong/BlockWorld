using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public struct Timer
    {
        float targetTime;
        public readonly float TargetTime => targetTime;
        public Timer(float delay)
        {
            targetTime = Time.time + delay;
        }
        public void Reset()
        {
            targetTime = Time.time;
        }
        public void Reset(float delay)
        {
            targetTime = Time.time + delay;
        }
        public bool Ready()
        {
            if (targetTime < Time.time)
            {
                targetTime = Time.time;
                return true;
            }
            return false;
        }
        public bool Ready(float delay)
        {
            if (targetTime < Time.time)
            {
                targetTime = Time.time + delay;
                return true;
            }
            return false;
        }
        // 注意百分比不能用用作是否冷却完毕的依据// 已经判断了的targetTime - Time.time永远都是正数
        public readonly float InversePercent(float coolingDelay)
        {
            return targetTime <= Time.time ? 0f : ((targetTime - Time.time) / Math.Max(coolingDelay, 0.000001f));
        }
        /// <summary>
        /// 冷却完毕时为1f
        /// </summary>
        public readonly float Percent(float coolingDelay)
        {
            return targetTime <= Time.time ? 1f : 1f - Math.Min((targetTime - Time.time) / Math.Max(coolingDelay, 0.000001f), 1f);// 使用min,取最大是1f被1f减
        }
        public void AppendDelay(float v)
        {
            targetTime += v;
        }
    }
}
