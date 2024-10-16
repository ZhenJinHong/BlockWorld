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
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld
{
    // 首先要查到所有实体的位置
    // 然后在所有的实体位置，查询体素图，放置碰撞体
    // 关键在于怎么回收碰撞体，和设置碰撞体的位置
    [UpdateInGroup(typeof(FixedUpdateSystemMiaoGroup))]// 在固定帧原因，物理体的运动更新在固定帧，体素的碰撞体必须同步多次运算
    //[UpdateAfter(typeof(BuildBeingsTreeSystem))]
    [BurstCompile]
    public partial struct VoxelColliderSystem : ISystem, ISystemStartStop
    {
        EntityQuery voxelColliderPrefabQuery;
        EntityQuery voxelColliderQuery;
        EntityQuery voxelWorldQuery;
        EntityQuery playerQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAll<VoxelCollider, Prefab>();
            voxelColliderPrefabQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAllRW<VoxelCollider, PhysicsCollider>()
                .WithAllRW<LocalTransform>();
            voxelColliderQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<VoxelWorldTag, VoxelShapeColliderAsset, VoxelWorldMap>();
            voxelWorldQuery = builder.Build(ref state);

            builder.Reset();
            builder.WithAll<FirstPersonPlayer, BeingsData, LocalTransform>();
            playerQuery = builder.Build(ref state);

            state.RequireForUpdate(voxelColliderPrefabQuery);
            state.RequireForUpdate(voxelWorldQuery);
            state.RequireForUpdate(playerQuery);
        }
        Entity cubeVoxelColliderPrefab;
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            if (lastAlreadyPosSet.IsCreated)
                lastAlreadyPosSet.Dispose();
            if (alreadyPosSet.IsCreated)
                alreadyPosSet.Dispose();
            if (newPosList.IsCreated)
                newPosList.Dispose();
        }
        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            cubeVoxelColliderPrefab = voxelColliderPrefabQuery.GetSingletonEntity();
            if (!lastAlreadyPosSet.IsCreated)
                lastAlreadyPosSet = new NativeHashSet<int4>(512, Allocator.Persistent);
            if (!alreadyPosSet.IsCreated)
                alreadyPosSet = new NativeHashSet<int4>(512, Allocator.Persistent);
            if (!newPosList.IsCreated)
                newPosList = new NativeList<int4>(512, Allocator.Persistent);
        }
        NativeHashSet<int4> lastAlreadyPosSet;
        NativeHashSet<int4> alreadyPosSet;
        NativeList<int4> newPosList;
        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
            // 世界停止，清理// 当实际应该不用，对于上次世界就存在目标位置的碰撞体，依旧可以存在目标位置而不更改
            lastAlreadyPosSet.Clear();
            alreadyPosSet.Clear();
            newPosList.Clear();
        }
        // TODO 对于原本就有的位置，不要重新设置
        // 过快的速度会导致穿过体素（空中的情况，过快会导致下方此时还没有布置碰撞体）或许需要预测
        // 对于很多实体的情况，先计算全部实体周围的空间，再把这里除重的空间去体素图查是否存在体素
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            newPosList.Clear();
            // 获取所有生物位置
            // 保留上帧布置的
            int alreadColliderCount = voxelColliderQuery.CalculateEntityCount();

            Entity voxelWorldEntity = voxelWorldQuery.GetSingletonEntity();
            VoxelWorldMap chunkMap = state.EntityManager.GetComponentData<VoxelWorldMap>(voxelWorldEntity);
            VoxelShapeColliderAsset voxelShapeColliderAsset = state.EntityManager.GetComponentData<VoxelShapeColliderAsset>(voxelWorldEntity);
            // 交换为上帧已存在
            NativeHashSet<int4> temp = lastAlreadyPosSet;
            lastAlreadyPosSet = alreadyPosSet;
            alreadyPosSet = temp;
            alreadyPosSet.Clear();

            JobHandle buildNeedCollierPosJobHandle = new BuildNeedCollierPosJob()
            {
                ChunkMap = chunkMap.AsReadOnly(),
                LastAlreadyPosSet = lastAlreadyPosSet,
                AlreadyPosSet = alreadyPosSet,
                NewPosList = newPosList,
            }.Schedule(playerQuery, state.Dependency);

            buildNeedCollierPosJobHandle.Complete();

            int needColliderCount = newPosList.Length + alreadyPosSet.Count;
#if UNITY_EDITOR
            if (needColliderCount > Settings.VoxelColliderLimit)
            {
                UnityEngine.Debug.LogWarning($"体素碰撞体非常多，达到了{needColliderCount}");
            }
