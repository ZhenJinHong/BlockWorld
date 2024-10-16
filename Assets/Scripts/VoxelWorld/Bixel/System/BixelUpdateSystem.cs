using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    [UpdateInGroup(typeof(BixelUpdateSystemGroup), OrderFirst = true)]
    [BurstCompile]
    public partial struct BixelUpdateSystem : ISystem
    {
        ComponentTypeHandle<Bixel> bixelTypeHandle;
        EntityQuery bixelQueryRW;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            bixelTypeHandle = state.GetComponentTypeHandle<Bixel>(false);

            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAllRW<Bixel>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            bixelQueryRW = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<VoxelWorldTag>();
            var voxelWorldQuery = builder.Build(ref state);

            state.RequireForUpdate(bixelQueryRW);
            state.RequireForUpdate(voxelWorldQuery);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            bixelTypeHandle.Update(ref state);
            state.Dependency = new BixelUpdateJob()
            {
                bixelTypeHandle = bixelTypeHandle,
                CurTime = (float)SystemAPI.Time.ElapsedTime,
            }.ScheduleParallel(bixelQueryRW, state.Dependency);
        }
    }
    [BurstCompile]
    public struct BixelUpdateJob : IJobChunk
    {
        public ComponentTypeHandle<Bixel> bixelTypeHandle;
        public float CurTime;
        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            System.Span<Bixel> bixels = chunk.GetNativeArray<Bixel>(ref bixelTypeHandle).AsSpan();
            for (int i = 0; i < bixels.Length; i++)
            {
                chunk.SetComponentEnabled(ref bixelTypeHandle, i, BixelFunction.BixelIsReady(ref bixels[i], in CurTime));
            }
        }
    }
}
