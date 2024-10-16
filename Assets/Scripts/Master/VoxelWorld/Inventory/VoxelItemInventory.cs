using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorld
{
    public class VoxelItemStorage : IUlatticeItemStorage, IVoxelItemStorage, IMergedable<IUlatticeItemStorage>, ITraitsMergeable
    {
        public Texture2D ItemImage => VoxelItemInfo?.Icon;
        public Texture2D CornerImage => VoxelShapeInfo?.Icon;
        public string Label => itemCount.ToString();
        public string Name => VoxelItemInfo?.VoxelName;

        public IVoxelItemInfo VoxelItemInfo { get; set; }
        public IVoxelShapeInfo VoxelShapeInfo { get; set; }
        public VoxelType VoxelType => VoxelItemInfo == null ? VoxelType.EmptyVoxelType : VoxelItemInfo.VoxelType;
        public Voxel Voxel => VoxelItemInfo == null ? Voxel.Empty : VoxelItemInfo.Voxel;
        public Voxel VoxelWithShape
        {
            get
            {
                if (VoxelItemInfo == null) return Voxel.Empty;
                Voxel voxel = VoxelItemInfo.Voxel;
                if (VoxelShapeInfo != null)
                    voxel.ShapeIndex = VoxelShapeInfo.ShapeIndex;
                return voxel;
            }
        }
        public ushort VoxelID => VoxelItemInfo == null ? (ushort)0 : VoxelItemInfo.VoxelType.IndexInTypeArray;
        public long ItemCount => itemCount;

        public long itemCount;
        public VoxelItemStorage() { }
        public VoxelItemStorage(IVoxelItemInfo voxelItemInfo)
        {
            VoxelItemInfo = voxelItemInfo;
        }
        public VoxelItemStorage Clone(long count)
        {
            return new VoxelItemStorage() { VoxelItemInfo = VoxelItemInfo, VoxelShapeInfo = VoxelShapeInfo, itemCount = count };
        }
        /// <summary>
        /// 均分
        /// </summary>
        public VoxelItemStorage Average()
        {
            long n = itemCount / 2;
            itemCount -= n;
            var storage = Clone(n);
            return storage;
        }
        public bool CountEnough(long need)
        {
            return itemCount > need;
        }
        public void UpdateCount(long count)
        {
            if (count > 0)
                Increase(count);
            else
                Decrease(count);
        }
        public void Increase(VoxelItemStorage storage)
        {
            Increase(storage.itemCount);
        }
        public void Increase(long count)
        {
            itemCount = long.MaxValue - itemCount > count ? itemCount + count : long.MaxValue;
        }
        public void Decrease(long count)
        {
            itemCount -= count;
        }
        public bool TryMerged(IUlatticeItemStorage itemToPut, out IUlatticeItemStorage exessItem)
        {
            if (itemToPut == this)
            {
                exessItem = itemToPut;
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("自身与自身合并了");
                return false;
            }
            if (itemToPut is VoxelItemStorage storage && storage.VoxelID == VoxelID)
            {
                Increase(storage.itemCount);
                exessItem = null;
                return true;
            }
            exessItem = itemToPut;
            return false;
        }

        public bool TryOtherItemDrop(IItemStorage item)
        {
            if (item is VoxelShapeStorage shapeInfo)
            {
                VoxelShapeInfo = shapeInfo.voxelShapeInfo;
                return true;
            }
            return false;
        }
        public bool TypeEquals(VoxelItemStorage other) => other.VoxelItemInfo == VoxelItemInfo;

        public bool TypeIsMatch(IItemStorage storage)
        {
            return storage is VoxelItemStorage;
        }
    }
    public class VoxelItemInventory : IItemStorageCollection<IUlatticeItemStorage>
    {
        IVoxelDefinitionDataBase voxelItemInfoDataBase;
        IShapeDefinitionDataBase shapeDefinitionDataBase;

        Dictionary<ushort, VoxelItemStorage> itemMap;
        List<VoxelItemStorage> items;
        public VoxelItemInventory(IVoxelDefinitionDataBase voxelItemInfoDataBase, IShapeDefinitionDataBase shapeDefinitionDataBase)
        {
            this.voxelItemInfoDataBase = voxelItemInfoDataBase;
            this.shapeDefinitionDataBase = shapeDefinitionDataBase;
            itemMap = new Dictionary<ushort, VoxelItemStorage>();
            items = new List<VoxelItemStorage>();
        }
        #region 库存操作
        public void Add(ushort ID, long count)
        {
            if (itemMap.TryGetValue(ID, out VoxelItemStorage itemStorage))
            {
                itemStorage.Increase(count);
            }
            else if (count > 0)
            {
                itemStorage = new VoxelItemStorage
                {
                    VoxelItemInfo = voxelItemInfoDataBase.GetVoxelItemInfo(ID),
                    VoxelShapeInfo = shapeDefinitionDataBase.GetDefaultVoxelShapeInfo(),
                    itemCount = count,
                };
                itemMap.Add(ID, itemStorage);
                items.Add(itemStorage);
            }
        }
        /// <summary>
        /// 增加到相同的体素上或追加到库存中
        /// </summary>
        public void Add(VoxelItemStorage itemToAdd)
        {
            if (itemMap.TryGetValue(itemToAdd.VoxelID, out VoxelItemStorage itemStorage))
            {
                if (itemStorage == itemToAdd)
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning("尝试添加已添加的物品" + itemToAdd);
                    return;
                }
                itemStorage.Increase(itemToAdd);
            }
            else
            {
                itemMap.Add(itemToAdd.VoxelID, itemToAdd);
                items.Add(itemToAdd);
            }
        }
        #endregion
        #region 常规方法
        public bool InventoryIsLocked => false;
        public int Count => items.Count;
        public IUlatticeItemStorage GetItem(int index) => items[index];
        public bool IndexInRange(int targetIndex)
        {
            return targetIndex > -1 && targetIndex < Count;
        }
        public bool IndexIsEmpty(int targetIndex)
        {
            return items[targetIndex] == null;
        }
        #endregion
        #region 视图回调
        public int CurrentSelectedIndex { get; set; }
        public bool ItemMatchType(IUlatticeItemStorage ulatticeItem)
            => ulatticeItem is VoxelItemStorage;
        public void SwapItemInInternal(int selectedIndex, int targetIndex)
        {
            //if (IndexInRange(selectedIndex))
            //{
            //    VoxelItemStorage s = items[selectedIndex];
            //    if (IndexInRange(targetIndex))
            //    {
            //        VoxelItemStorage t = items[targetIndex];
            //        items[targetIndex] = s;
            //        items[selectedIndex] = t;
            //        if (t == null)
            //        {
            //            items.RemoveAt(selectedIndex);
            //            if (ConsoleCat.Enable)
            //                ConsoleCat.LogWarning("将放置到的目标格子处于索引范围内,但是却为空");
            //        }
            //    }
            //    else
            //    {
            //        items.RemoveAt(selectedIndex);
            //        items.Add(s);
            //    }
            //}
            ItemStorageCollectionExtension.SwapOrMoveToLastInList<VoxelItemStorage>(this, items, selectedIndex, targetIndex);
        }
        public void RemoveItemAt(int itemIndex)
        {
            VoxelItemStorage original = items[itemIndex];
            itemMap.Remove(original.VoxelID);
            items.RemoveAt(itemIndex);
        }
        // 参考字典,目标索引替换掉
        public void SetItem(IUlatticeItemStorage item, int itemIndex)
        {
            VoxelItemStorage original = items[itemIndex];
            itemMap.Remove(original.VoxelID);
            items.RemoveAt(itemIndex);
            AddItem(item);
        }
        public void TryMergedOrSetItem(IUlatticeItemStorage item, int index, out IUlatticeItemStorage ret)
        {
            TryAddItem(item, out ret);// 已带有合并
        }
        // 因为这个每种只能占一格,如果允许指定索引置换的话,判断过复杂
        //// 能加就加,不能加就丢回去,不存在把库存内的物品换出去的可能,如果是从库存拖出去,该物品所有数量全部被拿走的话,就用RemoveAt移除这个物品
        //public void SetItem(IUlatticeItem item, int index)
        //{
        //    //RemoveAt(index);
        //    //Add(item);
        //    //if (item is VoxelItemStorage storage)
        //    //{
        //    //    if (itemMap.TryGetValue(storage.VoxelID, out var voxelItemStorage))
        //    //    {
        //    //        voxelItemStorage.Increase(storage.ItemCount);
        //    //    }
        //    //}
        //    //else
        //    //    Add
        //    //if (item is VoxelItemStorage voxelItemStorage)
        //    //{
        //    //    if (IndexInRange(index))
        //    //    {
        //    //        var target = items[index];
        //    //        if (target == item) return;
        //    //        if (itemMap.TryGetValue(voxelItemStorage.VoxelID, out var storage))
        //    //        {

        //    //        }
        //    //        itemMap.Remove(target.VoxelID);
        //    //        items[index] = voxelItemStorage;// 因为不允许有重复类型存在,所以不能直接就赋值上去
        //    //        itemMap.Add(voxelItemStorage.VoxelID, voxelItemStorage);
        //    //    }
        //    //    else
        //    //    {
        //    //        Add(voxelItemStorage);
        //    //    }
        //    //}
        //}
        //public void TrySetItem(IUlatticeItem item, int index, out IUlatticeItem ret)
        //{
        //    //RemoveAt(index);// 如果直接移除,应该把这个移除的丢回去
        //    //TryAdd(item, out ret);
        //}
        public void AddItem(IUlatticeItemStorage item)
        {
            if (item is VoxelItemStorage voxelItem)
                Add(voxelItem);
        }
        public void TryAddItem(IUlatticeItemStorage item, out IUlatticeItemStorage ret)
        {
            if (item is VoxelItemStorage voxelItem)
            {
                Add(voxelItem); ret = null;
            }
            else ret = item;
        }
        #endregion
    }
}
