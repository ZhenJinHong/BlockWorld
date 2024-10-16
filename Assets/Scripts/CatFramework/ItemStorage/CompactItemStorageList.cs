using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework
{
    public class CompactItemStorageList<T> : List<T>, IItemStorageCollection<T>
        where T : class, IItemStorage
    {
        public bool InventoryIsLocked { get; set; }
        public int CurrentSelectedIndex { get; set; }

        public T GetItem(int index) => base[index];
        public bool IndexInRange(int targetIndex) => targetIndex > -1 && targetIndex < Count;
        public bool IndexIsEmpty(int targetIndex) => base[targetIndex] == null ? throw new System.NullReferenceException() : false;
        public bool ItemMatchType(T item) => item != null;
        public void RemoveItemAt(int index) => base.RemoveAt(index);
        public void SwapItemInInternal(int selectedIndex, int targetIndex) => ItemStorageCollectionExtension.SwapOrMoveToLastInList(this, this, selectedIndex, targetIndex);
        public void SetItem(T item, int index) => base[index] = item;
        public void TryMergedOrSetItem(T item, int index, out T ret)
        {
            ItemStorageCollectionExtension.TryMergedOrSetItem<T>(this, item, index, out ret);
        }
        public void AddItem(T item)
        {
            if (item != null)
                base.Add(item);
        }
        public void TryAddItem(T item, out T ret)
        {
            if (item != null)
            {
                base.Add(item);
            }
            ret = null;// 非空添加了则余空,否则也空
        }
    }
    /// <summary>
    /// 紧凑的物品储存列表,不会存null
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    /// <typeparam name="Child"></typeparam>
    public class CompactItemStorageList<Parent, Child> : List<Child>, IItemStorageCollection<Parent>/*, IEnableSpecifyIndexItemStorageCollection<Parent>*/
        where Parent : class, IItemStorage
        where Child : class, Parent
    {
        public int CurrentSelectedIndex { get; set; }
        public Child CurrentItem => IndexInRange(CurrentSelectedIndex) ? this[CurrentSelectedIndex] : null;
        public CompactItemStorageList() { }
        public CompactItemStorageList(int capacity) : base(capacity) { }
        #region 常规
        public bool InventoryIsLocked { get; set; }
        public Parent GetItem(int index) => this[index];
        public bool IndexInRange(int index) => index > -1 && index < base.Count;
        public bool IndexIsEmpty(int index) => base[index] == null ? throw new System.NullReferenceException() : false;
        public bool ItemMatchType(Parent item) => item is Child;
        #endregion
        public void AddItem(Child child)
        {
            if (child != null)
                base.Add(child);
        }
        public void RemoveItemAt(int index)
        {
            //if (IndexInRange(index))
            base.RemoveAt(index);
        }
        public void SwapItemInInternal(int selectedIndex, int targetIndex)
        {
            ItemStorageCollectionExtension.SwapOrMoveToLastInList<Child>(this, this, selectedIndex, targetIndex);
        }
        public void SetItem(Parent item, int index)
        {
            //if (IndexInRange(index))
            {
                if (item is Child child)
                    base[index] = child;
                else
                    base.RemoveAt(index);
            }
        }
        public void TryMergedOrSetItem(Parent item, int index, out Parent ret)
        {
            //if (item == null)
            //{
            //    if (IndexInRange(index))
            //        base.RemoveAt(index);
            //    ret = item;
            //    return;
            //}
            ItemStorageCollectionExtension.TryMergedOrSetItem<Parent, Child>(this, this, item, index, out ret);
        }
        public void AddItem(Parent item)
        {
            if (item is Child child)
            {
                base.Add(child);
            }
        }

        public void TryAddItem(Parent item, out Parent ret)
        {
            if (item is Child child)
            {
                base.Add(child);
                ret = null;
            }
            else
                ret = item;
        }
    }
}
