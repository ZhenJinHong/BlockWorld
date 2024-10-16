namespace CatFramework.Tools
{
    public interface IPoolItemProvider<T> where T : class
    {
        public IPool<T> Pool { get; set; }
        /// <summary>
        /// 当对象从池子被取出时
        /// </summary>
        /// <param name="item"></param>
        void OnGet(T item);
        T Create();
        /// <summary>
        /// 当对象归还到池子时
        /// </summary>
        /// <param name="item"></param>
        void Reset(T item);
        void Destroy(T item);
    }
    public class CommonPoolItemProvider<T> : IPoolItemProvider<T>
        where T : class, new()
    {
        public IPool<T> Pool { get; set; }
        public virtual T Create()
        {
            return new T();
        }
        public virtual void Destroy(T item)
        {
        }
        public virtual void OnGet(T item)
        {
        }
        public virtual void Reset(T item)
        {
        }
    }
}
