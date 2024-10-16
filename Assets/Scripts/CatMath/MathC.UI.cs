using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.CatMath
{
    public static partial class MathC
    {
        public static int PageCount(int totalItemCount, int pageSize)
        {
            return (totalItemCount + pageSize - 1) / pageSize;
        }
        public static int ClampPageNumInRange(int value, int totalItemCount, int pageSize)
        {
            int pageCount = PageCount(totalItemCount, pageSize);
            value = value < 1 ? 1 : (value > pageCount ? pageCount : value);
            return value;
        }
        public static int ClampPageNumInRange(int value, int totalItemCount, int pageSize, out int pageCount)
        {
            pageCount = PageCount(totalItemCount, pageSize);
            value = value < 1 ? 1 : (value > pageCount ? pageCount : value);
            return value;
        }
        public static int LastPageItemCount(int totalItemCount, int pageSize)
        {
            return (totalItemCount - 1) % pageSize + 1;
        }
        /// <summary>
        /// 计算目标页有多少物品可以用以显示
        /// </summary>
        /// <remarks>
        /// 假定页码已经钳制有效
        /// </remarks>
        public static int PageItemCount(int totalItemCount, int pageNum, int pageSize, out int pageStartIndex)
        {
            pageStartIndex = PageStartIndexInItem(pageNum, pageSize);
            // 特别情况,如果物品数量0,则页开始索引0,最终余量0,返回0// 如果物品数量10,页大小10,页开始索引0,余量10,返回10
            int 余量 = totalItemCount - pageStartIndex;
            return 余量 > pageSize ? pageSize : 余量;
        }
        public static int PageStartIndexInItem(int pageNum, int pageSize)
        {
            return (pageNum - 1) * pageSize;
        }
    }
}
