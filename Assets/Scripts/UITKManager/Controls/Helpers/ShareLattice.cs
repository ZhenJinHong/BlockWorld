using UnityEngine;
using UnityEngine.UIElements;
namespace CatFramework.UiTK
{
    public interface IShareLattice
    {
        Vector2 ItemStartPosition { get; set; }
        Vector3 PointerStartPosition { get; set; }

        void Set(IUlatticeItem ulatticeItem);
        void SetPointerPosition(Vector3 pos);
    }
    public class ShareLattice : VisualElement, IShareLattice
    {
        public Image ItemImage { get; set; }
        public Image CornerImage { get; set; }
        public Label Label { get; set; }
        public Vector2 ItemStartPosition { get; set; }
        public Vector3 PointerStartPosition { get; set; }
    
        public ShareLattice()
        {
            style.position = Position.Absolute;
            usageHints = UsageHints.DynamicTransform;
            pickingMode = PickingMode.Ignore;
            visible = false;
        }
        public void Set(IUlatticeItem ulatticeItem)
        {
            if (ulatticeItem == null || ulatticeItem.ItemImage == null)
            {
                visible = false;
            }
            else
            {
                visible = true;
                if (ItemImage != null)
                    ItemImage.image = ulatticeItem?.ItemImage;
                if (CornerImage != null)
                    CornerImage.image = ulatticeItem?.CornerImage;
                if (Label != null)
                    Label.text = ulatticeItem?.Label ?? string.Empty;
            }
        }
        public void SetPointerPosition(Vector3 pos)
        {
            Vector3 pointerDelta = pos - PointerStartPosition;
            transform.position = new Vector2()
            {
                x = Mathf.Clamp(ItemStartPosition.x + pointerDelta.x, 0f, panel.visualTree.worldBound.width),
                y = Mathf.Clamp(ItemStartPosition.y + pointerDelta.y, 0f, panel.visualTree.worldBound.height),
            };
        }
    }
}
