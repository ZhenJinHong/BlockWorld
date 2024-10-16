using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class RayBlocker : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TextMiao tip;
        public string TipNoTranslate { set { if (tip != null) tip.TextValue = value; } }
        [SerializeField] Canvas Canvas;
        IBlockerReceiver receiver;
        Action click;
        private void Start()
        {
            if (Canvas == null)
                Canvas = GetComponent<Canvas>();
            if (UiManagerMiao.RayBlocker == null)
                UiManagerMiao.RayBlocker = this;
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            if (UiManagerMiao.RayBlocker == this)
                UiManagerMiao.RayBlocker = null;
        }
        public void Call(IBlockerReceiver blockerReceiver)
        {
            this.receiver = blockerReceiver;
            if (receiver != null && !receiver.IsDestory)
            {
                gameObject.SetActive(true);
                if (Canvas != null)
                    Canvas.sortingOrder = receiver.SortingOrder - 1;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void Call(Action click, int sortingOrder = 2999)
        {
            this.click = click;
            if (click != null)
            {
                gameObject.SetActive(true);
                if (Canvas != null)
                    Canvas.sortingOrder = sortingOrder;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (receiver != null)
            {
                if (receiver.IsDestory)
                {
                    receiver = null;
                }
                else
                {
                    receiver.Intercept();
                }
            }
            click?.Invoke();
            click = null;
            gameObject.SetActive(false);
        }
    }
}