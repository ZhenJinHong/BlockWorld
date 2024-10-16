using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    /// <summary>
    /// 画布组无法阻止父级射线器的射线投射(会导致依旧执行查询(但不会执行UI事件),浪费时间),
    /// 只有射线器可以阻止,父级射线器会在查到射线器后直接返回,不会继续查询子级UI
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class CanvasRaycasterUMCtr : ViewUMCtrBase
    {
        CanvasGroup canvasGroup;
        GraphicRaycaster graphicRaycaster;
        public override bool IsVisual => canvasGroup.alpha == 1 || graphicRaycaster.enabled;
        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }
        public override void Enable()
        {
            canvasGroup.alpha = 1;
            graphicRaycaster.enabled = true;
        }

        public override void Disable()
        {
            canvasGroup.alpha = 0.1f;
            graphicRaycaster.enabled = false;
        }

        public override void Show()
        {
            canvasGroup.alpha = 1;
            graphicRaycaster.enabled = true;
        }

        public override void Hide()
        {
            canvasGroup.alpha = 0;
            graphicRaycaster.enabled = false;
        }
    }
}
