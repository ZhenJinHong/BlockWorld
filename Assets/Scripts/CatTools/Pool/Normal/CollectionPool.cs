using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public class CollectionItemProvider<TCollection, TItem> : IPoolItemProvider<TCollection>
        where TCollection : class, ICollection<TItem>, new()
    {
        public IPool<TCollection> Pool { get; set; }
        public TCollection Create()
        {
            return new TCollection();
        }
        public void Destroy(TCollection item)
        {
            item.Clear();
        }
        public void OnGet(TCollection item)
        {
            item.Clear();
        }
        public void Reset(TCollection item)
        {
        }
    }
    public class CollectionPool<TCollection, TItem>
        where TCollection : class, ICollection<TItem>, new()
    {
        internal static readonly Pool<TCollection> pool = new Pool<TCollection>(new CollectionItemProvider<TCollection, TItem>());
        public static TCollection Get() => pool.Get();
        public static void Repaid(TCollection item) => pool.Repaid(item);
    }
    public class ListPool<TValue> : CollectionPool<List<TValue>, TValue>
    {
    }
}
