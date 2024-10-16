using System;

namespace CatFramework
{
    public static class UiExtension
    {
        static float fixedUpdateDelay = 0.02f;
        public static float FixedUpdateDelay
        {
            get => fixedUpdateDelay;
            set => fixedUpdateDelay = Math.Clamp(value, 0.02f, 5f);
        }
    }
}
