using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.Tools
{
    public static class ListUtility
    {
        public static int ListLength(IList list)
        {
            return list == null ? 0 : list.Count;
        }
        public static int SafeLength(this IList list)
        {
            return list == null ? 0 : list.Count;
        }
        public static void ListConverter<T>(IList source, IList<T> dst, Func<object, T> converter)
        {
            dst.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                object item = source[i];
                dst.Add(converter(item));
            }
        }
        public static void ListConverter<S, D>(IList<S> source, IList<D> dst, Func<S, D> converter)
        {
            dst.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                S item = source[i];
                dst.Add(converter(item));
            }
        }
        public static D[] ArrayConverter<S, D>(S[] source, Func<S, D> converter)
        {
            D[] ds = new D[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                ds[i] = converter(source[i]);
            }
            return ds;
        }
    }
}
