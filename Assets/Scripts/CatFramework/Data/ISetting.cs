namespace CatFramework
{
    /// <summary>
    /// 全局设置，关于UI设置，渲染设置之类的
    /// </summary>
    public interface ISetting
    {
        string Name { get; }
        void ApplySetting();
        //void SaveSetting();
        //void ApplySetting();
        /// <summary>
        /// 不抛出变更事件
        /// </summary>
        void ResetSetting();
    }
}
