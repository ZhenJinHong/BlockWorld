using CatDOTS.VoxelWorld;
using CatDOTS.VoxelWorld.Magics;
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
    public class MagicWandStorage : IUlatticeItemStorage
    {
        public Texture2D ItemImage => primaryMagicWand?.Icon;
        public Texture2D CornerImage => null;
        public string Label => Name;
        public string Name => primaryMagicWand?.Name ?? "魔杖";
        public MagicWandStorage()
        {
            this.primaryMagicWand = new MagicWand(1);
        }
        public MagicWandStorage(IMagicWand primaryMagicWand)
        {
            this.primaryMagicWand = primaryMagicWand;
        }
        IMagicWand primaryMagicWand;
        public IMagicWand GetMagicWand() => primaryMagicWand;
    }
    public class MagicWandInventory : CompactItemStorageList<IUlatticeItemStorage, MagicWandStorage>
    {
        IMagicWandDataBase magicWandDataBase;
        public VoxelCommandPools VoxelCommandPools { get; }
        public MagicWand PutVoxelMagicWand { get; }
        public MagicWand DestoryVoxelMagicWand { get; }
        public MagicWandInventory(IMagicWandDataBase magicWandDataBase)
        {
            this.magicWandDataBase = magicWandDataBase;
            VoxelCommandPools = new VoxelCommandPools();

            PutVoxelMagicWand = new MagicWand(1);
            DestoryVoxelMagicWand = new MagicWand(1);
            PutVoxelMagicWand[0] = new PutVoxel(VoxelCommandPools);
            DestoryVoxelMagicWand[0] = new DestoryVoxel(VoxelCommandPools);
        }
        //public ICollectionView InventoryViewController { get; set; }
        //public int CurrentSelectedIndex { get; set; }
        //#region 常规
        //public bool InventoryIsLocked { get; set; }
        //public int Count => items.Count;
        //public IUlatticeItem GetItem(int index) => items[index];
        //public bool IndexInRange(int targetIndex)
        //{
        //    return targetIndex > -1 && targetIndex < items.Count;
        //}
        //public bool IndexIsEmpty(int targetIndex)
        //{
        //    return items[targetIndex] == null;
        //}
        //public bool ItemMatchType(IUlatticeItem ulatticeItem)
        //{
        //    return ulatticeItem is MagicWandStorage;
        //}
        //#endregion
        //public void Add(MagicWandStorage magicWandStorage)
        //{
        //    if (ConsoleCat.Enable && items.Contains(magicWandStorage))
        //    {
        //        ConsoleCat.LogWarning($"有魔杖在添加到库存后,自身未丢弃引用");
        //    }
        //    if (magicWandStorage != null)
        //        items.Add(magicWandStorage);
        //}
        //#region
        //public void RemoveAt(int targetIndex)
        //{
        //    if (IndexInRange(targetIndex))
        //        items.RemoveAt(targetIndex);
        //}
        //public void SwapItemInInternal(int selectedIndex, int targetIndex)
        //{
        //    //if (IndexInRange(selectedIndex))
        //    //{
        //    //    MagicWandStorage selected = items[selectedIndex];
        //    //    if (IndexInRange(targetIndex))
        //    //    {
        //    //        items[selectedIndex] = items[targetIndex];
        //    //        items[targetIndex] = selected;
        //    //    }
        //    //    else
        //    //    {
        //    //        items.RemoveAt(selectedIndex);
        //    //        items.Add(selected);
        //    //    }
        //    //}
        //    this.SwapOrMoveToLastInList<MagicWandStorage>(items, selectedIndex, targetIndex);
        //}
        //public void SetItem(IUlatticeItem item, int index)
        //{
        //    //RemoveAt(index);
        //    Add(item);
        //}
        //public void TrySetItem(IUlatticeItem item, int index, out IUlatticeItem ret)
        //{
        //    TryAdd(item, out ret);
        //}
        //public void Add(IUlatticeItem item)
        //{
        //    if (item is MagicWandStorage magicWandStorage)
        //    {
        //        items.Add(magicWandStorage);
        //    }
        //}

        //public void TryAdd(IUlatticeItem item, out IUlatticeItem ret)
        //{
        //    ret = item;
        //    if (item is MagicWandStorage magicWandStorage)
        //    {
        //        items.Add(magicWandStorage);
        //        ret = null;
        //    }
        //}
        //#endregion
    }
}
