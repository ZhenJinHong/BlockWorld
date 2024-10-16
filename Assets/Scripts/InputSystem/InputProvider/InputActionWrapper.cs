namespace CatFramework.InputMiao
{
    public abstract class InputActionWrapper
    {
        protected abstract bool Active { get; }
        bool hasRegister;
        public void RegisterAction()
        {
            if (hasRegister)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("重复登记输入");
                return;
            }
            hasRegister = true;
            InternalRegister();
        }
        public void UnregisterAction()
        {
            if (!hasRegister) return;
            hasRegister = false;
            InternalUnregister();
        }
        protected abstract void InternalRegister();
        protected abstract void InternalUnregister();
        public void SetActive(bool value)
        {
            if (value) Enable();
            else Disable();
        }
        public abstract void Enable();
        public abstract void Disable();
    }
}
