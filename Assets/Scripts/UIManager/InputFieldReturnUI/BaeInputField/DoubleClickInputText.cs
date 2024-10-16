//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using System;
//namespace CatFramework
//{
//    /// <summary>
//    /// 单机以按钮触发，双击以输入字段触发，本身不应监听翻译，因为自身的文本就是可以修改的
//    /// </summary>
//    public abstract class DoubleClickInputText<T> : UiItem, ITextReceiver, IPointerClickHandler
//    {
//        protected T value;
//        protected Action<DoubleClickInputText<T>> onSubmit;
//        Action<UiClick> onClick;
//        public bool OneKeyShoot => false;
//        public string OriginalText => value.ToString();
//        public RectTransform CoverageRect => transform as RectTransform;

//        /// <summary>
//        /// 如果是双击将进入文本输入状态，触发文本提交事件
//        /// </summary>
//        public void AddListener(Action<DoubleClickInputText<T>> onSubmit)
//        {
//            this.onSubmit = onSubmit;
//        }
//        public void AddListener(Action<UiClick> onClick)
//        {
//            this.onClick = onClick;
//        }
//        /// <summary>
//        /// 父级中为空函数，只有需要进行范围限制的才重写
//        /// </summary>
//        public virtual void SetValueRange(T min, T max)
//        {

//        }
//        public void SetValueWithoutNotify(T value)
//        {
//            this.value = value;
//            Text = value.ToString();
//        }
//        public T GetValue()
//        {
//            return value;
//        }
//        public void SetValue(T value)
//        {
//            SetValueWithoutNotify(value);
//            onSubmit?.Invoke(this);
//        }
//        public void OnValueChange(string text)
//        {
//        }
//        /// <summary>
//        /// 回车提交时触发
//        /// </summary>
//        void ITextReceiver.OnSubmit(string text)
//        {
//            SetValueWithoutNotify(ProcessText(text));
//            onSubmit?.Invoke(this);
//#if UNITY_EDITOR
//            ConsoleCat.Log("已经触发输入提交");
//#endif
//        }
//        public void OnEndEdit(string text)
//        {

//        }
//        protected abstract T ProcessText(string text);

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (eventData.clickCount > 1)
//            {
//                //Entry.UiManager.OnceInputField(this);
//            }
//            else
//                onClick?.Invoke(new UiClick(this, false));
//        }
//    }
//}