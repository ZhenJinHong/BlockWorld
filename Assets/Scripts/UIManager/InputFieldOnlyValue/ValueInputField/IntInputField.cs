using UnityEngine;
using System;
using CatFramework.CatMath;
namespace CatFramework.UiMiao
{
    public sealed class IntInputField : TextInputField<int>
    {
        int min, max;
        public override void SetValueRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public override int ProcessText(string text)
        {
            if (int.TryParse(text, out int i))
            {
                if (min < max)
                {
                    i = MathC.Clamp(i, min, max);
                }
                return i;
            }
            return value;
        }
        public void Add()
        {
            SetValueWithoutNotify(value << 1);
        }
        public void Subtract()
        {
            SetValueWithoutNotify(value >> 1);
        }
    }
}