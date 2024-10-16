using System;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace CatDOTS
{
    public static class AuthoringExtension
    {
        public static Unity.Entities.Hash128 Hash128(string name)
        {
            return UnityEngine.Hash128.Compute(name);
        } 
    }
    public class BasicBlobAssetAuthoring : MonoBehaviour
    {
        public bool AddBeingsBlobAsset;
        public bool AllowToBeAttack;
        public float Hp;
        public float Attack;
        public float Defense;
        public float WalkSpeed;
        public float RunSpeed;
        public float JumpVelocity = 6f;
        public float RotateChangeRate = 0.1f;
        public float SpeedChangeRate = 0.1f;
        public float OverLoadFactor = 0.8f;
        public float GravityFactor = 1.5f;
        [SerializeField] Vector3 collideSize;
        [SerializeField] int3 gridSize;
        public VATAsset VATAsset;
        Unity.Entities.Hash128 beingsAssetHash;
        Unity.Entities.Hash128 animatorHash;
        protected class BasicPrefabBaker : Baker<BasicBlobAssetAuthoring>
        {
            public override void Bake(BasicBlobAssetAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                //先登记再加入组件以便消除重复的blob资产
                #region 基本属性
                if (authoring.AddBeingsBlobAsset)//创建基本鲍勃资产
                {
                    if (!TryGetBlobAssetReference(authoring.beingsAssetHash, out BlobAssetReference<BeingsDataBlobAsset> beingsBlobAsset))
                    {
                        beingsBlobAsset = BeingsDataBlobAsset.Create(authoring);
                        AddBlobAsset<BeingsDataBlobAsset>(ref beingsBlobAsset, out authoring.beingsAssetHash);
                    }
                    AddComponent<BeingsData>(entity, new BeingsData()
                    {
                        Data = beingsBlobAsset
                    });
                    if (authoring.AllowToBeAttack)//有基本属性的前提下才能被攻击
                    {
                        AddComponent<InjuryPercentage>(entity, new InjuryPercentage() { Value = 0f });
                        AddBuffer<DamageBufferElement>(entity);
                    }
                }
                #endregion
                #region 动画器
                //如果有动画数据便添加上动画器组件
                if (authoring.VATAsset != null && authoring.VATAsset.IsValid)
                {
                    if (!TryGetBlobAssetReference(authoring.animatorHash, out BlobAssetReference<VATAnimationController> anmationController))
                    {
                        anmationController = VATAnimationController.Create(authoring.VATAsset);
                        AddBlobAsset<VATAnimationController>(ref anmationController, out authoring.animatorHash);
                    }
                    AddComponent<VATAnimator>(entity, new VATAnimator()
                    {
                        AnimationTime = 0,
                        AnimatorController = anmationController
                    });
                    AddComponent<AnimationTimeMaterialProperty>(entity);
                }
                #endregion
            }
        }
        /// <summary>
        /// 碰撞大小/非整数
        /// </summary>
        public float3 CollideSize
        {
            get
            {
                if (math.any((float3)collideSize == float3.zero))//如果整个f3任一是0
                {
                    float3 bounds = TryGetComponent(out MeshRenderer renderer) ? (float3)renderer.bounds.size : new float3(1f);
                    collideSize.x = ClampSize(collideSize.x, bounds.x);
                    collideSize.y = ClampSize(collideSize.y, bounds.y);
                    collideSize.z = ClampSize(collideSize.z, bounds.z);
                }
                return collideSize;
            }
            private set { collideSize = value; }
        }
        public int3 GridSize
        {
            get
            {
                gridSize = new int3(math.ceil(CollideSize));
                return gridSize;
            }
        }
        float ClampSize(float original, float bounds)
        {                                 //第二次判定，设定最小值
            original = original < 0.01f ? (bounds < 0.01f ? 0.01f : bounds) : original;//第一次判定，如果没填值，按照包围盒的值
            return original;
        }
        public FixedString32Bytes Name
        {
            get
            {
                if (name.Length > NameLength)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("名称长度超过28");
#endif
                    return new FixedString32Bytes(name[..NameLength]);
                }
                return new FixedString32Bytes(name);
            }
        }
        public Unity.Entities.Hash128 GetHash128()
        {
            if (name.Length > NameLength)
            {
                return UnityEngine.Hash128.Compute(name[..NameLength]);
            }
            return UnityEngine.Hash128.Compute(name);
        }
        public const int NameLength = 28;
    }
}