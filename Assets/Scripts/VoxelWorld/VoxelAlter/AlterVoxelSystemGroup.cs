using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Scripting;

namespace CatDOTS.VoxelWorld
{
    [UpdateInGroup(typeof(LateUpdateSystemMiaoGroup))]
    public partial class AlterVoxelSystemGroup : ComponentSystemGroup
    {
        [Preserve]
        public AlterVoxelSystemGroup() { }
    }
}
