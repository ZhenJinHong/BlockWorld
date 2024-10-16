using System;
using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;

namespace CatDOTS
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile/*, DisableAutoCreation, Obsolete*/]
    public partial struct VATAnimationSystem : ISystem
    {
        EntityQuery animatorQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
            entityQueryBuilder
                .WithAllRW<VATAnimator, AnimationTimeMaterialProperty>();

            animatorQuery = entityQueryBuilder.Build(ref state);

            state.RequireForUpdate(animatorQuery);
            entityQueryBuilder.Dispose();
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateAnimatorJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel(animatorQuery, state.Dependency);
        }
    }
    [BurstCompile]
    partial struct UpdateAnimatorJob : IJobEntity
    {
        [ReadOnly] public float DeltaTime;
        [BurstCompile]
        public void Execute(ref VATAnimator vatAnimator, ref AnimationTimeMaterialProperty vATAnimationTimeProperty)
        {
            ref VATAnimationController animationController = ref vatAnimator.AnimatorController.Value;
            ref VATClip clip = ref animationController.WatchClipPtr.Value;

            //要考虑同贴图的其他动画
            vatAnimator.AnimationTime += DeltaTime;
            //循环播放
            vatAnimator.AnimationTime = vatAnimator.AnimationTime > clip.Length
                ? (clip.Loop ? vatAnimator.AnimationTime - clip.Length : clip.Length)
                : vatAnimator.AnimationTime;
            //进度时间乘以帧率获取已进行的帧数
            float frame = vatAnimator.AnimationTime * clip.FPS;
            //此处为Floor即不到1的为0
            float IntFrame = math.floor(frame);

            float frameProgress = frame - IntFrame;

            IntFrame = (IntFrame < clip.Frames)
                ? IntFrame
                : (clip.Loop ? clip.StartFrame : clip.EndFrame);

            float timeInMap = IntFrame * animationController.FrameHeightInMap;

            float nextTimeInMap = IntFrame + 1.0f < clip.Frames
                ? timeInMap + animationController.FrameHeightInMap
                : (clip.Loop ? clip.StartFrame * animationController.FrameHeightInMap : timeInMap);
            vATAnimationTimeProperty.Value = new float3(timeInMap, nextTimeInMap, frameProgress);
        }
    }
}