using CatFramework.CatMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public interface ILatticeCallBack<ItemStorage, Lattice>
    {
        ItemStorage GetLatticeItem(int itemIndex);
        void LatticeClick(Lattice lattice, PointerEventData pointerEventData);
        void LatticeBeginDrag(Lattice lattice, PointerEventData pointerEventData);
        void LatticeOnDrag(Lattice lattice, PointerEventData pointerEventData);
        void LatticeEndDrag(Lattice lattice, PointerEventData pointerEventData);
        void LatticeEnter(Lattice lattice, PointerEventData pointerEventData);
        void LatticeExit(Lattice lattice, PointerEventData pointerEventData);
    }
    public interface ILattice<ItemStorage, Lattice> : IUiItem
        where ItemStorage : class
        where Lattice : class
    {
        int ItemIndex { get; }
        ILatticeCallBack<ItemStorage, Lattice> LatticeCallBack { get; set; }
        void SetItem(ItemStorage itemStorage, int itemIndex);
    }
    public class PageLatticeViewController<Lattice, ItemStorage, Center, Collection> : PageListViewUMCtr<Lattice, Collection>, ICollectionView, ILatticeCallBack<ItemStorage, Lattice>
        where Lattice : Component, ILattice<ItemStorage, Lattice>
        where ItemStorage : class, IItemStorage
        where Collection : class, IReadonlyItemStorageCollection<ItemStorage>
        where Center : class, ILatticeDataInteractionCenter<Lattice, Center, Collection>
    {
        public Center dataInteractionModuleCenter;
        //public Lattice PreviewLattice;// 菜单栏时显示当前选中时用的,这个可以转换到模块中去
        public override int ItemCount => Items == null ? 0 : Items.Count;
        public PageLatticeViewController() { }
        protected override Lattice MakeItem()
        {
            var item = base.MakeItem();
            item.LatticeCallBack = this;
            return item;
        }
        protected override void BindItem(int itemIndex, Lattice visualItem)
        {
            visualItem.SetItem(Items.GetItem(itemIndex), itemIndex);
        }
        protected override void UnBindItem(int itemIndex, Lattice visualItem)
        {
            visualItem.SetItem(null, itemIndex);
        }
        public ItemStorage GetLatticeItem(int itemIndex)
        {
            return Items != null && Items.IndexInRange(itemIndex) ? Items.GetItem(itemIndex) : null;
        }
        protected override void FlushOther()
        {
            base.FlushOther();
            SelectedLattice(currentSelectedLattice, true);
        }


        public void OnPointerEnter()
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.LocatedInventory = Items;
        }
        public void OnPointerExit()
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.LocatedInventory = null;
        }
        public void LatticeClick(Lattice ulattice, PointerEventData pointerEventData)
        {
            SelectedLattice(ulattice.ItemIndex % VisualItemCount, true);
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerClick(ulattice, pointerEventData);
        }
        public void LatticeBeginDrag(Lattice ulattice, PointerEventData pointerEventData)
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerBeginDrag(ulattice, Items, pointerEventData);
        }
        public void LatticeEnter(Lattice ulattice, PointerEventData pointerEventData)
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerEnter(ulattice, pointerEventData);
        }
        public void LatticeExit(Lattice ulattice, PointerEventData pointerEventData)
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerExit(ulattice, pointerEventData);
        }
        public void LatticeOnDrag(Lattice ulattice, PointerEventData pointerEventData)
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerOnDrag(ulattice, pointerEventData);
        }
        public void LatticeEndDrag(Lattice ulattice, PointerEventData pointerEventData)
        {
            if (dataInteractionModuleCenter == null) return;
            dataInteractionModuleCenter.PointerEndDrag(ulattice, pointerEventData);
        }

        #region
        int currentSelectedLattice;
        public void SwitchLattice(int bias)
        {
            SelectedLattice(currentSelectedLattice + bias);
        }
        public void SelectedLattice(int latticeIndex, bool mustUpdate = false)
        {
            if (!CheckDataIsValid() || Items == null) return;
            latticeIndex = MathC.IndexLoopIClamp(latticeIndex, VisualItemCount);
            if (mustUpdate || currentSelectedLattice != latticeIndex)
            {
                currentSelectedLattice = latticeIndex;
                int itemIndex = GetItemIndex(latticeIndex);

                var CurrentHightLight = this[latticeIndex];
                if (mustUpdate || currentHightLight != CurrentHightLight)
                {
                    if (currentHightLight != null)
                        currentHightLight.ForcedDefaultStyle();
                    currentHightLight = CurrentHightLight;
                    if (currentHightLight != null)
                        currentHightLight.ForcedHoverStyle();
                }
                Items.CurrentSelectedIndex = itemIndex;
            }
        }
        Lattice currentHightLight;
        #endregion

    }
}
