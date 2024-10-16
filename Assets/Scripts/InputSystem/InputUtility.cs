using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.PlayerInput;

namespace CatFramework.InputMiao
{
    public static class InputUtility
    {
        //public static void AddTransferEvent(this InputActionReference inputAction, ActionEvent actionEvent)
        //    => inputAction.action.AddTransferEvent(actionEvent);
        //public static void RemoveTransferEvent(this InputActionReference inputAction, ActionEvent actionEvent)
        //   => inputAction.action.RemoveTransferEvent(actionEvent);
        //public static void AddTransferEvent(this InputActionReference inputAction, ActionEventWrapper actionEvent)
        //    => inputAction.action.AddTransferEvent(actionEvent);
        //public static void RemoveTransferEvent(this InputActionReference inputAction, ActionEventWrapper actionEvent)
        //   => inputAction.action.RemoveTransferEvent(actionEvent);
        //public static void AddTransferEvent(this InputAction inputAction, ActionEvent actionEvent)
        //{
        //    if (Assert.IsNull(inputAction)) return;
        //    inputAction.started += actionEvent.Invoke;
        //    inputAction.performed += actionEvent.Invoke;
        //    inputAction.canceled += actionEvent.Invoke;
        //}
        //public static void RemoveTransferEvent(this InputAction inputAction, ActionEvent actionEvent)
        //{
        //    if (Assert.IsNull(inputAction)) return;
        //    inputAction.started -= actionEvent.Invoke;
        //    inputAction.performed -= actionEvent.Invoke;
        //    inputAction.canceled -= actionEvent.Invoke;
        //}
        //public static void AddTransferEvent(this InputAction inputAction, ActionEventWrapper actionEvent)
        //{
        //    if (Assert.IsNull(inputAction)) return;
        //    if (actionEvent.Status.HasFlag(InputListenTargetStatus.Started))
        //        inputAction.started += actionEvent.ActionEvent.Invoke;
        //    if (actionEvent.Status.HasFlag(InputListenTargetStatus.Performed))
        //        inputAction.performed += actionEvent.ActionEvent.Invoke;
        //    if (actionEvent.Status.HasFlag(InputListenTargetStatus.Canceled))
        //        inputAction.canceled += actionEvent.ActionEvent.Invoke;
        //}
        //public static void RemoveTransferEvent(this InputAction inputAction, ActionEventWrapper actionEvent)
        //{
        //    if (Assert.IsNull(inputAction)) return;
        //    inputAction.started -= actionEvent.ActionEvent.Invoke;
        //    inputAction.performed -= actionEvent.ActionEvent.Invoke;
        //    inputAction.canceled -= actionEvent.ActionEvent.Invoke;
        //}
        public static void AddTransferEvent(this InputActionReferenceWithEvent inputAction)
        {
            if (Assert.IsNull(inputAction.InputActionReference) ||
                Assert.IsNull(inputAction.InputActionReference.action) ||
                Assert.IsNull(inputAction.ActionEvent)) return;
            InputAction action = inputAction.InputActionReference.action;
            ActionEvent actionEvent = inputAction.ActionEvent;
            InputListenTargetStatus status = inputAction.Status;
            if (status == InputListenTargetStatus.None)
            {
                action.started += actionEvent.Invoke;
                action.performed += actionEvent.Invoke;
                action.canceled += actionEvent.Invoke;
            }
            else
            {
                if (status.HasFlag(InputListenTargetStatus.Started))
                    action.started += actionEvent.Invoke;
                if (status.HasFlag(InputListenTargetStatus.Performed))
                    action.performed += actionEvent.Invoke;
                if (status.HasFlag(InputListenTargetStatus.Canceled))
                    action.canceled += actionEvent.Invoke;
            }
        }
        public static void RemoveTransferEvent(this InputActionReferenceWithEvent inputAction)
        {
            if (Assert.IsNull(inputAction.InputActionReference) ||
                Assert.IsNull(inputAction.InputActionReference.action) ||
                Assert.IsNull(inputAction.ActionEvent)) return;
            InputAction action = inputAction.InputActionReference.action;
            ActionEvent actionEvent = inputAction.ActionEvent;
            InputListenTargetStatus status = inputAction.Status;
            action.started -= actionEvent.Invoke;
            action.performed -= actionEvent.Invoke;
            action.canceled -= actionEvent.Invoke;
        }
    }
}
