using System;

namespace CatFramework.UiTK
{
    public interface IInvDataInteractionCenter
    {
        ICallBackInventory TargetInventory { get; set; }

        event Action<IUlatticeItem> OnItemIsClick;

        void PointerDown(Ulattice ulattice);
        void PointerUp(Ulattice ulattice);
        void PointerEndDrag(Ulattice ulattice, ICallBackInventory selectedInventory);
        void PointerEnter(Ulattice ulattice);
        void PointerExit(Ulattice ulattice);
       
    }
    /// <summary>
    /// 仅处理数据交换等,不刷新格子
    /// </summary>
    public class UlatticeDataInteraction : IInvDataInteractionCenter
    {
        public ICallBackInventory TargetInventory { get; set; }
        Ulattice targetUlattice;
        Ulattice selectedUlattice;
        public event Action<IUlatticeItem> OnItemIsClick;
        public void PointerDown(Ulattice ulattice)
        {
            selectedUlattice = ulattice;
        }
        public void PointerUp(Ulattice ulattice)
        {
            OnItemIsClick?.Invoke(ulattice.UlatticeItem);
        }
        public void PointerEnter(Ulattice ulattice)
        {
            targetUlattice = ulattice;
        }
        public void PointerExit(Ulattice ulattice)
        {
            targetUlattice = null;
        }
        public void PointerEndDrag(Ulattice ulattice, ICallBackInventory selectedInventory)
        {
            if (selectedUlattice == targetUlattice) return;// 拖回了原格子,不做变化
            if (selectedUlattice == null)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogError("选定格子是空");
                return;
            }
            if (selectedUlattice != ulattice)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("结束拖拽的时候,初始拖拽的格子与最终结束拖拽的格子不匹配");
                return;
            }

            if (selectedInventory is ICallItemInventory sItemInv)
            {
                if (targetUlattice == null)
                {
                    if (selectedInventory != TargetInventory)
                    {
                        if (TargetInventory is ICallItemInventory tItemInv)
                        {
                            if (tItemInv.TryAdd(selectedUlattice.UlatticeItem))
                            {
                                sItemInv.RemoveItem(selectedUlattice.ItemIndex);
                                sItemInv.InventoryViewController?.RecalculatePageNum();
                                tItemInv.InventoryViewController?.RecalculatePageNum();
                            }
                        }
                        else
                        {
                            sItemInv.RemoveItem(selectedUlattice.ItemIndex);
                            sItemInv.InventoryViewController?.RecalculatePageNum();
                        }
                    }
                }
                else if (TargetInventory is ICallItemInventory tItemInv)
                {
                    ItemToItem(sItemInv, tItemInv);
                }
            }
            else if (selectedInventory is ICallDefinitionInventory)
            {
                if (TargetInventory is ICallItemInventory)
                {
                    targetUlattice.UlatticeItem?.OtherItemDrop(selectedUlattice.UlatticeItem);
                    targetUlattice.Refresh();
                }
            }
        }
        void ItemToItem(ICallItemInventory selectedInventory, ICallItemInventory targetInventory)
        {
            IUlatticeItem selectedItem = selectedUlattice.UlatticeItem;
            IUlatticeItem targetItem = targetUlattice.UlatticeItem;
            #region 意外条件判断
            if (targetInventory == null
                || (!selectedInventory.IndexInRange(selectedUlattice.ItemIndex))
                || selectedInventory.IndexIsEmpty(selectedUlattice.ItemIndex)
                || selectedInventory.GetUlatticeItem(selectedUlattice.ItemIndex) != selectedItem)
            {
                if (ConsoleCat.Enable)
                {
                    if (targetInventory == null) ConsoleCat.LogWarning("目标格子的数据交互回调为空!");
                    else if (!selectedInventory.IndexInRange(selectedUlattice.ItemIndex)) ConsoleCat.LogWarning("拖拽的物品溢出范围");
                    else if (selectedInventory.IndexIsEmpty(selectedUlattice.ItemIndex)) ConsoleCat.LogWarning("拖的一个空物品");
                    else if (selectedInventory.GetUlatticeItem(selectedUlattice.ItemIndex) != selectedItem) ConsoleCat.LogWarning("拖拽的物品与库存里的物品不匹配");
                }
                return;
            }
            #endregion
            if (selectedInventory.DisableChangeItem() || targetInventory.DisableChangeItem())
                return;

            if (selectedInventory == targetInventory)
            {
                selectedInventory.SwapItemInInternal(selectedUlattice.ItemIndex, targetUlattice.ItemIndex);
                selectedInventory.InventoryViewController?.RecalculatePageNum();
            }
            else
            {
                // 选中的格子应该放置的物品,初始即为自身
                IUlatticeItem returnItem = selectedItem;
                if (targetItem == null || selectedInventory.ItemMatchType(targetItem))// 如果目标物品非空,那么目标应该可以被选中的库存可以容纳,才可以交换
                    targetInventory.SwapItemWithExternal(selectedItem, targetUlattice.ItemIndex, out returnItem);
                #region 选中的物品被更改后
                if (returnItem != selectedItem)
                {
                    if (returnItem == null)
                    {
                        selectedInventory.RemoveItem(selectedUlattice.ItemIndex);
                    }
                    else
                    {
                        selectedInventory.ItemIsChange(returnItem, selectedUlattice.ItemIndex);
                    }
                }
                #endregion
                selectedInventory.InventoryViewController?.RecalculatePageNum();
                targetInventory.InventoryViewController?.RecalculatePageNum();
            }
        }
    }
}
