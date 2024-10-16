using UnityEngine;
using UnityEngine.UIElements;
namespace CatFramework.UiTK
{
    public interface IUlatticeCallBack
    {
        IUlatticeItem GetUlatticeItem(int latticeIndex, out int itemIndex);
        void LatticeDown(Ulattice ulattice);
        void LatticeEndDrag(Ulattice ulattice);
        void LatticeEnter(Ulattice ulattice);
        void LatticeExit(Ulattice ulattice);
        void LatticeUp(Ulattice ulattice);
    }
    public interface IUlatticeItem
    {
        Texture2D ItemImage { get; }
        Texture2D CornerImage { get; }
        string Label { get; }
        string Name { get; }
        bool TryMerged(IUlatticeItem itemToPut, out IUlatticeItem exessItem);
        void OtherItemDrop(IUlatticeItem item);
    }
    public class Ulattice : VisualElement
    {
        int itemIndex;
        public int ItemIndex => itemIndex;
        readonly int latticeIndex;
        IUlatticeCallBack ulatticeCallBack;
        public IUlatticeCallBack UlatticeCallBack
        {
            set
            {
                ulatticeCallBack = value;
            }
        }
        public Image ItemImage { get; set; }
        public Image CornerImage { get; set; }
        public Label Label { get; set; }
        IUlatticeItem ulatticeItem;
        public IUlatticeItem UlatticeItem => ulatticeItem;
        public Ulattice(IShareLattice shareLattice, int latticeIndex)
        {
            this.shareLattice = shareLattice;
            this.latticeIndex = latticeIndex;
        }
        public void Refresh()
        {
            if (ulatticeCallBack == null) return;
            ulatticeItem = ulatticeCallBack.GetUlatticeItem(latticeIndex, out itemIndex);

            if (ItemImage != null)
                ItemImage.image = ulatticeItem?.ItemImage;
            if (CornerImage != null)
                CornerImage.image = ulatticeItem?.CornerImage;
            if (Label != null)
                Label.text = ulatticeItem?.Label ?? string.Empty;
        }
        readonly IShareLattice shareLattice;
        bool isPointerDown;
        bool SendPointerDragEvent;
        protected override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);
            if (ulatticeCallBack == null) return;
            if (evt is PointerEnterEvent)
            {
                ulatticeCallBack.LatticeEnter(this);
            }
            else if (evt is PointerLeaveEvent)
            {
                ulatticeCallBack.LatticeExit(this);
            }
            else if (ulatticeItem != null)
            {
                if (isPointerDown)
                {
                    if (evt is PointerMoveEvent pointerMoveEvent)
                    {
                        if (this.HasPointerCapture(pointerMoveEvent.pointerId))
                        {
                            if (!SendPointerDragEvent)
                            {
                                shareLattice.Set(ulatticeItem);
                                ulatticeCallBack.LatticeDown(this);
                                SendPointerDragEvent = true;
                            }
                            shareLattice.SetPointerPosition(pointerMoveEvent.position);
                        }
                    }
                    else if (evt is PointerUpEvent pointerUpEvent)
                    {
                        isPointerDown = false;
                        if (this.HasPointerCapture(pointerUpEvent.pointerId))
                            this.ReleasePointer(pointerUpEvent.pointerId);
                        if (SendPointerDragEvent)
                        {
                            shareLattice.Set(null);
                            ulatticeCallBack.LatticeEndDrag(this);
                            SendPointerDragEvent = false;
                        }
                        else
                        {
                            ulatticeCallBack.LatticeUp(this);
                        }
                    }//else if (evt is PointerCaptureOutEvent)// 是为了后于松开按键而执行?
                }
                else
                {
                    if (evt is PointerDownEvent pointerDownEvent)
                    {
                        shareLattice.ItemStartPosition = worldBound.position;
                        shareLattice.PointerStartPosition = pointerDownEvent.position;
                        this.CapturePointer(pointerDownEvent.pointerId);
                        isPointerDown = true;
                    }
                }
            }
        }
    }
}
