using CatFramework.EventsMiao;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class KeyDownActionTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReference inputActionReference;
        [SerializeField] TriggerEvent pressEvent = new TriggerEvent();
        private void Start()
        {
            inputActionReference.action.started += Press;
        }
        private void OnDestroy()
        {
            inputActionReference.action.started -= Press;
        }
        void Press(InputAction.CallbackContext context)
        {
            pressEvent.Invoke();
        }
    }
}