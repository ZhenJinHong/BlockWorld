using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace CatDOTS
{
    [MaterialProperty("_AnimationTime")]
    public struct AnimationTimeMaterialProperty : IComponentData
    {
        public float3 Value;
    }
}