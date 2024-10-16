using System;
using System.Collections.Generic;

namespace CatFramework.EventsMiao
{
    public static class FunctionNameEventCollection
    {

        static Dictionary<string, Dictionary<Type, GenericsEvent>> eventMap;
        static FunctionNameEventCollection()
        {
            eventMap = new Dictionary<string, Dictionary<Type, GenericsEvent>>();
        }
        internal static void Start()
        {

        }
        internal static void Shutdown()
        {
            eventMap.Clear();
        }
        // 方法内的局部函数的名称非常奇怪,所以这里不允许空了,否则取到的函数名非常怪
        public static void Register<T0>(EventMiao<T0> eventMiao, string functionName) where T0 : class
        {
            GetOrNew<T0>(functionName).Action += eventMiao;
        }
        public static void Unregister<T0>(EventMiao<T0> eventMiao, string functionName) where T0 : class
        {
            if (TryGet<T0>(functionName, out var genericsEvent))
                genericsEvent.Action -= eventMiao;
        }
        public static void Invoke<T0>(object sender, T0 arg0, string functionName) where T0 : class
        {
            if (TryGet<T0>(functionName, out var genericsEvent))
                genericsEvent.Invoke(sender, arg0);
        }
        static GenericsEvent<T0> GetOrNew<T0>(string functionName) where T0 : class
        {
            if (!eventMap.TryGetValue(functionName, out var tMap))
            {
                tMap = new Dictionary<Type, GenericsEvent>();
                eventMap.Add(functionName, tMap);
            }
            if (!tMap.TryGetValue(typeof(T0), out GenericsEvent e))
            {
                e = new GenericsEvent<T0>();
                tMap.Add(typeof(T0), e);
            }
            return (e as GenericsEvent<T0>);
        }
        static bool TryGet<T0>(string functionName, out GenericsEvent<T0> genericsEvent) where T0 : class
        {
            if (eventMap.TryGetValue(functionName, out var tMap) && tMap.TryGetValue(typeof(T0), out GenericsEvent e))
            {
                genericsEvent = e as GenericsEvent<T0>;
            }
            else genericsEvent = null;
            return genericsEvent != null;
        }
    }
}