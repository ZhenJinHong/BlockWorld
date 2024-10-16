using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework
{
    public static class ItemStorageCollectionExtension
    {
        public static void FindEmptySpaces<T>(List<int> spaces, T[] items)
        {
            spaces.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    spaces.Add(i);
            }
        }
        public static void MergedSameKind<T>(T[] items, int index)
            where T : class, IItemStorage
        {
            if (items[index] is IMergedable<T> item)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (index != i &&
                        items[i] is IMergedable<T> &&
                        item.TryMerged(items[i], out var exessItem))
                    {
                        items[i] = exessItem;
                        if (exessItem != null)
                            break;
                    }
                }
            }
        }
        // 不检查空
        public static void MergeOrSwapItemInList<T>(IItemStorageCollection itemStorageCollection, IList<T> items, int selectedIndex, int targetIndex)
            where T : IItemStorage
        {
            if (itemStorageCollection.IndexInRange(selectedIndex) && itemStorageCollection.IndexInRange(targetIndex))
            {
                T t = items[targetIndex];
                if (t is IMergedable<T> mergedable && mergedable.TryMerged(items[selectedIndex], out T ret))
                {
                    items[selectedIndex] = ret;
                }
                else
                {
                    items[targetIndex] = items[selectedIndex];
                    items[selectedIndex] = t;
                }
            }
            else IndexOutofBounds(itemStorageCollection, selectedIndex, targetIndex);
        }
        // 不检查空
        public static void SwapItemInList<T>(IItemStorageCollection itemStorageCollection, IList<T> items, int selectedIndex, int targetIndex)
        {
            if (itemStorageCollection.IndexInRange(selectedIndex) && itemStorageCollection.IndexInRange(targetIndex))
            {
                T t = items[targetIndex];
                items[targetIndex] = items[selectedIndex];
                items[selectedIndex] = t;
            }
            else IndexOutofBounds(itemStorageCollection, selectedIndex, targetIndex);
        }
        // 不检查空
        public static void SwapOrMoveToLastInList<T>(IItemStorageCollection itemStorageCollection, IList<T> items, int selectedIndex, int targetIndex)
        {
            if (itemStorageCollection.IndexInRange(selectedIndex))
            {
                T s = items[selectedIndex];
                if (itemStorageCollection.IndexInRange(targetIndex))
                {
                    items[selectedIndex] = items[targetIndex];
                    items[targetIndex] = s;
                }
                else
                {
                    items.RemoveAt(selectedIndex);
                    items.Add(s);
                }
            }
        }
        static void IndexOutofBounds(IItemStorageCollection itemStorageCollection, int selectedIndex, int targetIndex)
        {
            if (ConsoleCat.Enable)
                ConsoleCat.LogWarning($"交换集合:{itemStorageCollection}内的物品时,索引溢出->选定的:{selectedIndex},目标的:{targetIndex}");
        }
        public static void TryMergedOrSetItem<T>(IList<T> items, T item, int index, out T ret)
            where T : class, IItemStorage
        {
            if (index > -1 && index < items.Count)
            {
                T target = items[index];
                if (target is IMergedable<T> m && m.TryMerged(item, out ret))
                {
                    // 不考虑item是否是自身的;将item合并到目标
                }
                else
                {
                    items[index] = item;
                    ret = target;
                }
            }
            else
            {
                ret = item;
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning("指向的目标索引溢出");
            }
        }
        // set的定义替换目标,但这个有合并,要求非空
        public static void TryMergedOrSetItem<Parent, Child>(IItemStorageCollection<Parent> itemStorageCollection, IList<Child> items, Parent item, int index, out Parent ret)
            where Parent : class, IItemStorage
            where Child : class, Parent
        {
            Assert.IsNull(item);
            if (item is Child child)
            {
                TryMergedOrSetItem<Child>(items, child, index, out var ret1);
                ret = ret1;
                //if (itemStorageCollection.IndexInRange(index))
                //{
                //    Child target = items[index];
                //    if (target is IMergedable<Child> mergedable && mergedable.TryMerged(child, out var retc))
                //    {
                //        ret = retc;
                //    }
                //    else
                //    {
                //        ret = target;
                //        items[index] = child;
                //    }
                //}
                //else
                //{
                //    ret = item;
                //    if (ConsoleCat.Enable)
                //        ConsoleCat.LogWarning("指向的目标索引溢出");
                //}
            }
            //else if (Assert.IsNull(item)) 这个是setItem,允许直接设置到目标上null,至于集合自身是否允许,集合自己搞定
            //{
            //    itemStorageCollection.RemoveItemAt(index);// 对于紧凑列表,不能放置空,所以如果想放置空的索引位置在范围内,应将其移除
            //    ret = item;
            //}
            else
            {
                //if (itemStorageCollection.IndexInRange(index))
                //{
                //    items[index] = null;
                //}
                ret = item;
            }

        }
    }
}
