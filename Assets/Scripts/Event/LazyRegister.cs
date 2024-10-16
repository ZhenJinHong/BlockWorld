using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.EventsMiao
{
    public interface ILazyRegister
    {
        bool HasValue { get; }
        Type ContentType { get; }
    }
    public class LazyRegister
    {

    }
    public class LazyRegister<T> : LazyRegister, ILazyRegister where T : class
    {
        T Value;
        Action<T> CallBack;
        public Type ContentType => typeof(T);
        public bool HasValue => !object.Equals(Value, null);
        public T GetValue() => Value;
        public void Reset()
        {
            Value = null;
            CallBack = null;
        }
        public void Unregister(T value)
        {
            if (value == null || Value == null) return;
            if (value == Value)
            {
                Reset();
            }
            else if (ConsoleCat.Enable)
            {
                ConsoleCat.LogWarning($"错误的解除登记对象类型:{typeof(T)};对象内容:{value}");
            }
        }
        public void Register(T value)
        {
            if (value == null) return;
            if (this.Value != null || this.Value == value)
            {
                if (ConsoleCat.Enable)
                {
                    ConsoleCat.LogWarning($"重复登记:{typeof(T)}");
                }
            }
            else
            {
                this.Value = value;
                CallBack?.Invoke(this.Value);
                CallBack = null;
            }
        }
        public void AddListen(Action<T> action)
        {
            if (Value != null)
            {
                action?.Invoke(Value);
            }
            else
            {
                CallBack += action;
            }
        }
        public void RemoveListen(Action<T> action)
        {
            CallBack -= action;
        }
    }
}
