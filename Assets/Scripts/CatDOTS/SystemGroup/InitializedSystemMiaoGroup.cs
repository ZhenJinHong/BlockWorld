using System;
using Unity.Entities;
using UnityEngine.Scripting;

namespace CatDOTS
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    //[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public partial class InitializedSystemMiaoGroup : ComponentSystemGroup
    {
        [Preserve]
        public InitializedSystemMiaoGroup() { }
    }
}
