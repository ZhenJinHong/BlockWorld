using CatFramework.SLMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CatFramework.DataMiao
{
    public static class DataManagerMiao
    {
        //public class Instance : IDataManager
        //{
        //    public void RegisterDataCollection<Collection>(Collection dataCollection) where Collection : class, IDataCollection
        //        => DataManagerMiao.RegisterDataCollection<Collection>(dataCollection);
        //    public Collection GetDataCollection<Collection>() where Collection : class, IDataCollection, new()
        //        => DataManagerMiao.GetDataCollection<Collection>();
        //    public bool TryGetDataCollection<Collection>(out Collection collection) where Collection : class, IDataCollection
        //        => DataManagerMiao.TryGetDataCollection<Collection>(out collection);
        //    public Setting LoadOrCreateSetting<Setting>() where Setting : class, ISetting, new()
        //        => DataManagerMiao.LoadOrCreateSetting<Setting>();
        //}
        static readonly Dictionary<Type, IDataCollection> DataCollectionDic;
        static readonly Dictionary<Type, IDataEventLinked> SettingDic;
        //static readonly Instance instance = new Instance();
        //public static Instance GetInstance() => instance;
        public static IReadOnlyDictionary<Type, IDataEventLinked> GetReadonly()
            => SettingDic;
        static DataManagerMiao()
        {
            DataCollectionDic = new Dictionary<Type, IDataCollection>();
            SettingDic = new Dictionary<Type, IDataEventLinked>();
        }
        public static void NotifyApplyAllSetting()
        {
            foreach (var setting in SettingDic.Values)
            {
                (setting.Value as ISetting).ApplySetting();// 不打问号,如空,则报错
                setting.NotifyChange();
            }
        }
        public static void ResetAllSetting()
        {
            foreach (var setting in SettingDic.Values)
            {
                (setting.Value as ISetting).ResetSetting();
            }
        }
        public static void Start()
        {

        }
        public static void ShutDown()
        {
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log($"清理集合数量:{DataCollectionDic.Count}\n清理设置数量:{SettingDic.Count}");
            }
            foreach (var item in DataCollectionDic.Values)
            {
                item.Dispose();
            }
            //必须清理，否则会有事件一直引用
            DataCollectionDic.Clear();//编辑器状态下//由于其他管理器不会被垃圾回收，所以这里管理器引用的集合也不会被回收，所以管理器不应引用集合
            SettingDic.Clear();
        }
        #region
        public static void RegisterDataCollection<Collection>(Collection dataCollection) where Collection : class, IDataCollection
        {
            TryAddCollection(typeof(Collection), dataCollection);
        }
        public static Collection GetDataCollection<Collection>() where Collection : class, IDataCollection, new()
        {
            Type type = typeof(Collection);
            Collection collection;
            if (DataCollectionDic.TryGetValue(type, out IDataCollection original))
            {
                collection = original as Collection;
            }
            else
            {
                collection = new Collection();
                DataCollectionDic.Add(type, collection);
            }
            return collection;
        }
        static void TryAddCollection(Type type, IDataCollection collection)
        {
            if (!DataCollectionDic.TryAdd(type, collection))
            {
                ConsoleCat.LogWarning("集合重复添加，名称：" + type.FullName);
            }
        }
        public static bool TryGetDataCollection<Collection>(out Collection collection) where Collection : class, IDataCollection
        {
            DataCollectionDic.TryGetValue(typeof(Collection), out IDataCollection dataCollection);
            collection = dataCollection as Collection;
            return collection != null;
        }
        // 自己移除的则释放要自己调用
        //[Obsolete]
        public static bool RemoveCollection<Collection>() where Collection : class, IDataCollection
            => DataCollectionDic.Remove(typeof(Collection));
        //=> false;// 得做身份验证,验证其是否是这个数据集合的拥有者
        #endregion
        #region
        /// <summary>
        /// 路径默认为所有存档父级路径C_Save/type.Name + ".cgst"
        /// </summary>
        public static Setting LoadOrCreateSetting<Setting>() where Setting : class, ISetting, new()
        {
            return LoadOrCreateSettingEventLinked<Setting>().TValue;
        }
        static DataEventLinked<Setting> LoadOrCreateSettingEventLinked<Setting>() where Setting : class, ISetting, new()
        {
            Type type = typeof(Setting);
            DataEventLinked<Setting> settingEventLinked;
            if (SettingDic.TryGetValue(type, out IDataEventLinked dataEventLinked))
            {
                settingEventLinked = dataEventLinked as DataEventLinked<Setting>;
            }
            else
            {
                if (SerializationUniJsonSafe.LoadFormArchivePath(type, type.Name + ".cgst") is Setting t)
                    settingEventLinked = new DataEventLinked<Setting>(t);
                else
                    settingEventLinked = new DataEventLinked<Setting>(new Setting());
                SettingDic.Add(type, settingEventLinked);
            }
            return settingEventLinked;
        }
        /// <summary>
        /// 路径默认为所有存档父级路径C_Save/type.Name + ".cgst"
        /// </summary>
        public static void SaveSetting(this ISetting setting)
        {
            SerializationUniJsonSafe.SaveToArchivePath(setting, setting.GetType().Name + ".cgst");
        }
        public static Setting ListenSettingChange<Setting>(Action<Setting> action) where Setting : class, ISetting, new()
        {
            DataEventLinked<Setting> dataEventLinked = LoadOrCreateSettingEventLinked<Setting>();
            dataEventLinked.Change += action;
            return dataEventLinked.TValue;
        }
        public static void RemoveListenSettingChange<Setting>(Action<Setting> action) where Setting : class, ISetting, new()
        {
            if (SettingDic.TryGetValue(typeof(Setting), out IDataEventLinked dataEventLinked))
                (dataEventLinked as DataEventLinked<Setting>).Change -= action;
        }
        public static void NotifySettingChange(this ISetting setting)
        {
            if (SettingDic.TryGetValue(setting.GetType(), out IDataEventLinked dataEventLinked))
                dataEventLinked.NotifyChange();
        }
        #endregion
    }
}
