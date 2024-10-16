using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class Rayblocker : VisualElementController
    {
        Button message;
        public string Message
        {
            get => message.text;
            set
            {
                message.text = value ?? string.Empty;
            }
        }
        
        public Rayblocker(Action click) : base(CreateButton(click), PickingMode.Position)
        {
            AddToClassList(viewAbsFullClass);
            AddToClassList(btnClass);
            
            message = Target as Button;
        }
    }
}
