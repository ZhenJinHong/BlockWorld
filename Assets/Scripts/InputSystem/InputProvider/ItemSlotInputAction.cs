using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public class ItemSlotInputAction : InputActionWrapper
    {
        InputActionMap map;
        InputAction one;
        InputAction two;
        InputAction three;
        InputAction four;
        InputAction five;
        InputAction six;
        InputAction seven;
        InputAction eight;
        InputAction nine;
        InputAction scroll;
        public ItemSlotInputAction(InputActionAsset asset, string mapName = "ItemSlot")
        {
            map = asset.FindActionMap(mapName, true);
            one = map.FindAction("One", true);
            two = map.FindAction("Two", true);
            three = map.FindAction("Three", true);
            four = map.FindAction("Four", true);
            five = map.FindAction("Five", true);
            six = map.FindAction("Six", true);
            seven = map.FindAction("Seven", true);
            eight = map.FindAction("Eight", true);
            nine = map.FindAction("Nine", true);
            scroll = map.FindAction("Scroll", true);
        }
        public Action<int> OnSelected;
        public Action<int> OnScroll;
        protected override bool Active => map.enabled;
        public override void Enable()
        {
            map.Enable();
        }
        public override void Disable()
        {
            map.Disable();
        }
        protected override void InternalRegister()
        {
            scroll.started += Scroll;
            one.started += OnOne;
            two.started += OnTwo;
            three.started += OnThree;
            four.started += OnFour;
            five.started += OnFive;
            six.started += OnSix;
            seven.started += OnSeven;
            eight.started += OnEight;
            nine.started += OnNine;
        }

        protected override void InternalUnregister()
        {
            scroll.started -= Scroll;
            one.started -= OnOne;
            two.started -= OnTwo;
            three.started -= OnThree;
            four.started -= OnFour;
            five.started -= OnFive;
            six.started -= OnSix;
            seven.started -= OnSeven;
            eight.started -= OnEight;
            nine.started -= OnNine;
        }
        public void Scroll(InputAction.CallbackContext context) => OnScroll?.Invoke(context.ReadValue<Vector2>().y > 0 ? -1 : 1);
        void OnOne(InputAction.CallbackContext context) => OnSelected?.Invoke(0);
        void OnTwo(InputAction.CallbackContext context) => OnSelected?.Invoke(1);
        void OnThree(InputAction.CallbackContext context) => OnSelected?.Invoke(2);
        void OnFour(InputAction.CallbackContext context) => OnSelected?.Invoke(3);
        void OnFive(InputAction.CallbackContext context) => OnSelected?.Invoke(4);
        void OnSix(InputAction.CallbackContext context) => OnSelected?.Invoke(5);
        void OnSeven(InputAction.CallbackContext context) => OnSelected?.Invoke(6);
        void OnEight(InputAction.CallbackContext context) => OnSelected?.Invoke(7);
        void OnNine(InputAction.CallbackContext context) => OnSelected?.Invoke(8);
    }
}
