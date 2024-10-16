using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class TwoInputActionTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReferenceWithEvent actionOne;
        [SerializeField] InputActionReferenceWithEvent actionTwo;
        private void Start()
        {
            actionOne.AddTransferEvent();
            actionTwo.AddTransferEvent();
        }
        private void OnDestroy()
        {
            actionOne.RemoveTransferEvent();
            actionTwo.RemoveTransferEvent();
        }
    }
}