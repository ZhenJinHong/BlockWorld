using System;
using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine.Scripting;

namespace CatDOTS
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]// 在固定帧原因，物理体的运动更新在固定帧，体素的碰撞体必须同步多次运算
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class FixedUpdateSystemMiaoGroup : ComponentSystemGroup
    {
        [Preserve]
        public FixedUpdateSystemMiaoGroup() { }
    }
}
