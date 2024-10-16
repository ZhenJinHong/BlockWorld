using System.Collections.Generic;
using UnityEngine;
using System;
namespace CatFramework.Tools
{
    public interface IItemPageCallBack<T> where T : MonoBehaviour
    {
        /// <summary>
        /// 翻页按钮监听该事件
        /// </summary>
        Action LastPage { set; }
        /// <summary>
        /// 翻页按钮监听该事件
        /// </summary>
        Action NextPage { set; }
        /// <summary>
        /// 加Option用以区分Line，这里的ItemCount是选项要显示的信息物的总数
        /// </summary>
        int TotalItemCount { get; }
        T InstantiateItem();
        void ItemUpdateDisplay(T monoBehaviour, int itemPointer);
        void ClearItem(T monoBehaviour);
    }
    public class PageController<T> where T : MonoBehaviour
    {
        /// <summary>
        /// 当前页码，非物品索引
        /// </summary>
        int currentPageNum;
        /// <summary>
        /// 当前显示的物品数量
        /// </summary>
        int currentVisibleOptionCount;
        T[] options = new T[0];
        bool isInit;
        IItemPageCallBack<T> itemPageCallBack;
        /// <summary>
        /// 初始化
        /// </summary>
        public void SetCallBack(IItemPageCallBack<T> itemPageCallBack)
        {
            if (isInit) return;
            isInit = true;
            this.itemPageCallBack = itemPageCallBack;
            itemPageCallBack.LastPage = LastPage;
            itemPageCallBack.NextPage = NextPage;
        }
        public void FlushPage()
        {
            UpdatePage(1);
        }
        public void ToFirstPage()
        {
            UpdatePage(1);
        }
        public void ToEndPage()
        {
            UpdatePage(-1);
        }
        public void ToSelectedItem(int itemPointer)
        {
            UpdatePage(currentPageNum = itemPointer / options.Length + 1);
        }
        void LastPage()
        {
            UpdatePage(currentPageNum--);
        }
        void NextPage()
        {
            UpdatePage(currentPageNum++);
        }
        public void AdjustPageSize(int pageSize)
        {
            if (itemPageCallBack != null && pageSize > 0 && pageSize != options.Length)
            {
                T[] temp = new T[pageSize];
                if (pageSize < options.Length)
                {
                    System.Array.Copy(options, temp, pageSize);
                    for (int i = pageSize; i < options.Length; i++)
                    {
                        UnityEngine.Object.Destroy(options[i].gameObject);
                    }
                }
                else
                {
                    System.Array.Copy(options, temp, options.Length);
                    for (int i = options.Length; i < pageSize; i++)
                    {
                        temp[i] = itemPageCallBack.InstantiateItem();
                    }
                }
                options = temp;
            }
            currentVisibleOptionCount = options.Length;
        }
        /// <summary>
        /// 页码从1开始（不同于索引的方式），当传来的页码超出总页数，则页码归1，如果页码低于1，则转为总页数
        /// </summary>
        /// <param name="pageNum"></param>
        void UpdatePage(int pageNum)
        {
            if (itemPageCallBack == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("未赋值回调");
#endif
                return;
            }
            int totalItemCount = itemPageCallBack.TotalItemCount;
            if (itemPageCallBack == null || options.Length == 0 || totalItemCount == 0) return;
            
            int totalPageCount = (totalItemCount - 1) / options.Length + 1;
            int needVisibleOptionCount;
            if (pageNum < 1)
            {
                pageNum = totalPageCount;
            }
            else if (pageNum > totalItemCount)
            {
                pageNum = 1;
            }
            currentPageNum = pageNum;//当前页码需要全局使用
            if (pageNum == totalPageCount)//如果页码为最后一页
            {
                needVisibleOptionCount = (totalItemCount - 1) % options.Length + 1;//计算需要显示的物品数，最后一页不一定全页满
            }
            else
            {
                needVisibleOptionCount = options.Length;
            };

            //如果小于已显示的数量，说明要从显示的数量里关闭
            if (needVisibleOptionCount < currentVisibleOptionCount)
            {
                for (int i = needVisibleOptionCount; i < currentVisibleOptionCount; i++)
                {
                    itemPageCallBack.ClearItem(options[i]);
                }
            }
            //保存当次显示的选项数
            currentVisibleOptionCount = needVisibleOptionCount;
            int firstItemIndex = (currentPageNum - 1) * options.Length;
            for (int i = 0; i < currentVisibleOptionCount; i++, firstItemIndex++)
            {
                itemPageCallBack.ItemUpdateDisplay(options[i], firstItemIndex);
            }
        }
    }
}