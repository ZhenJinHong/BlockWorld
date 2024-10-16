using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class StringFPLVUMCtr : PageListViewUMCtr<ButtonMiao, IReadOnlyList<string>>
    {
        public override int ItemCount => Items == null ? 0 : Items.Count;
        public StringFPLVUMCtr() { }
        public event Action<int> OnSelectedItem;
        protected override ButtonMiao MakeItem()
        {
            var item = base.MakeItem();
            item.OnClick += Click;
            return item;
        }
        protected override void BindItem(int itemIndex, ButtonMiao visualItem)
        {
            visualItem.Label = Items[itemIndex];
            visualItem.useData = itemIndex;
        }
        protected override void UnBindItem(int itemIndex, ButtonMiao visualItem)
        {
            visualItem.Label = string.Empty;
            visualItem.useData = null;
        }
        void Click(UiClick uiClick)
        {
            //int index = Array.IndexOf(VisualItems, uiClick.ButtonMiao);
            //if (index != -1)
            //    OnSelectedItem?.Invoke(GetItemIndex(index));
            if (uiClick.ButtonMiao.useData is int index)
                OnSelectedItem?.Invoke(index);
        }
    }
}