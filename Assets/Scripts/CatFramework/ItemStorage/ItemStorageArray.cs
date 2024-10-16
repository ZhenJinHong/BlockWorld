using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework
{
    public class ItemStorageArray<T> : IItemStorageCollection<T>/*, IEnableSpecifyIndexItemStorageCollection<T>*/
        where T : class, IItemStorage
    {
        public ItemStorageArray(int capacity)
        {
            Items = new T[capacity];
        }
        public ItemStorageArray(T[] items)
        {
            Items = items;
        }
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
        public T[] Items { get; private set; }
        public bool InventoryIsLocked { get; set; }
        public int CurrentSelectedIndex { get; set; }
        public int Count => Items.Length;
        public T Current => IndexInRange(CurrentSelectedIndex) ? Items[CurrentSelectedIndex] : null;
        public T GetItem(int index) => Items[index];
        public void SetItem(T item, int index) => Items[index] = item;
        public bool IndexInRange(int targetIndex) => targetIndex > -1 && targetIndex < Items.Length;
        public bool IndexIsEmpty(int targetIndex) => Items[targetIndex] == default;
        public bool ItemMatchType(T item) => item != null;
        public void RemoveItemAt(int index) => Items[index] = default;
        public void SwapItemInInternal(int selectedIndex, int targetIndex)
        {
            ItemStorageCollectionExtension.MergeOrSwapItemInList(this, Items, selectedIndex, targetIndex);
        }
        public void AddItem(T item)
        {
            int index = Array.IndexOf(Items, default);
            if (index != -1)
                Items[index] = item;
        }
        public void TryAddItem(T item, out T ret)
        {
            int index = Array.IndexOf(Items, default);
            if (index != -1)
            {
                Items[index] = item;
                ret = null;
            }
            else ret = item;
        }
        public void TryMergedOrSetItem(T item, int index, out T ret)
        {
            ItemStorageCollectionExtension.TryMergedOrSetItem(Items, item, index, out ret);
        }

        public int FindEmptySpace()
        {
            return Array.IndexOf(Items, null);
        }
        public void FindEmptySpaces(List<int> indexs)
        {
            ItemStorageCollectionExtension.FindEmptySpaces(indexs, Items);
        }
        public void MergedSameKind(int index)
        {
            ItemStorageCollectionExtension.MergedSameKind(Items, index);
        }
        public T Find(Predicate<T> match)
        {
            return Array.Find(Items, match);
        }
        public int FindIndex(Predicate<T> match)
        {
            return Array.FindIndex(Items, match);
        }
    }
    // 注意的是,父类提供给UI使用,子类是具体类型,但子类可以和父类相同,这样任何实现父类的都可以放置进来,那么子类(父类)可合并的情况下就需要实现IMergedable<Parent>
    public class ItemStorageArray<Parent, Child> : IEnumerable, IItemStorageCollection<Parent>/*, IEnableSpecifyIndexItemStorageCollection<Parent>*/
        where Parent : class, IItemStorage
        where Child : class, Parent
    {
        public Child[] Items { get; private set; }
        public Child this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
        public Child Current => IndexInRange(CurrentSelectedIndex) ? Items[CurrentSelectedIndex] : null;
        public ItemStorageArray(int length)
        {
            Items = new Child[length];
        }
        #region 常规方法
        public bool InventoryIsLocked { get; set; }
        public int Count => Items.Length;
        public bool IndexInRange(int itemIndex)
        {
            return itemIndex > -1 && itemIndex < Items.Length;
        }
        public bool IndexIsEmpty(int targetIndex)
        {
            return Items[targetIndex] == null;
        }
        public Parent GetItem(int index) => Items[index];

        public void PutToTarget(Child commonStorage, int itemIndex)
        {
            Items[itemIndex] = commonStorage;
        }
        #endregion
        public int FindEmptySpace()
        {
            return Array.IndexOf(Items, null);
        }
        public void FindEmptySpaces(List<int> indexs)
        {
            ItemStorageCollectionExtension.FindEmptySpaces(indexs, Items);
        }
        public void MergedSameKind(int index)
        {
            ItemStorageCollectionExtension.MergedSameKind(Items, index);
        }
        #region 视图回调
        public int CurrentSelectedIndex { get; set; }
        public bool ItemMatchType(Parent ulatticeItem)
        {
            return ulatticeItem is not null;
        }
        public void SwapItemInInternal(int selectedIndex, int targetIndex)
        {
            ItemStorageCollectionExtension.MergeOrSwapItemInList<Parent>(this, Items, selectedIndex, targetIndex);
        }
        public void RemoveItemAt(int itemIndex)
        {
            //if (IndexInRange(itemIndex))
                Items[itemIndex] = null;
        }
        public void SetItem(Parent item, int index)
        {
            //if (IndexInRange(index))
            Items[index] = item as Child;// 无论如何都替换掉
        }
        public void TryMergedOrSetItem(Parent item, int index, out Parent ret)
        {
            ItemStorageCollectionExtension.TryMergedOrSetItem<Parent, Child>(this, Items, item, index, out ret);
        }
        public void AddItem(Parent item)
        {
            if (item is Child child)
            {
                int index = FindEmptySpace();
                if (index != -1)
                {
                    Items[index] = child;
                }
            }
        }
        public void TryAddItem(Parent item, out Parent ret)
        {
            if (item is Child child)
            {
                int index = FindEmptySpace();
                if (index != -1)
                {
                    Items[index] = child;
                    ret = null;
                }
                else
                    ret = item;
            }
            else
            {
                ret = item;
            }
        }
        public Parent Find(Predicate<Parent> match)
        {
            return Array.Find(Items, match);
        }
        public int FindIndex(Predicate<Parent> match)
        {
            return Array.FindIndex(Items, match);
        }
        #endregion
        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
