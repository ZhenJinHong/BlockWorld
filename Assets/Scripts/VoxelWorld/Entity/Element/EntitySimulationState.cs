using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    [Flags]
    public enum EntitySimulationState
    {
        Unknown = 0,
        /// <summary>
        /// 处于地面
        /// </summary>
        OnGround = 1 << 0,
        /// <summary>
        /// 坠落中
        /// </summary>
        Falling = 1 << 1,
    }
}
