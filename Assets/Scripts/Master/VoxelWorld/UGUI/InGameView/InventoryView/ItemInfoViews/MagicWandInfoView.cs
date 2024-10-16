using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class MagicWandInfoView : ViewModule
    {
        [SerializeField] RawImage itemImage;
        [SerializeField] TextMiao Name;
        public override bool Show(object content)
        {
            if (content is MagicWandStorage magicWandStorage)
            {
                itemImage.texture = magicWandStorage.ItemImage;
                Name.TextValue = magicWandStorage.Name;
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