using Unity.Entities;
using UnityEngine.Scripting;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 不修改体素的系统组
    /// </summary>
    [UpdateInGroup(typeof(UpdateSystemMiaoGroup))]
    public partial class BixelUpdateSystemGroup : ComponentSystemGroup
    {
        [Preserve]
        public BixelUpdateSystemGroup() { }
    }
}
