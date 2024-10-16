using CatFramework.CatMath;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class TopViewDragScreen : MonoBehaviour
    {
        public InputActionReference wasdMove;
        public InputActionReference pointMove;
        public InputActionReference liftingMove;
        public Vector3 min;
        public Vector3 max;
        public float HeightBlessings;
        public float MoveSpeed;
        public float DragSpeed;
        public float LiftingSpeed;
        // Use this for initialization
        void Start()
        {
            Register();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;

            pos = MathC.V3AddV2(pos, Time.deltaTime * MoveSpeed * move);

            pos = MathC.V3AddV2(pos, DragSpeed * (pointDelta * (1f + HeightBlessings * pos.y)));
            float liftingDelta = LiftingSpeed * lifting;
            pos.y += liftingDelta;

            transform.position = MathC.Clamp(pos, min, max);
        }
        private void OnDestroy()
        {
            UnRegister();
        }
        void Register()
        {
            InputAction action = wasdMove.action;
            action.performed += WASD;
            action.canceled += WASD;

            action = pointMove.action;
            action.performed += PointDelta;
            action.canceled += PointDelta;

            action = liftingMove.action;
            action.performed += Lifting;
            action.canceled += Lifting;
        }
        void UnRegister()
        {
            InputAction action = wasdMove.action;
            action.performed -= WASD;
            action.canceled -= WASD;

            action = pointMove.action;
            action.performed -= PointDelta;
            action.canceled -= PointDelta;

            action = liftingMove.action;
            action.performed -= Lifting;
            action.canceled -= Lifting;
        }
        Vector2 move;
        Vector2 pointDelta;
        float lifting;
        public void WASD(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }
        public void PointDelta(InputAction.CallbackContext context)
        {
            pointDelta = context.ReadValue<Vector2>();
        }
        public void Lifting(InputAction.CallbackContext context)
        {
            lifting = context.ReadValue<Vector2>().y;
        }
    }
}