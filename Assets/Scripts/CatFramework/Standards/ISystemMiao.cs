using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public interface ISystemMiao
    {
        string Name { get; }
        void GetInfo(StringBuilder stringBuilder);
    }
}
