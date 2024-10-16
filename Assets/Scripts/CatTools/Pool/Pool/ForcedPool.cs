using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.Tools
{
    public abstract class ForcedPool<T> : IPool<T>, IDisposable where T : class, IPoolItem
    {
        HashSet<T> dispatched;
        Stack<T> pool;
        public int Cordon { get; set; }
        public int CountAll => dispatched.Count + pool.Count;
        public int CountInactive => pool.Count;
        public int CountActive => dispatched.Count;

        public override string ToString()
        {
            return IPool.GetPoolInfo(this);
        }
        public ForcedPool() : this(64, 64) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initCapacity"></param>
        /// <param name="cordon">警戒线</param>
        public ForcedPool(int initCapacity, int cordon)
        {
            this.Cordon = cordon;
            this.pool = new Stack<T>(initCapacity);
            this.dispatched = new HashSet<T>(initCapacity);
            if (ConsoleCat.Enable)
            {
                if (initCapacity > 10240)
                    ConsoleCat.LogWarning("池子初始容量超过10240，请确定是否正确");
                if (cordon > 10240)
                    ConsoleCat.LogWarning("池子警戒容量超过10240，请确定是否正确");
            }
        }
        public virtual void Dispose()
        {
            foreach (T t in pool)
            {
                t.Dispose();
            }
            foreach (T t in dispatched)
            {
                t.Dispose();
            }
            pool = null;
            dispatched = null;
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log($"已释放池子：{this.GetType()}");
            }
        }
        public virtual void ForcedRepaid()
        {
            foreach (T t in dispatched)
            {
                pool.Push(t);
                t.Reset();
            }
            dispatched.Clear();
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log($"已强制回收池子元素：{typeof(T)}");
            }
        }
        public virtual T Get()
        {
            if (!pool.TryPop(out var t))
            {
                t = CreateElement();
            }
            if (!dispatched.Add(t))// add操作是必须执行的
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogError($"已派出的元素已存在于已派出集合{typeof(T)}");
            }
            if (dispatched.Count > Cordon && ConsoleCat.Enable)
            {
                ConsoleCat.LogWarning($"已派出元素数量超过最大允许容量：{Cordon}；当前已派出数量为：{dispatched.Count}");
            }
            return t;
        }
        public PoolObject<T> GetPoolObject()
        {
            return new PoolObject<T>(Get(), this);
        }
        public virtual void Repaid(T poolElement)
        {
            if (poolElement == null) return;
            if (dispatched.Remove(poolElement))
            {
                poolElement.Reset();
                pool.Push(poolElement);
            }
            else
            {
                if (ConsoleCat.Enable)
                {
                    ConsoleCat.LogWarning($"要返还的池元素不存在于出借集合中，类型：{typeof(T)}");
                }
            }
        }
        public void Repaid(T[] poolElements)
        {
            for (int i = 0; i < poolElements.Length; i++)
            {
                Repaid(poolElements[i]);
                poolElements[i] = null;
            }
        }
        public void Repaid(IList<T> poolElements)
        {
            for (int i = 0; i < poolElements.Count; i++)
            {
                Repaid(poolElements[i]);
                poolElements[i] = null;
            }
        }
        protected abstract T CreateElement();
    }
}
