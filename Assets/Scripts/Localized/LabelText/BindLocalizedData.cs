using UnityEngine.UIElements;

namespace CatFramework.Localized
{
    public class BindLocalizedData
    {
        public object userData;
        protected TextElement target;
        public TextElement Target => target;
        ILanguage Language { get; set; }
        public string Original => Language.Original;
        public string Translation => Language.Translation;
        public BindLocalizedData(TextElement target, LocalizedDataKey key)
        {
            this.target = target;
            Language = LocalizedManagerMiao.LanguageCollection.GetLanguage(key);
            UpdateLocalizedData();// 这个方法里需要用到target，所以是必须有target参数的；
        }
        public void ReplaceLanguage(string key)
        {
            Language = LocalizedManagerMiao.LanguageCollection.GetLanguage(key);
            UpdateLocalizedData();
        }
        // 除了更新target的语言以外不能处理别的，基类构造函数调用里这个，并且基类的构造函数是最先执行的
        public virtual void UpdateLocalizedData()
        {
            target.text = Translation;
        }
    }
}
