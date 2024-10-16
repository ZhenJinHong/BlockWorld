using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld
{
    //[DisableAutoCreation]
    [BurstCompile]
    public partial struct BixelSaveLoadSystem : ISystem, ISystemStartStop
    {
        EntityQuery voxelWorldQuery;
        EntityQuery bixelQuery;
        EntityQuery bixelVoxelReplaceQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAll<VoxelWorldTag>();
            voxelWorldQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<Bixel>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            bixelQuery = builder.Build(ref state);

            // 还缺卸载标记,或者其他方式卸载
            builder.Reset();
            builder.WithAll<LocalTransform, VoxelReplace, Bixel>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            bixelVoxelReplaceQuery = builder.Build(ref state);

            state.RequireForUpdate(voxelWorldQuery);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
            state.EntityManager.DestroyEntity(bixelQuery);
        }
        // 就算按照数组整组设置组件数据,其内部实际上还是按照一个个实体lookup设置的
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //bixelVoxelReplaceQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            //bixelVoxelReplaceQuery.ToComponentDataArray<BixelReplace>(Allocator.Temp);
            //bixelVoxelReplaceQuery.ToComponentDataArray<Bixel>(Allocator.Temp);
        }
    }
}
