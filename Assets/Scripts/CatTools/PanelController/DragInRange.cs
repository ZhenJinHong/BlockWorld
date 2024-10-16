using CatFramework.CatMath;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.Tools
{
    [DisallowMultipleComponent]
    public class DragInRange : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform Range;
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
                Target = GetComponent<RectTransform>();
            if (Range == null)
                Range = transform.parent.GetComponent<RectTransform>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Target != null && Range != null)
            {
                isDragActive = true;
                //RectTransformUtility.ScreenPointToWorldPointInRectangle();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Range, eventData.position, eventData.pressEventCamera, out offset);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Target.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPosition);
                //Debug.Log(localPosition);// 两个值接近,但因为target的localposition是固定有轴心相对于父级的,所以一旦点击的不是中心,会导致偏移有偏差
                //Debug.Log(Target.localPosition);
                xMax = Range.rect.xMax - Target.rect.xMax;
                yMax = Range.rect.yMax - Target.rect.yMax;
                xMin = Range.rect.xMin - Target.rect.xMin;
                yMin = Range.rect.yMin - Target.rect.yMin;
                offset -= new Vector2(localPosition.x, localPosition.y);
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
                // newPointerPosition 和 offset都是从Range获取的,而offset开始时减去了localposition,现在又被newPointerPosition减,
                // 即(new-(offset-local))
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