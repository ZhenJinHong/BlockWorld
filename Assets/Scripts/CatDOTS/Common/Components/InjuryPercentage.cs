using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Rendering;

namespace CatDOTS
{
    [MaterialProperty("_InjuryPercentage")]
    public struct InjuryPercentage : IComponentData
    {
        public float Value;
    }
}
