using CatFramework.EventsMiao;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class KeyBoolActionTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReference inputActionReference;
        [SerializeField] BoolEvent boolEvent = new BoolEvent();// 原因:必须确保具体是按下还是松开
        private void Start()
        {
            inputActionReference.action.started += Press;
            inputActionReference.action.canceled += Press;
        }
        private void OnDestroy()
        {
            inputActionReference.action.started -= Press;
            inputActionReference.action.canceled -= Press;
        }
        void Press(InputAction.CallbackContext context)
        {
            boolEvent.Invoke(context.started);// 只订阅了started和canceled,不存在performed
        }
    }
    //public class KeyBoolsActionTransfer : MonoBehaviour
    //{

    //}
}