using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public interface IViewControl
    {
        public bool IsVisual { get; }
        VisualElement Parent { get; }
        VisualElement Self { get; }
        event Action<IViewControl> OnOpen;
        event Action<IViewControl> OnClose;

        void Close();
        void CloseWithoutNotify();
        void Hide();
        void HideWithoutNotify();
        void Open();
        void OpenOrClose(bool open);
        void OpenOrClose();
        void OpenWithoutNotify();
        void Show();
        void ShowOrHide(bool show);
        void ShowOrHide();
        void ShowOrHideWithoutNotify(bool show);
        void ShowWithoutNotify();
    }
}
