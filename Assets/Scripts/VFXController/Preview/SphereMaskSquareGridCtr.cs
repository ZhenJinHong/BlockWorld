using System;
using UnityEngine;

namespace CatFramework.VFX
{
    public class SphereMaskSquareGridCtr : VFXController
    {
        Material material;
        int radiusID = Shader.PropertyToID("_Radius");
        int centerID = Shader.PropertyToID("_Center");
        int tillingID = Shader.PropertyToID("_Tilling");
        bool hasProperty;
        Vector2 tilling;
        public Color passColor = new Color(0f, 0.5f, 1.7f, 1f);
        public Color noPassColor = new Color(1.7f, 0f, 0.15f, 1f);
        public Vector2 Tilling
        {
            get => tilling;
            set
            {
                if (hasProperty)
                {
                    tilling = value;
                    material.SetVector(tillingID, tilling);
                }
            }
        }
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                material = meshRenderer.sharedMaterial;
                if (material != null)
                {
                    hasProperty = material.HasColor(baseColorID)
                        && material.HasVector(tillingID)
                        && material.HasVector(centerID)
                        && material.HasFloat(radiusID);
                    if (hasProperty) Tilling = new Vector2(32f, 32f);
                }
#if UNITY_EDITOR
                if (material == null) Debug.Log("未获取到材质");
                if (!hasProperty) Debug.LogWarning("未找到属性");
#endif
            }
        }
        /// <summary>
        /// range建筑物占地大小，offset建筑物相对于所处单格子的偏移即0或0.5f，用以在奇数range情况下偏移遮罩区域
        /// </summary>
        public void SetRange(Vector2 range, Vector2 offset)// radius为0.5f就填满了// 如果范围为8，则半径为8除以2除以32
        {
            if (hasProperty)
            {
                float radius = ((range.x > range.y ? range.x : range.y)) / tilling.x * 0.5f;
                Vector2 center = new Vector2(0.5f, 0.5f) - /*按照正常逻辑应该是+才是，但实际是需要-*/offset / tilling.x;//使用Unity自带的Plane也是要用减号
                material.SetFloat(radiusID, radius);
                material.SetVector(centerID, center);
            }
        }
        public void SetPosition(Vector3 position, bool pass)
        {
            position.y += 0.1f;
            SetPosition(position);
            if (hasProperty)
            {
                material.SetColor(baseColorID, pass ? passColor : noPassColor);
            }
        }
    }
}
