using System;

namespace CatFramework.UiMiao
{
    public class ButtonFieldListViewItem : IButtonFieldListViewItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Action<UiClick> OnClick;
        public void Click(UiClick uiClick)
        {
            OnClick?.Invoke(uiClick);
        }
    }
}