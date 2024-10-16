using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace CatFramework.UiMiao
{
    public class Palette : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] Image grayscale;
        [SerializeField] Image hue;
        [SerializeField] RectTransform positionIndicator;
        RectTransform rectTransform;
        Action<Vector2> setHsv;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void AddListener(Action<Vector2> setHsv)
        {
            this.setHsv = setHsv;
        }
        public void SetValueWithoutNotify(Color color, Vector2 pointAnchor)
        {
            hue.color = color;
            positionIndicator.anchorMin = pointAnchor;
            positionIndicator.anchorMax = pointAnchor;
        }
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out Vector2 localpoint);
            localpoint = Rect.PointToNormalized(rectTransform.rect, localpoint);
            //if (positionIndicator.anchorMin != localpoint)
            //{

            //}
            positionIndicator.anchorMin = localpoint;
            positionIndicator.anchorMax = localpoint;
            setHsv?.Invoke(localpoint);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out Vector2 localpoint);
            localpoint = Rect.PointToNormalized(rectTransform.rect, localpoint);
            positionIndicator.anchorMin = localpoint;
            positionIndicator.anchorMax = localpoint;
            setHsv?.Invoke(localpoint);
        }
    }
}