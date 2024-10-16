using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace CatFramework.Tools
{
    [DisallowMultipleComponent]
    public class SimpleScrollController : MonoBehaviour, IScrollHandler
    {
        [SerializeField] RectTransform Viewport;
        [SerializeField] RectTransform Content;
        [SerializeField] bool isHorizontal = false;
        [SerializeField] float speed = 3;
        public float Speed { get { return speed; } set { speed = value; } }
        public void OnScroll(PointerEventData eventData)
        {
            if (Content == null || Viewport == null) return;
            float enableMax;
            float enableMin;
            float offset;
            if (isHorizontal)
            {
                enableMax = Viewport.rect.xMax - Content.rect.xMax;
                enableMin = Viewport.rect.xMin - Content.rect.xMin;
            }
            else
            {
                enableMax = Viewport.rect.yMax - Content.rect.yMax;
                enableMin = Viewport.rect.yMin - Content.rect.yMin;
            }
            Vector3 localPosition = Content.localPosition; //RectTransform.localposition是轴心相对父级中心的像素点坐标
            if (isHorizontal)
            {
                if (Content.rect.width < Viewport.rect.width)
                {
                    Content.localPosition = new Vector3(enableMin, localPosition.y, localPosition.z);
                    return;
                }
                offset = localPosition.x + eventData.scrollDelta.y * speed;//滚动的移动量
                offset = offset < enableMax ? enableMax : (offset > enableMin ? enableMin : offset);
                Content.localPosition = new Vector3(offset, localPosition.y, localPosition.z);
            }
            else
            {
                if (Content.rect.height < Viewport.rect.height)
                {
                    Content.localPosition = new Vector3(localPosition.x, enableMax, localPosition.z);
                    return;
                }
                offset = localPosition.y - eventData.scrollDelta.y * speed;
                offset = offset < enableMax ? enableMax : (offset > enableMin ? enableMin : offset);
                Content.localPosition = new Vector3(localPosition.x, offset, localPosition.z);
            }
        }
    }
}