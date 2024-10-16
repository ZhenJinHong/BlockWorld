using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class UInventoryViewTransfer : PageLatticeViewTransfer<Ulattice, IUlatticeItemStorage, DataInteractionModuleCenter, IReadonlyItemStorageCollection<IUlatticeItemStorage>, UInventoryViewCtr>
    {
    }
}