using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class MultiInputActionTransfer : MonoBehaviour
    {
        [SerializeField] InputActionReferenceWithEvent[] events;
        private void Start()
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].AddTransferEvent();
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].RemoveTransferEvent();
            }
        }
    }
}