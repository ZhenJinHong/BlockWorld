using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public interface IUlatticeItemStorage : IItemStorage
    {
        Texture2D ItemImage { get; }
        Texture2D CornerImage { get; }
        string Label { get; }
        string Name { get; }
    }
    public class Ulattice : BaseLattice<IUlatticeItemStorage, Ulattice>
    {
        [SerializeField] RawImage ItemImage;
        [SerializeField] RawImage CornerImage;
        [SerializeField] Text Name;
        [SerializeField] Text Label;
        protected override void Refresh(IUlatticeItemStorage itemStorage)
        {
            UGUIUtility.SetTexture(ItemImage, itemStorage?.ItemImage);
            UGUIUtility.SetTexture(CornerImage, itemStorage?.CornerImage);
            UGUIUtility.SetText(Name, itemStorage?.Name);
            UGUIUtility.SetText(Label, itemStorage?.Label);
        }
    }
}