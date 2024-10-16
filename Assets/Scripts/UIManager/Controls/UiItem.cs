using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public interface IUiItem
    {
        bool IsDestroy { get; }
        object UseData { get; set; }

        void HoverStyle();
        void DefaultStyle();
        void ForcedDefaultStyle();
        void ForcedHoverStyle();
    }
    public abstract class UiItem : MonoBehaviour, IUiItem
    {
        public bool IsDestroy => this == null;
        public object useData;
        public object UseData { get => useData; set => useData = value; }
        public abstract void HoverStyle();
        public abstract void DefaultStyle();
        public abstract void ForcedHoverStyle();
        public abstract void ForcedDefaultStyle();
    }
    [DisallowMultipleComponent]
    public abstract class UiItem<StyleData> : UiItem, IPointerEnterHandler, IPointerExitHandler where StyleData : StyleObject
    {
        [SerializeField] StyleData styleObj;
        Style style;
        protected Style Style => style;
        protected StyleData StyleObj => styleObj;
        protected virtual void Awake()
        {

        }
        protected virtual void OnEnable()
        {

        }
        protected virtual void Start()
        {
            if (styleObj != null)
                UiManagerMiao.WaitforStyle(WaitforStyle);
        }
        protected virtual void OnDestroy()
        {
            style?.UnregisterStyleChange<StyleData>(styleObj, ApplyStyle);
        }
        void WaitforStyle(Style style)
        {
            this.style = style;
            style.RegisterStyleChange<StyleData>(styleObj, ApplyStyle);
        }
        public abstract void ApplyStyle(StyleData styleData);
        public abstract void ApplyHoverStyle(StyleData styleData);
        bool forcedHoverStyle;
        public bool IsForcedHoverStyle => forcedHoverStyle;
        public void ForcedHoverStyle(bool hover)
        {
            if (hover)
                ForcedHoverStyle();
            else
                ForcedDefaultStyle();
        }
        public override void HoverStyle()
        {
            if (forcedHoverStyle) return;
            if (styleObj != null)
                ApplyHoverStyle(styleObj);
        }
        public override void DefaultStyle()
        {
            if (forcedHoverStyle) return;
            if (styleObj != null)
                ApplyStyle(styleObj);
        }
        public override void ForcedHoverStyle()
        {
            if (forcedHoverStyle) return;
            if (styleObj != null)
                ApplyHoverStyle(styleObj);
            forcedHoverStyle = true;
        }
        public override void ForcedDefaultStyle()
        {
            if (!forcedHoverStyle) return;// 已是默认,则返回
            if (styleObj != null)
                ApplyStyle(styleObj);
            forcedHoverStyle = false;
        }
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            HoverStyle();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            DefaultStyle();
        }
    }
}
