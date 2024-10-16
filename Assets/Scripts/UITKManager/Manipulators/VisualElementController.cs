using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class VisualElementController
    {
        public static VisualElement CreateContainer()
        {
            VisualElement visualElement = new VisualElement()
            {
                pickingMode = PickingMode.Ignore,
            };
            return visualElement;
        }
        public static Label CreateTextLine(string text = null, object useData = null)
        {
            Label lbl = new Label().Default(text, useData);
            lbl.AddToClassList(virtualtextfieldClass);
            return lbl;
        }
        #region
        public static Button CreateButton(Action clickAction, string text = null, object useData = null)
        {
            Button btn = new Button(clickAction).Default(text, useData);
            btn.AddToClassList(btnClass);
            btn.AddToClassList(colorBgSqueakyClass);
            return btn;
        }
        public static Button CreateTopTagBtn(Action clickAction, string text = null, object useData = null)
        {
            Button btn = new Button(clickAction).Default(text, useData);
            btn.AddToClassList(btnTagTopClass);
            btn.AddToClassList(colorBgSqueakyOpacityClass);
            return btn;
        }
        public static Button CreateIconBtn(Action clickAction)
        {
            Button btn = new Button(clickAction).Default();
            btn.AddToClassList(bthIconClass);
            return btn;
        }
        public static Button CreateBottomTagBtn(Action clickAction, string text = null, object useData = null)
        {
            Button btn = new Button(clickAction).Default(text, useData);
            btn.AddToClassList(btnTagBottomClass);
            btn.AddToClassList(colorImageSqueaky);
            return btn;
        }
        public static Button CreateMainMenuButton(Action clickAction, string text, object useData = null)
        {
            Button btn = new Button(clickAction).Default(text, useData);
            btn.AddToClassList(btnMainMenuClass);
            btn.AddToClassList(colorImageSqueakyGradientClass);
            return btn;
        }
        #endregion
        // 关键信息，修改脚本的时候注意保留
        // 视觉元素的子级指的直系子级
        #region
        public static readonly string hideClass = "hide";
        public static readonly string noDisplayClass = "noneDisplay";
        #endregion
        #region 基础布局 对子级生效的布局一般是只针对次一级即子级，而对子级的子级不生效
        public static readonly string viewFullClass = "view-full";
        public static readonly string viewFullMargin50px = "view-full-margin50px";
        public static readonly string viewTagContentBottom = "view-tab-content-bottom";
        public static readonly string viewAbsFullClass = "view-absfull";
        public static readonly string viewAbsClass = "view-abs";
        //public static readonly string viewContainerClass = "view-container";
        /// <summary>
        /// 左右平均
        /// </summary>
        public static readonly string viewChildAroundClass = "view-child-around";
        public static readonly string viewChildCenterCenterClass = "view-child-center-center";
        public static readonly string viewChildLeftClass = "view-child-left";
        public static readonly string viewRowClass = "view-row";
        // 排布并不会传递，而默认的排布就是column的，并不需要另外加，来阻止row
        [Obsolete]
        public static readonly string columnClass = "column";
        #endregion
        #region 按钮控件
        public static readonly string btnMainMenuClass = "btn-mainMenu";
        public static readonly string btnClass = "btn";
        public static readonly string bthIconClass = "btn-icon";
        public static readonly string btnTagTopClass = "btn-tag-top";
        public static readonly string btnTagBottomClass = "btn-tag-bottom";
        #endregion
        #region 颜色
        public static readonly string colorBgSqueakyOpacityClass = "color-bg-squeaky-opacity";
        public static readonly string colorBgSqueakyClass = "color-bg-squeaky";
        public static readonly string colorBgGreyGradientClass = "color-bg-grey-gradient";
        public static readonly string colorBgWhiteOpacityClass = "color-bg-white-opacity";
        public static readonly string colorImageSqueakyGradientClass = "color-image-squeaky-gradient";
        public static readonly string colorImageSqueaky = "color-image-squeaky";
        #endregion
        #region 字段控件
        public static readonly string titleClass = "title";
        public static readonly string h3Class = "h3";
        public static readonly string h4Class = "h4";
        public static readonly string virtualtextfieldClass = "virtual-text-field";
        public static readonly string previewTextClass = "preview-text";
        #endregion
        #region 图形控件
        public static readonly string previewImageClass = "preview-image";
        public static readonly string itemLatticeClass = "item-lattice";
        public static readonly string itemLatticeCornerClass = "item-lattice-corner";
        public static readonly string itemLatticeLblClass = "item-lattice-lbl";
        public static readonly string itemLatticeSelectedClass = "item-lattice-selected";
        public static readonly string itemLatticeShareClass = "item-lattice-share";
        public static readonly string itemLineClass = "item-line";
        public static readonly string itemLineImageClass = "item-line-image";
        public static readonly string itemLineLblClass = "item-line-lbl";
        #endregion
        public VisualElement Target { get; protected set; }
        public bool IsVisual => !Target.ClassListContains(noDisplayClass);
        public VisualElementController(VisualElement target, PickingMode pickingMode = PickingMode.Ignore)
        {
            Target = target;
            target.pickingMode = pickingMode;
        }
        public virtual void Open() => Target.RemoveFromClassList(noDisplayClass);
        public virtual void Close() => Target.AddToClassList(noDisplayClass);
        public virtual void Show() => Target.RemoveFromClassList(noDisplayClass);
        public virtual void Hide() => Target.AddToClassList(noDisplayClass);

        public void OpenWithoutNotify() => Target.RemoveFromClassList(noDisplayClass);
        public void CloseWithoutNotify() => Target.AddToClassList(noDisplayClass);
        public void ShowWithoutNotify() => Target.RemoveFromClassList(noDisplayClass);
        public void HideWithoutNotify() => Target.AddToClassList(noDisplayClass);
        public bool ClassListContains(string cls)
            => Target.ClassListContains(cls);
        public void Add(VisualElement child)
            => Target.Add(child);
        public void Add(VisualElementController visualElementController)
            => Target.Add(visualElementController.Target);
        public void Remove(VisualElement child)
            => Target.Remove(child);
        public void Remove(VisualElementController visualElementController)
            => Target.Remove(visualElementController.Target);
        public void AddToClassList(string cls)
            => Target.AddToClassList(cls);
        public void RemoveFromClassList(string cls)
            => Target.RemoveFromClassList(cls);


        public void OpenOrClose(bool open)
        {
            if (open) Open();
            else Close();
        }
        public void OpenOrClose()
        {
            if (Target.ClassListContains(noDisplayClass)) Open();
            else Close();
        }
        public void ShowOrHide(bool show)
        {
            if (show) Show();
            else Hide();
        }
        public void ShowOrHide()
        {
            if (Target.ClassListContains(noDisplayClass)) Show();
            else Hide();
        }
        public void ShowOrHideWithoutNotify(bool show)
        {
            if (show) ShowWithoutNotify();
            else HideWithoutNotify();
        }
    }
}
