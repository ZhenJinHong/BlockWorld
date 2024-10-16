using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.Magics;
using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorld
{
    public class CommonInventory : ItemStorageArray<IUlatticeItemStorage>
    {
        public CommonInventory(int pageheight = 4, int pageWidth = 9) : base(pageheight * pageWidth)
        {
        }
        //readonly IUlatticeItem[] items;
        //public IUlatticeItem Current => IndexInRange(CurrentSelectedIndex) ? this[CurrentSelectedIndex] : null;
        //public ICollectionView InventoryViewController { get; set; }
        
        //#region 常规方法
        //public int ItemCount => items.Length;
        //IUlatticeItem this[int index]
        //{
        //    get => items[index];
        //    set => items[index] = value;
        //}
        //public bool IndexInRange(int itemIndex)
        //{
        //    return itemIndex > -1 && itemIndex < ItemCount;
        //}
        //public bool IndexIsEmpty(int targetIndex)
        //{
        //    return items[targetIndex] == null;
        //}
        //public IUlatticeItem GetItem(int index) => this[index];
        //public bool InventoryIsLocked => false;
        //public int Count => items.Length;
        //public void PutToTarget(IUlatticeItem commonStorage, int itemIndex)
        //{
        //    this[itemIndex] = commonStorage;
        //}
        //#endregion
        //public int FindEmptySpace()
        //{
        //    return Array.IndexOf(items, null);
        //}
        //public void FindEmptySpaces(List<int> indexs)
        //{
        //    indexs.Clear();
        //    for (int i = 0; i < items.Length; i++)
        //    {
        //        if (items[i] == null)
        //            indexs.Add(i);
        //    }
        //}
        //public void MergedSameKind(int index)
        //{
        //    if (IndexInRange(index) && this[index] is IMergedable<IUlatticeItem> item)
        //    {
        //        for (int i = 0; i < Count; i++)
        //        {
        //            if (index != i &&
        //                this[i] is IMergedable<IUlatticeItem> &&
        //                //item.TypeIsMatch(this[i]) &&// 合并时已检查类型
        //                item.TryMerged(this[i], out var exessItem))
        //            {
        //                //items[i] = exessItem;
        //                this[i] = exessItem;
        //                if (exessItem != null)
        //                    break;
        //            }
        //        }
        //    }
        //}
        //#region 视图回调
        //public int CurrentSelectedIndex { get; set; }
        //public bool ItemMatchType(IUlatticeItem ulatticeItem)
        //{
        //    return ulatticeItem is not null;
        //}
        //public void SwapItemInInternal(int selectedIndex, int targetIndex)
        //{
        //    //if (IndexInRange(selectedIndex) && IndexInRange(targetIndex))
        //    //{
        //    //    IUlatticeItem t = this[targetIndex];
        //    //    if (t is IMergedable<IUlatticeItem> mergedable && mergedable.TryMerged(this[selectedIndex], out IUlatticeItem ret))
        //    //    {
        //    //        //items[selectedIndex] = ret;
        //    //        this[selectedIndex] = ret;
        //    //    }
        //    //    else
        //    //    {
        //    //        //items[targetIndex] = items[selectedIndex];
        //    //        //items[selectedIndex] = t;
        //    //        this[targetIndex] = this[selectedIndex];
        //    //        this[selectedIndex] = t;
        //    //    }
        //    //}
        //    this.MergeOrSwapItemInList<IUlatticeItem>(items, selectedIndex, targetIndex);
        //}
        //public void RemoveAt(int itemIndex)
        //{
        //    if (IndexInRange(itemIndex))
        //        //items[itemIndex] = null;
        //        this[itemIndex] = null;
        //}
        //public void SetItem(IUlatticeItem item, int index)
        //{
        //    if (IndexInRange(index))
        //    {
        //        this[index] = item;
        //    }
        //}
        //public void TrySetItem(IUlatticeItem item, int index, out IUlatticeItem ret)
        //{
        //    if (item is IUlatticeItem s)
        //    {
        //        if (IndexInRange(index))
        //        {
        //            IUlatticeItem t = this[index];
        //            if (t is IMergedable<IUlatticeItem> mergedable && mergedable.TryMerged(s, out IUlatticeItem exessItem))// 可以合并则合并
        //            {
        //                ret = exessItem;
        //            }
        //            else
        //            {
        //                this[index] = s;
        //                ret = t;// 否则互换
        //            }
        //        }
        //        else
        //        {
        //            ret = item;
        //            if (ConsoleCat.Enable)
        //                ConsoleCat.LogWarning("物品移动通用库存时,指向的目标索引溢出");
        //        }
        //    }
        //    else
        //    {
        //        ret = item;
        //    }
        //}
        //public void Add(IUlatticeItem item)
        //{
        //    TryAdd(item, out _);
        //}

        //public void TryAdd(IUlatticeItem item, out IUlatticeItem ret)
        //{
        //    int index = FindEmptySpace();
        //    if (index != -1)
        //    {
        //        this[index] = item;
        //        ret = null;
        //    }
        //    else
        //    {
        //        ret = item;
        //    }
        //}
        //public bool Find(Predicate<IUlatticeItem> match)
        //{
        //    return Array.Find(items, match) != null;
        //}
        //#endregion
    }
}
