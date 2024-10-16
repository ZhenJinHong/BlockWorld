using System;

namespace CatFramework
{
    public interface IDataEventLinked
    {
        Type Type { get; }
        object Value { get; }

        void NotifyChange();
    }
    public interface IDataEventLinked<T> : IDataEventLinked
    {
        T TValue { get; }

        event Action<T> Change;
    }
    public class DataEventLinked<T> : IDataEventLinked<T> where T : class
    {
        public event Action<T> Change;
        public Type Type => typeof(T);
        public T TValue { get; private set; }
        public object Value => TValue;
        public DataEventLinked(T value)
        {
            TValue = value;
        }
        //public void AddListen(Action<object> action)
        //{
        //    Change += action;
        //}
        //public void TT<C>(Action<C> action)
        //{
        //    if (this.TValue is C)
        //        Change += action;// 无法执行
        //}
        //public void AddListen<C>(Action<C> action) where C : T
        //{

        //}
        public void NotifyChange()
        {
            Change?.Invoke(TValue);
        }
    }
}
