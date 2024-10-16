using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class MutexViewController : VisualElementController
    {
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
        readonly List<IViewControl> lists;
        public MutexViewController(VisualElement target) : base(target)
        {
            lists = new List<IViewControl>();
        }
        /// <summary>
        /// 如果视图是可见的,登记后会顶掉原本打开的
        /// </summary>
        public void RegisterTabContent(IViewControl tagContent)
        {
            if (lists.Contains(tagContent)) return;
            if (tagContent.IsVisual)
            {
                currentView?.Close();
                currentView = tagContent;
            }
            tagContent.OnOpen += WhenTabContentOpen;
            tagContent.OnClose += WhenTabContentClose;
        }
        // 操纵器的唯一作用，就是当新的标签打开了，但是上个标签内容没有关闭的时候，自动关闭上个标签的内容
        // 当新的视图打开的时候
        void WhenTabContentOpen(IViewControl visualElementView)
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
        void WhenTabContentClose(IViewControl visualElementView)
        {
            if (currentView == visualElementView) // 是上个视图？清空上个视图
            {
                currentView = null;
            }
            else
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.DebugInfo("关闭的窗口与当前窗口不匹配");
            }
            OnTabContentClose?.Invoke(visualElementView);
        }
        public override void Show()
        {
            base.Show();
            currentView?.ShowWithoutNotify();
        }
        public override void Hide()
        {
            base.Hide();
            currentView?.HideWithoutNotify();
        }
    }
}
