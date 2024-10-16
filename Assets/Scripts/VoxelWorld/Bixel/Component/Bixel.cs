using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    // TODO 想办法在放置的时候放置体素通知重复位置的乙素,以清除重复乙素,比如设置一个特别的体素存在标记此处存在乙素
    public struct Bixel : IComponentData, IEnableableComponent
    {
        public float TargetTime;
        public float Delay;
    }
    //public struct BixelTimer : IComponentData
    //{
    //    public float TargetTime;
    //}
}
