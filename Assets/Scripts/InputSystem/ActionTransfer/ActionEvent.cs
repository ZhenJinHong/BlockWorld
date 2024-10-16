using CatFramework.EventsMiao;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    [System.Serializable]
    public class ActionEvent : UnityEvent<InputAction.CallbackContext> { }
    [Serializable]
    public struct InputActionReferenceWithEvent
    {
        [SerializeField] InputListenTargetStatus status;
        [SerializeField] InputActionReference inputAction;
        [SerializeField] ActionEvent actionEvent;
        public readonly InputActionReference InputActionReference => inputAction;
        public readonly ActionEvent ActionEvent => actionEvent;
        public readonly InputListenTargetStatus Status => status;
    }
    //[Serializable]
    //public struct InputActionReferenceWithBoolEvent
    //{
    //    [SerializeField] InputActionReference inputAction;
    //    [SerializeField] BoolEvent boolEvent;
    //    public readonly InputActionReference InputActionReference => inputAction;
    //    public readonly BoolEvent BoolEvent => boolEvent;
    //}
    //[Serializable]
    //public class InputNumberActionEvent
    //{
    //    [SerializeField] InputListenTargetStatus status;
    //    [SerializeField] InputActionReference inputAction;
    //    [SerializeField] ActionEvent actionEvent;
    //    public InputActionReference InputActionReference => inputAction;
    //    public InputListenTargetStatus Status => status;
        
    //}
    [Serializable]
    [Flags]
    public enum InputListenTargetStatus
    {
        None = 0,
        Started = 1 << 0,
        Performed = 1 << 1,
        Canceled = 1 << 2,
    }
}