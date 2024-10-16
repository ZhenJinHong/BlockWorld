using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

namespace CatFramework.UiMiao
{
    public class DropdownMiao : TInputField<int>, IPointerClickHandler
    {
        [SerializeField] TextMiao valueText;
        protected List<string> optionsData = new List<string>();
        public List<string> OptionsData => optionsData;
        public void AddOption(string option)
        {
            optionsData.Add(option);
        }
        public void AddOptions<T>(IList<T> res, Func<T, string> converter)
        {
            for (int i = 0; i < res.Count; i++)
            {
                T item = res[i];
                AddOption(converter(item));
            }
        }
        public void AddOptions(string[] options)
        {
            optionsData.AddRange(options);
        }
        public void AddOptions(List<string> options)
        {
            optionsData.AddRange(options);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var d = UiManagerMiao.shareDropdownList;
            if (d != null)
            {
                d.Show(Selected, optionsData, transform as RectTransform);
            }
        }
        void Selected(int index)
        {
            SetValueWithoutNotify(index);
            onSubmit?.Invoke(this);
        }
        public override void SetValueWithoutNotify(int index)
        {
            if (index > -1 && index < optionsData.Count)
            {
                this.value = index;
                if (valueText != null)
                    valueText.TranslationKey = optionsData[index];
            }
        }
    }
}