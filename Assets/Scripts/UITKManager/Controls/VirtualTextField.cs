using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static CatFramework.UiTK.VisualElementController;

namespace CatFramework.UiTK
{
    public class VirtualTextField : VisualElement
    {
        readonly Label label;
        public string Label
        {
            get { return label.text; }
            set { label.text = value; }
        }
        readonly Label value;
        public string Value
        {
            get { return value.text; }
            set { this.value.text = value; }
        }
        public VirtualTextField() : base()
        {
            AddToClassList(viewRowClass);
            AddToClassList(virtualtextfieldClass);// 为了有互动
            label = new Label().Default();
            label.AddToClassList(virtualtextfieldClass);
            value = new Label().Default();
            value.AddToClassList(virtualtextfieldClass);
            Add(label);
            Add(value);
        }
    }
}
