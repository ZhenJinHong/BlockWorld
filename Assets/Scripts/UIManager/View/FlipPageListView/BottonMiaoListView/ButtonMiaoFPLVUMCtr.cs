using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ButtonMiaoFPLVUMCtr : PageListViewUMCtr<ButtonMiao, IReadOnlyList<IButtonListViewItem>>
    {
        public override int ItemCount => Items == null ? 0 : Items.Count;
        public ButtonMiaoFPLVUMCtr() { }
        protected override ButtonMiao MakeItem()
        {
            var item = base.MakeItem();
            item.OnClick += Click;
            return item;
        }
        protected override void BindItem(int itemIndex, ButtonMiao visualItem)
        {
            IButtonListViewItem item = Items[itemIndex];
            visualItem.Label = item.Name;
            visualItem.useData = item;
        }
        protected override void UnBindItem(int itemIndex, ButtonMiao visualItem)
        {
            visualItem.Label = string.Empty;
            visualItem.useData = null;
        }
        void Click(UiClick uiClick)
        {
            if (uiClick.ButtonMiao.useData is IButtonListViewItem buttonListViewItem)
                buttonListViewItem.Click(uiClick);
        }
    }
}