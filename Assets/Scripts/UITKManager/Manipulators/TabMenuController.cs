using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    /// <summary>
    /// 视窗的开关是按钮直接控制的
    /// </summary>
    [Obsolete]
    public class TabMenuController
    {
        VisualElement TabContent { get; }
        public TabMenuController(VisualElement tabContent)
        {
            TabContent = tabContent;
        }
        /// <summary>
        /// 传递的是现在新打开的视窗
        /// </summary>
        public Action<IViewControl> OnTabContentOpen;
        /// <summary>
        /// 传递的是现在刚关闭的视窗
        /// </summary>
        public Action<IViewControl> OnTabContentClose;
        IViewControl currentView;
        public IViewControl CurrentView => currentView;
        /// <summary>
        /// 内容初始为不显示
        /// </summary>
        public void AddTabContent(IViewControl tagContent)
        {
            if (tagContent.Parent == TabContent) return;
            TabContent.Add(tagContent.Self);
            tagContent.CloseWithoutNotify();// 标签内容默认情况下关闭
            tagContent.OnOpen += WhenTabContentOpen;
            tagContent.OnClose += WhenTabContentClose;
        }
        public void RemoveTabContent(IViewControl tagContent)
        {
            TabContent.Remove(tagContent.Self);
            tagContent.OnOpen -= WhenTabContentOpen;
            tagContent.OnClose -= WhenTabContentClose;
        }
        // 操纵器的唯一作用，就是当新的标签打开了，但是上个标签内容没有关闭的时候，自动关闭上个标签的内容
        // 当新的视图打开的时候
        protected virtual void WhenTabContentOpen(IViewControl visualElementView)
        {
            if (currentView != null && currentView != visualElementView) // 上个视图非空并且不是当前已打开的视图？关闭上个视图
            {
                currentView.Close();
            }
            if (currentView != null && ConsoleCat.Enable)
            {
                ConsoleCat.DebugInfo("上个标签未正常关闭，逻辑有误？");
            }
            currentView = visualElementView; // 赋值到当前视图，注意不要再重复Open
            OnTabContentOpen?.Invoke(visualElementView);
        }
        // 当视图关闭的时候
        protected virtual void WhenTabContentClose(IViewControl visualElementView)
        {
            if (currentView == visualElementView) // 是上个视图？清空上个视图
            {
                currentView = null;
            }
            OnTabContentClose?.Invoke(visualElementView);
        }
        public void ShowOrHide(bool show)
        {
            currentView?.ShowOrHideWithoutNotify(show);
        }
    }
}
