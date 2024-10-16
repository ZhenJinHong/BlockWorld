using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class VoxelItemAmountSplitModule : IPointerClickHanderModule<DataInteractionModuleCenter>, IPointerDragHanderModule<DataInteractionModuleCenter>, IPointerOnDragHanderModule<DataInteractionModuleCenter>
    {
        public ShareDragUlattice shareDragUlattice;
        public bool IsUsable { get; set; }
        List<int> indexs = new List<int>();
        public bool Split;
        public GameCassette gameCassette;
        public bool PointerClick(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            Ulattice target = dataInteractionModuleCenter.LocatedLattice;
            bool hasChange = false;
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                if (target.LatticeItem is VoxelItemStorage voxelItemStorage &&
                    voxelItemStorage.itemCount > 1 &&
                    dataInteractionModuleCenter.LocatedInventory is CommonInventory commonInventory)
                {
                    if (Split)
                    {
                        commonInventory.FindEmptySpaces(indexs);
                        if (indexs.Count > 0)
                        {
                            long average = voxelItemStorage.itemCount / (indexs.Count + 1);
                            if (average > 0)
                            {
                                voxelItemStorage.Decrease(average * indexs.Count);
                                for (int i = 0; i < indexs.Count; i++)
                                {
                                    commonInventory.SetItem(voxelItemStorage.Clone(average), indexs[i]);
                                }
                            }
                            indexs.Clear();
                        }
                    }
                    else
                    {
                        int empty = commonInventory.FindEmptySpace();
                        if (empty != -1)
                        {
                            commonInventory.SetItem(voxelItemStorage.Average(), empty);
                        }
                    }
                    hasChange = true;
                }
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                if (Split && !Assert.IsNull(gameCassette))// 这个是关于从快捷栏与背包的互相转移
                {
                    if (target.LatticeItem != null)
                    {
                        if (dataInteractionModuleCenter.LocatedInventory == gameCassette.CommonInventoryInBar)
                        {
                            MoveItem(target, gameCassette.CommonInventoryInBar, gameCassette.CommonInventoryInLattice);
                            hasChange = true;
                        }
                        else if (dataInteractionModuleCenter.LocatedInventory == gameCassette.CommonInventoryInLattice)
                        {
                            MoveItem(target, gameCassette.CommonInventoryInLattice, gameCassette.CommonInventoryInBar);
                            hasChange = true;
                        }
                        static void MoveItem(Ulattice xU, CommonInventory x, CommonInventory y)
                        {
                            x.NotifyChange();
                            y.NotifyChange();
                            int empty = y.FindEmptySpace();
                            if (empty != -1)
                            {
                                y.SetItem(xU.LatticeItem, empty);
                                x.SetItem(null, xU.ItemIndex);
                            }
                        }
                    }
                }
                else
                {
                    if (pointerEventData.clickCount > 1 &&
                        target.LatticeItem is VoxelItemStorage &&
                        dataInteractionModuleCenter.LocatedInventory is CommonInventory commonInventory)
                    {
                        commonInventory.MergedSameKind(target.ItemIndex);
                        hasChange = true;
                    }
                }
            }
            if (hasChange)
            {
                dataInteractionModuleCenter.LocatedInventory.NotifyChange();
            }
            return hasChange;
        }


        public bool PointerBeginDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right && dataInteractionModuleCenter.DragLattice.LatticeItem is VoxelItemStorage)
            {
                if (shareDragUlattice != null)
                    shareDragUlattice.BeginDrag(dataInteractionModuleCenter.DragLattice, pointerEventData);
                return true;
            }
            return false;
        }
        public void PointerOnDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (shareDragUlattice != null)
                shareDragUlattice.OnDrag(pointerEventData);
        }
        public void PointerEndDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (shareDragUlattice != null)
                shareDragUlattice.EndDrag();
            Ulattice draglattice = dataInteractionModuleCenter.DragLattice;
            Ulattice targetlattice = dataInteractionModuleCenter.LocatedLattice;
            if (draglattice == null) return;
            // 处理的右键拖拽
            if (draglattice.LatticeItem is VoxelItemStorage dragStorage
                && dataInteractionModuleCenter.LocatedInventory is IItemStorageCollection<IUlatticeItemStorage> targetInventory
                && targetInventory.ItemMatchType(dragStorage))
            {
                static void SplitToAdd(IItemStorageCollection<IUlatticeItemStorage> targetInventory, VoxelItemStorage dragStorage)
                {
                    var storage = dragStorage.Average();
                    targetInventory.TryAddItem(storage, out var ret);
                    storage = ret as VoxelItemStorage;
                    if (storage != null)
                        dragStorage.Increase(storage);
                }
                if (targetlattice == null)
                {
                    // 没指向格子的,尝试拆分放到目标库存
                    SplitToAdd(targetInventory, dragStorage);
                }
                else// 格子非空的时候
                {
                    // 格子里无物品
                    if (targetlattice.LatticeItem == null)
                    {
                        if (targetInventory.IndexInRange(targetlattice.ItemIndex))
                        {
                            var storge = dragStorage.Average();
                            targetInventory.SetItem(storge, dataInteractionModuleCenter.LocatedLattice.ItemIndex);
                        }
                        else
                        {
                            //targetInventory.AddItem(storge);
                            SplitToAdd(targetInventory, dragStorage);
#if UNITY_EDITOR
                            Debug.Log("分拆物品时指向的索引溢出");
#endif
                        }
                    }
                    // 格子有物品,这里不考虑互换,只考虑能否叠加,或者加到目标库存里
                    else
                    {
                        if (targetlattice.LatticeItem is VoxelItemStorage targetStorage && targetStorage.TypeEquals(dragStorage))
                        {
                            var storage = dragStorage.Average();
                            targetStorage.Increase(storage);
                        }
                    }
                }
                dataInteractionModuleCenter.DragInventory.NotifyChange();
                dataInteractionModuleCenter.LocatedInventory.NotifyChange();
            }
        }
    }
}