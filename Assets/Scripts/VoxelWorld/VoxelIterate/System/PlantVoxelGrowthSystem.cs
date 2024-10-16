using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(AlterVoxelSystemGroup))]
    [BurstCompile]
    public partial struct VoxelBehaviorSystem : ISystem
    {
        EntityQuery voxelWorldQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAll<VoxelWorldTag, SmallChunkVariationList>();

            voxelWorldQuery = builder.Build(ref state);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            SmallChunkVariationList smallChunkVariationList = voxelWorldQuery.GetSingleton<SmallChunkVariationList>();
            if (smallChunkVariationList.Has)
            {
                var enumerator = smallChunkVariationList.Enumerator();
                while (enumerator.MoveNext())
                {

                }
                enumerator.Dispose();
            }
        }
    }
    [BurstCompile]
    public partial struct VoxelBehaviorJob : IJob
    {
        public VoxelWorldMap VoxelWorldMap;
        public int3 SmallChunkIndex;
        public void Execute()
        {

        }
    }
    [DisableAutoCreation]
    [UpdateInGroup(typeof(AlterVoxelSystemGroup))]
    [BurstCompile]
    public partial struct PlantVoxelGrowthSystem : ISystem
    {
        EntityQuery voxelWorldQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAll<VoxelWorldTag, SmallChunkVariationList>();

            voxelWorldQuery = builder.Build(ref state);
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
    [DisableAutoCreation]
    [BurstCompile]
    public partial struct VoxelStatusJumpSystem : ISystem
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
    public struct VoxelStatusJumpJob : IJob
    {
        public void Execute()
        {
        }
    }
}
