namespace CatFramework.Localized
{
    public interface ILanguage//不应该叫binder，这个应当是存数据的类
    {
        /// <summary>
        /// 原文
        /// </summary>
        string Original { get; }
        /// <summary>
        /// 译文
        /// </summary>
        string Translation { get; }
        void SetContent(string translation);
    }
}
