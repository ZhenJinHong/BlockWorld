using CatFramework.CatMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public interface IPageListViewController : IEnableDirtyView
    {
        int PageNum { get; }
        int PageCount { get; }
        int VisualItemCount { get; }

        event Action<IPageListViewController> ViewIsUpdate;
    }
}