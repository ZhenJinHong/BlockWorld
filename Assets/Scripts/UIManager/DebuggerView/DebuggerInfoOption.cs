using System;
using System.Text;

namespace CatFramework.UiMiao
{

    public class DebuggerInfoOption : IDebuggerInfoItem
    {
        public string Name { get; }
        public void GetInfo(StringBuilder stringBuilder)
        {
            getInfo?.Invoke(stringBuilder);
        }
        Action<StringBuilder> getInfo;
        public DebuggerInfoOption(string name, Action<StringBuilder> getInfo)
        {
            Name = name;
            this.getInfo = getInfo;
        }
    }
}
