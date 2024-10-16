using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public interface ILatticeDataInteractionModule<LatticeDataInteractionCenter> : IModule
        where LatticeDataInteractionCenter : class
    {
    }
    public interface IPointerClickHanderModule<LatticeDataInteractionCenter> : ILatticeDataInteractionModule<LatticeDataInteractionCenter>
          where LatticeDataInteractionCenter : class
    {
        bool PointerClick(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
    }
    public interface IPointerEnterHanderModule<LatticeDataInteractionCenter> : ILatticeDataInteractionModule<LatticeDataInteractionCenter>
          where LatticeDataInteractionCenter : class
    {
        bool PointerEnter(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
        void PointerExit(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
    }
    public interface IPointerDragHanderModule<LatticeDataInteractionCenter> : ILatticeDataInteractionModule<LatticeDataInteractionCenter>
          where LatticeDataInteractionCenter : class
    {
        bool PointerBeginDrag(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
        void PointerEndDrag(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
    }
    public interface IPointerOnDragHanderModule<LatticeDataInteractionCenter> : ILatticeDataInteractionModule<LatticeDataInteractionCenter>
          where LatticeDataInteractionCenter : class
    {
        void PointerOnDrag(LatticeDataInteractionCenter latticeDataInteractionCenter, PointerEventData pointerEventData);
    }
}
