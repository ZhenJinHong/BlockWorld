using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(BixelUpdateSystemGroup))]
    [BurstCompile]
    public partial struct BixelLinearMoveSystem : ISystem
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
    public partial struct BixelLinearMoveJob : IJobEntity
    {
        public VoxelWorldMap.ReadOnly VoxelWorldMap;
        public void Execute(in Bixel bixel, ref LocalTransform transform)
        {
        }
    }
}
