//using System;
//using System.Collections.Generic;

//namespace CatFramework.UiMiao
//{
//    public class StringListViewContentProvider : BaseListViewContentProvider<ButtonMiao>
//    {
//        public IList<string> items;
//        public override int ContentCount => items == null ? 0 : items.Count;

//        public event Action<int> OnSelectedItem;
//        public override ButtonMiao MakeItem()
//        {
//            var button = base.MakeItem();
//            button.OnClick += Click;
//            return button;
//        }
//        public override void BindItem(int itemIndex, ButtonMiao visualItem)
//        {
//            visualItem.Label = items[itemIndex];
//            visualItem.useData = itemIndex;
//        }
//        public override void UnBindItem(int itemIndex, ButtonMiao visualItem)
//        {
//            visualItem.Label = string.Empty;
//            visualItem.useData = null;
//        }
//        void Click(UiClick uiClick)
//        {
//            if (uiClick.ButtonMiao.useData is int index)
//                OnSelectedItem?.Invoke(index);
//        }
//    }
//}
