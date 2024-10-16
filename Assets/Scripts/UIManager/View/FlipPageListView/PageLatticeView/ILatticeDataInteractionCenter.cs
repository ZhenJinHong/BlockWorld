using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public interface ILatticeDataInteractionCenter<Lattice, Center, Collection>
        where Lattice : class
        where Center : class, ILatticeDataInteractionCenter<Lattice, Center, Collection>
        where Collection : class
    {
        Collection LocatedInventory { get; set; }
        Collection DragInventory { get; }
        Lattice LocatedLattice { get; }
        Lattice DragLattice { get; }

        void AddModules(IModule module);
        void RemoveModules(IModule module);
        void PointerClick(Lattice lattice, PointerEventData pointerEventData);
        void PointerBeginDrag(Lattice drag, Collection dragInventory, PointerEventData pointerEventData);
        void PointerOnDrag(Lattice lattice, PointerEventData pointerEventData);
        void PointerEndDrag(Lattice lattice, PointerEventData pointerEventData);
        void PointerEnter(Lattice target, PointerEventData pointerEventData);
        void PointerExit(Lattice lattice, PointerEventData pointerEventData);
        
    }
}