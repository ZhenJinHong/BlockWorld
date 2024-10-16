using CatFramework.UiMiao;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class VoxelDisplayMoudle : ProductDisplayModule<VoxelItemStorage>
    {
        protected override void InternalSet(DataInteractionModuleCenter dataInteractionModuleCenter, VoxelItemStorage voxelItemStorage)
        {
            if (ProductCommonDisplay != null)
            {
                ProductCommonDisplay.SetItemIcon(voxelItemStorage.ItemImage);
            }
        }
    }
}