using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public sealed class SelectColor : TInputField<Color>, IPointerClickHandler, IColorReceiver
    {
        [SerializeField] Image valueImage;
        public Color OriginalColor => valueImage.color;
        public bool UseAlpha => useAlpha;
        public bool UseHDR => useHDR;
        public bool RealTimeUpdata => realTimeUpdate;

        bool useAlpha = true, useHDR, realTimeUpdate;
        public override void SetValueWithoutNotify(Color value)
        {
            this.value = value;
            if (valueImage != null)
                valueImage.color = value;
        }
        public void SetRule(bool useAlpha, bool useHDR, bool realTimeUpdate)
        {
            this.useAlpha = useAlpha;
            this.useHDR = useHDR;
            this.realTimeUpdate = realTimeUpdate;
        }
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //Entry.UiManager.ColorPicker(this);
        }
        public void CallBack(Color color)
        {
            SetValueWithoutNotify(color);
            onSubmit?.Invoke(this);
        }
    }
}