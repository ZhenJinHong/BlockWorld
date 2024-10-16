using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public abstract class BaseLattice<ItemStorage, Lattice> : MaskableGraphicMiao, ILattice<ItemStorage, Lattice>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
        where ItemStorage : class
        where Lattice : BaseLattice<ItemStorage, Lattice>
    {
        int itemIndex;
        public int ItemIndex => itemIndex;
        ILatticeCallBack<ItemStorage, Lattice> latticeCallBack;
        public ILatticeCallBack<ItemStorage, Lattice> LatticeCallBack
        {
            get => latticeCallBack;
            set => latticeCallBack = value;
        }
        Lattice self;
        protected override void Awake()
        {
            base.Awake();
            self = (Lattice)this;
        }
        public ItemStorage LatticeItem => latticeCallBack?.GetLatticeItem(ItemIndex);
        public void SetItem(ItemStorage itemStorage, int itemIndex)
        {
            this.itemIndex = itemIndex;
            Refresh(itemStorage);
        }
        public void Refresh()
        {
            Refresh(LatticeItem);
        }
        protected abstract void Refresh(ItemStorage itemStorage);
        public void HighLight(bool v)
        {
            ForcedHoverStyle(v);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (latticeCallBack == null) return;
            latticeCallBack.LatticeClick(self, eventData);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (latticeCallBack == null) return;
            latticeCallBack.LatticeEnter(self, eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (latticeCallBack == null) return;
            latticeCallBack.LatticeExit(self, eventData);
        }
        bool onDrag;
        public void OnBeginDrag(PointerEventData eventData)
        {
            //#if UNITY_EDITOR
            //            if (UlatticeItem == null && ConsoleCat.Enable) ConsoleCat.LogWarning("空");
            //#endif
            if (latticeCallBack == null || LatticeItem == null) return;
            onDrag = true;
#if UNITY_EDITOR
            Debug.Log("拖拽");
#endif
            latticeCallBack.LatticeBeginDrag(self, eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag)
                latticeCallBack.LatticeOnDrag(self, eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDrag)
                latticeCallBack.LatticeEndDrag(self, eventData);
            onDrag = false;
#if UNITY_EDITOR
            Debug.Log("结束拖拽");
#endif
        }
    }
}