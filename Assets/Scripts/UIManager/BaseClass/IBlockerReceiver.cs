using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.UiMiao
{
    public interface IBlockerReceiver
    {
        bool IsDestory { get; }
        int SortingOrder { get; }
        /// <summary>
        /// 拦截射线后回调
        /// </summary>
        void Intercept();
    }
}
