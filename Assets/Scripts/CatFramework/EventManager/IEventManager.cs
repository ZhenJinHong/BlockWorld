using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public interface IEventManager
    {
        void AddListen<T>(T args, Action<T> action);// 在已经知道数据的情况下,监听数据的变动
        void RemoveListen<T>(T args, Action<T> action);
    }
}
