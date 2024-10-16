using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class CanvasEnableUMCtr : ViewUMCtrBase
    {
        public override bool IsVisual => gameObject.activeSelf;

        public override void Disable()
        {
            gameObject.SetActive(false);
        }

        public override void Enable()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}