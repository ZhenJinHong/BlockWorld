using System.Text;

namespace CatFramework
{
    public interface IDebuggerInfoItem
    {
        string Name { get; }

        void GetInfo(StringBuilder stringBuilder);
    }
}
