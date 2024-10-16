using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public interface IKeyBoolInputProvider
    {
        bool Press { get; }
    }
    public class KeyBoolInputProvider : InputActionWrapper, IKeyBoolInputProvider
    {
        InputAction inputAction;// 要序列化得用reference
        public bool Press { get; private set; }
        protected override bool Active => inputAction.enabled;
        public KeyBoolInputProvider(InputAction inputAction)
        {
            this.inputAction = inputAction;
            Assert.IsNull(inputAction);
        }
        public KeyBoolInputProvider(InputActionReference inputAction) : this(inputAction.action) { }
        void Key(InputAction.CallbackContext context) => Press = context.started;
        protected override void InternalRegister()
        {
            inputAction.started += Key;
            inputAction.canceled += Key;
        }
        protected override void InternalUnregister()
        {
            inputAction.started -= Key;
            inputAction.canceled -= Key;
        }
        public override void Enable()
        {
            inputAction.Enable();
        }
        public override void Disable()
        {
            inputAction.Disable();
        }
    }
}