#endif
            if (needColliderCount > Settings.VoxelColliderLimit)
            {
                needColliderCount = Settings.VoxelColliderLimit;
            }
            // 已有的碰撞体少于需要的么？
            if (alreadColliderCount < needColliderCount)
            {
                // 需要立刻实例化的
                //NativeArray<Entity> newColliders = 
                state.EntityManager.Instantiate(cubeVoxelColliderPrefab, needColliderCount - alreadColliderCount, Allocator.Temp);
            }
            state.Dependency = new SetColliderJob()
            {
                Colliders = voxelShapeColliderAsset.SolidColliderAsset,
                NonSolidCube = voxelShapeColliderAsset.NonSolidCube,
                AlreadyPosSet = alreadyPosSet,
                NewPosList = newPosList,
            }.Schedule(voxelColliderQuery, state.Dependency);
        }
    }
    [BurstCompile]
    partial struct BuildNeedCollierPosJob : IJobEntity
    {
        // 只给玩家使用
        [ReadOnly] public VoxelWorldMap.ReadOnly ChunkMap;
        public NativeHashSet<int4> LastAlreadyPosSet;
        public NativeHashSet<int4> AlreadyPosSet;
        public NativeList<int4> NewPosList;
        public static int CombineData(ushort shapeIndex, byte shapeDirection, bool solid)
        {
            return (shapeIndex << 16) | (shapeDirection << 8) | (solid ? 1 : 0);
        }
        public static void SplitData(int data, out ushort shapeIndex, out byte shapeDirection, out bool solid)
        {
            shapeIndex = (ushort)(data >> 16);
            shapeDirection = (byte)((data >> 8) & byte.MaxValue);
            solid = (data & 1) == 1;
        }
        //public NativeList<int> ColliderInfo;
        public void Execute(in LocalTransform localTransform)
        {
            int3 beingsIndex = new int3(math.floor(localTransform.Position));
            int half = 5;
            int lengthsq = half * half;
            //int3 forward = new int3(math.ceil(2f * localTransform.Forward()));
            int3 max = half/* + forward*/;
            int3 min = -half/* + forward*/;
            int range = half * 2;
            int length = range * range * range;

            int x = min.x, y = min.y, z = min.z;
            for (int i = 0; i < length; i++)
            {
                int3 colliderPos = new int3(beingsIndex.x + x, beingsIndex.y + y, beingsIndex.z + z);
                if (math.lengthsq(new float3(x, y, z)) < lengthsq)
                {
                    Voxel voxel = ChunkMap.GetVoxelOrEmpty(colliderPos);
                    if (Voxel.NonAir(voxel.VoxelTypeIndex)/* || Voxel.Water(voxel.VoxelMaterial)*/)
                    //&& 
                    //((voxelType.Solid && !voxelType.FullSolidCube)
                    //|| (voxelType.FullSolidCube && ChunkMap.CheckVoxelExposed(in colliderPos, ref voxelTypes))))
                    {
                        int4 data = new int4(colliderPos, CombineData(voxel.ShapeIndex, voxel.ShapeDirection, Voxel.Solid(voxel.VoxelMaterial)));
                        if (LastAlreadyPosSet.Contains(data))// 需要新生成的位置，上帧已存在
                        {
                            AlreadyPosSet.Add(data);// 则放入已存在集合，告知不用再修改
                        }
                        else
                        {
                            NewPosList.Add(data);// 否则加入需要的新位置列表
                        }
                    }
                }
                z++;
                if (z == max.z)
                {
                    z = min.z;
                    x++;
                    if (x == max.x)
                    {
                        x = min.x;
                        y++;
                    }
                }
            }
        }
    }
    [BurstCompile]
    partial struct SetColliderJob : IJobEntity
    {
        [ReadOnly] public NativeArray<BlobAssetReference<Collider>>.ReadOnly Colliders;
        [ReadOnly] public BlobAssetReference<Collider> NonSolidCube;
        public NativeHashSet<int4> AlreadyPosSet;
        public NativeList<int4> NewPosList;

        int xIndex;// 移动至备用区域的
        int yIndex;
        public void Execute(ref VoxelCollider voxelCollider, ref PhysicsCollider physicsCollider, ref LocalTransform localTransform)
        {
            int3 pos = new int3(math.floor(localTransform.Position));
            int4 data = new int4(pos, BuildNeedCollierPosJob.CombineData(voxelCollider.ShapeIndex, voxelCollider.ShapeDirection, voxelCollider.solid));
            if (NewPosList.Length == 0 && (!AlreadyPosSet.Contains(data)))// 如果已经不需要了，并且不是已经放置在目标位置的
            {
                // 则转移到其它位置
                localTransform.Position = new float3(xIndex, 1000f, yIndex);
                physicsCollider.Value = BlobAssetReference<Collider>.Null;
                xIndex++;
                if (xIndex > 256)
                {
                    xIndex = 0;
                    yIndex++;
                }
            }
            else if (!AlreadyPosSet.Contains(data))// 先检查这个碰撞体是否已经放置在目标位置
            {
                int4 dataT = NewPosList[^1];
                BuildNeedCollierPosJob.SplitData(dataT.w, out ushort shapeIndex, out byte shapeDirection, out bool solid);
                voxelCollider.ShapeIndex = shapeIndex;
                voxelCollider.ShapeDirection = shapeDirection;
                voxelCollider.solid = solid;
                float3 newPos = dataT.xyz;// 如果不存在，就从新位置列表拿取一个
                NewPosList.RemoveAt(NewPosList.Length - 1);// 并移除后放入已存在位置集合里
                physicsCollider.Value = solid ? Colliders[shapeIndex] : NonSolidCube;
                AlreadyPosSet.Add(dataT);
                localTransform.Position = newPos + 0.5f;
                localTransform.Rotation = VoxelShapeHeader.GetRotation(shapeDirection);
            }
        }
    }
}
