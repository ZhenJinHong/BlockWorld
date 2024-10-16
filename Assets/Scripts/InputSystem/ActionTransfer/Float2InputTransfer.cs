using CatFramework.EventsMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class Float2InputTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReference inputActionReference;
        [SerializeField] Float2Event float2Event = new Float2Event();
        void Start()
        {
            inputActionReference.action.performed += OnVector2;
            inputActionReference.action.canceled += OnVector2;
        }
        private void OnDestroy()
        {
            inputActionReference.action.performed -= OnVector2;
            inputActionReference.action.canceled -= OnVector2;
        }
        public void OnVector2(InputAction.CallbackContext context)
        {
            float2Event.Invoke(context.ReadValue<Vector2>());
        }
    }
}