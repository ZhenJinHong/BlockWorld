using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine;

namespace CatFramework.InputMiao
{
    public interface ITopViewTarget : IFirstPersonTarget
    {

    }
    public interface ITopViewSettingProvider
    {
        float MovementSpeed { get; }
        float RunSpeed { get; }
        float JumpSpeed { get; }
        float RotationSpeed { get; }
        float RotationThreshold { get; }
    }
    public interface ITopviewInputProvider : IPersonInputProvider
    {
        //Ray Ray { get; }
    }
    public class TopViewInputWarpper : InputActionWrapper, ITopviewInputProvider
    {
        FirstPersonInput inputActions;
        FirstPersonInput InputActions { get { inputActions ??= InputManagerMiao.GetShapeInputAsset<FirstPersonInput>(); return inputActions; } }
        protected override bool Active => inputActions.PlayerInWorld.enabled;
        #region 输入的读取
        public bool LeftPress { get; private set; }
        public bool RightPress { get; private set; }
        // 只在按的时候切换
        public bool Fly { get; private set; }
        public bool Jump { get; private set; }
        public bool Crouch { get; private set; }
        // 长按的时候处于
        public bool Sprint { get; private set; }
        //public Ray Ray { get; private set; }
        public Vector2 PointerCoord { get; private set; }
        [Obsolete]
        public Vector2 PointerDelta { get; private set; }
        public Vector2 MoveDelta { get; private set; }
        #endregion

        public override void Enable()
        {
            InputActions.PlayerInWorld.Enable();
        }
        public override void Disable()
        {
            InputActions.PlayerInWorld.Disable();
        }

        protected override void InternalRegister()
        {
            var playeraction = InputActions.PlayerInWorld;

            playeraction.PrimaryAction.started += LeftBtn;
            playeraction.PrimaryAction.canceled += LeftBtn;
            playeraction.SecondAction.started += RightBtn;
            playeraction.SecondAction.canceled += RightBtn;

            playeraction.ScreenPoint.performed += PointerPosition;
            playeraction.ScreenPoint.canceled += PointerPosition;
            playeraction.Move.performed += OnMove;
            playeraction.Move.canceled += OnMove;

            playeraction.Fly.started += OnFlying;

            playeraction.JumpOrElevated.started += OnJump;
            playeraction.JumpOrElevated.canceled += OnJump;
            playeraction.CrouchOrFalling.started += OnCrouch;
            playeraction.CrouchOrFalling.canceled += OnCrouch;

            playeraction.Sprint.started += OnSprint;
            playeraction.Sprint.canceled += OnSprint;
        }

        protected override void InternalUnregister()
        {
            var playeraction = InputActions.PlayerInWorld;


            playeraction.PrimaryAction.started -= LeftBtn;
            playeraction.PrimaryAction.canceled -= LeftBtn;// 松开也需要监听,否则都布置什么是否松开的
            playeraction.SecondAction.started -= RightBtn;
            playeraction.SecondAction.canceled -= RightBtn;

            playeraction.ScreenPoint.performed -= PointerPosition;
            playeraction.ScreenPoint.canceled -= PointerPosition;
            playeraction.Move.performed -= OnMove;
            playeraction.Move.canceled -= OnMove;

            playeraction.Fly.started -= OnFlying;

            playeraction.JumpOrElevated.started -= OnJump;
            playeraction.JumpOrElevated.canceled -= OnJump;
            playeraction.CrouchOrFalling.started -= OnCrouch;
            playeraction.CrouchOrFalling.canceled -= OnCrouch;

            playeraction.Sprint.started -= OnSprint;
            playeraction.Sprint.canceled -= OnSprint;
        }

        #region 输入
        void LeftBtn(InputAction.CallbackContext context) => LeftPress = context.started;
        void RightBtn(InputAction.CallbackContext context) => RightPress = context.started;
        void PointerPosition(InputAction.CallbackContext context)
        {
            PointerCoord = context.ReadValue<Vector2>();
            //Ray = Camera.main.ScreenPointToRay(PointerCoord);
        }

        void OnFlying(InputAction.CallbackContext context) => Fly = !Fly;
        void OnJump(InputAction.CallbackContext context) => Jump = context.started;
        void OnCrouch(InputAction.CallbackContext context) => Crouch = context.started;
        void OnMove(InputAction.CallbackContext context) => MoveDelta = context.ReadValue<Vector2>();
        void OnSprint(InputAction.CallbackContext context) => Sprint = context.started;
        #endregion
    }
}
