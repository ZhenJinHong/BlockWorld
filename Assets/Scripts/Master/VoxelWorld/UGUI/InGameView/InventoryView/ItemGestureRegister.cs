using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class ItemGestureRegister : MonoBehaviour
    {
        [SerializeField] DataInteractionModuleCenter dataInteractionModuleCenter;
        [SerializeField] ShareDragUlattice shareDragUlattice;
        [Header("模块对象")]
        [SerializeField] ItemInfoBox itemInfoBox;
        [SerializeField] ProductDisplayTable productDisplayTable;

        UlatticeDataInteraction ulatticeDataInteraction;
        VoxelItemAmountSplitModule voxelItemAmountSplitModule;
        //ItemInfoBoxModule itemInfoBoxModule;
        void Start()
        {

            ulatticeDataInteraction = new UlatticeDataInteraction()
            {
                shareDragUlattice = shareDragUlattice,
            };
            voxelItemAmountSplitModule = new VoxelItemAmountSplitModule()
            {
                shareDragUlattice = shareDragUlattice,
            };
            //itemInfoBoxModule = new ItemInfoBoxModule(itemInfoBox);// 接口的判空和UnityObject不一样,但是这里的判空用的是isEnable里的,是Unity的判断方式所以无错

            ulatticeDataInteraction.IsUsable = true;
            voxelItemAmountSplitModule.IsUsable = true;
            itemInfoBox.IsUsable = true;
            productDisplayTable.IsUsable = true;

            dataInteractionModuleCenter.AddModules(ulatticeDataInteraction);
            dataInteractionModuleCenter.AddModules(voxelItemAmountSplitModule);
            dataInteractionModuleCenter.AddModules(itemInfoBox);
            dataInteractionModuleCenter.AddModules(productDisplayTable);
            var events = EventManagerMiao.GetEvents<GameCassette.Events>();
            events.OnEnter += Enter;
        }
        void OnDestroy()
        {
            if (dataInteractionModuleCenter != null)
            {
                dataInteractionModuleCenter.RemoveModules(ulatticeDataInteraction);
                dataInteractionModuleCenter.RemoveModules(voxelItemAmountSplitModule);
                dataInteractionModuleCenter.RemoveModules(itemInfoBox);
                dataInteractionModuleCenter.RemoveModules(productDisplayTable);
            }
            var events = EventManagerMiao.GetEvents<GameCassette.Events>();
            events.OnEnter -= Enter;
        }
        public void SplitItem(bool v)
        {
            voxelItemAmountSplitModule.Split = v;
        }
        private void Enter(GameCassette cassette)
        {
            voxelItemAmountSplitModule.gameCassette = cassette;
        }
        //void CursorEvent(CursorLockMode cursorLockMode)
        //{
        //    if (Ingame && !IsPause)
        //    {
        //        if (cursorLockMode == CursorLockMode.None)
        //            asset.Enable();
        //        else
        //            asset.Disable();
        //    }
        //}
    }
}