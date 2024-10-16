using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public static class UGUIUtility
    {
        public static IPageSizeProvider CreatePageSizeProvider(RectTransform prefabRect, RectTransform content)
        {
            if (content.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup))
            {
                return new HorizontalPageSizeProvider(horizontalLayoutGroup, prefabRect, content);
            }
            else if (content.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup))
            {
                return new VerticalPageSizeProvider(verticalLayoutGroup, prefabRect, content);
            }
            else if (content.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
            {
                return new GridPageSizeProvider(gridLayoutGroup, content);
            }
            else
            {
                return new FixedPageSizeProvider(1);
            }
        }
        public static void ClampInRange(RectTransform Target)
        {
            RectTransform Range = Target.parent as RectTransform;
            //float xMax = Range.rect.xMax - Target.rect.xMax;
            //float yMax = Range.rect.yMax - Target.rect.yMax;
            //float xMin = Range.rect.xMin - Target.rect.xMin;
            //float yMin = Range.rect.yMin - Target.rect.yMin;
            //// 需要的是target在range里local
            //Vector3 localPosition = Target.localPosition;
            //localPosition.x = Mathf.Clamp(localPosition.x, xMin, xMax);
            //localPosition.y = Mathf.Clamp(localPosition.y, yMin, yMax);
            //Target.localPosition = localPosition;
            Target.localPosition = ClampInRect(Range.rect, Target.rect, Target.localPosition);
        }
        public static Vector2 ClampInRect(Rect range, Rect self, Vector2 position)
        {
            float xMax = range.xMax - self.xMax;
            float yMax = range.yMax - self.yMax;
            float xMin = range.xMin - self.xMin;
            float yMin = range.yMin - self.yMin;
            // 需要的是target在range里local
            position.x = Mathf.Clamp(position.x, xMin, xMax);
            position.y = Mathf.Clamp(position.y, yMin, yMax);
            return position;
        }
        public static void ClampFloatView(RectTransform Target, Vector2 screePoint)
        {
            RectTransform Range = Target.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Range, screePoint, null, out Vector2 localPoint);
            Target.localPosition = ClampInRect(Range.rect, Target.rect, localPoint);
        }
        public static void SetText(Text text, string v)
        {
            if (text != null)
                text.text = v;
        }
        public static void SetText(TextMiao text, string v)
        {
            if (text != null)
                text.TranslationKey = v;
        }
        public static void SetTexture(RawImage rawImage, Texture texture)
        {
            if (rawImage != null)
            {
                var color = rawImage.color;
                color.a = texture == null ? 0f : 1f;
                rawImage.color = color;
                rawImage.texture = texture;
            }
        }
        public static void SetSprite(Image image, Sprite sprite)
        {
            if (image != null)
            {
                var color = image.color;
                color.a = sprite == null ? 0f : 1f;
                image.color = color;
                image.overrideSprite = sprite;
            }
        }
        // 方法需要共用时,且允许外部调用
        //public static void SetDirty(this IEnableDirtyView enableDirtyView)
        //{
        //    if (enableDirtyView == null || !enableDirtyView.IsUsable || enableDirtyView.IsDirty) return;
        //    enableDirtyView.IsDirty = true;
        //    UiManagerMiao.AddDirtyView(enableDirtyView);
        //}
        //public static void CleanDirty(this IEnableDirtyView enableDirtyView)
        //{
        //    if (enableDirtyView == null || !enableDirtyView.IsUsable || !enableDirtyView.IsDirty) return;
        //    enableDirtyView.IsDirty = false;
        //    enableDirtyView.Rebuild();
        //    enableDirtyView.Flush();
        //}
        //public static bool SetCollection(this IFlipPageListView flipPageListView, IItemStorageCollection collection)
        //{
        //    if (itemStorageCollection != collection)
        //    {
        //        if (itemStorageCollection != null)
        //        {
        //            itemStorageCollection.OnCollectionChange -= ContentIsChange;
        //        }
        //        itemStorageCollection = collection;
        //        if (itemStorageCollection != null)
        //        {
        //            itemStorageCollection.OnCollectionChange += ContentIsChange;
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //public static VisualItem[] RebuildListViewItems<VisualItem>(IPageSizeProvider pageSizeProvider, IListViewItemProvider<VisualItem> contentProvider, VisualItem[] visualItems)
        //    where VisualItem : class
        //{
        //    if (Assert.IsNull(pageSizeProvider) || Assert.IsNull(contentProvider)) return visualItems;
        //    int pageSize = pageSizeProvider.RecalculatePageSize();
        //    if (pageSize <= 0)
        //    {
        //        if (ConsoleCat.Enable) ConsoleCat.LogWarning($"0大小的页面:{pageSizeProvider.GetType()}|{contentProvider.GetType()} ");
        //        return visualItems;
        //    }

        //    if (visualItems == null)
        //    {
        //        visualItems = new VisualItem[pageSize];
        //        for (int i = 0; i < pageSize; i++)
        //        {
        //            visualItems[i] = contentProvider.MakeItem();
        //        }
        //    }
        //    else if (visualItems.Length != pageSize)
        //    {
        //        VisualItem[] temp = new VisualItem[pageSize];
        //        if (visualItems.Length > pageSize)
        //        {
        //            Array.Copy(visualItems, temp, pageSize);
        //            for (int i = pageSize; i < visualItems.Length; i++)
        //            {
        //                contentProvider.DestroyItem(visualItems[i]);
        //            }
        //        }
        //        else if (visualItems.Length < pageSize)
        //        {
        //            Array.Copy(visualItems, temp, visualItems.Length);
        //            for (int i = visualItems.Length; i < pageSize; i++)
        //            {
        //                temp[i] = contentProvider.MakeItem();
        //            }
        //        }
        //        visualItems = temp;
        //    }
        //    return visualItems;
        //}
    }
}
