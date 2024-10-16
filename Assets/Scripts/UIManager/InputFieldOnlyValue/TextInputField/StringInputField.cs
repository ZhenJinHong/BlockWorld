using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public sealed class StringInputField : TextInputField<string>
    {
        public override void SetValueWithoutNotify(string value)
        {
            value ??= string.Empty;
            base.SetValueWithoutNotify(value);
        }
        public override string ProcessText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            return value;
        }
    }
}