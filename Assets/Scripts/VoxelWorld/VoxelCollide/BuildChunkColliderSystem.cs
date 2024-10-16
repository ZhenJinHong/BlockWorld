using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    [UpdateInGroup(typeof(InitializedSystemMiaoGroup))]
    [UpdateAfter(typeof(VoxelWorldChunkSystem))]
    [BurstCompile]
    public partial struct BuildChunkColliderSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}
