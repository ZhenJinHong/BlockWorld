using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ButtonField : ButtonMiao
    {
        [SerializeField] TextMiao value;
        public string Value
        {
            get => value != null ? value.TranslationKey : string.Empty;
            set { if (this.value != null) this.value.TranslationKey = value; }
        }
    }
}