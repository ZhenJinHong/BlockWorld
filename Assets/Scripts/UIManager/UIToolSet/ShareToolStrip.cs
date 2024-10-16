using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class ShareToolStrip : PageListViewTransfer<ToolStripMenuItem, IReadOnlyList<IToolStripContent>, ToolStripUMCtr>, IPointerClickHandler
    {
        [SerializeField] RectTransform view;
        Vector3[] worldPoss = new Vector3[4];
        Action<IToolStripContent> click;
        protected override void Start()
        {
            base.Start();
            viewController.OnItemHasClick += ItemHasClick;

            if (UiManagerMiao.shareToolStrip == null)
                UiManagerMiao.shareToolStrip = this;
            else
                Destroy(gameObject);
            Close();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (UiManagerMiao.shareToolStrip == this)
                UiManagerMiao.shareToolStrip = null;
        }
        public void Show(Action<IToolStripContent> click, IReadOnlyList<IToolStripContent> contents, RectTransform stripHead)
        {
            viewController.Items = contents;
            this.click = click;

            stripHead.GetWorldCorners(worldPoss);
            // 覆盖在屏幕空间的,无相机的,转换时不需要相机,世界坐标就是屏幕点
            RectTransform listRect = view.transform as RectTransform;
            listRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stripHead.rect.width);
            UGUIUtility.ClampFloatView(listRect, worldPoss[3]);

            Open();
        }
        void ItemHasClick(IToolStripContent content)
        {
            click?.Invoke(content);
            Close();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }
        public override void Open()
        {
            base.Open();
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            base.Close();
            gameObject.SetActive(false);
            Items = null;
            click = null;
        }
    }
}