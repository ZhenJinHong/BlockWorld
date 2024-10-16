using CatFramework;
using CatFramework.DataMiao;
using CatFramework.UiMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class ToolStripContentForInventory : IToolStripContent
    {
        public string Label { get; set; }
        public IList<ToolStripContent> Childrens { get; set; }
        public Func<IReadonlyItemStorageCollection<IUlatticeItemStorage>> GetInventory;
        public InventorysListTableModule InventorysListTableModule;
        public void Click()
        {
            if (InventorysListTableModule != null)
            {
                IReadonlyItemStorageCollection<IUlatticeItemStorage> inventory = GetInventory?.Invoke();
                InventorysListTableModule.OpenInventory(inventory);
            }
        }
    }
    // 以视图模块方式的原因 -> 到时左边视图不一定只显示列表
    public class InventorysListTableModule : ViewModule
    {
        [SerializeField] ToolStripMiao classifys;
        [SerializeField] UInventoryViewTransfer uInventoryViewUMCtr;
        public void OpenInventory(IReadonlyItemStorageCollection<IUlatticeItemStorage> callBackInventory)
        {
            uInventoryViewUMCtr.Items = callBackInventory;
            uInventoryViewUMCtr.SetPageNum(1);
        }

        public override bool Show(object content)
        {
            if (content == classifys.Contents) return true;
            if (content is IReadOnlyList<IToolStripContent> contents)
            {
                // 之前#if放在这里,是为啥?
                foreach (var item in contents)
                {
                    if (item is ToolStripContentForInventory forInventory)
                    {
                        forInventory.InventorysListTableModule = this;
                    }
#if UNITY_EDITOR
                    else
                    {
                        if (ConsoleCat.Enable) ConsoleCat.LogWarning("错误的分类菜单内容");
                        continue;
                    }
#endif
                }

                classifys.SetContent(contents);
                return true;
            }
            return false;
        }
        public override void Open()
        {
            base.Open();
            uInventoryViewUMCtr.Open();
        }
        public override void Close()
        {
            base.Close();
            uInventoryViewUMCtr.Close();
        }
    }
}