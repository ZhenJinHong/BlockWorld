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
using Unity.Transforms;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    //[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true/*OrderFirst = true*/)]
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
    public partial class FinishPlyerInputSystem : SystemBase
    {
        public interface IOutPutReceiver
        {
            /// <summary>
            /// 投射射线的时候需要使用
            /// </summary>
            float3 EyeOffset { get; }
            bool EnableInput { get; }

            float3 Position { set; }
            quaternion CameraRotation { set; }
            quaternion PlayerRotation { set; }

            void FinishInput(NativeReference<VoxelRayResult>.ReadOnly voxelRayResult);
        }
        EntityQuery playerQuery;
        //EntityQuery inGameQuery;
        EntityQuery voxelWorldQuery;
        public IOutPutReceiver outPutReceiver;
        NativeReference<VoxelRayResult> voxelRayResult;
        protected override void OnCreate()
        {
            base.OnCreate();
            EntityQueryBuilder builder = new(Allocator.Temp);
            builder.WithAll<LocalTransform>()
                .WithAllRW<FirstPersonPlayerOutputCache>();
            playerQuery = builder.Build(this);

            //builder.Reset();
            //builder.WithAll<InGame>();
            //inGameQuery = builder.Build(this);

            builder.Reset();
            builder.WithAll<VoxelWorldTag, VoxelWorldMap>();
            voxelWorldQuery = builder.Build(this);

            RequireForUpdate(playerQuery);
            //RequireForUpdate(inGameQuery);
            RequireForUpdate(voxelWorldQuery);
            builder.Dispose();

            voxelRayResult = new NativeReference<VoxelRayResult>(Allocator.Persistent);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            voxelRayResult.Dispose();
        }
        protected override void OnUpdate()
        {
            if (outPutReceiver != null)
            {
                Entity player = playerQuery.GetSingletonEntity();
                LocalTransform localTransform = EntityManager.GetComponentData<LocalTransform>(player);
                outPutReceiver.Position = localTransform.Position;

                if (outPutReceiver.EnableInput)
                {
                    FirstPersonPlayerOutputCache playerOutputCache = EntityManager.GetComponentData<FirstPersonPlayerOutputCache>(player);

                    Entity voxelWorld = voxelWorldQuery.GetSingletonEntity();

                    float3 Start = localTransform.Position + outPutReceiver.EyeOffset;
                    JobHandle putVoxelJobHandle = new FinishInputRayVoxelWorldJob()
                    {
                        VoxelWorldMap = EntityManager.GetComponentData<VoxelWorldMap>(voxelWorld).AsReadOnly(),
                        CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                        Rotation = playerOutputCache.CameraRotation,
                        Start = Start,
                        Result = voxelRayResult,
                    }.Schedule(Dependency);

                    outPutReceiver.CameraRotation = playerOutputCache.CameraRotation;
                    outPutReceiver.PlayerRotation = playerOutputCache.PlayerRotation;

                    putVoxelJobHandle.Complete();
                    outPutReceiver.FinishInput(voxelRayResult.AsReadOnly());
                }
            }
        }
    }
    public class VoxelRayResultManaged : IVoxelRayResult
    {
        public int3 TargetIndex { get; private set; }
        public int3 TargetFaceForwardIndex { get; private set; }
        public float3 Position { get; private set; }
        public float3 Normal { get; private set; }
        public byte ShapeDirection { get; private set; }
        public bool FaceForwardIsEmpty { get; private set; }
        public bool FaceForwardHasCreature { get; private set; }
        public bool Hit { get; private set; }
        public Voxel Target { get; private set; }
        public Voxel TargetFaceFroward { get; private set; }
        public Voxel VoxelInEyePos { get; private set; }
        public void Update(NativeReference<VoxelRayResult>.ReadOnly voxelRayResult)
        {
            var value = voxelRayResult.Value;
            TargetIndex = value.TargetIndex;
            TargetFaceForwardIndex = value.TargetFaceForwardIndex;
            Position = value.Position;
            Normal = value.Normal;
            ShapeDirection = value.ShapeDirection;
            FaceForwardIsEmpty = value.FaceForwardIsEmpty;
            FaceForwardHasCreature = value.FaceForwardHasCreature;
            Hit = value.Hit;
            Target = value.Target;
            TargetFaceFroward = value.TargetFaceFroward;
            VoxelInEyePos = value.VoxelInEyePos;
        }
        public bool CanDestroyTargetFaceFroward()
        {
            if (Voxel.NonAir(TargetFaceFroward.VoxelTypeIndex))
            {
                if (Voxel.Solid(TargetFaceFroward.VoxelMaterial))
                {
#if UNITY_EDITOR
                    Debug.LogWarning("固体被穿透");
#endif
                }
                else
                {
                    return true;
                }
            };
            return false;
        }
        public bool CanDestroyTarget()
        {
            if (Voxel.NonAir(Target.VoxelTypeIndex))
            {
                return true;
            };
            return false;
        }
        public bool CanPutToTarget(Voxel voxel)
        {
            if (Voxel.Solid(voxel.VoxelMaterial))
            {
                return !Voxel.Solid(Target.VoxelMaterial);
            }
            return false;
        }
        public bool CanPutToTargetFaceForward(Voxel voxel)
        {
            if (Voxel.Solid(voxel.VoxelMaterial))
            {
                return (FaceForwardIsEmpty || (!Voxel.Solid(TargetFaceFroward.VoxelMaterial))) && !FaceForwardHasCreature;
            }
            else
            {
                return FaceForwardIsEmpty;
            }
        }
    }
    public interface IVoxelRayResult
    {
        int3 TargetIndex { get; }
        int3 TargetFaceForwardIndex { get; }
        float3 Position { get; }
        float3 Normal { get; }
        byte ShapeDirection { get; }
        bool FaceForwardIsEmpty { get; }
        bool FaceForwardHasCreature { get; }
        bool Hit { get; }
        Voxel Target { get; }
        Voxel TargetFaceFroward { get; }
        Voxel VoxelInEyePos { get; }

        bool CanDestroyTargetFaceFroward();
        bool CanDestroyTarget();
        /// <summary>
        /// 先尝试替换到目标位置
        /// </summary>
        /// <param name="voxel"></param>
        /// <returns></returns>
        bool CanPutToTarget(Voxel voxel);
        /// <summary>
        /// 否则尝试放置到目标面前方位置
        /// </summary>
        /// <param name="voxel"></param>
        /// <returns></returns>
        bool CanPutToTargetFaceForward(Voxel voxel);
    }
    public struct VoxelRayResult
    {
        public int3 TargetIndex;
        public int3 TargetFaceForwardIndex;
        public float3 Position;
        public float3 Normal;
        public byte ShapeDirection;
        public bool FaceForwardIsEmpty;
        public bool FaceForwardHasCreature;
        public bool Hit;
        public Voxel Target;
        public Voxel TargetFaceFroward;
        public Voxel VoxelInEyePos;
    }
    [BurstCompile]
    struct FinishInputRayVoxelWorldJob : IJob
    {
        [ReadOnly] public VoxelWorldMap.ReadOnly VoxelWorldMap;
        [ReadOnly] public CollisionWorld CollisionWorld;
        public quaternion Rotation;
        public float3 Start;
        public NativeReference<VoxelRayResult> Result;
        public void Execute()
        {
            float3 dir = math.rotate(Rotation, math.forward());
            RaycastInput raycastInput = new RaycastInput()
            {
                Start = Start,
                End = Start + dir * 10f,
                Filter = new CollisionFilter()
                {
                    CollidesWith = DOTSLayer.ExcludePlayer,
                    BelongsTo = DOTSLayer.Player,
                    GroupIndex = 0,
                }
            };
            VoxelRayResult result = new VoxelRayResult();
            if (CollisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
            {
                float3 normal = raycastHit.SurfaceNormal;
                // 首先是集中的那块体素的位置,从集中位置往里缩一点,再floor,即是索引
                float3 hitVoxelIndex = math.floor(raycastHit.Position - normal * 0.025f);// TODO修改判断方式
                // 集中的体素索引加0.5f,转移到集中体素的中心,再加上法向获得面向的体素的大概中心
                float3 hitFaceVoxelCenter = hitVoxelIndex + 0.5f + normal;
                int putDirection;

                if (math.abs(normal.y) > 0.1f)
                {
                    putDirection = (math.abs(dir.z) >= math.abs(dir.x)// 比如Z大于X那么方向肯定是较Z轴近的
                     ? (dir.z < 0f ? VoxelShapeHeader.正向 : VoxelShapeHeader.后向)
                     : (dir.x < 0f ? VoxelShapeHeader.右向 : VoxelShapeHeader.左向))
                     + (normal.y > 0f ? 0 : 4);// 法向大于0为从上面放置,否则应该颠倒
                }
                else
                {
                    putDirection = math.abs(normal.z) > 0.1f// 是顺Z轴?
                        ? (normal.z > 0f ? VoxelShapeHeader.正向 : VoxelShapeHeader.后向)//是,判断前还是后
                        : (normal.x > 0f ? VoxelShapeHeader.右向 : VoxelShapeHeader.左向);//否,判断左右
                    if (raycastHit.Position.y % 1f > 0.5f)
                        putDirection += 4;
                }
                result.ShapeDirection = (byte)putDirection;
                result.TargetIndex = new int3(hitVoxelIndex);
                result.TargetFaceForwardIndex = new int3(math.floor(hitFaceVoxelCenter));

                result.Target = VoxelWorldMap.GetVoxelOrEmpty(result.TargetIndex);
                result.TargetFaceFroward = VoxelWorldMap.GetVoxelOrEmpty(result.TargetFaceForwardIndex);

                result.Position = raycastHit.Position;
                result.Normal = normal;
                result.FaceForwardIsEmpty = !VoxelWorldMap.CheckHasVoxel(result.TargetFaceForwardIndex);
                result.FaceForwardHasCreature = CollisionWorld.CheckBox(hitFaceVoxelCenter, quaternion.identity, 0.5005f, new CollisionFilter()
                {
                    BelongsTo = DOTSLayer.AllDynamic,
                    CollidesWith = DOTSLayer.AllCreature,
                    GroupIndex = 0,
                });
                result.Hit = true;
            }
            else
            {
                result.Hit = false;
            }
            result.VoxelInEyePos = VoxelWorldMap.GetVoxelOrEmpty(new int3(math.floor(Start)));
            Result.Value = result;
        }
    }
}
