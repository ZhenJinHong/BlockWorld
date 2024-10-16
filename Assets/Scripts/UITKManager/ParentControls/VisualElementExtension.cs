using CatFramework.Localized;
using UnityEngine;
using UnityEngine.UIElements;
using static CatFramework.UiTK.VisualElementController;

namespace CatFramework.UiTK
{
    public static class VisualElementExtension
    {
        /// <summary>
        /// 不调整布局
        /// </summary>
        public static void Visible_C(this VisualElement element, bool visible)
        {
            if (visible)
                element.RemoveFromClassList(hideClass);
            else
                element.AddToClassList(hideClass);
        }
        /// <summary>
        /// 会调整布局
        /// </summary>
        public static void Display_C(this VisualElement element, bool display)
        {
            if (display)
                element.RemoveFromClassList(noDisplayClass);
            else
                element.AddToClassList(noDisplayClass);
        }
        public static Button Default(this Button btn, string text = null, object useData = null)
        {
            btn.RemoveFromClassList(Button.ussClassName);
            btn.RemoveFromClassList(TextElement.ussClassName);
            btn.BindLanguage(text, useData);
            return btn;
        }
        public static Label Default(this Label label, string text = null, object useData = null)
        {
            label.pickingMode = PickingMode.Ignore;
            label.RemoveFromClassList(Label.ussClassName);
            label.RemoveFromClassList(TextElement.ussClassName);
            label.BindLanguage(text, useData);
            return label;
        }
        public static Image Default(this Image image)
        {
            image.pickingMode = PickingMode.Ignore;
            return image;
        }
        public static ListView Default(this ListView listView)
        {
            return listView;
        }
        public static TextField Default(this TextField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static SliderInt Default(this SliderInt slider, string text = null, object useData = null)
        {
            slider.BindLanguage(text, useData);
            return slider;
        }
        public static IntegerField Default(this IntegerField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static Toggle Default(this Toggle toggle, string text = null, object useData = null)
        {
            toggle.BindLanguage(text, useData);
            return toggle;
        }
        public static UnsignedIntegerField Default(this UnsignedIntegerField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static FloatField Default(this FloatField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static EnumField Default(this EnumField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static DropdownField Default(this DropdownField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        public static LongField Default(this LongField field, string text = null, object useData = null)
        {
            field.BindLanguage(text, useData);
            return field;
        }
        /// <summary>
        /// 如果已有数据，将替换语言键
        /// </summary>
        public static void BindLanguage(this TextElement textElement, string text, object useData = null)
        {
            if (string.IsNullOrEmpty(text))// 对于有些文本元素有初始的文字,不需要更新?,应该不存在的,都需要绑定
            {
                return;
            }
            if (textElement.userData is BindLocalizedData data)
            {
                data.ReplaceLanguage(text);
                data.userData = useData;
            }
            else
            {
                textElement.userData = new BindLocalizedData(textElement, new LocalizedDataKey(text))
                {
                    userData = useData,
                };
            }
        }
        public static BindLocalizedData GetBindLocalizedData(this TextElement textElement)
        {
            return (BindLocalizedData)textElement.userData;
        }
        public static void UpdateText(this TextElement textElement)
        {
            if (textElement.userData is BindLocalizedData data)
                data.UpdateLocalizedData();
        }
        public static void UpdateLabel<T>(this BaseField<T> field)
            => field.labelElement.UpdateText();
        /// <summary>
        /// 如果已有数据，将替换语言键
        /// </summary>
        public static void BindLanguage<T>(this BaseField<T> textField, string text, object useData = null)
        {
            textField.label = text ?? string.Empty;// 用以保证labelElement处于视图层级中
            textField.labelElement.BindLanguage(text, useData);
        }
        /// <summary>
        /// 如果未绑定本地数据则返回元素自身useData
        /// </summary>
        public static object GetUseData(this TextElement textElement)
        {
            if (textElement.userData is BindLocalizedData bindLocalizedData)
                return bindLocalizedData.userData;
            else
                return textElement.userData;
        }
    }
}
