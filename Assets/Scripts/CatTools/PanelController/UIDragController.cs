using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.Tools
{
    [DisallowMultipleComponent]
    public class UIDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Vector2 newPointerPosition;
        Vector2 offset;
        Vector2 originalLocalPosition;
        [SerializeField] RectTransform Viewport;
        [SerializeField] RectTransform Content;

        bool isRectNonNull;
        private void Awake()
        {
            if (Content == null)
                Content = GetComponent<RectTransform>();
            if (Viewport == null)
                Viewport = Content.parent.GetComponent<RectTransform>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Content != null && Viewport != null)
            {
                isRectNonNull = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewport, eventData.position, eventData.pressEventCamera, out offset);
                originalLocalPosition = Content.localPosition;
                offset -= originalLocalPosition;
            }
            else
                isRectNonNull = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isRectNonNull)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewport, eventData.position, eventData.pressEventCamera, out newPointerPosition);
                Content.localPosition = newPointerPosition - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isRectNonNull)
            {
                Content.localPosition = originalLocalPosition;
                isRectNonNull = false;
            }
        }
        private void OnDisable()
        {
            if (isRectNonNull)
            {
                Content.localPosition = originalLocalPosition;
                isRectNonNull = false;
            }
        }
    }
}