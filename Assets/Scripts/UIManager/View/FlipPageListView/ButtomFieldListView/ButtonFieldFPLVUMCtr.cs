using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ButtonFieldFPLVUMCtr : PageListViewUMCtr<ButtonField, IReadOnlyList<IButtonFieldListViewItem>>
    {
        public override int ItemCount => Items == null ? 0 : Items.Count;
        public ButtonFieldFPLVUMCtr() { }
        protected override ButtonField MakeItem()
        {
            var item = base.MakeItem();
            item.OnClick += ItemClick;
            return item;
        }
        protected override void BindItem(int itemIndex, ButtonField visualItem)
        {
            IButtonFieldListViewItem item = Items[itemIndex];
            visualItem.Label = item.Name;
            visualItem.Value = item.Value;
            visualItem.useData = item;
        }
        protected override void UnBindItem(int itemIndex, ButtonField visualItem)
        {
            visualItem.Label = string.Empty;
            visualItem.Value = string.Empty;
            visualItem.useData = null;
        }
        private void ItemClick(UiClick click)
        {
            if (click.ButtonMiao.useData is IButtonFieldListViewItem item)
                item.Click(click);
        }
    }
}