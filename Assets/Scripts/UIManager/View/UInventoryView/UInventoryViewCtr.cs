using CatFramework.CatMath;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class UInventoryViewCtr : PageLatticeViewController<Ulattice, IUlatticeItemStorage, DataInteractionModuleCenter, IReadonlyItemStorageCollection<IUlatticeItemStorage>>, ICollectionView
    {
    }
}