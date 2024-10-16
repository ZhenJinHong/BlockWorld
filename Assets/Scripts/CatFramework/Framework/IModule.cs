namespace CatFramework
{
    public interface IModule
    {
        // 需要知道的是,有的模块不允许使用的情况下一些相关联的东西也失活,比如gameobject
        /// <summary>
        /// 模块是否允许使用
        /// </summary>
        bool IsUsable { get; }
    }
    public interface IEnableIterateModule : IModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>false -> 结束迭代</returns>
        bool Update();
        void EndUpdate();
    }
}
