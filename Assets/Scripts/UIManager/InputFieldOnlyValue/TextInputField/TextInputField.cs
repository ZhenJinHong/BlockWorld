using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    /// <summary>
    /// 需要进行读取文本获得值的，继承该类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TextInputField<T> : TInputField<T>, ITextReceiver
    {
        [SerializeField] protected TextMiao valueText;
        [SerializeField] protected ButtonMiao clickInput;
        bool ITextReceiver.OneKeyShoot => false;
        string ITextReceiver.OriginalText => valueText.TextValue;//不使用value是因为有null的可能
        RectTransform ITextReceiver.CoverageRect { get { if (valueText) return valueText.transform as RectTransform; return null; } }
        public event Action<TInputField<T>> OnEndEdit
        {
            add => onEndEdit += value;
            remove => onEndEdit -= value;
        }
        protected Action<TInputField<T>> onEndEdit;
        protected override void Start()
        {
            if (clickInput)
                clickInput.OnClick += OnPointerClick;
            base.Start();
        }
        public override void SetValueWithoutNotify(T value)
        {
            this.value = value;
            if (valueText)
                valueText.TextValue = value.ToString();
        }
        /// <summary>
        /// 进入文本输入状态
        /// </summary>
        protected void OnPointerClick(UiClick uiClick)
        {
            UiManagerMiao.CallOnceInputField(this);
        }
        void ITextReceiver.OnValueChange(string text)
        {

        }
        /// <summary>
        /// 回车提交时触发
        /// </summary>
        void ITextReceiver.OnSubmit(string text)
        {
            SetValueWithoutNotify(ProcessText(text));
            onSubmit?.Invoke(this);
        }
        /// <summary>
        /// 取消输入时触发
        /// </summary>
        void ITextReceiver.OnEndEdit(string text)
        {
            SetValueWithoutNotify(ProcessText(text));
            onEndEdit?.Invoke(this);
        }
        public abstract T ProcessText(string text);
    }
}