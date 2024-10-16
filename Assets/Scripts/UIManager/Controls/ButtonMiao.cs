using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public struct UiClick
    {
        public ButtonMiao ButtonMiao { get; private set; }
        public int ClickCount { get; private set; }
        public readonly bool DoubleClick => ClickCount >= 2;
        public UiClick(int clickCount, ButtonMiao buttonMiao)
        {
            ClickCount = clickCount;
            ButtonMiao = buttonMiao;
        }
    }
    public class ButtonMiao : MaskableGraphicMiao, IPointerClickHandler
    {
        [SerializeField] TextMiao label;
        public string LabelNoTranslate
        {
            get => label != null ? label.TextValue : string.Empty;
            set { if (label != null) label.TextValue = value; }
        }
        public string Label
        {
            get => label != null ? label.TranslationKey : string.Empty;
            set { if (label != null) label.TranslationKey = value; }
        }
        public event Action<UiClick> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(new UiClick(eventData.clickCount, this));
        }
    }
}