using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class ShareDropdownList : PageListViewTransfer<ButtonMiao, IReadOnlyList<string>, StringFPLVUMCtr>, IPointerClickHandler
    {
        [SerializeField] RectTransform view;
        Action<int> click;
        Vector3[] worldPoss = new Vector3[4];
        protected override void Start()
        {
            base.Start();
            viewController.OnSelectedItem += Click;
            if (UiManagerMiao.shareDropdownList == null)
                UiManagerMiao.shareDropdownList = this;
            Close();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (UiManagerMiao.shareDropdownList == this)
                UiManagerMiao.shareDropdownList = null;
        }
        public void Show(Action<int> click, IReadOnlyList<string> items, RectTransform stripHead)
        {
            this.click = click;
            Items = items;

            stripHead.GetWorldCorners(worldPoss);
            // 覆盖在屏幕空间的,无相机的,转换时不需要相机,世界坐标就是屏幕点
            RectTransform listRect = view.transform as RectTransform;
            listRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stripHead.rect.width);
            UGUIUtility.ClampFloatView(listRect, worldPoss[3]);

            Open();
        }
        void Click(int itemIndex)
        {
            click?.Invoke(itemIndex);
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
            Items = null;
            click = null;
            gameObject.SetActive(false);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }
    }
}