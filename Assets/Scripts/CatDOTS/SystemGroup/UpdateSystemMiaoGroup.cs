using System;
using Unity.Entities;
using UnityEngine.Scripting;

namespace CatDOTS
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UpdateSystemMiaoGroup : ComponentSystemGroup
    {
        [Preserve]
        public UpdateSystemMiaoGroup() { }
    }
}
