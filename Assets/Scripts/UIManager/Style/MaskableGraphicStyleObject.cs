using CatFramework.EventsMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    [CreateAssetMenu(fileName = "New MaskableGraphicStyle", menuName = "UGUIStyle/MaskableGraphicStyle")]
    public class MaskableGraphicStyleObject : StyleObject
    {

        public MaskableGraphicStyleData StyleData => styleData;
        public MaskableGraphicStyleData StyleDataOnHover => styleDataOnHover;

        [SerializeField] MaskableGraphicStyleData styleData;
        [SerializeField] MaskableGraphicStyleData styleDataOnHover;
        public void Apply(MaskableGraphic maskableGraphic)
        {
            styleData.Apply(maskableGraphic);
        }
        public void ApplyOnHover(MaskableGraphic maskableGraphic)
        {
            styleDataOnHover.Apply(maskableGraphic);
        }
        public override IDataEventLinked AsDataEventLinked()
        {
            return new DataEventLinked<MaskableGraphicStyleObject>(this);
        }
    }
    [System.Serializable]
    public class MaskableGraphicStyleData
    {
        [SerializeField] Sprite sprite;
        [SerializeField] Image.Type imageType;
        [SerializeField] Color color;
        [SerializeField] bool raycastTarget;
        [SerializeField] bool maskable;

        public Color Color { get => color; set => color = value; }
        public bool RaycastTarget { get => raycastTarget; set => raycastTarget = value; }
        public bool Maskable { get => maskable; set => maskable = value; }

        public void Apply(MaskableGraphic maskableGraphic)
        {
            if (maskableGraphic == null) return;
            if (sprite != null && maskableGraphic is Image image)
            {
                image.sprite = sprite;
                image.type = imageType;
            }
            maskableGraphic.color = color;
            maskableGraphic.raycastTarget = raycastTarget;
            maskableGraphic.maskable = maskable;
        }
    }
}