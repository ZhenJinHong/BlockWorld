using CatFramework.CatMath;
using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class UIntInputField : TextInputField<uint>
    {
        uint min, max;
        public override void SetValueRange(uint min, uint max)
        {
            this.min = min;
            this.max = max;
        }
        public override uint ProcessText(string text)
        {
            if (uint.TryParse(text, out uint i))
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