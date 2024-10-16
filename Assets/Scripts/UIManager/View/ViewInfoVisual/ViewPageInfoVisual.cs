using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ViewPageInfoVisual : MonoBehaviour
    {
        [SerializeField] TextMiao pageNum;
        [SerializeField] GameObject targetView;
        void Start()
        {
            if (targetView && targetView.TryGetComponent<IPageListViewTransfer>(out var flipPageListViewTransfer))
            {
                flipPageListViewTransfer.PageListViewController.ViewIsUpdate += ViewIsUpdate;
            }
            else
            {
                ConsoleCat.LogWarning("未获取到视图");
            }
        }
        private void OnDestroy()
        {
            if (targetView && targetView.TryGetComponent<IPageListViewTransfer>(out var flipPageListViewTransfer))
            {
                flipPageListViewTransfer.PageListViewController.ViewIsUpdate -= ViewIsUpdate;//如果目标视图销毁了,自然就不用退订了
            }
        }
        void ViewIsUpdate(IPageListViewController flipPageListView)
        {
            pageNum.TextValue = $"{flipPageListView.PageNum} / {flipPageListView.PageCount}; {flipPageListView.VisualItemCount}";
        }
    }
}