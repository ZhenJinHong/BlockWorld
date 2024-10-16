namespace CatFramework.Tools
{
    public interface IPoolItem
    {
        void Dispose();
        /// <summary>
        /// 当被归还后元素要重置初始
        /// </summary>
        void Reset();
    }
}
