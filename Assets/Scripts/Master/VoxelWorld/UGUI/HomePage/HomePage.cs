using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.GameManager;
using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    internal class HomePage : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        [SerializeField] RectTransform content;
        VoxelWorldManager voxelWorldManager;
        void Start()
        {
            if (viewUMCtrBase == null)
                viewUMCtrBase = GetComponent<ViewUMCtrBase>();
            MainMenuItem.Flush(content, false);
            EventManagerMiao.Waiting<VoxelWorldManager>(nameof(VoxelWorldManager), ManagerIsReady);
            this.ListenEvent();
        }
        private void OnDestroy()
        {
            this.RemoveListen();
        }
        private void ManagerIsReady(VoxelWorldManager manager)
        {
            voxelWorldManager = manager;
            viewUMCtrBase.Open();
        }

        
        public void ShowReturnHome()
        {
            UiManagerMiao.shareEnquireView.Show("结束游戏", ReturnHome);
        }
        void ReturnHome()
        {
            voxelWorldManager.ReturnHome();
        }
        public void Continue()
        {
            voxelWorldManager.Continue();
        }
        public void ShowEndGame()
        {
            UiManagerMiao.shareEnquireView.Show("退出游戏", EndGame);
        }
        void EndGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Enter(GameCassette data)
        {
            viewUMCtrBase.Close();
        }

        public void Exit(GameCassette data)
        {
            viewUMCtrBase.Open();
            MainMenuItem.Flush(content, false);
        }

        public void Pause(GameCassette data)
        {
            viewUMCtrBase.Open();
            MainMenuItem.Flush(content, true);
        }

        public void Continue(GameCassette data)
        {
            viewUMCtrBase.Close();
        }
    }
}
