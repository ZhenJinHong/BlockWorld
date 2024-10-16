using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace CatDOTS
{
    public readonly partial struct FirstPersonPlayerAspect : IAspect
    {
        //readonly RefRO<PlayerTag> playerTag;
        readonly RefRO<FirstPersonPlayer> firstPersonPlayer;
        readonly RefRW<PhysicsVelocity> velocity;
        readonly RefRW<PhysicsGravityFactor> gravityFactor;
        //readonly RefRO<EntitySimulationResult> simulationResult;
        readonly RefRW<LocalTransform> localTransform;
        public readonly RefRO<BeingsData> beingsData;
        readonly RefRW<FirstPersonPlayerOutputCache> outputCache;
        //readonly RefRO<PlayerSettings> playerSettings;
        //public PlayerTag PlayerTag
        //{
        //    get => playerTag.ValueRO;
        //}
        public float3 Linear
        {
            get => velocity.ValueRO.Linear;
            set => velocity.ValueRW.Linear = value;
        }
        public float3 Angular
        {
            get => velocity.ValueRO.Angular;
            set => velocity.ValueRW.Angular = value;
        }
        public float GravityFactor
        {
            get => gravityFactor.ValueRO.Value;
            set => gravityFactor.ValueRW.Value = value;
        }
        //public EntitySimulationState EntitySimulationState
        //{
        //    get => simulationResult.ValueRO.State;
        //}
        public float3 PlayerPosition
        {
            get => localTransform.ValueRO.Position;
            set => localTransform.ValueRW.Position = value;
        }
        public quaternion PlayerRotation
        {
            get => localTransform.ValueRO.Rotation;
            set
            {
                outputCache.ValueRW.PlayerRotation = value;
                localTransform.ValueRW.Rotation = value;
            }
        }
        public float CameraXAngle
        {
            get => outputCache.ValueRO.CameraXAngle;
            set => outputCache.ValueRW.CameraXAngle = ClampXAngle(value);
        }
        public float CameraYAngle
        {
            get => outputCache.ValueRO.CameraYAngle;
            set => outputCache.ValueRW.CameraYAngle = value;
        }
        public quaternion CameraRotation
        {
            get => outputCache.ValueRO.CameraRotation;
            set => outputCache.ValueRW.CameraRotation = value;
        }
        public float ClampXAngle(float value)
        {
            return math.clamp(value, firstPersonPlayer.ValueRO.BottomAngle, firstPersonPlayer.ValueRO.TopAngle);
        }
        //public float3 OutputPosition
        //{
        //    get => outputCache.ValueRO.Position;
        //    set => outputCache.ValueRW.Position = value;
        //}
        public float3 TransformDirection(float3 dir)
        {
            return localTransform.ValueRO.TransformDirection(dir);
        }
    }
}
