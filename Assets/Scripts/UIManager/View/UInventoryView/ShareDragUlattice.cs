using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public class ShareDragUlattice : MonoBehaviour/*, DragDrop.IDragDropTarget*/
    {
        [SerializeField] RawImage ItemImage;
        [SerializeField] RawImage CornerImage;
        [SerializeField] Text Label;
        public RectTransform GridRectTrans { get; private set; }
        UguiDragHelper uguiDragHelper;
        private void Start()
        {
            GridRectTrans = transform as RectTransform;
            uguiDragHelper = new UguiDragHelper();
            gameObject.SetActive(false);
        }
        public void Show(IUlatticeItemStorage ulatticeItem)
        {
            UGUIUtility.SetTexture(ItemImage, ulatticeItem?.ItemImage);
            UGUIUtility.SetTexture(CornerImage, ulatticeItem?.CornerImage);
            //Ulattice.SetText(Name, ulatticeItem?.Name);
            UGUIUtility.SetText(Label, ulatticeItem?.Label);
        }
        public void BeginDrag(Ulattice ulattice, PointerEventData pointerEventData)
        {
            Show(ulattice.LatticeItem);
            uguiDragHelper.BeginDrag(ulattice.transform.position, GridRectTrans);
            gameObject.SetActive(true);
        }
        public void OnDrag(PointerEventData pointerEventData)
        {
            uguiDragHelper.OnDrag(pointerEventData);
        }
        public void EndDrag()
        {
            uguiDragHelper.EndDrag();
            gameObject.SetActive(false);
        }
    }
}