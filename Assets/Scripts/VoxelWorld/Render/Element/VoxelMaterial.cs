using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    [Flags]
    public enum VoxelMaterial : byte
    {
        None = 0,
        Solid = 1 << 0,
        /// <summary>
        /// 弹性
        /// </summary>
        Elastic = 1 << 1,
        Water = 1 << 2,
        /// <summary>
        /// 粘稠
        /// </summary>
        Viscous = 1 << 3,
        // 火焰
        Blaze = 1 << 4,
    }
}
