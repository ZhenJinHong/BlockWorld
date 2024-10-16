using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    public enum VoxelRenderType : byte
    {
        OpaqueBlock = 0,
        TransparentBlock = 1,
        Grass = 2,
        Other = 255,
    }
}
