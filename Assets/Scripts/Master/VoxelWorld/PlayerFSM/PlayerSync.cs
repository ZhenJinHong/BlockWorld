using CatFramework;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using CatFramework.Tools;
using UnityEngine.InputSystem;
using CatFramework.EventsMiao;
using Unity.Entities;
using CatDOTS;
using CatFramework.InputMiao;
using Unity.Mathematics;
using CatFramework.GameManager;

namespace VoxelWorld
{
    public class PlayerSync : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        public PlayerGameStatus playerGameStatus;
        GameCassette gameCassette;
        EntityQuery playerQuery;
        EntityQuery PlayerQuery
        {
            get
            {
                if (!World.DefaultGameObjectInjectionWorld.EntityManager.IsQueryValid(playerQuery))
                {
                    EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
                    builder.WithAll<FirstPersonPlayer>().WithOptions(EntityQueryOptions.IncludeDisabledEntities);
                    playerQuery = builder.Build(World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<FirstPersonPlayerSystem>());
                }
                return playerQuery;
            }
        }
        void Start()
        {
            EnableInput = false;
            this.ListenEvent();
        }

        private void Update()
        {
            if (enableInput)
                m_PlayerStatus?.Update();
        }
        void OnDestroy()
        {
            SetStatus(null);
            this.RemoveListen();
        }
        #region 游戏状态监听
        public void Enter(GameCassette data)
        {
            this.gameCassette = data;
            InputManagerMiao.OnCursorLockEvent += CursorEvent;
            if (ConsoleCat.Enable)
                ConsoleCat.Log("进入游戏");
            SetStatus(playerGameStatus);
            if (!PlayerQuery.IsEmpty)
            {
                World.DefaultGameObjectInjectionWorld.EntityManager.SetEnabled(playerQuery.GetSingletonEntity(), true);
            }
            EnableInput = true;
        }

        public void Exit(GameCassette data)
        {
            this.gameCassette = null;
            InputManagerMiao.OnCursorLockEvent -= CursorEvent;
            SetStatus(null);
            if (!PlayerQuery.IsEmpty)
            {
                World.DefaultGameObjectInjectionWorld.EntityManager.SetEnabled(playerQuery.GetSingletonEntity(), false);
            }
            EnableInput = false;
        }
        public void Pause(GameCassette data)
        {
            EnableInput = false;
        }
        public void Continue(GameCassette data)
        {
            EnableInput = true;
        }
        #endregion
        #region 状态更换
        private bool enableInput;
        PlayerStatus m_PlayerStatus;
        public bool EnableInput
        {
            get => enableInput;
            set
            {
                enableInput = value;
                if (m_PlayerStatus != null) { m_PlayerStatus.EnableInput = value; }
            }
        }
        public bool IsCurrentStatus(PlayerStatus status) => status == m_PlayerStatus;
        public void SetDefaultStatus()
        {
            if (gameCassette != null && gameCassette.InGame)
                SetStatus(playerGameStatus);
            else
                SetStatus(null);
        }
        public void SetStatus(PlayerStatus status)
        {
            if (m_PlayerStatus != status)
            {
                if (m_PlayerStatus != null)
                {
                    m_PlayerStatus.OnExit(gameCassette);
                    m_PlayerStatus.EnableInput = false;
                }
                m_PlayerStatus = status;
                if (m_PlayerStatus != null)
                {
                    m_PlayerStatus.OnEnter(gameCassette);
                    m_PlayerStatus.EnableInput = enableInput;
                }
            }
        }

        public void CursorEvent(CursorLockMode cursorLockMode)
        {
            EnableInput = cursorLockMode == CursorLockMode.Locked;
        }
        #endregion
    }
}