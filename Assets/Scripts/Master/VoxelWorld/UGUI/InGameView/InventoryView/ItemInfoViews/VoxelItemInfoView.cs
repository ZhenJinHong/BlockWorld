using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class VoxelItemInfoView : ViewModule
    {
        [SerializeField] RawImage itemImage;
        [SerializeField] TextMiao Name;
        public override bool Show(object content)
        {
            if (content is VoxelItemStorage voxelItemStorage && voxelItemStorage.VoxelItemInfo != null)
            {
                itemImage.texture = voxelItemStorage.ItemImage;
                Name.TextValue = voxelItemStorage.VoxelItemInfo.VoxelName;
                Open();
                return true;
            }
            else
            {
                Close();
                return false;
            }
        }
    }
}