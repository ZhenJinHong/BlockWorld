using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CatFramework.Tools
{
    [DisallowMultipleComponent]
    public class PanelSizeController : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] float edgeWidth = 15;
        public float EdgeWidth { set => edgeWidth = value; }
        [SerializeField] float minPanelSize = 128;
        public float MinPanelSize { set => minPanelSize = value; }
        RectTransform parentRectTrans;
        RectTransform panelTransform;
        //鼠标位置
        Vector2 oldPoint;
        //原X边距宽（相对于父级）
        float originalXedgewidth;
        //原Y边距宽（相对于父级）
        float originalYedgewidth;
        //主画布宽高
        Vector2 mainCanvasSize;

        bool isMoveDrag;
        //是否XY同时拖拽
        bool isXAndYEdge;
        bool isRight;
        bool isUp;
        bool isOnlyXedge;
        bool isOnlyYedge;
        float Xmax;
        float Ymax;
        float Xmin;
        float Ymin;
        //相反边的枚举，因为相反的边要被固定的，而实际要改变的是高宽
        RectTransform.Edge Xedge;
        RectTransform.Edge Yedge;
        bool isActive;
        private void Awake()
        {
            panelTransform = GetComponent<RectTransform>();
            parentRectTrans = panelTransform.parent.GetComponent<RectTransform>();
        }
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (parentRectTrans != null)
            {
                //获取在点在父级中的坐标
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, eventData.position, eventData.pressEventCamera, out oldPoint);
                isActive = true;
            }
            else
                isActive = false;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            isMoveDrag = false;
            isXAndYEdge = false;
            isRight = false;
            isUp = false;
            isOnlyXedge = false;
            isOnlyYedge = false;
            if (panelTransform == null || parentRectTrans == null)
            {
                isActive = false;
                return;
            }

            //面板父级在屏幕内允许的最大点减最小点获取得屏幕矩形大小，用以限制宽度最大为屏幕大小
            mainCanvasSize = parentRectTrans.rect.max - parentRectTrans.rect.min;
            //相对于父级的坐标
            Vector2 panelLocalPos = panelTransform.localPosition;
            //父级矩形大小点
            Rect rect = parentRectTrans.rect;
            Vector2 parentMax = rect.max;
            Vector2 parentMin = rect.min;
            //获取面板的矩形
            rect = panelTransform.rect;
            //矩形大小点，坐标系转到父级中
            Xmax = rect.xMax + panelLocalPos.x;
            Xmin = rect.xMin + panelLocalPos.x;
            Ymax = rect.yMax + panelLocalPos.y;
            Ymin = rect.yMin + panelLocalPos.y;

            //点在了右边缘
            if (Math.Abs(Xmax - oldPoint.x) < edgeWidth)
            {
                isRight = true;
                Xedge = RectTransform.Edge.Left;
                //计算左边缘相对于父级画布左边缘的距离，括号内计算的是父级大小点相对于面板轴心的距离，因为面板的大小点也是相对于面板轴心
                originalXedgewidth = Xmin - parentMin.x;
                //也点在了顶部边缘
                if (Math.Abs(Ymax - oldPoint.y) < edgeWidth)
                {
                    isXAndYEdge = true;
                    isUp = true;
                    Yedge = RectTransform.Edge.Bottom;
                    originalYedgewidth = Ymin - parentMin.y;
                }
                //点在了底部边缘
                else if (Math.Abs(oldPoint.y - Ymin) < edgeWidth)
                {
                    isXAndYEdge = true;
                    Yedge = RectTransform.Edge.Top;
                    originalYedgewidth = parentMax.y - Ymax;
                }
                //仅点击了右边
                else
                {
                    isOnlyXedge = true;
                }
            }
            //点在了左边缘
            else if (Math.Abs(oldPoint.x - Xmin) < edgeWidth)
            {
                Xedge = RectTransform.Edge.Right;

                originalXedgewidth = parentMax.x - Xmax;
                //也点在了顶部边缘
                if (Math.Abs(Ymax - oldPoint.y) < edgeWidth)
                {
                    isXAndYEdge = true;
                    isUp = true;
                    Yedge = RectTransform.Edge.Bottom;
                    originalYedgewidth = Ymin - parentMin.y;
                }
                //点在了底部边缘
                else if (Math.Abs(oldPoint.y - Ymin) < edgeWidth)
                {
                    isXAndYEdge = true;
                    Yedge = RectTransform.Edge.Top;
                    originalYedgewidth = parentMax.y - Ymax;
                }
                //仅点击了左边
                else
                {
                    isOnlyXedge = true;
                }
            }
            //点在了顶部边缘
            else if (Math.Abs(Ymax - oldPoint.y) < edgeWidth)
            {
                isOnlyYedge = true;
                isUp = true;
                Yedge = RectTransform.Edge.Bottom;
                originalYedgewidth = Ymin - parentMin.y;
            }
            //点在了底部边缘
            else if (Math.Abs(oldPoint.y - Ymin) < edgeWidth)
            {
                isOnlyYedge = true;
                Yedge = RectTransform.Edge.Top;
                originalYedgewidth = parentMax.y - Ymax;
            }
            else
            {
                isMoveDrag = true;
                //父级里允许的最大小点，再考虑上点与边的距离：计算点与当前拖动的矩形的边距
                Xmax = parentRectTrans.rect.xMax - (Xmax - oldPoint.x);
                Ymax = parentRectTrans.rect.yMax - (Ymax - oldPoint.y);
                Xmin = parentRectTrans.rect.xMin + (oldPoint.x - Xmin);
                Ymin = parentRectTrans.rect.yMin + (oldPoint.y - Ymin);
                //偏移
                oldPoint -= panelLocalPos;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!isActive) return;
            float xNewWidth;
            float yNewWidth;
            //鼠标新的位置
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, eventData.position, eventData.pressEventCamera, out Vector2 newPointerPosition);
            if (isMoveDrag)
            {
                newPointerPosition.Set(
                    Mathf.Clamp(newPointerPosition.x, Xmin, Xmax),
                    Mathf.Clamp(newPointerPosition.y, Ymin, Ymax)
                    );
                panelTransform.localPosition = newPointerPosition - oldPoint;
            }
            else if (isXAndYEdge)
            {
                if (isRight)
                {
                    xNewWidth = newPointerPosition.x - Xmin;
                    if (isUp)
                    {
                        yNewWidth = newPointerPosition.y - Ymin;
                    }
                    else
                    {
                        yNewWidth = Ymax - newPointerPosition.y;
                    }
                }
                else
                {
                    xNewWidth = Xmax - newPointerPosition.x;
                    if (isUp)
                    {
                        yNewWidth = newPointerPosition.y - Ymin;
                    }
                    else
                    {
                        yNewWidth = Ymax - newPointerPosition.y;
                    }
                }
                panelTransform.SetInsetAndSizeFromParentEdge(Xedge, originalXedgewidth, Mathf.Clamp(xNewWidth, minPanelSize, mainCanvasSize.x));
                panelTransform.SetInsetAndSizeFromParentEdge(Yedge, originalYedgewidth, Mathf.Clamp(yNewWidth, minPanelSize, mainCanvasSize.y));
            }
            else if (isOnlyXedge)
            {
                if (isRight)
                {
                    xNewWidth = newPointerPosition.x - Xmin;
                }
                else
                {
                    xNewWidth = Xmax - newPointerPosition.x;
                }
                panelTransform.SetInsetAndSizeFromParentEdge(Xedge, originalXedgewidth, Mathf.Clamp(xNewWidth, minPanelSize, mainCanvasSize.x));
            }
            else if (isOnlyYedge)
            {
                if (isUp)
                {
                    yNewWidth = newPointerPosition.y - Ymin;
                }
                else
                {
                    yNewWidth = Ymax - newPointerPosition.y;
                }
                panelTransform.SetInsetAndSizeFromParentEdge(Yedge, originalYedgewidth, Mathf.Clamp(yNewWidth, minPanelSize, mainCanvasSize.y));
            }
        }
    }
}