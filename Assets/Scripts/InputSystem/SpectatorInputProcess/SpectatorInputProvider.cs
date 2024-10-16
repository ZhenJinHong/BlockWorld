using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    [Serializable]
    public class SpectatorInputProvider : InputActionWrapper, ISpectatorInputProvider
    {
        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference look;
        [SerializeField] InputActionReference sprint;
        [SerializeField] InputActionReference elevated;
        [SerializeField] InputActionReference falling;
        public InputAction MoveAction => move.action;
        public InputAction LookAction => look.action;
        public InputAction SprintAction => sprint.action;
        public InputAction ElevatedAction => elevated.action;
        public InputAction FallingAction => falling.action;
        public Vector2 MoveDelta { get; private set; }
        public bool Sprint { get; private set; }
        public Vector2 PointDelta { get; private set; }
        public float Lifting { get; private set; }
        protected override bool Active => MoveAction.enabled;

        public float XAngle { get; set; }
        public float YAngle { get; set; }

        public SpectatorInputProvider()
        {
        }
        public override void Enable()
        {
            MoveAction.actionMap.Enable();
        }
        public override void Disable()
        {
            MoveAction.actionMap.Disable();
        }

        void OnMoveDelta(InputAction.CallbackContext context) 
            => MoveDelta = context.ReadValue<Vector2>();
        void OnSprint(InputAction.CallbackContext context)
            => Sprint = !context.canceled;
        // 松开的时候是0
        void OnRise(InputAction.CallbackContext context)
        {
            Lifting = context.ReadValue<float>();
            Lifting = Lifting == 0 ? 0 : Lifting;
        }
        void OnFall(InputAction.CallbackContext context)
        {
            Lifting = context.ReadValue<float>();
            Lifting = Lifting == 0 ? 0 : -Lifting;
        }
        void OnViewScroll(InputAction.CallbackContext context) 
            => PointDelta = context.ReadValue<Vector2>();

        protected override void InternalRegister()
        {
            MoveAction.started += OnMoveDelta;
            MoveAction.performed += OnMoveDelta;
            MoveAction.canceled += OnMoveDelta;

            SprintAction.started += OnSprint;
            SprintAction.canceled += OnSprint;

            LookAction.started += OnViewScroll;
            LookAction.canceled += OnViewScroll;

            ElevatedAction.started += OnRise;
            ElevatedAction.canceled += OnRise;
            FallingAction.started += OnFall;
            FallingAction.canceled += OnFall;
        }

        protected override void InternalUnregister()
        {
            MoveAction.started -= OnMoveDelta;
            MoveAction.performed -= OnMoveDelta;
            MoveAction.canceled -= OnMoveDelta;

            SprintAction.started -= OnSprint;
            SprintAction.canceled -= OnSprint;

            LookAction.started -= OnViewScroll;
            LookAction.canceled -= OnViewScroll;

            ElevatedAction.started -= OnRise;
            ElevatedAction.canceled -= OnRise;
            FallingAction.started -= OnFall;
            FallingAction.canceled -= OnFall;
        }
    }
}
