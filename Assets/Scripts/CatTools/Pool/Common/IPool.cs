using System;

namespace CatFramework.Tools
{
    public interface IPool : IDisposable
    {
        public int Cordon { get; }
        public int CountAll { get; }
        public int CountActive { get; }
        public int CountInactive { get; }
        public static string GetPoolInfo(IPool pool)
        {
            return $"{pool.GetType()} \n 当前总对象数量 : {pool.CountAll};\n 活跃数量 : {pool.CountActive} \n 非活跃数量 : {pool.CountInactive} \n 池子警戒上限值 : {pool.Cordon}";
        }
    }
    public interface IPool<T> : IPool
    {
        public T Get();
        public void Repaid(T value);
    }
}
