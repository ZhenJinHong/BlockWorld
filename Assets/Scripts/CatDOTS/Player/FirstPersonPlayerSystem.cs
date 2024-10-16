using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Physics.GraphicsIntegration;
using CatFramework.CatMath;
using UnityEngine.InputSystem;
using UnityEngine;
using CatFramework.InputMiao;

namespace CatDOTS
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), /*OrderLast = true*/OrderFirst = true)]//为了保证物理放置物体检测能正确检测
    //[UpdateAfter(typeof(PhysicsSystemGroup))]
    //[UpdateInGroup(typeof(EntityWorldSimulationSystemGroup), OrderFirst = true)]
    //[UpdateAfter(typeof(AfterPhysicsSystemGroup))]
    //[UpdateBefore(typeof(BeginFixedStepSimulationEntityCommandBufferSystem))]
    //[UpdateInGroup(typeof(SimulationSystemGroup))]
    //[UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
    public partial class FirstPersonPlayerSystem : SystemBase
    {
        public interface IInputProvider
        {
            float3 DefaultPosition { get; }
            bool EnableInput { get; }
            PlayerState State { get; }
            float2 Look { get; }
            float2 Move { get; }
        }
        public IInputProvider inputProvider;
        public IFirstPersonSettingProvider setting;
        EntityQuery playerQuery;
        //EntityQuery inGameQuery;
        NativeReference<float3> playerPos;
        protected override void OnCreate()
        {
            base.OnCreate();
            EntityQueryBuilder builder = new(Allocator.Temp);
            builder.WithAspect<FirstPersonPlayerAspect>();
            playerQuery = builder.Build(this);//目前只需要读写物理速度，并经由当前帧更新物理

            //builder.Reset();
            //builder.WithAll<InGame>();
            //inGameQuery = builder.Build(this);

            RequireForUpdate(playerQuery);
            //RequireForUpdate(inGameQuery);

            builder.Dispose();
            playerPos = new NativeReference<float3>(Allocator.Persistent);
        }
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            //playerEnbleInput = false;
            if (inputProvider != null)
            {
                //playerPosition = inputProvider.DefaultPosition;
                playerPos.Value = inputProvider.DefaultPosition;
                // 后续该成读档方式
                Dependency = new SetPlayerDefaultJob()
                {
                    DefaultPostion = inputProvider.DefaultPosition,
                }.Schedule(playerQuery, Dependency);
            }
        }
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            //playerEnbleInput = false;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //playerEnbleInput = false;
            playerPos.Dispose();
        }
        //private static float3 playerPosition;
        //private static bool playerEnbleInput;
        //public static bool PlayerEnableInput { get => playerEnbleInput; }
        //public static float3 PlayerPosition { get => playerPosition; }
        protected override void OnUpdate()
        {
            if (inputProvider != null && setting != null && inputProvider.EnableInput)
            {
                //playerEnbleInput = true;
                //playerPosition = playerPos.Value;
                var PlayerActionJob = new PlayerActionJob
                {
                    playerPos = playerPos,
                    RotateSpeed = setting.RotationSpeed,
                    deltaThreshold = setting.RotationThreshold,
                    speedChangeRate = setting.MovementChangeRate,
                    State = inputProvider.State,
                    Look = inputProvider.Look,
                    Move = inputProvider.Move,
                    CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                    DeltaTime = SystemAPI.Time.DeltaTime,
                };
                var playerActionJobHandle = PlayerActionJob.Schedule(playerQuery, Dependency);
                Dependency = playerActionJobHandle/*.Complete()*/;
            }
            else
            {
                //if (playerEnbleInput == true)
                //{
                //    playerEnbleInput = false;
                //    //Dependency = new DisablePlayerJob().Schedule(playerQuery, Dependency);
                //}
            }
        }
    }
    [BurstCompile]
    partial struct SetPlayerDefaultJob : IJobEntity
    {
        public float3 DefaultPostion;
        public void Execute(FirstPersonPlayerAspect playerAspect)
        {
            playerAspect.PlayerPosition = DefaultPostion;
        }
    }
    //[BurstCompile]
    //partial struct DisablePlayerJob : IJobEntity
    //{
    //    public void Execute(PlayerAspect playerAspect)
    //    {
    //        playerAspect.Angular = 0f;
    //        playerAspect.Linear = 0f;
    //    }
    //}
    [BurstCompile]
    partial struct PlayerActionJob : IJobEntity
    {
        public float RotateSpeed;
        public PlayerState State;
        public float2 Look;
        public float2 Move;
        public float deltaThreshold;
        public float speedChangeRate;
        //const float groundThreshold = 0.001f;//认定为落地的最小速度
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public float DeltaTime;
        public NativeReference<float3> playerPos;
        //readonly static float3 Up = new(0f, 0.1f, 0f);
        [BurstCompile]
        public void Execute(FirstPersonPlayerAspect playerAspect)
        {
            playerPos.Value = playerAspect.PlayerPosition;
            ref BeingsDataBlobAsset beingsData = ref playerAspect.beingsData.ValueRO.Data.Value;
            //EntitySimulationState state = playerAspect.EntitySimulationState;
            //float rotateSpeed = beingsData.RotateSpeed;
            float rotateChangeSpeed = beingsData.RotateChangeRate;
            float overLoadFactor = beingsData.OverLoadFactor;
            float jumpVelocity = beingsData.JumpVelocity;
            float moveSpeed = beingsData.WalkSpeed;
            //float speedChangeRate = beingsData.SpeedChangeRate;
            #region 旋转
            //float lastYAngle = playerAspect.CameraYAngle;
            //float lastXAngle = playerAspect.CameraXAngle;
            playerAspect.CameraYAngle += (math.abs(Look.x) > deltaThreshold ? Look.x : 0f) * RotateSpeed;
            playerAspect.CameraYAngle = MathC.LoopAngleIn360(playerAspect.CameraYAngle);
            //扶正玩家
            playerAspect.PlayerRotation = quaternion.RotateY(math.radians(playerAspect.CameraYAngle));//因为需要角度钳制，所以当角度用，而不弧度
            //playerAspect.PlayerRotation = quaternion.RotateY(math.radians(math.lerp(lastYAngle, playerAspect.CameraYAngle, 0.8f)));//因为需要角度钳制，所以当角度用，而不弧度
            //上下角度
            playerAspect.CameraXAngle += (math.abs(Look.y) > deltaThreshold ? Look.y : 0f) * RotateSpeed;
            //playerAspect.CameraXAngle = math.clamp(playerAspect.CameraXAngle, -89f, 89f);

            //playerAspect.OutputPosition = playerAspect.PlayerPosition;
            playerAspect.CameraRotation = math.mul(playerAspect.PlayerRotation, quaternion.RotateX(math.radians(playerAspect.CameraXAngle)));
            //quaternion.EulerZXY(0f, math.radians(playerAspect.CameraXAngle), math.radians(playerAspect.CameraYAngle));
            //playerAspect.CameraRotation =f(playerAspect.CameraRotation, math.mul(playerAspect.PlayerRotation, quaternion.RotateX(math.radians(playerAspect.CameraXAngle))),rotateChangeSpeed);
            playerAspect.Angular = 0f;//置空角速度，不由物理控制
            #endregion
            bool enbleOverLoad = true && (State & PlayerState.Sprint) == PlayerState.Sprint;
            overLoadFactor = enbleOverLoad ? overLoadFactor + 1f : 1f;
#if UNITY_EDITOR
            overLoadFactor = enbleOverLoad ? 20f : 1f;
#endif
            #region 跳跃检测
            //playerAspect.GravityFactor = ((playerAspect.PlayerState & PlayerState.Flying) == PlayerState.Flying) ? 0.0f : 1.0f;
            float verticalVelocity = 0f;
            if ((State & PlayerState.Flying) == PlayerState.Flying)
            {
                playerAspect.GravityFactor = 0.0f;
                if ((State & PlayerState.Jump) == PlayerState.Jump)
                {
                    verticalVelocity = jumpVelocity * overLoadFactor;
                }
                else if ((State & PlayerState.Crouch) == PlayerState.Crouch)
                {
                    verticalVelocity = -jumpVelocity * overLoadFactor;
                }
            }
            else
            {
                playerAspect.GravityFactor = beingsData.GravityFactor;
                //RaycastInput raycastInput = new RaycastInput()
                //{
                //    Start = playerAspect.PlayerPosition + Up,
                //    End = playerAspect.PlayerPosition - Up,
                //    Filter = new CollisionFilter()
                //    {
                //        BelongsTo = DOTSLayer.Player,
                //        CollidesWith = DOTSLayer.ExcludePlayer,//落地检测
                //        GroupIndex = 0,
                //    }
                //};
                CollisionFilter collisionFilter = new CollisionFilter()
                {
                    BelongsTo = DOTSLayer.Player,
                    CollidesWith = DOTSLayer.ExcludePlayer,//落地检测
                    GroupIndex = 0,
                };
                if ((State & PlayerState.Jump) == PlayerState.Jump /*&& CollisionWorld.CastRay(raycastInput)*/ && CollisionWorld.CheckSphere(playerAspect.PlayerPosition, 0.25f, collisionFilter))
                //math.abs(playerAspect.Linear.y) < groundThreshold ||不可以使用这个这会导致物体撞到顶部的时候此时速度0，就可以无限顶着顶部
                {
                    verticalVelocity = jumpVelocity * overLoadFactor;
                }
                else
                {
                    verticalVelocity = playerAspect.Linear.y;
                }
            }
            #endregion
            float targetSpeed = moveSpeed * overLoadFactor;
            float3 speed = 0f;
            if (math.any(Move != float2.zero))
            {
                Move *= targetSpeed;
                float3 dir = playerAspect.TransformDirection(new float3(Move.x, 0.0f, Move.y));
                //speed = dir;
                speed = math.lerp(playerAspect.Linear, dir, DeltaTime * speedChangeRate);
                speed = math.round(speed * 1000f) / 1000f;
            }

            playerAspect.Linear = new float3(speed.x, verticalVelocity, speed.z);
        }
    }
}
