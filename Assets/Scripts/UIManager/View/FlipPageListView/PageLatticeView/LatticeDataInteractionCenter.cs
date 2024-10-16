using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public abstract class LatticeDataInteractionCenter<Lattice, Center, Collection> : MonoBehaviour, ILatticeDataInteractionCenter<Lattice, Center, Collection>
        where Lattice : class
        where Center : LatticeDataInteractionCenter<Lattice, Center, Collection>
        where Collection : class
    {
        public int maxModuleCount = 24;
        Center center;
        List<IModule> modules = new List<IModule>();
        IEnableIterateModule enableIterateModule;
        IEnableIterateModule EnableIterateModule
        {
            set
            {
                enabled = value != null;
                if (enableIterateModule != null && enableIterateModule.IsUsable)
                {
                    enableIterateModule.EndUpdate();
                }
                enableIterateModule = value;
            }
        }
        protected virtual void Awake()
        {
            center = (Center)this;// 强制转换失败会直接报错
        }
        protected virtual void Update()
        {
            if (enableIterateModule != null)
            {
                if (!enableIterateModule.IsUsable || !enableIterateModule.Update())
                {
                    EnableIterateModule = null;
                }
            }
        }
        // 在disable中调用exit没有太大意义,因为关闭是不一定是用gameobject.setActive
        protected virtual void OnDestroy()
        {

        }
        public void AddModules(IModule module)
        {
            if (module is not ILatticeDataInteractionModule<Center>)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("模块所需的交互中心与当前不匹配" + typeof(Center));
                return;
            }
            if (Assert.IsNull(module) || modules.Contains(module)) return;
            if (Assert.CountOutofLimit(modules, maxModuleCount)) return;
            modules.Add(module);
        }
        public void RemoveModules(IModule module)
        {
            modules.Remove(module);
        }
        public Collection LocatedInventory { get; set; }
        public Collection DragInventory { get; private set; }
        Lattice locatedLattice;
        Lattice dragLattice;
        /// <summary>
        /// 指针下方的格子
        /// </summary>
        public Lattice LocatedLattice => locatedLattice;
        /// <summary>
        /// 拖拽的格子(如果有拖拽时)
        /// </summary>
        public Lattice DragLattice => dragLattice;

        public void PointerClick(Lattice lattice, PointerEventData pointerEventData)
        {
            //targetUlattice = ulattice;
            for (int i = 0; i < modules.Count; i++)
            {
                IModule module = modules[i];
                if (module.IsUsable &&
                    module is IPointerClickHanderModule<Center> pointerClickHanderModule &&
                    pointerClickHanderModule.PointerClick(center, pointerEventData))
                {
                    EnableIterateModule = pointerClickHanderModule as IEnableIterateModule;
                    break;
                }
            }
        }
        #region 拖拽的
        public void PointerBeginDrag(Lattice drag, Collection dragInventory, PointerEventData pointerEventData)
        {
            dragLattice = drag;
            DragInventory = dragInventory;
            for (int i = 0; i < modules.Count; i++)
            {
                IModule module = modules[i];
                if (module.IsUsable &&
                    module is IPointerDragHanderModule<Center> pointerDragHanderModule &&
                    pointerDragHanderModule.PointerBeginDrag(center, pointerEventData))
                {
                    currentDragModule = pointerDragHanderModule;
                    EnableIterateModule = pointerDragHanderModule as IEnableIterateModule;
                    break;
                }
            }
        }
        IPointerDragHanderModule<Center> currentDragModule;
        public void PointerOnDrag(Lattice lattice, PointerEventData pointerEventData)
        {
            if (currentDragModule != null && currentDragModule.IsUsable && currentDragModule is IPointerOnDragHanderModule<Center> pointerOnDragHanderModule)
            {
                pointerOnDragHanderModule.PointerOnDrag(center, pointerEventData);
            }
        }
        public void PointerEndDrag(Lattice lattice, PointerEventData pointerEventData)
        {
            if (CheckIsValid(currentDragModule)) currentDragModule.PointerEndDrag(center, pointerEventData);
            currentDragModule = null;// 近期修改增加
            dragLattice = null;
            DragInventory = null;
        }
        #endregion
        bool CheckIsValid(IModule module) => module != null && module.IsUsable;
        #region 进入退出的
        // 要注意的拖拽过程中,进入退出的集合的是当前所在
        public void PointerEnter(Lattice target, PointerEventData pointerEventData)
        {
            locatedLattice = target;
            for (int i = 0; i < modules.Count; i++)
            {
                IModule module = modules[i];
                if (module.IsUsable &&
                    module is IPointerEnterHanderModule<Center> pointerEnterHanderModule &&
                    pointerEnterHanderModule.PointerEnter(center, pointerEventData))
                {
                    currentEnterMudule = pointerEnterHanderModule;
                    EnableIterateModule = pointerEnterHanderModule as IEnableIterateModule;
                    break;
                }
            }
        }
        IPointerEnterHanderModule<Center> currentEnterMudule;
        public void PointerExit(Lattice lattice, PointerEventData pointerEventData)
        {
            if (CheckIsValid(currentEnterMudule)) currentEnterMudule.PointerExit(center, pointerEventData);
            currentEnterMudule = null;// 近期修改增加
            locatedLattice = null;
            //TargetInventory = null;// 离开格子不代表离开库存
        }
        #endregion
    }
}