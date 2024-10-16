using System.Collections;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS
{
    public struct VATAnimator : IComponentData
    {
        /// <summary>
        /// 动画进程时间
        /// </summary>
        public float AnimationTime;
        public BlobAssetReference<VATAnimationController> AnimatorController;
    }
}