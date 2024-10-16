using CatFramework.UiMiao;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.UGUICTR
{
    public class ProductCommonDisplay : MonoBehaviour
    {
        [SerializeField] RawImage itemIcon;
        public void SetItemIcon(Texture texture)
        {
            UGUIUtility.SetTexture(itemIcon, texture);
        }
        /// <summary>
        /// 当所有模块不符合时才调用,
        /// </summary>
        public void ClearDisplay()
        {
            UGUIUtility.SetTexture(itemIcon, null);
        }
    }
}