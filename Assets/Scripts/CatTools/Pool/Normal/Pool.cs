using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public class Pool<T> : IPool<T> where T : class
    {
        public override string ToString()
        {
            return IPool.GetPoolInfo(this);
        }
        readonly List<T> pool;
        readonly int maxCapacity;
        public int Cordon => maxCapacity;
        int countAll;
        public int CountAll => countAll;
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => pool.Count;
        readonly IPoolItemProvider<T> poolItemProvider;
        public Pool(IPoolItemProvider<T> poolItemProvider, int initCapacity = 4, int maxCapacity = 2048)
        {
            this.poolItemProvider = poolItemProvider;
            poolItemProvider.Pool = this;
            this.maxCapacity = maxCapacity;
            pool = new List<T>(initCapacity);
        }
        public void Clear()
        {
            countAll -= pool.Count;
            foreach (var item in pool)
            {
                poolItemProvider.Destroy(item);
            }
            pool.Clear();
        }
        public void Dispose()
        {
            Clear();
        }

        public T Get()
        {
            T item;
            if (pool.Count != 0)
            {
                int index = pool.Count - 1;
                item = pool[index];
                pool.RemoveAt(index);
            }
            else
            {
                item = poolItemProvider.Create();
                countAll++;
            }
            poolItemProvider.OnGet(item);
            return item;
        }
        public void Repaid(T item)
        {
            if (pool.Count < maxCapacity)
            {
                poolItemProvider.Reset(item);
                pool.Add(item);
            }
            else
            {
                poolItemProvider.Destroy(item);
                countAll--;

#if UNITY_EDITOR
                Debug.LogWarning("出现对象池销毁对象.");
#endif
            }
        }
    }
}
