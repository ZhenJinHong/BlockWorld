using CatFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace CatFramework.UiMiao
{
    public class UguiDragHelper
    {
        //新点击的本地坐标
        //Vector3 newPosition;
        //旧的点击的本地坐标
        //Vector3 offset;
        //原本的坐标
        Vector3 originalPosition;
        RectTransform gridRectTrans;
        bool DragIsActive;
        public void BeginDrag(Vector3 originalWorldPosition, RectTransform rect)
        {
            originalPosition = originalWorldPosition;
            gridRectTrans = rect;
            if (gridRectTrans)
            {
                DragIsActive = true;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (DragIsActive)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(gridRectTrans, eventData.position, eventData.pressEventCamera, out var newPosition);
                gridRectTrans.position = newPosition ;
            }
        }
        public void EndDrag()
        {
            Recover();
        }
        void Recover()
        {
            if (DragIsActive)
            {
                gridRectTrans.position = originalPosition;
                DragIsActive = false;
            }
        }
    }
    [Obsolete]
    public sealed class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Obsolete]
        public interface IDragDropTarget
        {
            Vector3 OriginalWorldPosition { get; }
            RectTransform GridRectTrans { get; }
            bool LineNon_Empty { get; }
            void BeginDrag(PointerEventData pointerEventData);
            void EndDrag();
            void Maskable(bool v);
        }
        //新点击的本地坐标
        Vector3 newPosition;
        //旧的点击的本地坐标
        //Vector3 offset;
        //原本的坐标
        Vector3 originalPosition;
        RectTransform gridRectTrans;
        bool DragIsActive;
        IDragDropTarget dragDropTarget;
        private void Start()
        {
            if (!TryGetComponent<IDragDropTarget>(out dragDropTarget))
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("拖拽组件未找到拖拽目标");
                Destroy(this);
            }
        }
        void GetTargetData()
        {
            originalPosition = dragDropTarget.OriginalWorldPosition;
            gridRectTrans = dragDropTarget.GridRectTrans;
        }
        void ReleaseTargetData()
        {
            gridRectTrans = null;
        }
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (dragDropTarget != null && dragDropTarget.LineNon_Empty)
            {
                GetTargetData();
                if (gridRectTrans)
                {
                    DragIsActive = true;
                    //RectTransformUtility.ScreenPointToWorldPointInRectangle(gridRectTrans, eventData.position, eventData.pressEventCamera, out offset);
                    //offset -= originalPosition;
                    dragDropTarget.Maskable(false);
                    dragDropTarget.BeginDrag(eventData);
                }
            }
        }
        //这个是每帧都响应的
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (DragIsActive)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(gridRectTrans, eventData.position, eventData.pressEventCamera, out newPosition);
                gridRectTrans.position = newPosition /*- offset*/;
            }
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            Recover();
        }
        void Recover()
        {
            if (DragIsActive)
            {
                gridRectTrans.position = originalPosition;
                DragIsActive = false;
                dragDropTarget.Maskable(true);
                dragDropTarget.EndDrag();
                ReleaseTargetData();
            }
        }
        private void OnDisable()
        {
            Recover();
        }
    }
}