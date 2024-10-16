using System;

namespace CatFramework.UiMiao
{
    public interface IButtonListViewItem
    {
        string Name { get; }
        void Click(UiClick uiClick);// 以此方便子类自定Action传递不同类型的数据
    }
}