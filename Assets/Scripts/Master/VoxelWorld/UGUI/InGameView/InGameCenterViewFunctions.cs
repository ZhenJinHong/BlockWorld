using CatFramework;
using CatFramework.DataMiao;
using CatFramework.EventsMiao;
using CatFramework.Tools;
using CatFramework.UiMiao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class InGameCenterViewFunctions : MonoBehaviour
    {
        [SerializeField] ViewModule[] leftViewModules;
        SimilarViewModuleController leftViewModuleCtr;
        void Start()
        {
            leftViewModuleCtr = new SimilarViewModuleController(leftViewModules);
            var events = EventManagerMiao.GetEvents<GameCassette.Events>();
            events.OnEnter += Enter;
            events.OnExit += Exit;
        }
        private void OnDestroy()
        {
            var events = EventManagerMiao.GetEvents<GameCassette.Events>();
            events.OnEnter -= Enter;
            events.OnExit -= Exit;
        }
        public void ShowProductsInventory()
        {
            leftViewModuleCtr.Show(uiCallDataFuctions.ToolStripContentforProductInv);
        }
        public void ShowBluePrintsInventory()
        {
            leftViewModuleCtr.Show(uiCallDataFuctions.ToolStripContentforBlueprintInv);
        }
        UiCallDataFuctions uiCallDataFuctions;
        public void Enter(GameCassette data)
        {
            uiCallDataFuctions = data.UiCallDataFuctions;
        }
        public void Exit(GameCassette data)
        {
            uiCallDataFuctions = null;
            leftViewModuleCtr.Hide();
        }
    }
}