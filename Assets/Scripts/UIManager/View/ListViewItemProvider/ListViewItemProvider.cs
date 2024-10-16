//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CatFramework.UiMiao
//{
//    public interface IListViewItemProvider
//    {
//        int ItemCount { get; }
//    }
//    public abstract class UGUIListViewItemProvider<VisualItem> : IListViewItemProvider where VisualItem : Component
//    {
//        public abstract int ItemCount { get; }
//        public VisualItem prefab;
//        public RectTransform content;
//        public UGUIListViewItemProvider(VisualItem prefab, RectTransform content)
//        {
//            this.prefab = prefab;
//            this.content = content;
//        }
//        public virtual VisualItem MakeItem()
//        {
//            return UnityEngine.Object.Instantiate(prefab, content);
//        }
//        public virtual void DestroyItem(VisualItem visualItem)
//        {
//            UnityEngine.Object.Destroy(visualItem.gameObject);
//        }
//        public abstract void BindItem(int itemIndex, VisualItem visualItem);
//        public abstract void UnbindItem(int itemIndex, VisualItem visualItem);
//    }
//}
