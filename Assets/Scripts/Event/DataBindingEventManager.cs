using CatFramework.Tools;
using System.Collections.Generic;

namespace CatFramework.EventsMiao
{
    public static class DataBindingEventManager
    {
        static readonly Dictionary<object, GenericsEvent<object>> map;
        static readonly Pool<GenericsEvent<object>> pool;
        static DataBindingEventManager()
        {
            map = new Dictionary<object, GenericsEvent<object>>();
            pool = new Pool<GenericsEvent<object>>(new CommonPoolItemProvider<GenericsEvent<object>>(), 4, 16);
        }
        internal static void Start() { }
        internal static void Shutdown()
        {
            PrintInfo();
            map.Clear();
            pool.Clear();
        }
        public static void PrintInfo()
        {
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log("事件池:" + pool);
                ConsoleCat.Log($"已绑定的数量 : {map.Count}");
            }
        }
        public static void Bind(object args, EventMiao<object> action)
        {
            if (Assert.IsNull(args)) return;
            if (Assert.IsNull(action)) return;
            if (!map.TryGetValue(args, out var genericsEvent))
            {
                genericsEvent = pool.Get();
                map.Add(args, genericsEvent);
            }
            genericsEvent.Action += action;
        }
        public static void UnBind(object args, EventMiao<object> eventMiao)
        {
            if (Assert.IsNull(args)) return;
            if (map.TryGetValue(args, out var genericsEvent))
            {
                genericsEvent.Action -= eventMiao;
                if (genericsEvent.EventIsEmpty)
                {
                    map.Remove(args);
                    pool.Repaid(genericsEvent);
                }
            }
        }
        public static void Invoke(object sender, object args)
        {
            if (Assert.IsNull(args)) return;
            if (map.TryGetValue(args, out var genericsEvent))
            {
                genericsEvent.Invoke(sender, args);
            }
        }
        public static void NotifyChange(this IItemStorageCollection itemStorageCollection, object sender = null)
        {
            Invoke(sender, itemStorageCollection);
        }
    }
}