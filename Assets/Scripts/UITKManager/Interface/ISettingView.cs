using System;
using System.Collections.Generic;

namespace CatFramework.UiTK
{
    public interface ISettingView
    {
        // 应用并保存，尽量逻辑相同
        void Apply();
        /// <summary>
        /// 重置的时候不要应用,让用户选中是否应用
        /// </summary>
        void Reset();
    }
}
