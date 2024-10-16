using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelWorldObserver : IComponentData
    {
        public quaternion AngleOriented;
        public int2 WorldCenterChunkIndex;
    }
}
