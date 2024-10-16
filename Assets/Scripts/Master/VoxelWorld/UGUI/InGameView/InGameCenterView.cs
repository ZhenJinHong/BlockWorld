using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.InputMiao;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class InGameCenterView : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        public ViewUMCtrBase ViewUMCtrBase => viewUMCtrBase;
        private void Start()
        {
            this.ListenEvent();
        }
        private void OnDestroy()
        {
            this.RemoveListen();
        }
        GameCassette gameCassette;
        public void Enter(GameCassette data)
        {
            gameCassette = data;
            InputManagerMiao.OnCursorLockEvent += CursorEvent;
        }

        public void Exit(GameCassette data)
        {
            gameCassette = null;
            InputManagerMiao.OnCursorLockEvent -= CursorEvent;
        }

        public void Pause(GameCassette data)
        {
            if (viewUMCtrBase.IsVisual)
                viewUMCtrBase.Close();
        }

        public void Continue(GameCassette data)
        {
        }
        void CursorEvent(CursorLockMode mode)
        {
            if (gameCassette == null || gameCassette.InPause)
            {
                ViewUMCtrBase.Close();
                //if (ConsoleCat.IsDebug)
                //    ConsoleCat.DebugInfo("暂停状态不打开");
                return;
            }
            if (gameCassette.InGame)// 未做暂停时的判断,导致暂停时,也打开中心视图,但此时的整个游戏面板是看不见的所以,未察觉
            {
                ViewUMCtrBase.OpenOrClose(mode == CursorLockMode.None);
            }
            else
            {
                if (ViewUMCtrBase.IsVisual)
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.DebugWarning("当前未处于游戏中,中心视图未关闭");
                    ViewUMCtrBase.Close();
                }
                if (ConsoleCat.Enable)
                    ConsoleCat.DebugWarning("未处于游戏中时,中心视图不应该监听鼠标事件");
            }
        }


    }
}