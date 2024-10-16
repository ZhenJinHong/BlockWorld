namespace CatFramework.UiMiao
{
    public interface IButtonFieldListViewItem
    {
        string Name { get; }
        string Value { get; }
        void Click(UiClick uiClick);// 以此方便子类自定Action传递不同类型的数据
    }
}