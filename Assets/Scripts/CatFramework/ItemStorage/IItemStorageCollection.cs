using System;

namespace CatFramework
{
    // 对于物品集合,模块知道的键只有index
    // 允许指定索引时应当允许置换目标位置的格子内容
    public interface IItemStorageCollection
    {
        int CurrentSelectedIndex { get; set; }
        int Count { get; }
        bool IndexInRange(int targetIndex);
        bool IndexIsEmpty(int targetIndex);
    }
    //public interface IReadonlyItemStorageCollection
    //{
    //    object GetItem(int index);
    //}
    /// <summary>
    /// 只读是针对玩家的操作Ui之类而言,如果游戏程序本身有需要根据游戏进度更新集合,可以修改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadonlyItemStorageCollection<T> : IItemStorageCollection
    {
        T GetItem(int index);
    }
    public interface IItemStorageCollection<T> : IItemStorageCollection, IReadonlyItemStorageCollection<T>
    {
        bool InventoryIsLocked { get; }
        //void RemoveItemAt(int index);// 去掉,直接setnull,对于不允许空的库存由库存自己决定是设置空,还是remove//但是这样就得set两次,一次空,一次放置换过来的物品
        void AddItem(T item);
        void TryAddItem(T item, out T ret);// 必要的,用以在如果没有拖到格子上即无指定索引时使用
        void SwapItemInInternal(int selectedIndex, int targetIndex);
        bool ItemMatchType(T item);
        /// <summary>
        /// set当为直接替换
        /// </summary>
        void SetItem(T item, int index);
        /// <summary>
        /// 可合并则合并,可放置则替换,否则丢回
        /// </summary>
        void TryMergedOrSetItem(T item, int index, out T ret);
    }
    ///// <summary>
    ///// 允许指定目标索引为空
    ///// </summary>
    //public interface IEnableSpecifyIndexItemStorageCollection<T>
    //{
    //    /// <summary>
    //    /// set当为直接替换
    //    /// </summary>
    //    void SetItem(T item, int index);
    //    /// <summary>
    //    /// 可合并则合并,可放置则替换,否则丢回
    //    /// </summary>
    //    void TryMergedOrSetItem(T item, int index, out T ret);
    //}
}
