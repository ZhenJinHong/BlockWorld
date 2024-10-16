using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public sealed class BoolInputField : TInputField<bool>, IPointerClickHandler
    {
        [SerializeField] Image boolDisplay;
        public override void SetValueWithoutNotify(bool value)
        {
            this.value = value;
            if (boolDisplay)
                boolDisplay.enabled = value;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            SetValueWithoutNotify(!value);
            onSubmit?.Invoke(this);
        }
    }
}