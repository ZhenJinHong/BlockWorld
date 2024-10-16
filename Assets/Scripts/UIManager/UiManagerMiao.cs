using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
using CatFramework.EventsMiao;

namespace CatFramework.UiMiao
{
    public static class UiManagerMiao
    {
        class Events
        {

        }
        public static OnceInputField OnceInputField;
        public static void CallOnceInputField(ITextReceiver textReceiver) { if (OnceInputField != null) OnceInputField.Call(textReceiver); }
        public static RayBlocker RayBlocker;
        public static CommonUIPrefabs commonUIPrefabs;
        public static ShareDropdownList shareDropdownList;
        public static ShareToolStrip shareToolStrip;
        public static ShareEnquireView shareEnquireView;


        static Events events;
        static LazyRegister<Style> lazyStyle;
        //static readonly Dictionary<Type, LazyRegister> uiRegisters;
        static readonly List<IEnableDirtyView> enableDirtyViews;

        static UiManagerMiao()
        {
            events = new Events();
            lazyStyle = new LazyRegister<Style>();
            //uiRegisters = new Dictionary<Type, LazyRegister>();
            enableDirtyViews = new List<IEnableDirtyView>();
            isShutdown = true;
        }
        static bool isShutdown;
        public static void Start()
        {
            isShutdown = false;
        }
        public static void Shutdown()
        {
            isShutdown = true;
            events = new Events();
            lazyStyle.Reset();
            //uiRegisters.Clear();
            enableDirtyViews.Clear();
        }
        public static void Update()
        {

        }
        public static void LateUpdate()
        {
            CleanDirtyView();
        }
        #region 视图迭代
        public static void AddDirtyView(IEnableDirtyView enableDirtyView)
        {
            if (ConsoleCat.IsDebug && (isShutdown || enableDirtyViews.Contains(enableDirtyView)))
            {
                ConsoleCat.DebugWarning("当前已关机或存在相同DirtyView");
                return;
            }
            enableDirtyViews.Add(enableDirtyView);
        }
        static void CleanDirtyView()
        {
            if (enableDirtyViews.Count == 0) return;
            for (int i = 0; i < enableDirtyViews.Count; i++)
            {
                if (enableDirtyViews[i].IsUsable)
                    enableDirtyViews[i].CleanDirty();
            }
            enableDirtyViews.Clear();
        }
        #endregion
        #region 样式登记与获取
        public static void WaitforStyle(Action<Style> action)
        {
            lazyStyle.AddListen(action);
        }
        public static void SetStyle(Style style)
        {
            lazyStyle.Register(style);
        }
        public static void ReleaseStyle()
        {
            lazyStyle.Reset();
        }
        #endregion
        #region UI登记与获取
        //public static void RegisterUI<T>(T panel) where T : class
        //{
        //    GetLazyRegister<T>().Register(panel);
        //}
        //public static void Unregister<T>(T panel) where T : class
        //{
        //    GetLazyRegister<T>().Unregister(panel);
        //}
        //public static bool TryGetUI<T>(out T panel) where T : class
        //{
        //    panel = GetLazyRegister<T>().GetValue();
        //    return panel != null;
        //}
        //static LazyRegister<T> GetLazyRegister<T>() where T : class
        //{
        //    if (!uiRegisters.TryGetValue(typeof(T), out LazyRegister lazyRegister))
        //    {
        //        lazyRegister = new LazyRegister<T>();
        //        uiRegisters.Add(typeof(T), lazyRegister);
        //    }
        //    return lazyRegister as LazyRegister<T>;
        //}
        //public static void WaitforUI<T>(Action<T> action) where T : class
        //{
        //    GetLazyRegister<T>().AddListen(action);
        //}
        #endregion
        #region 仅供于UI中很少使用的面板去生成或读取输入字段的值
        /// <summary>
        /// "Value"值自身准确类型
        /// 值如果继承了接口，并且InputField《接口》是以接口实现的泛型
        /// </summary>
        /// <remarks>
        /// 仅供于UI中很少使用的面板去生成或读取输入字段的值
        /// </remarks>
        /// <typeparam name="Value">值自身准确类型</typeparam>
        public static void InputFieldReadValue<Value>(object data, IInputField<Value>[] inputFields)
        {
            //Entry.Console.Info("输入字段的数量：" + inputFields.Length);
            if (inputFields.Length > 0)
            {
                System.Type dataType = data.GetType();
                System.Type valueType = typeof(Value);
                for (int i = 0; i < inputFields.Length; i++)
                {
                    FieldInfo fieldInfo = dataType.GetField(inputFields[i].Label);
                    if (fieldInfo != null && fieldInfo.FieldType == valueType && inputFields[i].GetValue() is Value v)//这里用is 是因为存在空值的意外可能
                    {
                        fieldInfo.SetValue(data, v); //此处需要用到fieldinfo，必须确定其类型正确
                    }
                    else
                    {
                        ConsoleCat.LogWarning(dataType.Name + "未能重新获取字段");
                    }
                }
            }
            else
            {
                ConsoleCat.LogWarning("输入字段数组为空");
            }
        }
        #endregion
    }
}