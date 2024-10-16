namespace CatFramework.UiTK
{
    public interface ICallBackInventory
    {
        int ItemCount { get; }
        IInventoryViewController InventoryViewController { get; set; }
        int CurrentSelectedIndex { get; set; }

        /// <remarks>
        /// 已经判断格子存的物品索引是否有效
        /// </remarks>
        IUlatticeItem GetUlatticeItem(int targetIndex);
        bool IndexInRange(int targetIndex);
        /// <remarks>
        /// 已经判断格子存的物品索引是否有效
        /// </remarks>
        bool IndexIsEmpty(int targetIndex);
    }
    public interface ICallDefinitionInventory : ICallBackInventory
    {

    }
    public interface ICallItemInventory : ICallBackInventory
    {
        bool DisableChangeItem();
        /// <summary>
        /// 尝试追加到库存中
        /// </summary>
        /// <remarks>
        /// 在未拖拽到格子上,但是拖到了库存面板时触发
        /// </remarks>
        bool TryAdd(IUlatticeItem itemToAdd);
        /// <summary>
        /// 库存与外部交换
        /// </summary>
        /// <remarks>
        /// target格子物品类型已经判定符合selected格子归属的库存,但selected格子里的物品未判定是否符合target的库存
        /// </remarks>
        void SwapItemWithExternal(IUlatticeItem selectedItem, int targetIndex, out IUlatticeItem returnItem);
        /// <summary>
        /// 库存内部交换
        /// </summary>
        void SwapItemInInternal(int selectedIndex, int targetIndex);
        /// <summary>
        /// 当selected与target交换成功时,需要更新目标索引
        /// </summary>
        void ItemIsChange(IUlatticeItem newItem, int targetIndex);
        bool ItemMatchType(IUlatticeItem ulatticeItem);
        /// <summary>
        /// 当selected物品被取走并换回了一个空物品时
        /// </summary>
        /// <param name="targetIndex"></param>
        void RemoveItem(int targetIndex);
    }
}
