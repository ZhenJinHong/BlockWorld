using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class ToolStripMiao : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TextMiao valueText;
        IReadOnlyList<IToolStripContent> options;
        public IReadOnlyList<IToolStripContent> Contents
            => options;
        public void SetContentWithoutNotify(IReadOnlyList<IToolStripContent> contents)
        {
            options = contents;
        }
        public void SetContent(IReadOnlyList<IToolStripContent> contents)
        {
            options = contents;
            if (options != null && options.Count != 0)
            {
                Click(options[0]);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (UiManagerMiao.shareToolStrip != null)
            {
                UiManagerMiao.shareToolStrip.Show(Click, options, transform as RectTransform);
            }
        }

        private void Click(IToolStripContent content)
        {
            if (content != null)
            {
                content.Click();
                valueText.TranslationKey = content.Label;
            }
        }
    }
}