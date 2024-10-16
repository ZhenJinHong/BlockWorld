using UnityEngine;

namespace CatFramework.UiMiao
{
    [RequireComponent(typeof(ButtonMiao))]
    public class ButtonEventTransfer : MonoBehaviour
    {
        [SerializeField] ClickedEvent onClick = new ClickedEvent();
        ButtonMiao buttonMiao;
        private void Start()
        {
            buttonMiao = GetComponent<ButtonMiao>();
            buttonMiao.OnClick += Click;
        }
        void Click(UiClick _)
        {
            onClick.Invoke();
        }
    }
}