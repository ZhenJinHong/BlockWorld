using System;
using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupUMCtr : ViewUMCtrBase
    {
        CanvasGroup canvasGroup;
        public override bool IsVisual => canvasGroup.alpha == 1 || canvasGroup.blocksRaycasts;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public override void Disable()
        {
            canvasGroup.alpha = 0.3f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public override void Enable()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public override void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public override void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }
}