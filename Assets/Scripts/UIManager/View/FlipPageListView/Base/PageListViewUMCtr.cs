using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public abstract class PageListViewUMCtr<VisualItem, Collection> : PageListViewController<VisualItem, Collection>
        where VisualItem : Component, IUiItem
        where Collection : class
    {
        public VisualItem Prefab;
        public RectTransform Parent;
        protected override VisualItem MakeItem()
        {
            return UnityEngine.Object.Instantiate(Prefab, Parent);
        }
        protected override void DestroyItem(VisualItem visualItem)
        {
            UnityEngine.Object.Destroy(visualItem.gameObject);
        }
    }
}