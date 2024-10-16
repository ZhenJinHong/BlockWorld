using CatFramework.DataMiao;
using CatFramework.EventsMiao;
using CatFramework.GameManager;
using CatFramework.InputMiao;
using CatFramework.Localized;
using CatFramework.UiMiao;
using CatFramework.UiTK;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework
{
    /// <summary>
    /// 框架自身不订阅任何事件（包括硬件输入），由调用者去订阅事件后调用框架
    /// </summary>
    public static class Entry
    {
        static readonly Dictionary<Type, Manager> managerMap;
        static Entry()
        {
            managerMap = new Dictionary<Type, Manager>();
        }
        static bool isActive;
        /// <summary>
        /// 这个应当由游戏管理器设置；程序是否活动状态，编辑器下使用
        /// </summary>
        public static bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    if (isActive)
                    {
                        ConsoleCat.Start();
                        DataManagerMiao.Start();
                        EventManagerMiao.Start();
                        InputManagerMiao.Start();
                        GameManagerMiao.Start();
                        LocalizedManagerMiao.Start();
                        UITKManagerMiao.Start();
                        UiManagerMiao.Start();
                        FrameSetting.ApplySetting(DataManagerMiao.LoadOrCreateSetting<FrameSetting>());
                        loopComplete = true;
                        foreach (var manager in managerMap.Values)
                        {
                            manager.Start();
                        }
#if UNITY_EDITOR
                        Debug.Log("管理已开机，当前管理器数：" + managerMap.Count);
#endif
                    }
                    else
                    {
                        foreach (Manager manager in managerMap.Values)
                        {
                            manager.ShutDown();
                        }
                        UiManagerMiao.Shutdown();
                        UITKManagerMiao.ShutDown();
                        LocalizedManagerMiao.Shutdown();
                        GameManagerMiao.ShutDown();
                        InputManagerMiao.ShutDown();
                        EventManagerMiao.Shutdown();
                        DataManagerMiao.ShutDown();
                        ConsoleCat.ShutDown();

#if UNITY_EDITOR
                        Debug.Log("管理已关机，当前管理器数：" + managerMap.Count);
#endif
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning("重复开关");
#endif
                }
            }
        }
        static bool loopComplete;
        public static void BeginLoop()
        {
#if UNITY_EDITOR
            if (!loopComplete)// 重复调用设置false时,发生
            {
                Debug.LogWarning("循环重复");
            }
#endif
            loopComplete = false;
        }
        public static void Update()
        {
            if (!isActive) return;
            if (!loopComplete)
            {
                UiManagerMiao.Update();
            }
        }
        public static void LateUpdate()
        {
            if (!isActive) return;
            if (!loopComplete)
            {
                UiManagerMiao.LateUpdate();
            }
        }
        public static void CompleteLoop()
        {
            loopComplete = true;
        }
        public static T GetManager<T>() where T : Manager, new()
        {
            return GetManager(typeof(T)) as T;
        }
        static Manager GetManager(Type type)
        {
            if (!managerMap.TryGetValue(type, out Manager manager))
            {
                manager = Activator.CreateInstance(type) as Manager;
                managerMap.Add(type, manager);
            }
            return manager;
        }
    }
}
