using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public interface IColorReceiver
    {
        bool IsDestroy { get; }
        Color OriginalColor { get; }
        bool UseAlpha { get; }
        bool UseHDR { get; }
        bool RealTimeUpdata { get; }
        void CallBack(Color color);
    }
}
