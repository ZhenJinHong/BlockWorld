using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class PageLatticeViewTransfer<Lattice, ItemStorage, Center, Collection, ViewController> : PageListViewTransfer<Lattice, Collection, ViewController>, IPointerEnterHandler, IPointerExitHandler
        where Lattice : Component, ILattice<ItemStorage, Lattice>
        where ItemStorage : class, IItemStorage
        where Center : class, ILatticeDataInteractionCenter<Lattice, Center, Collection>
        where Collection : class, IReadonlyItemStorageCollection<ItemStorage>
        where ViewController : PageLatticeViewController<Lattice, ItemStorage, Center, Collection>, new()
    {
        [SerializeField] Center dataInteractionModuleCenter;
        //[SerializeField] Lattice previewLattice;
        protected override void PopulateOtherViewArgs(ViewController listView)
        {
            base.PopulateOtherViewArgs(listView);
            listView.dataInteractionModuleCenter = dataInteractionModuleCenter;
            //listView.PreviewLattice = previewLattice;
        }
        #region 视图回调

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
            => viewController.OnPointerEnter();
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            => viewController.OnPointerExit();
        #endregion
        #region
        //public int CurrentSelectedLattice
        //{
        //    get => viewController.CurrentSelectedLattice;
        //    set => viewController.CurrentSelectedLattice = value;
        //}
        public void SwitchLattice(int v)
            => viewController.SwitchLattice(v);
        public void SelectedLattice(int latticeIndex)
            => viewController.SelectedLattice(latticeIndex);
        #endregion
    }
}