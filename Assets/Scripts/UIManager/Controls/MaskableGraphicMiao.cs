using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public class MaskableGraphicMiao : UiItem<MaskableGraphicStyleObject>
    {
        [SerializeField] MaskableGraphic maskableGraphic;
        public MaskableGraphic MaskableGraphic => maskableGraphic;
        protected override void Awake()
        {
            base.Awake();
            if (maskableGraphic == null)
                maskableGraphic = GetComponent<MaskableGraphic>();
        }
        public override void ApplyStyle(MaskableGraphicStyleObject styleData)
        {
            styleData.Apply(maskableGraphic);
        }
        public override void ApplyHoverStyle(MaskableGraphicStyleObject styleData)
        {
            styleData.ApplyOnHover(maskableGraphic);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (maskableGraphic == null)
                maskableGraphic = GetComponent<MaskableGraphic>();
        }
#endif
    }
}