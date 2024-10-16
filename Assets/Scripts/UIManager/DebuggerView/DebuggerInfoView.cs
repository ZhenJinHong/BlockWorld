using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public class DebuggerInfoView : MonoBehaviour
    {
        [SerializeField] Text title;
        [SerializeField] Text text;
        IDebuggerInfoItem debuggerInfoOption;
        public void Set(IDebuggerInfoItem debuggerInfoOption)
        {
            this.debuggerInfoOption = debuggerInfoOption;
            title.text = debuggerInfoOption?.Name ?? string.Empty;
        }
        public void UpdateInfo(StringBuilder stringBuilder)
        {
            stringBuilder.Clear();
            debuggerInfoOption?.GetInfo(stringBuilder);
            text.text = stringBuilder.ToString() ?? string.Empty;
        }
    }
}
