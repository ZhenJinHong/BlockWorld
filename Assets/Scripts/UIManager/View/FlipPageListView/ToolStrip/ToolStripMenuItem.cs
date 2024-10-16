using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class ToolStripMenuItem : MaskableGraphicMiao, IPointerClickHandler
    {
        [SerializeField] TextMiao label;
        public string Label
        {
            get => label != null ? label.TranslationKey : string.Empty;
            set { if (label != null) label.TranslationKey = value; }
        }
        IToolStripContent content;
        IToolStripHeader header;
        public void SetContent(IToolStripContent content, IToolStripHeader header)
        {
            this.content = content;
            this.header = header;
            Label = content?.Label;
        }
        public void Clear()
        {
            content = null;
            this.header = null;
            Label = string.Empty;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (content != null)
            {
                header?.ItemHasClick(content);
            }

        }
    }
}