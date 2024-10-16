using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework
{
    public static class Assert
    {
        public static bool NotNull(object obj, string message = "非空对象")
        {
            if (obj != null)
            {
                if (ConsoleCat.Enable) { ConsoleCat.LogWarning(message); }
                return true;
            }
            return false;
        }
        public static bool IsNull(object obj, string message = "空对象")
        {
            if (obj == null)
            {
                if (ConsoleCat.Enable) { ConsoleCat.LogWarning(message); }
                return true;
            }
            return false;
        }
        public static bool IsTrue(bool v, string message = "是正确的")
        {
            if (v && ConsoleCat.Enable)
                ConsoleCat.LogWarning(message);
            return v;
        }
        public static bool IsFalse(bool v, string message = "是错误的")
        {
            if (!v && ConsoleCat.Enable)
                ConsoleCat.LogWarning(message);
            return !v;
        }
        public static bool CountOutofLimit(IList list, int limit)
        {
            if (list.Count > limit)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning($"{list.GetType().FullName} 超过上限 : {limit} !");
                return true;
            }
            return false;
        }
        public static bool RepeatInitialized(object self, bool repeat)
        {
            if (repeat && ConsoleCat.Enable)
                ConsoleCat.LogWarning($"对象 : {self ?? "NULL"} 重复初始化");
            return repeat;
        }
        public static bool HasCancel(bool cancel)
        {
            if (cancel && ConsoleCat.Enable)
                ConsoleCat.LogWarning("取消");
            return cancel;
        }
    }
}
