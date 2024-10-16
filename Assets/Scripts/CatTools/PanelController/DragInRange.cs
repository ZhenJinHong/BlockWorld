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
                //Debug.Log(localPosition);// ����ֵ�ӽ�,����Ϊtarget��localposition�ǹ̶�����������ڸ�����,����һ������Ĳ�������,�ᵼ��ƫ����ƫ��
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
                // newPointerPosition �� offset���Ǵ�Range��ȡ��,��offset��ʼʱ��ȥ��localposition,�����ֱ�newPointerPosition��,
                // ��(new-(offset-local))
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