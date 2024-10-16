using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class EnquireViewController : VisualViewController
    {
        public EnquireViewController(VisualElement target, Action ok, Action cancel = null) : base(target)
        {
            AddToClassList(viewRowClass);
            AddToClassList(viewChildCenterCenterClass);

            Button okBtn = CreateButton(ok, "确定");
            okBtn.clicked += Close;

            Button cancelBtn = CreateButton(cancel, "取消");
            cancelBtn.clicked += Close;

            Add(okBtn);
            Add(cancelBtn);
        }
    }
}
