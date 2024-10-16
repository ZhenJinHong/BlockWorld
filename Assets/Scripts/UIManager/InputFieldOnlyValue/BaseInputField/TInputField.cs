using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    /// <summary>
    /// 单个值/对象输入
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <remarks>
    /// 不可以限制为结构体类型，因为需要被字符串输入继承
    /// </remarks>
    public abstract class TInputField<TValue> : UiItem, IInputField<TValue>
    {
        public Type ValueType => typeof(TValue);
        [SerializeField] TextMiao label;
        protected TValue value;
        public event Action<TInputField<TValue>> OnSubmit
        {
            add => onSubmit += value;
            remove => onSubmit -= value;
        }
        protected Action<TInputField<TValue>> onSubmit;
        public string Label
        {
            get => label != null ? label.TranslationKey : string.Empty;
            set { if (label != null) label.TranslationKey = value; }
        }
        protected virtual void Start()
        {

        }
        /// <summary>
        /// 父级中为空函数，只有需要进行范围限制的才重写
        /// </summary>
        public virtual void SetValueRange(TValue min, TValue max)
        {

        }
        public abstract void SetValueWithoutNotify(TValue value);
        public TValue GetValue() => value;
        public void SetValue(TValue value)
        {
            SetValueWithoutNotify(value);
            onSubmit?.Invoke(this);
        }
        public override string ToString()
        {
            return $" {Label} : {value}";
        }

        public override void HoverStyle()// 设计时未考虑嵌套问题,手动风格不要对嵌套的处理
        {
        }

        public override void DefaultStyle()
        {
        }

        public override void ForcedHoverStyle()
        {
        }

        public override void ForcedDefaultStyle()
        {
        }
    }
}