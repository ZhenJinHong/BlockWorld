using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public class DataInteractionModuleCenter : LatticeDataInteractionCenter<Ulattice, DataInteractionModuleCenter, IReadonlyItemStorageCollection<IUlatticeItemStorage>>
    {
        //List<IModule> dataInteractionModules = new List<IModule>();
        //IEnableIterateModule enableIterateModule;
        //IEnableIterateModule EnableIterateModule
        //{
        //    set
        //    {
        //        enabled = value != null;
        //        if (enableIterateModule != null && enableIterateModule.IsUsable)
        //        {
        //            enableIterateModule.EndUpdate();
        //        }
        //        enableIterateModule = value;
        //    }
        //}
        //private void Update()
        //{
        //    if (enableIterateModule != null)
        //    {
        //        if (!enableIterateModule.IsUsable || !enableIterateModule.Update())
        //        {
        //            EnableIterateModule = null;
        //        }
        //    }
        //}
        ////private void OnDestroy()
        ////{

        ////}
        //public void AddModules(IModule module)
        //{
        //    if (Assert.IsNull(module) || dataInteractionModules.Contains(module)) return;
        //    if (Assert.CountOutofLimit(dataInteractionModules, 24)) return;
        //    //if (ConsoleCat.Enable) ConsoleCat.Log("添加模块" + module.GetType());
        //    dataInteractionModules.Add(module);
        //}
        //public void RemoveModules(IModule module)
        //{
        //    dataInteractionModules.Remove(module);
        //}
        //public IReadonlyItemStorageCollection<IUlatticeItemStorage> TargetInventory { get; set; }
        //public IReadonlyItemStorageCollection<IUlatticeItemStorage> DragInventory { get; private set; }
        //Ulattice targetUlattice;
        //Ulattice dragUlattice;
        ///// <summary>
        ///// 指针下方的格子
        ///// </summary>
        //public Ulattice TargetUlattice => targetUlattice;
        ///// <summary>
        ///// 拖拽的格子(如果有拖拽时)
        ///// </summary>
        //public Ulattice DragUlattice => dragUlattice;

        //public void PointerClick(Ulattice ulattice, PointerEventData pointerEventData)
        //{
        //    //targetUlattice = ulattice;
        //    for (int i = 0; i < dataInteractionModules.Count; i++)
        //    {
        //        IModule module = dataInteractionModules[i];
        //        if (module.IsUsable &&
        //            module is IPointerClickHanderModule pointerClickHanderModule &&
        //            pointerClickHanderModule.PointerClick(this, pointerEventData))
        //        {
        //            EnableIterateModule = pointerClickHanderModule as IEnableIterateModule;
        //            break;
        //        }
        //    }
        //}
        //#region 拖拽的
        //public void PointerBeginDrag(Ulattice drag, IReadonlyItemStorageCollection<IUlatticeItemStorage> dragInventory, PointerEventData pointerEventData)
        //{
        //    dragUlattice = drag;
        //    DragInventory = dragInventory;
        //    for (int i = 0; i < dataInteractionModules.Count; i++)
        //    {
        //        IModule module = dataInteractionModules[i];
        //        if (module.IsUsable &&
        //            module is IPointerDragHanderModule pointerDragHanderModule &&
        //            pointerDragHanderModule.PointerBeginDrag(this, pointerEventData))
        //        {
        //            currentDragModule = pointerDragHanderModule;
        //            EnableIterateModule = pointerDragHanderModule as IEnableIterateModule;
        //            break;
        //        }
        //    }
        //}
        //IPointerDragHanderModule currentDragModule;
        //public void PointerOnDrag(PointerEventData pointerEventData)
        //{
        //    if (currentDragModule != null && currentDragModule.IsUsable && currentDragModule is IPointerOnDragHanderModule pointerOnDragHanderModule)
        //    {
        //        pointerOnDragHanderModule.PointerOnDrag(this, pointerEventData);
        //    }
        //}
        //public void PointerEndDrag(PointerEventData pointerEventData)
        //{
        //    if (CheckIsValid(currentDragModule)) currentDragModule.PointerEndDrag(this, pointerEventData);
        //    currentDragModule = null;// 近期修改增加
        //    dragUlattice = null;
        //    DragInventory = null;
        //}
        //#endregion
        //bool CheckIsValid(IModule module) => module != null && module.IsUsable;
        //#region 进入退出的
        //public void PointerEnter(Ulattice target, PointerEventData pointerEventData)
        //{
        //    targetUlattice = target;
        //    for (int i = 0; i < dataInteractionModules.Count; i++)
        //    {
        //        IModule module = dataInteractionModules[i];
        //        if (module.IsUsable &&
        //            module is IPointerEnterHanderModule pointerEnterHanderModule &&
        //            pointerEnterHanderModule.PointerEnter(this, pointerEventData))
        //        {
        //            currentEnterMudule = pointerEnterHanderModule;
        //            EnableIterateModule = pointerEnterHanderModule as IEnableIterateModule;
        //            break;
        //        }
        //    }
        //}
        //IPointerEnterHanderModule currentEnterMudule;
        //public void PointerExit(PointerEventData pointerEventData)
        //{
        //    if (CheckIsValid(currentEnterMudule)) currentEnterMudule.PointerExit(this, pointerEventData);
        //    currentEnterMudule = null;// 近期修改增加
        //    targetUlattice = null;
        //    //TargetInventory = null;// 离开格子不代表离开库存
        //}
        //#endregion
    }
}