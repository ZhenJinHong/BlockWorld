using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.UiMiao
{
    public interface IToolStripContent
    {
        string Label { get; }
        IList<ToolStripContent> Childrens { get; set; }

        void Click();
    }
    public class ToolStripContent : IToolStripContent
    {
        public string Label { get; set; }
        public Action<ToolStripContent> action;
        public IList<ToolStripContent> Childrens { get; set; }
        public void Click()
        {
            action?.Invoke(this);
        }
    }
}
