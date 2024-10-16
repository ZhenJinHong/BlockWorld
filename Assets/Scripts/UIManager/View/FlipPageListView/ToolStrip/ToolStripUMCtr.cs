using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public interface IToolStripHeader
    {
        void ItemHasClick(IToolStripContent content);
    }
    public class ToolStripUMCtr : PageListViewUMCtr<ToolStripMenuItem, IReadOnlyList<IToolStripContent>>, IToolStripHeader
    {

        public event Action<IToolStripContent> OnItemHasClick;
        public override int ItemCount => Items == null ? 0 : Items.Count;
        public ToolStripUMCtr() { }
        protected override void BindItem(int itemIndex, ToolStripMenuItem visualItem)
        {
            visualItem.SetContent(Items[itemIndex], this);
        }
        protected override void UnBindItem(int itemIndex, ToolStripMenuItem visualItem)
        {
            visualItem.Clear();
        }

        public void ItemHasClick(IToolStripContent content)
        {
            var contents = content.Childrens;
            if (contents != null && contents.Count != 0)
            {
                // TODO 多级链
            }
            else
            {
                OnItemHasClick?.Invoke(content);
            }
        }
    }
}