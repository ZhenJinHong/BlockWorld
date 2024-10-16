namespace CatFramework.Localized
{
    public struct LocalizedDataKey
    {
        public string LabelKey;
        public LocalizedDataKey(string labelKey)
        {
            LabelKey = labelKey;
        }

        public static implicit operator string(LocalizedDataKey lacalizedDataKey)
        {
            return lacalizedDataKey.LabelKey;
        }
    }
}
