using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.Tools
{
    public struct PoolObject<T> : IDisposable where T : class, IPoolItem
    {
        T toReturn;
        IPool<T> forcedPool;
        public PoolObject(T obj, IPool<T> forcedPool)
        {
            toReturn = obj;
            this.forcedPool = forcedPool;
        }
        public void Dispose()
        {
            forcedPool.Repaid(toReturn);
        }
    }
    public class PoolUnion<T> where T : class, IPoolItem
    {
        protected ForcedPool<T> pool;
        public bool HasPool => pool != null;
        T m_Value;
        public T Value
        {
            get
            {
                m_Value ??= pool.Get();
                return m_Value;
            }
        }
        public PoolUnion(ForcedPool<T> pool)
        {
            this.pool = pool;
        }
        /// <summary>
        /// 主动归还或被强制归还，后续仍可以重新借用
        /// </summary>
        public void RepaidElement()
        {
            if (m_Value != null)
            {
                pool.Repaid(m_Value);
                m_Value = null;
            }
        }
        /// <summary>
        /// 彻底回收，不再允许借用
        /// </summary>
        public void Dispose()
        {
            RepaidElement();
            pool = null;//
        }
    }
}
