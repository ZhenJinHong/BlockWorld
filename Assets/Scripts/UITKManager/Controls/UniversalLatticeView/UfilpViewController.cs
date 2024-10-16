using CatFramework.CatMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public interface IInventoryViewController
    {
        void RecalculatePageNum(int value, bool refresh = false);
        void RecalculatePageNum();
        void RefreshLattice(int itemIndex);
    }
    public class UfilpViewController : VisualElementController, IUlatticeCallBack, IInventoryViewController
    {
        ICallBackInventory callBackInventory;
        /// <summary>
        /// 需要手动刷新
        /// </summary>
        public ICallBackInventory CallBack
        {
            get => callBackInventory;
            set
            {
                if (value != null && callBackInventory != value)
                {
                    if (callBackInventory != null)
                        callBackInventory.InventoryViewController = null;
                    callBackInventory = value;
                    callBackInventory.InventoryViewController = this;
                }
            }
        }
        int pageNum;
        int startIndex;
        public void RecalculatePageNum()
        {
            RecalculatePageNum(pageNum, true);
        }
        public void RecalculatePageNum(int value, bool refresh = false)
        {
            value = MathC.ClampPageNumInRange(value, callBackInventory.ItemCount, ulattices.Length);
            pageNum = value;
            startIndex = MathC.PageStartIndexInItem(pageNum, ulattices.Length);
            if (refresh)
                Refresh();
        }
        public void RefreshLattice(int itemIndex)
        {
            int latticeIndex = itemIndex - startIndex;
            if (latticeIndex > -1 && latticeIndex < ulattices.Length)
            {
                ulattices[latticeIndex].Refresh();
            }
            else if (ConsoleCat.Enable)
            {
                ConsoleCat.LogWarning($"刷新格子时,索引越界,目标物品索引为{itemIndex},目标格子索引为{latticeIndex}");
            }
        }
        readonly Ulattice[] ulattices;
        public Label CurrentItemNameLabel;
        public UfilpViewController(VisualElement target, ICallBackInventory callBack, IInvDataInteractionCenter invDataInteractionCenter, Ulattice[] ulattices) : base(target, PickingMode.Position)
        {
            CallBack = callBack;
            this.invDataInteractionCenter = invDataInteractionCenter;
            this.ulattices = ulattices;
            for (int i = 0; i < ulattices.Length; i++)
            {
                ulattices[i].UlatticeCallBack = this;
            }

            target.RegisterCallback<PointerEnterEvent>(PointerEnterView);
            target.RegisterCallback<PointerLeaveEvent>(PointerLeaveView);
        }
        // 私有,以确保重新计算了正确页码后才可以刷新
        void Refresh()
        {
            for (int i = 0; i < ulattices.Length; i++)
            {
                ulattices[i].Refresh();
            }
        }




        #region 几个构建格子数组的静态方法
        public static ShareLattice ShareLatticeLatticeLayout()
        {
            Image itemImage = new Image().Default();
            Image cornerImage = new Image().Default();
            cornerImage.AddToClassList(itemLatticeCornerClass);
            Label countlbl = new Label().Default();
            countlbl.AddToClassList(itemLatticeLblClass);
            ShareLattice shareLattice = new ShareLattice()
            {
                ItemImage = itemImage,
                CornerImage = cornerImage,
                Label = countlbl,
            };
            shareLattice.AddToClassList(itemLatticeClass);

            shareLattice.Add(itemImage);
            shareLattice.Add(cornerImage);
            shareLattice.Add(countlbl);
            return shareLattice;
        }
        public static Ulattice[] LatticeLayout(VisualElement target, int pageHeight, int pageWidth, IShareLattice shareLattice)
        {
            Ulattice[] ulattices = new Ulattice[pageHeight * pageWidth];
            for (int h = 0; h < pageHeight; h++)
            {
                VisualElement row = new VisualElement();
                row.AddToClassList(viewRowClass);
                row.pickingMode = PickingMode.Ignore;
                target.Add(row);
                for (int w = 0; w < pageWidth; w++)
                {
                    int latticeIndex = h * pageWidth + w;
                    Image itemImage = new Image().Default();
                    Image cornerImage = new Image().Default();
                    cornerImage.AddToClassList(itemLatticeCornerClass);
                    Label countlbl = new Label().Default();
                    countlbl.AddToClassList(itemLatticeLblClass);

                    Ulattice ulattice = new Ulattice(shareLattice, latticeIndex)
                    {
                        ItemImage = itemImage,
                        CornerImage = cornerImage,
                        Label = countlbl,
                    };
                    ulattice.AddToClassList(itemLatticeClass);

                    ulattice.Add(itemImage);
                    ulattice.Add(cornerImage);
                    ulattice.Add(countlbl);

                    ulattices[latticeIndex] = ulattice;
                    row.Add(ulattice);
                }
            }
            return ulattices;
        }
        public static ShareLattice ShareLatticeListLayout()
        {
            Image itemImage = new Image().Default();
            itemImage.AddToClassList(itemLineImageClass);
            Label countLbl = new Label().Default();
            countLbl.AddToClassList(itemLineLblClass);
            ShareLattice shareLattice = new ShareLattice()
            {
                ItemImage = itemImage,
                Label = countLbl,
            };
            shareLattice.AddToClassList(itemLineClass);
            IStyle style = shareLattice.style;
            style.width = 350;
            style.height = 40;
            shareLattice.Add(itemImage);
            shareLattice.Add(countLbl);
            return shareLattice;
        }
        public static Ulattice[] ListLayout(VisualElement target, int pageHeight, IShareLattice shareLattice)
        {
            Ulattice[] ulattices = new Ulattice[pageHeight];
            for (int i = 0; i < ulattices.Length; i++)
            {
                Image itemImage = new Image().Default();
                itemImage.AddToClassList(itemLineImageClass);
                Label countLbl = new Label().Default();
                countLbl.AddToClassList(itemLineLblClass);
                Ulattice ulattice = new Ulattice(shareLattice, i)
                {
                    ItemImage = itemImage,
                    Label = countLbl,
                };
                ulattice.AddToClassList(itemLineClass);
                ulattice.Add(itemImage);
                ulattice.Add(countLbl);

                target.Add(ulattice);
                ulattices[i] = ulattice;
            }
            return ulattices;
        }
        #endregion
        #region 视图回调
        IInvDataInteractionCenter invDataInteractionCenter;
        void PointerEnterView(PointerEnterEvent pointerEnterEvent)
        {
            invDataInteractionCenter.TargetInventory = callBackInventory;
        }
        void PointerLeaveView(PointerLeaveEvent pointerLeaveEvent)
        {
            invDataInteractionCenter.TargetInventory = null;
        }
        public void LatticeDown(Ulattice ulattice)
        {
            invDataInteractionCenter.PointerDown(ulattice);
        }
        public void LatticeUp(Ulattice ulattice)
        {
            invDataInteractionCenter.PointerUp(ulattice);
        }
        public void LatticeEnter(Ulattice ulattice)
        {
            invDataInteractionCenter.PointerEnter(ulattice);
        }
        public void LatticeExit(Ulattice ulattice)
        {
            invDataInteractionCenter.PointerExit(ulattice);
        }
        public void LatticeEndDrag(Ulattice ulattice)
        {
            invDataInteractionCenter.PointerEndDrag(ulattice, callBackInventory);
        }
        #endregion
        #region 数据交互回调
        public IUlatticeItem GetUlatticeItem(int latticeIndex, out int itemIndex)
        {
            itemIndex = GetItemIndex(latticeIndex);
            return callBackInventory.IndexInRange(itemIndex) ? callBackInventory.GetUlatticeItem(itemIndex) : null;
        }
        public ICallBackInventory CallBackInventory => callBackInventory;
        public int GetItemIndex(int latticeIndex) => startIndex + latticeIndex;
        #endregion
        #region 
        Ulattice currentHightLight;
        int currentSelectedLattice;
        public int CurrentSelected
        {
            get => currentSelectedLattice;
            set => currentSelectedLattice = MathC.IndexLoopIClamp(value, ulattices.Length);
        }
        public void SwitchLattice(int v)
        {
            SelectedLattice(currentSelectedLattice + v);
        }
        public void SelectedLattice(int latticeIndex)
        {
            CurrentSelected = latticeIndex;

            currentHightLight?.RemoveFromClassList(itemLatticeSelectedClass);
            currentHightLight = ulattices[currentSelectedLattice];
            currentHightLight.AddToClassList(itemLatticeSelectedClass);

            int itemIndex = GetItemIndex(currentSelectedLattice);
            if (callBackInventory.IndexInRange(itemIndex))
            {
                IUlatticeItem item = callBackInventory.GetUlatticeItem(itemIndex);
                if (CurrentItemNameLabel != null)
                    CurrentItemNameLabel.text = item?.Name ?? string.Empty;
            }
            callBackInventory.CurrentSelectedIndex = itemIndex;
        }
        #endregion
    }
}
