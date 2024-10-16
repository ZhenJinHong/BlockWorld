using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;
using CatFramework_TestDOTS.Assets.Scripts.CatFramework_TestDOTS.Test;
using Unity.Mathematics;

namespace CatFramework_TestDOTS
{
    [DisableAutoCreation]
    [BurstCompile]
    internal partial struct TestSpawnSystem : ISystem
    {
        EntityQuery voxelQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAll<VoxelTestTag>();
            voxelQuery = builder.Build(ref state);

            state.RequireForUpdate(voxelQuery);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            VoxelTestTag voxelTestTag = state.EntityManager.GetComponentData<VoxelTestTag>(voxelQuery.GetSingletonEntity());
            int width = voxelTestTag.width;
            NativeArray<Entity> entities = new NativeArray<Entity>(width * width * width, Allocator.Temp);
            state.EntityManager.Instantiate(voxelTestTag.Entity, entities);
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
            int x = 0, y = 0, z = 0;
            for (int i = 0; i < entities.Length; i++)
            {
                ECB.SetComponent<LocalTransform>(entities[i], new LocalTransform()
                {
                    Position = new Unity.Mathematics.float3(x, y, z),
                    Rotation = quaternion.identity,
                    Scale = 1f,
                });
                x++;
                if (x == width)
                {
                    x = 0;
                    z++;
                    if (z == width)
                    {
                        z = 0;
                        y++;
                    }
                }
            }
            ECB.DestroyEntity(voxelQuery, EntityQueryCaptureMode.AtPlayback);
            ECB.Playback(state.EntityManager);
            ECB.Dispose();
            state.Enabled = false;
        }
    }
}
