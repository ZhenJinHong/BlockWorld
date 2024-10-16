//using System;
//using UnityEngine;

//namespace CatFramework.UiMiao
//{
//    public abstract class BaseListViewContentProvider<VisualItem> : IListViewItemProvider<VisualItem>
//        where VisualItem : Component, IUiItem
//    {
//        public VisualItem Prefab;
//        public RectTransform Content;
//        public virtual void DestroyItem(VisualItem visualItem)
//        {
//            UnityEngine.Object.Destroy(visualItem.gameObject);
//        }
//        public virtual VisualItem MakeItem()
//        {
//            return UnityEngine.Object.Instantiate(Prefab, Content);
//        }
//        public abstract void BindItem(int itemIndex, VisualItem visualItem);
//        public abstract void UnBindItem(int itemIndex, VisualItem visualItem);
//    }
//}
