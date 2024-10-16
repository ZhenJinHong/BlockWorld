using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace CatFramework_TestDOTS
{
    [DisableAutoCreation]
    public partial class SystemT : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
    [DisableAutoCreation]
    [BurstCompile]
    public partial struct TriggerEventTestSystem : ISystem
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
            SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        }
    }
    partial struct TriggerEventProcessJob : ITriggerEventsJob, ICollisionEventsJob
    {
        public void Execute(TriggerEvent triggerEvent)
        {

        }

        public void Execute(CollisionEvent collisionEvent)
        {
        }
    }
}
