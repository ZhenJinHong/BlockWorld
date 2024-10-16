using System;
using System.Collections;
using UnityEngine;
using CatFramework.CatMath;
namespace CatFramework.UiMiao
{
    public sealed class FloatInputField : TextInputField<float>
    {
        float min, max;
        public override void SetValueRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public override float ProcessText(string text)
        {
            if (float.TryParse(text, out float i))
            {
                if (min < max)
                {
                    i = MathC.Clamp(i, min, max);
                }
                return i;
            }
            return value;
        }
    }
}