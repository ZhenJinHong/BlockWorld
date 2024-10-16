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
using Unity.Transforms;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [UpdateInGroup(typeof(AlterVoxelSystemGroup))]
    [BurstCompile]
    public partial struct VoxelReplaceSystem : ISystem
    {
        EntityQuery voxelWorldQuery;
        EntityQuery voxelReplaceQuery;
        EntityQuery voxelSwapQuery;
        NativeArray<float3> cubefacePoint;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAllRW<VoxelWorldMap>();
            voxelWorldQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAllRW<VoxelReplace>().WithAll<Bixel, LocalTransform>();
            voxelReplaceQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAllRW<VoxelSwap>().WithAll<Bixel, LocalTransform>();
            voxelSwapQuery = builder.Build(ref state);

            state.RequireForUpdate(voxelWorldQuery);
            //state.RequireForUpdate(voxelReplaceQuery);

            cubefacePoint = new NativeArray<float3>(6, Allocator.Persistent, NativeArrayOptions.UninitializedMemory)
            {
                [0] = new float3(0f, 0f, 1f),
                [1] = new float3(0f, 0f, -1f),
                [2] = new float3(0f, 1f, 0f),
                [3] = new float3(0f, -1f, 0f),
                [4] = new float3(1f, 0f, 0f),
                [5] = new float3(-1f, 0f, 0f),
            };
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            cubefacePoint.Dispose();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            VoxelWorldMap voxelWorldMap = voxelWorldQuery.GetSingleton<VoxelWorldMap>();
            JobHandle handle = new BixelVoxelReplaceJob()
            {
                VoxelWorldMap = voxelWorldMap,
            }.Schedule(voxelReplaceQuery, state.Dependency);
            handle = new BixelVoxelSwapJob()
            {
                CubeFacePoint = cubefacePoint,
                VoxelWorldMap = voxelWorldMap,
            }.Schedule(voxelSwapQuery, handle);
            state.Dependency = handle;
        }
    }
    [BurstCompile]
    public partial struct BixelVoxelReplaceJob : IJobEntity
    {
        public VoxelWorldMap VoxelWorldMap;
        public void Execute(ref VoxelReplace bixelVoxelReplace, in LocalTransform transform)
        {
            bixelVoxelReplace.Hold = VoxelWorldMap.TryReplaceVoxel(in bixelVoxelReplace.Hold, in transform.Position);
        }
    }
    [BurstCompile]
    public partial struct BixelVoxelSwapJob : IJobEntity
    {
        [ReadOnly] public NativeArray<float3> CubeFacePoint;
        public VoxelWorldMap VoxelWorldMap;
        public void Execute(ref VoxelSwap voxelCheck, in LocalTransform transform)
        {
#if UNITY_EDITOR
            if (voxelCheck.Point1 == voxelCheck.Point2)
            {
                Debug.LogWarning("检查点重叠");
            }
#endif
            VoxelMath.PositionToVoxelIndexInWorldAndBigChunkIndex(transform.Position + CubeFacePoint[(int)voxelCheck.Point1], out int3 voxelIndexInWorldPoint1, out int3 bigChunkIndexPoint1);
            if (VoxelWorldMap.TryGetSliceIndexByBigChunkIndex(in bigChunkIndexPoint1, out int sliceIndexPoint1))
            {
                int voxelIndexInTotalArrayPoint1 = VoxelWorldMap.ConvertedVoxelIndexInTotalArray(voxelIndexInWorldPoint1, sliceIndexPoint1);
                Voxel point1 = VoxelWorldMap[voxelIndexInTotalArrayPoint1];
                if ((voxelCheck.CheckMask & point1) != (voxelCheck.Last & voxelCheck.CheckMask))// 首先确定变化了
                {
                    voxelCheck.Last = point1;
                    VoxelMath.PositionToVoxelIndexInWorldAndBigChunkIndex(transform.Position + CubeFacePoint[(int)voxelCheck.Point2], out int3 voxelIndexInWorldPoint2, out int3 bigChunkIndexPoint2);
                    if (math.any(bigChunkIndexPoint1 != bigChunkIndexPoint2))// 然后如果不在同一个区块
                    {
                        if (VoxelWorldMap.TryGetSliceIndexByBigChunkIndex(in bigChunkIndexPoint2, out int sliceIndexPoint2))
                        {
                            int voxelIndexInTotalArrayPoint2 = VoxelWorldMap.ConvertedVoxelIndexInTotalArray(voxelIndexInWorldPoint2, sliceIndexPoint2);
                            Voxel point2 = VoxelWorldMap[voxelIndexInTotalArrayPoint2];
                            Voxel.SwapData(ref point1, ref point2, voxelCheck.SwapMask);
                            VoxelWorldMap.SetVoxel(in point1, voxelIndexInTotalArrayPoint1, in voxelIndexInWorldPoint1);
                            VoxelWorldMap.SetVoxel(in point2, voxelIndexInTotalArrayPoint2, in voxelIndexInWorldPoint2);
                        }
                    }
                    else
                    {
                        int voxelIndexInTotalArrayPoint2 = VoxelWorldMap.ConvertedVoxelIndexInTotalArray(voxelIndexInWorldPoint2, sliceIndexPoint1);
                        Voxel point2 = VoxelWorldMap[voxelIndexInTotalArrayPoint2];
                        Voxel.SwapData(ref point1, ref point2, voxelCheck.SwapMask);
                        VoxelWorldMap.SetVoxel(in point1, voxelIndexInTotalArrayPoint1, in voxelIndexInWorldPoint1);
                        VoxelWorldMap.SetVoxel(in point2, voxelIndexInTotalArrayPoint2, in voxelIndexInWorldPoint2);
                    }
                }
            }
        }
    }
}
