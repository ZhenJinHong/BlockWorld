using System;
using System.Collections.Generic;

namespace CatFramework.EventsMiao
{
    public static class EventManagerMiao
    {
        static readonly Dictionary<Type, IUniqueEvents> uniqueEventMap;
        //static readonly Dictionary<Type, IUniquePublisher> uniquePublisherMap;
        static readonly Dictionary<string, ILazyRegister> lazyRegisterMap;
        static EventManagerMiao()
        {
            uniqueEventMap = new Dictionary<Type, IUniqueEvents>();
            //uniquePublisherMap = new Dictionary<Type, IUniquePublisher>();
            lazyRegisterMap = new Dictionary<string, ILazyRegister>();
        }
        public static void Start()
        {
            FunctionNameEventCollection.Start();
            DataBindingEventManager.Start();
        }
        public static void Shutdown()
        {
            if (ConsoleCat.IsDebug)
            {
                ConsoleCat.Log($"清理唯一事件数量 : {uniqueEventMap.Count}");
                ConsoleCat.Log($"清理懒加载器数量 : {lazyRegisterMap.Count}");
                //foreach (var item in lazyRegisterMap)
                //{
                //    var value = item.Value;
                //    if (!value.HasValue)
                //    {
                //        ConsoleCat.LogWarning("懒加载器:" + item.Key + ";未等到其所需的内容:" + value.ContentType);
                //    }
                //}这个是允许取消登记的,取消后,也就没了这个东西
            }
            uniqueEventMap.Clear();
            //uniquePublisherMap.Clear();
            lazyRegisterMap.Clear();
            FunctionNameEventCollection.Shutdown();
            DataBindingEventManager.Shutdown();
        }

        public static T GetEvents<T>() where T : class, IUniqueEvents, new()
        {
            return GetUniqueEvents(typeof(T)) as T;
        }
        static IUniqueEvents GetUniqueEvents(Type type)
        {
            if (!uniqueEventMap.TryGetValue(type, out IUniqueEvents uniqueEvents))
            {
                uniqueEvents = Activator.CreateInstance(type) as IUniqueEvents;
                uniqueEventMap.Add(type, uniqueEvents);
            }
            return uniqueEvents;
        }
        //public static T GetPulisher<T>() where T : class, IUniquePublisher, new()
        //{
        //    return GetUniquePublisher(typeof(T)) as T;
        //}
        //static IUniquePublisher GetUniquePublisher(Type type)
        //{
        //    if (!uniquePublisherMap.TryGetValue(type, out IUniquePublisher uniquePublisher))
        //    {
        //        uniquePublisher = Activator.CreateInstance(type) as IUniquePublisher;
        //        uniquePublisherMap.Add(type, uniquePublisher);
        //    }
        //    return uniquePublisher;
        //}
        #region 懒加载器
        public static T GetLazyObject<T>(string key) where T : class
        {
            TryGetLazyRegister<T>(key, out var tR);
            return tR?.GetValue();
        }
        public static void Waiting<T>(string key, Action<T> callBack) where T : class
        {
            GetLazyRegister<T>(key)?.AddListen(callBack);
        }
        public static void CancelWaiting<T>(string key, Action<T> callBack) where T : class
        {
            if (TryGetLazyRegister<T>(key, out var tR))
            {
                tR.RemoveListen(callBack);
            }
        }
        public static void RegisterLazyObject<T>(string key, T obj) where T : class
        {
            GetLazyRegister<T>(key)?.Register(obj);
        }
        public static void UnregisterLazyObject<T>(string key, T obj) where T : class
        {
            GetLazyRegister<T>(key)?.Unregister(obj);
        }
        static bool TryGetLazyRegister<T>(string key, out LazyRegister<T> obj) where T : class
        {
            if (lazyRegisterMap.Count == 0 || (!lazyRegisterMap.TryGetValue(key, out var lazyRegister)) || lazyRegister is not LazyRegister<T> tR)
            {
                obj = null;
                return false;
            }
            obj = tR;
            return true;
        }
        static LazyRegister<T> GetLazyRegister<T>(string key) where T : class
        {
            if (lazyRegisterMap.TryGetValue(key, out ILazyRegister lazyRegister))
            {
                if (lazyRegister is LazyRegister<T> tR)
                {
                    return tR;
                }
                else
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning($"懒加载器键--{key}--已被使用,类型为--{lazyRegister.ContentType}--");
                    return null;
                }
            }
            else
            {
                LazyRegister<T> tR = new LazyRegister<T>();
                lazyRegisterMap.Add(key, tR);
                return tR;
            }
        }
        #endregion
        //public static T GetEventss<T>()where T :data
        //{
        //    UniqueEvents<T>.TypeID //针对数据类型唯一时,那为什么不用数据的Type?
        //}
        #region 扩展
        public static void ListenEvent<Events, Data>(this IGameEventsListener<Events, Data> eventListen)
            where Data : class
            where Events : class, IGameEvents<Data>, new()
        {
            var gameEvents = GetEvents<Events>();
            gameEvents.OnEnter += eventListen.Enter;
            gameEvents.OnExit += eventListen.Exit;
            gameEvents.OnContinue += eventListen.Continue;
            gameEvents.OnPause += eventListen.Pause;
        }
        public static void RemoveListen<Events, Data>(this IGameEventsListener<Events, Data> eventListen)
            where Data : class
            where Events : class, IGameEvents<Data>, new()
        {
            var gameEvents = GetEvents<Events>();
            gameEvents.OnEnter -= eventListen.Enter;
            gameEvents.OnExit -= eventListen.Exit;
            gameEvents.OnContinue -= eventListen.Continue;
            gameEvents.OnPause -= eventListen.Pause;
        }
        #endregion
    }
}