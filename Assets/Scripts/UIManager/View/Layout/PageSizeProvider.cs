using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public interface IPageSizeProvider
    {
        int MaxPageSize { get; set; }
        int PageSize { get; }
        int RecalculatePageSize();
    }
    public abstract class PageSizeProvider : IPageSizeProvider
    {
        public static int PageSizeLimit = 128;
        protected readonly RectTransform content;
        int maxPageSize;
        public int MaxPageSize
        {
            get => maxPageSize; set => maxPageSize = value < 1 ? 64 : Math.Min(value, PageSizeLimit);
        }
        public int PageSize { get; protected set; }
        public PageSizeProvider(RectTransform content)
        {
            this.content = content;
        }
        public abstract int RecalculatePageSize();
    }
    public class HorizontalPageSizeProvider : PageSizeProvider
    {
        readonly HorizontalLayoutGroup horizontalLayoutGroup;
        readonly RectTransform prefabRect;
        public HorizontalPageSizeProvider(HorizontalLayoutGroup horizontalLayoutGroup, RectTransform prefabRect, RectTransform content) : base(content)
        {
            this.horizontalLayoutGroup = horizontalLayoutGroup;
            this.prefabRect = prefabRect;
            MaxPageSize = 64;
            PageSize = 1;
        }
        public override int RecalculatePageSize()
        {
            RectOffset padding = horizontalLayoutGroup.padding;
            float unitWidth = prefabRect.rect.width + horizontalLayoutGroup.spacing;
            float contentWidth = content.rect.width;
            contentWidth = contentWidth - padding.left - padding.right;
            PageSize = Mathf.Clamp((int)(contentWidth / unitWidth), 1, MaxPageSize);
            return PageSize;
        }
    }
    public class VerticalPageSizeProvider : PageSizeProvider
    {
        readonly VerticalLayoutGroup verticalLayoutGroup;
        readonly RectTransform prefabRect;
        public VerticalPageSizeProvider(VerticalLayoutGroup verticalLayoutGroup, RectTransform prefabRect, RectTransform content) : base(content)
        {
            this.verticalLayoutGroup = verticalLayoutGroup;
            this.prefabRect = prefabRect;
            MaxPageSize = 64;
            PageSize = 1;
        }
        public override int RecalculatePageSize()
        {
            RectOffset padding = verticalLayoutGroup.padding;
            float unitHeight = prefabRect.rect.height + verticalLayoutGroup.spacing;
            float contentHeight = content.rect.height;
            contentHeight = contentHeight - padding.top - padding.bottom;
            PageSize = Mathf.Clamp((int)(contentHeight / unitHeight), 1, MaxPageSize);
            //Debug.Log($"单元高度:{unitHeight};内容高度:{contentHeight};页大小:{PageSize}");
            return PageSize;
        }

    }
    public class GridPageSizeProvider : PageSizeProvider
    {
        readonly GridLayoutGroup gridLayoutGroup;
        public GridPageSizeProvider(GridLayoutGroup gridLayoutGroup, RectTransform content) : base(content)
        {
            this.gridLayoutGroup = gridLayoutGroup;
            MaxPageSize = 64;
            PageSize = 1;
        }
        public override int RecalculatePageSize()
        {
            RectOffset padding = gridLayoutGroup.padding;
            Vector2 unitSize = gridLayoutGroup.cellSize + gridLayoutGroup.spacing;
            Vector2 contentSize = content.rect.size;
            int PageHeight = (int)((contentSize.y - padding.left - padding.right) / unitSize.y);
            if (PageHeight < 1)
                PageHeight = 1;
            int PageWidth = (int)((contentSize.x - padding.top - padding.bottom) / unitSize.x);
            if (PageWidth < 1)
                PageWidth = 1;
            PageSize = Mathf.Clamp(PageHeight * PageWidth, 1, MaxPageSize);
            if (gridLayoutGroup.constraint != GridLayoutGroup.Constraint.FixedRowCount)
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            if (gridLayoutGroup.constraintCount != PageHeight)
                gridLayoutGroup.constraintCount = PageHeight;
            return PageSize;
        }
    }
    public class FixedPageSizeProvider : IPageSizeProvider
    {
        public int MaxPageSize { get; set; }
        public int PageSize { get; set; }
        public FixedPageSizeProvider(int pageSize)
        {
            PageSize = pageSize;
        }
        public int RecalculatePageSize()
        {
            return PageSize;
        }
    }
}