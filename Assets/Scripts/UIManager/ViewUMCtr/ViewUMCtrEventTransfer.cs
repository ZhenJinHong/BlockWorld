using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ViewUMCtrEventTransfer : MonoBehaviour
    {
        ViewUMCtrBase viewUMCtrBase;
        [SerializeField] ClickedEvent onOpen = new ClickedEvent();
        [SerializeField] ClickedEvent onClose = new ClickedEvent();
        private void Start()
        {
            if (TryGetComponent<ViewUMCtrBase>(out viewUMCtrBase))
            {
                viewUMCtrBase.OnOpen += Open;
                viewUMCtrBase.OnClose += Close;
            }
            else if (ConsoleCat.Enable) ConsoleCat.LogWarning("未找到视图控制器");
        }
        void Open(ViewUMCtrBase viewUMCtr)
        {
            onOpen.Invoke();
        }
        void Close(ViewUMCtrBase viewUMCtr)
        {
            onClose.Invoke();
        }
    }
}