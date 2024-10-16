namespace CatFramework.Localized
{
    public interface ILanguageCollection
    {
        ILanguage GetLanguage(string origianl);
        string Translate(string key);
    }
}