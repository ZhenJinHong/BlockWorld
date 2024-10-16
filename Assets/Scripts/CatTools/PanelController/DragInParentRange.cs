using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.Tools
{
    [DisallowMultipleComponent]
    public class DragInParentRange : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        RectTransform Range;
        [SerializeField] RectTransform Target;
        Vector2 offset;
        float xMax;
        float yMax;
        float xMin;
        float yMin;
        bool isDragActive;
        private void Awake()
        {
            if (Target == null)
            {
                Target = GetComponent<RectTransform>();
            }
            Range = transform.parent.GetComponent<RectTransform>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Target != null && Range != null)
            {
                isDragActive = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Range, eventData.position, eventData.pressEventCamera, out offset);
                xMax = Range.rect.xMax - Target.rect.xMax - Target.localPosition.x + offset.x;
                yMax = Range.rect.yMax - Target.rect.yMax - Target.localPosition.y + offset.y;
                xMin = Range.rect.xMin - Target.rect.xMin - Target.localPosition.x + offset.x;
                yMin = Range.rect.yMin - Target.rect.yMin - Target.localPosition.y + offset.y;
                offset -= new Vector2(Target.localPosition.x, Target.localPosition.y);
            }
            else
                isDragActive = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (isDragActive)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Range, eventData.position, eventData.pressEventCamera, out Vector2 newPointerPosition);
                newPointerPosition.Set(
                    Mathf.Clamp(newPointerPosition.x, xMin, xMax),
                    Mathf.Clamp(newPointerPosition.y, yMin, yMax)
                    );
                Target.localPosition = newPointerPosition - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDragActive)
            {
                isDragActive = false;
            }
        }
    }
}