using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    // 数据提供者要作为数据的持有者?
    public class DataManager : IDataManager, IDataProvider
    {
        Dictionary<string, IDataCollection> dataDics;// 不一类型为键的话可以多种,并且允许接口
        //HashSet<IDataEventLinked> dirtys;
        public void Clear()
        {
            dataDics.Clear();
        }
        public DataManager()
        {
            dataDics = new Dictionary<string, IDataCollection>();
            //dirtys = new HashSet<IDataEventLinked>();
        }
        public DataManager(Dictionary<string, IDataCollection> dataDics)
        {
            this.dataDics = dataDics;
            //dirtys = new HashSet<IDataEventLinked>();
        }
        //public void CleanDirty()
        //{
        //    if (dirtys.Count != 0)
        //    {
        //        foreach (var dataEventLinked in dirtys)
        //        {
        //            dataEventLinked.NotifyChange();
        //        }
        //        dirtys.Clear();
        //    }
        //}
        //public void DirtyData(string key)
        //{
        //    dirtys.Add(dataDics[key]);
        //}
        public void AddDataCollection<T>(string key, T value) where T : class, IDataCollection
        {
            dataDics.Add(key, value);
        }
        public void RemoveDataCollection<T>(string key) where T : class, IDataCollection
        {
            dataDics.TryGetValue(key, out var v);
            if (v is T)
                dataDics.Remove(key);
            else
                Error(key, v.GetType(), typeof(T));
        }
        void Error(string key, Type original, Type n)
        {
            if (ConsoleCat.Enable) ConsoleCat.LogWarning($"键:{key}对应数据类型为:{original},并非{n}");
        }
        #region 数据的提供
        // 
        public T GetDataCollection<T>(string key) where T : class, IDataCollection
        {
            dataDics.TryGetValue(key, out var v);
            T t = v as T;
#if UNITY_EDITOR
            if (t == null) Error(typeof(T));
#endif
            return t;
        }
        public bool TryGetDataCollection<T>(string key, out T dataCollection) where T : class, IDataCollection
        {
            dataDics.TryGetValue(key, out var v);
            dataCollection = v as T;
#if UNITY_EDITOR
            if (dataCollection == null) Error(typeof(T));
#endif
            return dataCollection != null;
        }
        void Error(Type a)
        {
            if (ConsoleCat.Enable) ConsoleCat.LogWarning($"未获取到所需数据类型:{a};");
        }
        //public void AddListen<T>(string key, Action<object> action) where T : class
        //{
        //    if (TryGetDataEventLinked<T>(key, out var dataEventLinked))
        //        dataEventLinked.Change += action;
        //}
        //public void RemoveListen<T>(string key, Action<T> action)
        //{

        //}
        //public void RemoveListen<T>(string key, Action<object> action) where T : class
        //{
        //    if (TryGetDataEventLinked<T>(key, out var dataEventLinked))
        //        dataEventLinked.Change -= action;
        //}
        //// 不能指明类型,如果想要的类型是接口,但是因为添加集合的时候,类型是具体类型(非接口),转换的数据事件链是不对的
        //bool TryGetDataEventLinked<T>(string key, out IDataEventLinked<T> dataEventLinked)
        //{
        //    dataDics.TryGetValue(key, out var linked);
        //    dataEventLinked = linked as IDataEventLinked<T>;
        //    return dataEventLinked != null;
        //}
        #endregion
    }
}
