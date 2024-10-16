using CatFramework;
using CatFramework.InputMiao;
using CatFramework.Localized;
using CatFramework.UiMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class BindingKeyView : PageListViewTransfer<ButtonField, IReadOnlyList<IButtonFieldListViewItem>, ButtonFieldFPLVUMCtr>
    {
        public class InputActionDataMiao : IButtonFieldListViewItem
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public BindingKeyView BindingKeyView;
            public void Click(UiClick uiClick)
            {
                if (BindingKeyView != null)
                    BindingKeyView.ItemClick(this);
            }
        }
        List<IButtonFieldListViewItem> items = new List<IButtonFieldListViewItem>();
        InputActionMap currentMap;
        public InputActionMap CurrentMap
        {
            get => currentMap;
            set
            {
                if (currentMap != value)
                {
                    currentMap = value;
                    ReloadData();
                }
            }
        }
        protected override void PopulateOtherViewArgs(ButtonFieldFPLVUMCtr listView)
        {
            base.PopulateOtherViewArgs(listView);
            listView.Items = items;
        }
        public void ItemClick(InputActionDataMiao inputActionDataMiao)
        {
            string actionName = inputActionDataMiao.Name;
            InputManagerMiao.BindingAction(CurrentMap, actionName, CompleteBinding, CancelBinding);
            if (UiManagerMiao.RayBlocker != null)
            {
                UiManagerMiao.RayBlocker.Call(RayBlockerCancel);
                UiManagerMiao.RayBlocker.TipNoTranslate = $"正在重绑定按键：{LocalizedManagerMiao.LanguageCollection.Translate(actionName)}";
            }
        }
        void CompleteBinding(string bindingKey)
        {
            ReloadData();
            ViewController.SetPageNum(ViewController.PageNum);
            if (UiManagerMiao.RayBlocker != null)
            {
                UiManagerMiao.RayBlocker.Call(null, 2999);
            }
        }
        void CancelBinding(string bindingKey)
        {
            if (UiManagerMiao.RayBlocker != null)
            {
                UiManagerMiao.RayBlocker.Call(null, 2999);
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManagerMiao.CancelRebindingOperation();
        }
        public void RayBlockerCancel()
        {
            InputManagerMiao.CancelRebindingOperation();// 点击里空白处时认定为取消绑定
        }
        public void SaveBind()
        {
            CurrentMap.asset.SaveBinding();
        }
        public void ResetBind()
        {
            CurrentMap.asset.ResetBinding();
            ReloadData();
            ViewController.SetPageNum(1);
        }
        void ReloadData()
        {
            items.Clear();
            CurrentMap.ForeachActions((name, binding) => items.Add(new InputActionDataMiao()
            {
                Name = name,
                Value = binding,
                BindingKeyView = this,
            }));
        }
    }
}