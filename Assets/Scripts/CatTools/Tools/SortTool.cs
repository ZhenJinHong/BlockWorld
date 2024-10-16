using System;
using System.Collections.Generic;

namespace CatFramework.Tools
{
    [Obsolete]
    public class SortTool
    {
        /// <summary>
        /// 降序
        /// </summary>
        public static void SortLongDown(long[] longs)
        {
            int i, j;
            int max;
            long temp;
            int length = longs.Length;
            for (i = 0; i < length-1; i++)
            {
                max = i;
                for (j = i+1; j < length; j++)
                {
                    if (longs[j] > longs[max])
                    {
                        max = j;
                    }
                }
                if (max != i)
                {
                    temp = longs[max];
                    longs[max] = longs[i];
                    longs[i] = temp;
                }
            }
        }
        /// <summary>
        /// 降序
        /// </summary>
        public static void SortLongDown<T>(long[] longs, T[] values)
        {
            if(longs.Length == values.Length)
            {
                int i, j;
                int max;
                long temp;
                T valuestemp;
                int length = longs.Length;
                for (i = 0; i < length - 1; i++)
                {
                    max = i;
                    for (j = i + 1; j < length; j++)
                    {
                        if (longs[j] > longs[max])
                        {
                            max = j;
                        }
                    }
                    if (max != i)
                    {
                        temp = longs[max];
                        valuestemp = values[max];
                        longs[max] = longs[i];
                        values[max] = values[i];
                        longs[i] = temp;
                        values[i] = valuestemp;
                    }
                }
            }
        }
        /// <summary>
        /// 降序
        /// </summary>
        public static void SortLongDown<T>(long[] longs, List<T> values)
        {
            //if(longs.Length != values.Count)
            //{
            //    ConsoleMgr.Error("将排序的键值长度不一致");
            //}
            //else
            //{
            //    int i, j;
            //    bool swapped;
            //    long temp;
            //    T tempT;
            //    for (i = 0; i < longs.Length - 1; i++) 
            //    {
            //        swapped = false;
            //        for (j = i + 1; j < longs.Length - 1 - i; j++)
            //        {
            //            if(longs[j] < longs[j + 1])
            //            {
            //                temp = longs[j];

            //                longs[j] = longs[j + 1];
            //                longs[j + 1] = temp;
            //                if (!swapped)
            //                    swapped = true;
            //            }
            //        }
            //        if (!swapped)
            //            return;
            //    }
            //}
            if (longs.Length == values.Count)
            {
                int i, j;
                int max;
                long temp;
                T valuestemp;
                int length = longs.Length;
                for (i = 0; i < length - 1; i++)
                {
                    max = i;
                    for (j = i + 1; j < length; j++)
                    {
                        if (longs[j] > longs[max])
                        {
                            max = j;
                        }
                    }
                    if (max != i)
                    {
                        //不可以用值元组交换，否则无法对原数组处理
                        temp = longs[max];
                        valuestemp = values[max];
                        longs[max] = longs[i];
                        values[max] = values[i];
                        longs[i] = temp;
                        values[i] = valuestemp;
                    }
                }

            }
        }
    }
}