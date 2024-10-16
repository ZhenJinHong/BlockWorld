using CatFramework.EventsMiao;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    // Panel如果主视图不随着开关，容易忘了主视图自身关了，却未打开，就算要去掉其中一种，也应该去掉root的开关，换之只开关主视图
    // 但root也跟着开关的消耗根本可以忽视，所以没必要去修改
    // 需要在开关时另行执行逻辑的，去监听事件
    // 定义所有视觉元素开关行为为OpenOrClose为执行通知行为，ShowOrHide为不通知的开关

    // Open和Close事件应当只用来面板逻辑上，一个面板通知另一个面板
    public static class UITKManagerMiao
    {
        class Events
        {
        }
        static Events events;
        //static readonly Dictionary<Type, LazyRegister> uiRegisters;
        static UITKManagerMiao()
        {
            events = new Events();
            //uiRegisters = new Dictionary<Type, LazyRegister>();
        }
        public static void Start()
        {
        }
        public static void ShutDown()
        {
            events = new Events();
            //uiRegisters.Clear();
        }
        //public static void RegisterUI<T>(T panel) where T : Panel
        //{
        //    GetLazyRegister<T>().Register(panel);
        //}
        //public static void Unregister<T>(T panel) where T : Panel
        //{

        //}
        //public static LazyRegister<T> GetLazyRegister<T>() where T : Panel
        //{
        //    if (!uiRegisters.TryGetValue(typeof(T), out LazyRegister lazyRegister))
        //    {
        //        lazyRegister = new LazyRegister<T>();
        //        uiRegisters.Add(typeof(T), lazyRegister);
        //    }
        //    return lazyRegister as LazyRegister<T>;
        //}
        //public static void RegisterGetUI<T>(Action<T> action) where T : Panel
        //{
        //    GetLazyRegister<T>().AddListen(action);
        //}
    }
}
