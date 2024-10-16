using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine.Scripting;

namespace CatDOTS
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class LateUpdateSystemMiaoGroup : ComponentSystemGroup
    {
        [Preserve]
        public LateUpdateSystemMiaoGroup() { }
    }
}
