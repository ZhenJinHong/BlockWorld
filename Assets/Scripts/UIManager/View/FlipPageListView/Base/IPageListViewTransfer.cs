using CatFramework.CatMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public interface IPageListViewTransfer
    {
        IPageListViewController PageListViewController { get; }
    }
}