using System;

namespace CatFramework.UiMiao
{
    public class ButtonListViewItem : IButtonListViewItem
    {
        public string Name { get; set; }
        public Action<UiClick> OnClick { get; set; }
        public void Click(UiClick uiClick)
        {
            OnClick?.Invoke(uiClick);
        }
    }
}