using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class VisualViewController : VisualElementController, IViewControl
    {
        public VisualViewController(VisualElement target) : base(target)
        {
        }

        public VisualElement Parent => Target.parent;
        public VisualElement Self => Target;

        public event Action<IViewControl> OnOpen;
        public event Action<IViewControl> OnClose; 
        public override void Open()
        {
            base.Open();
            OnOpen?.Invoke(this);
        }
        public override void Close()
        {
            base.Close();
            OnClose?.Invoke(this);
        }
    }
}
