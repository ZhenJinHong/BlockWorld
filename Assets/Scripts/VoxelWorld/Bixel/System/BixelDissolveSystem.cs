using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(BixelUpdateSystemGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct BixelDissolveSystem : ISystem
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
    [BurstCompile]
    public struct BixelDissolveJob : IJobChunk
    {
        [ReadOnly] public VoxelWorldMap.ReadOnly VoxelWorldMap;
        public ComponentTypeHandle<Bixel> bixelTypeHandle;
        public float CurTime;
        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            System.Span<Bixel> bixels = chunk.GetNativeArray<Bixel>(ref bixelTypeHandle).AsSpan();
            for (int i = 0; i < bixels.Length; i++)
            {
                //chunk.SetComponentEnabled(ref bixelTypeHandle, i, BixelFunction.BixelIsReady(ref bixels[i], in CurTime));
            }
        }
    }
}
