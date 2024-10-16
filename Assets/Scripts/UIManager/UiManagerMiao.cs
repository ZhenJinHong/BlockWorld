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
        #region ��ͼ����
        public static void AddDirtyView(IEnableDirtyView enableDirtyView)
        {
            if (ConsoleCat.IsDebug && (isShutdown || enableDirtyViews.Contains(enableDirtyView)))
            {
                ConsoleCat.DebugWarning("��ǰ�ѹػ��������ͬDirtyView");
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
        #region ��ʽ�Ǽ����ȡ
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
        #region UI�Ǽ����ȡ
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
        #region ������UI�к���ʹ�õ����ȥ���ɻ��ȡ�����ֶε�ֵ
        /// <summary>
        /// "Value"ֵ����׼ȷ����
        /// ֵ����̳��˽ӿڣ�����InputField���ӿڡ����Խӿ�ʵ�ֵķ���
        /// </summary>
        /// <remarks>
        /// ������UI�к���ʹ�õ����ȥ���ɻ��ȡ�����ֶε�ֵ
        /// </remarks>
        /// <typeparam name="Value">ֵ����׼ȷ����</typeparam>
        public static void InputFieldReadValue<Value>(object data, IInputField<Value>[] inputFields)
        {
            //Entry.Console.Info("�����ֶε�������" + inputFields.Length);
            if (inputFields.Length > 0)
            {
                System.Type dataType = data.GetType();
                System.Type valueType = typeof(Value);
                for (int i = 0; i < inputFields.Length; i++)
                {
                    FieldInfo fieldInfo = dataType.GetField(inputFields[i].Label);
                    if (fieldInfo != null && fieldInfo.FieldType == valueType && inputFields[i].GetValue() is Value v)//������is ����Ϊ���ڿ�ֵ���������
                    {
                        fieldInfo.SetValue(data, v); //�˴���Ҫ�õ�fieldinfo������ȷ����������ȷ
                    }
                    else
                    {
                        ConsoleCat.LogWarning(dataType.Name + "δ�����»�ȡ�ֶ�");
                    }
                }
            }
            else
            {
                ConsoleCat.LogWarning("�����ֶ�����Ϊ��");
            }
        }
        #endregion
    }
}