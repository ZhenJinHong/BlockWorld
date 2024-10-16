using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class OneInputActionTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReferenceWithEvent inputAction;
        private void Start()
        {
            inputAction.AddTransferEvent();
        }
        private void OnDestroy()
        {
            inputAction.RemoveTransferEvent();
        }
    }
}