using CatFramework.Tools;
using System.Text;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class DebuggerView : MonoBehaviour
    {
        [SerializeField] DebuggerInfoView infoPrefab;
        [SerializeField] ButtonMiao btnPrefab;
        [SerializeField] RectTransform infoParent;
        [SerializeField] RectTransform btnsParent;
        StringBuilder stringBuilder = new StringBuilder();
        private IDebuggerInfoItem[] options;
        DebuggerInfoView[] debuggerInfoViews;
        public IDebuggerInfoItem[] Options
        {
            get => options;
            set
            {
                if (options != value)
                {
                    options = value;
                    Rebuild();
                }
            }
        }
        Timer timer;
        private void Update()
        {
            if (debuggerInfoViews == null) return;
            if (timer.Ready())
            {
                timer.AppendDelay(0.5f);
                for (int i = 0; i < debuggerInfoViews.Length; i++)
                {
                    var debuggerInfoView = debuggerInfoViews[i];
                    if (debuggerInfoView != null)
                        debuggerInfoView.UpdateInfo(stringBuilder);
                    stringBuilder.Clear();
                }
            }
        }
        void Rebuild()
        {
            UnityUtility.DestroyAllChild(btnsParent);
            if (debuggerInfoViews != null)
            {
                UnityUtility.DestroyAllChild(infoParent);
                debuggerInfoViews = null;
            }
            if (options == null || options.Length == 0) return;
            debuggerInfoViews = new DebuggerInfoView[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                var buttonMiao = Instantiate(btnPrefab, btnsParent);
                var option = options[i];
                buttonMiao.Label = option.Name;
                buttonMiao.useData = option;
                buttonMiao.OnClick += OptionClick;
            }
        }
        void OptionClick(UiClick uiClick)
        {
            if (uiClick.ButtonMiao.useData is IDebuggerInfoItem debuggerInfoOption)
            {
                int index = uiClick.ButtonMiao.transform.GetSiblingIndex();
                if (index > -1 && index < options.Length)
                {
                    DebuggerInfoView debuggerInfoView = debuggerInfoViews[index];
                    if (debuggerInfoView == null)
                    {
                        debuggerInfoView = Instantiate(infoPrefab, infoParent);
                        debuggerInfoViews[index] = debuggerInfoView;
                        debuggerInfoView.Set(debuggerInfoOption);
                    }
                    else
                    {
                        UnityUtility.Destroy(debuggerInfoView.gameObject);
                        debuggerInfoViews[index] = null;
                    }
                }
            }
        }
    }
}
