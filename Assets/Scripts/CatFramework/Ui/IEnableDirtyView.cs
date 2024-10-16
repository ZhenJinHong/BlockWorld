using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public interface IEnableDirtyView : IModule
    {
        bool IsDirty { get; set; }
        void CleanDirty();
    }
}
