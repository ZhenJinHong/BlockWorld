using CatFramework.CatMath;
using CatFramework.EventsMiao;
using System;
using System.Collections;

namespace CatFramework.UiMiao
{
    public abstract class PageListViewController<VisualItem, Collection> : IPageListViewController
        where VisualItem : class, IUiItem
        where Collection : class
    {
        public bool destroyItemWhenClose;
        public abstract int ItemCount { get; }
        IPageSizeProvider pageSizeProvider;
        VisualItem[] visualItems;
        public IPageSizeProvider PageSizeProvider
        {
            get => pageSizeProvider;
            set => pageSizeProvider = value;
        }
        Collection items;
        public Collection Items
        {
            get => items;
            set
            {
                if (items != value)
                {
                    if (items != null)
                        DataBindingEventManager.UnBind(items, CollectionIsChange);
                    items = value;
                    if (items != null)
                        DataBindingEventManager.Bind(items, CollectionIsChange);
                }
            }
        }
        protected VisualItem[] VisualItems => visualItems;
        public event Action<IPageListViewController> ViewIsUpdate;
        public PageListViewController() { }
        #region EnableDirtyInterface
        bool isDirty;
        bool IEnableDirtyView.IsDirty { get => isDirty; set => isDirty = value; }
        bool isUsable = true;
        public bool IsUsable
        {
            get => isUsable;
            set => isUsable = value;
        }
        // 由数据变更时调用
        protected void CollectionIsChange(object sender, object data)
        {
            if (active)
                SetPageNum(pageNum);
        }
        void IEnableDirtyView.CleanDirty()
        {
            if (!IsUsable) return;
            if (!isDirty)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning("非脏的情况下执行了清理脏");
                return;
            }
            isDirty = false;
            Rebuild();
            int pageSize = VisualItemCount;
            pageNum = MathC.ClampPageNumInRange(pageNum, ItemCount, pageSize, out pageCount);
            startIndex = MathC.PageStartIndexInItem(pageNum, pageSize);
            Flush();
        }
        // 对于数据修改后的行为最好是通过数据绑定的事件通知视图,除非有需要重新指定(非重算)页码的需要
        public void SetPageNum(int targetPage)
        {
            if (!IsUsable) return;
            pageNum = targetPage;
            if (!isDirty)
            {
                isDirty = true;
                UiManagerMiao.AddDirtyView(this);
            }
        }
        void Rebuild()
        {
            int pageSize = pageSizeProvider.RecalculatePageSize();
            if (pageSize <= 0)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning($"0大小的页面 : {this.GetType()} ");
                return;
            }
            if (visualItems == null)
            {
                visualItems = new VisualItem[pageSize];
                for (int i = 0; i < pageSize; i++)
                {
                    visualItems[i] = MakeItem();
                }
            }
            else if (visualItems.Length != pageSize)
            {
                VisualItem[] temp = new VisualItem[pageSize];
                if (visualItems.Length > pageSize)
                {
                    Array.Copy(visualItems, temp, pageSize);
                    for (int i = pageSize; i < visualItems.Length; i++)
                    {
                        DestroyItem(visualItems[i]);
                    }
                }
                else if (visualItems.Length < pageSize)
                {
                    Array.Copy(visualItems, temp, visualItems.Length);
                    for (int i = visualItems.Length; i < pageSize; i++)
                    {
                        temp[i] = MakeItem();
                    }
                }
                visualItems = temp;
            }
        }

        void Flush()
        {
            if (CheckDataIsValid())
            {
                int length = ItemCount;
                for (int i = 0; i < visualItems.Length; i++)
                {
                    VisualItem visualItem = visualItems[i];
                    int itemIndex = i + startIndex;
                    if (itemIndex > -1 && itemIndex < length)
                    {
                        BindItem(itemIndex, visualItem);
                    }
                    else
                    {
                        UnBindItem(itemIndex, visualItem);
                    }
                }
                ViewIsUpdate?.Invoke(this);
                FlushOther();
            }
        }
        protected virtual void FlushOther() { }
        #endregion
        public virtual int GetVisualItemIndex(VisualItem visualItem)
        {
            return Array.IndexOf(visualItems, visualItem);
        }
        bool active;
        int pageNum = 1;
        int pageCount;
        int startIndex;
        public int PageNum => pageNum;
        public int PageCount => pageCount;
        public int VisualItemCount => visualItems == null ? 0 : visualItems.Length;
        public VisualItem this[int index] => visualItems[index];
        #region 响应外界行为
        public virtual void Open()
        {
            active = true;
            SetPageNum(pageNum);
        }
        public virtual void Close()
        {
            active = false;
            if (visualItems != null)
            {
                if (destroyItemWhenClose)
                {
                    for (int i = 0; i < visualItems.Length; i++)
                    {
                        DestroyItem(visualItems[i]);
                    }
                    visualItems = null;
                }
                else
                {
                    for (int i = 0; i < visualItems.Length; i++)
                    {
                        VisualItem item = visualItems[i];
                        item.DefaultStyle();// 对于具体悬停在哪个上,未知;只能全部都应用默认
                        UnBindItem(GetItemIndex(i), item);
                    }
                }
            }
        }
        #endregion
        #region 物品可视化实例与更新
        public void OnScroll(int v)
        {
            SetPageNum(pageNum + v);
        }
        public void OnScroll(float delta)
        {
            SetPageNum(pageNum + (delta > 0f ? -1 : 1));
        }

        protected abstract VisualItem MakeItem();
        protected abstract void DestroyItem(VisualItem visualItem);
        protected abstract void BindItem(int itemIndex, VisualItem visualItem);
        /// <summary>
        /// 索引是溢出范围的
        /// </summary>
        protected abstract void UnBindItem(int itemIndex, VisualItem visualItem);

        public int GetItemIndex(int visulItemIndex)
            => startIndex + visulItemIndex;
        //public int GetItemIndex(VisualItem visualItem)
        //    => startIndex + GetVisualItemIndex(visualItem);
        //public bool TryGetItemIndex(VisualItem visualItem, out int itemIndex)
        //{
        //    itemIndex = GetItemIndex(visualItem);
        //    return Items != null && Items.IndexInRange(itemIndex);
        //}
        #endregion
        public virtual bool CheckDataIsValid()
        {
            // 0个物品也有效,否则会导致0个物品时无法刷新列表
            return /*ItemCount != 0 && */visualItems != null && visualItems.Length != 0;
        }
    }
}