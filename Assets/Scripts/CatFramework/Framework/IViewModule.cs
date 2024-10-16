using System.Collections.Generic;

namespace CatFramework
{
    public interface IViewModule : IModule
    {
        bool Show(object content);
        void Open();
        void Close();
    }
    public class SimilarViewModuleController
    {
        List<IViewModule> modules;
        public SimilarViewModuleController() { modules = new List<IViewModule>(); }
        public SimilarViewModuleController(List<IViewModule> modules)
        {
            this.modules = modules;
        }
        public SimilarViewModuleController(IViewModule[] modules)
        {
            this.modules = new List<IViewModule>();
            foreach (var module in modules)
            {
                AddModule(module);
            }
        }
        IViewModule current;
        IViewModule Current
        {
            set
            {
                if (current != value)
                {
                    if (current != null && current.IsUsable) current.Close();
                    current = value;
                    if (current != null && current.IsUsable) current.Open();
                }
            }
        }
        public bool Contains(IViewModule module) => modules.Contains(module);
        public void AddModule(IViewModule module)
        {
            if (module != null)
                modules.Add(module);
        }
        public bool RemoveModule(IViewModule module) => modules.Remove(module);
        public void Show(object data)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                var view = modules[i];
                if (view.IsUsable && view.Show(data))
                {
                    Current = view;
                    break;
                }
            }
        }
        public void Hide()
        {
            Current = null;
        }
    }
}