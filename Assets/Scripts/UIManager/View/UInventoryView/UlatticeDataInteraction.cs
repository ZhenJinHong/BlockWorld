using CatFramework.EventsMiao;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class UlatticeDataInteraction : IPointerDragHanderModule<DataInteractionModuleCenter>, IPointerOnDragHanderModule<DataInteractionModuleCenter>
    {
        public ShareDragUlattice shareDragUlattice;
        public UlatticeDataInteraction()
        {
        }
        public bool IsUsable { get; set; }
        public bool PointerBeginDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                if (shareDragUlattice != null)
                {
                    shareDragUlattice.BeginDrag(dataInteractionModuleCenter.DragLattice, pointerEventData);
                }
                return true;
            }
            return false;
        }

        public void PointerOnDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (shareDragUlattice != null)
            {
                shareDragUlattice.OnDrag(pointerEventData);
            }
        }
        public void PointerEndDrag(DataInteractionModuleCenter dataInteractionModuleCenter, PointerEventData pointerEventData)
        {
            if (shareDragUlattice != null)
            {
                shareDragUlattice.EndDrag();
            }
            var dragUlattice = dataInteractionModuleCenter.DragLattice;
            var targetUlattice = dataInteractionModuleCenter.LocatedLattice;
            if (dragUlattice == targetUlattice) return;// 拖回了原格子,不做变化
            if (dragUlattice == null)
            {
                if (ConsoleCat.IsDebug) ConsoleCat.DebugError("选定格子是空");
                return;
            }
            var dragInventory = dataInteractionModuleCenter.DragInventory;
            var targetInventory = dataInteractionModuleCenter.LocatedInventory;
            if (dragInventory is IItemStorageCollection<IUlatticeItemStorage> sItemInv)
            {
                if (ConsoleCat.IsDebug)
                    ConsoleCat.DebugInfo("拖拽的库存是物品库存");
                if (targetUlattice == null)
                {
                    if (dragInventory != targetInventory)
                    {
                        if (targetInventory is IItemStorageCollection<IUlatticeItemStorage> tItemInv)
                        {
                            if (ConsoleCat.IsDebug)
                                ConsoleCat.DebugInfo("执行itemtoinventory");
                            tItemInv.TryAddItem(dragUlattice.LatticeItem, out IUlatticeItemStorage ret);
                            if (ret == null)// 非空的情况下,返回什么?
                                //sItemInv.RemoveItemAt(dragUlattice.ItemIndex);
                                sItemInv.SetItem(null, dragUlattice.ItemIndex);
                            sItemInv.NotifyChange();
                            tItemInv.NotifyChange();
                        }
                        else
                        {
                            if (ConsoleCat.IsDebug)
                                ConsoleCat.DebugInfo("执行丢弃物品");
                            //sItemInv.RemoveItemAt(dragUlattice.ItemIndex);
                            sItemInv.SetItem(null, dragUlattice.ItemIndex);
                            sItemInv.NotifyChange();
                        }

                    }
                }
                else if (targetInventory is IItemStorageCollection<IUlatticeItemStorage> tItemInv)
                {
                    if (ConsoleCat.IsDebug)
                        ConsoleCat.DebugInfo("执行itemtoitem");
                    ItemToItem(dragUlattice, targetUlattice, sItemInv, tItemInv);
                }
                else
                {
                    if (ConsoleCat.IsDebug)
                        ConsoleCat.DebugInfo("目标格子非空且目标也未存在库存");
                }
            }
            else if (dragInventory is IReadonlyItemStorageCollection<IUlatticeItemStorage>)
            {
                if (ConsoleCat.IsDebug)
                    ConsoleCat.DebugInfo("拖拽的定义库存");
                if (targetInventory is IItemStorageCollection<IUlatticeItemStorage> &&
                    targetUlattice.LatticeItem != null &&
                    dragUlattice.LatticeItem != null)
                {
                    if (targetUlattice.LatticeItem is not ITraitsMergeable traitsMergeable ||
                        (!traitsMergeable.TryOtherItemDrop(dragUlattice.LatticeItem)))
                    {
                        if (ConsoleCat.Enable)
                            ConsoleCat.LogWarning($"目标物品不支持该定义类型:{dragUlattice.LatticeItem.GetType()}");
                    }
                    targetUlattice.Refresh();
                }
            }
        }
        void ItemToItem(Ulattice dragUlattice, Ulattice targetUlattice, IItemStorageCollection<IUlatticeItemStorage> dragInventory, IItemStorageCollection<IUlatticeItemStorage> targetInventory)
        {
            if (dragInventory.InventoryIsLocked || targetInventory.InventoryIsLocked)
                return;
            IUlatticeItemStorage dragItem = dragUlattice.LatticeItem;
            IUlatticeItemStorage targetItem = targetUlattice.LatticeItem;
            #region 意外条件判断
            if (targetInventory == null
                || (!dragInventory.IndexInRange(dragUlattice.ItemIndex))
                || dragInventory.IndexIsEmpty(dragUlattice.ItemIndex)
                || dragInventory.GetItem(dragUlattice.ItemIndex) != dragItem)
            {
                if (ConsoleCat.Enable)
                {
                    if (targetInventory == null) ConsoleCat.LogWarning("目标格子的数据交互回调为空!");
                    else if (!dragInventory.IndexInRange(dragUlattice.ItemIndex)) ConsoleCat.LogWarning("拖拽的物品溢出范围");
                    else if (dragInventory.IndexIsEmpty(dragUlattice.ItemIndex)) ConsoleCat.LogWarning("拖的一个空物品");
                    else if (dragInventory.GetItem(dragUlattice.ItemIndex) != dragItem) ConsoleCat.LogWarning("拖拽的物品与库存里的物品不匹配");
                }
                return;
            }
            #endregion


            if (dragInventory == targetInventory)
            {
                if (ConsoleCat.IsDebug)
                    ConsoleCat.DebugInfo("执行库存内部交换");
                dragInventory.SwapItemInInternal(dragUlattice.ItemIndex, targetUlattice.ItemIndex);
                dragInventory.NotifyChange();
            }
            else
            {
                if (ConsoleCat.IsDebug)
                    ConsoleCat.DebugInfo("执行两个库存之间的交换");
                // 选中的格子应该放置的物品,初始即为自身
                IUlatticeItemStorage returnItem = dragItem;
                if (targetInventory.IndexInRange(targetUlattice.ItemIndex))
                {
                    if (targetItem == null || dragInventory.ItemMatchType(targetItem))// 如果目标物品非空,那么目标应该可以被选中的库存可以容纳,才可以交换
                    {
                        //if (targetInventory is IEnableSpecifyIndexItemStorageCollection<IUlatticeItemStorage> ti)
                            targetInventory.TryMergedOrSetItem(dragItem, targetUlattice.ItemIndex, out returnItem);
                        //else
                            //targetInventory.TryAddItem(dragItem, out returnItem);
                    }
                }
                else
                {
                    targetInventory.TryAddItem(dragItem, out returnItem);// 溢出范围意味着,目标格子没物品,所以返回的要么是dragType,要么空
                }
                #region 选中的物品被更改后
                if (returnItem != dragItem)
                {
                    //if (dragInventory is IEnableSpecifyIndexItemStorageCollection<IUlatticeItemStorage> di)
                        dragInventory.SetItem(returnItem, dragUlattice.ItemIndex);
                    //else
                    //{
                    //    dragInventory.RemoveItemAt(dragUlattice.ItemIndex);
                    //    if (returnItem != null)
                    //    {
                    //        dragInventory.AddItem(returnItem);// 对于没有指定总重量/数量之类库存才可以这样在先加到目标上,再反过来加到拖拽的库存上
                    //    }
                    //}
                }
                #endregion
                dragInventory.NotifyChange();
                targetInventory.NotifyChange();
            }
        }
    }
}