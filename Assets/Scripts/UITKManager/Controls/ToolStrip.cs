using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class ToolStrip : VisualElementController
    {
        public ToolStrip(VisualElement target) : base(target)
        {

        }
        public void Show(PointerUpEvent pointerUpEvent)
        {
            ConsoleCat.Log(pointerUpEvent.position);
        }
    }
}
