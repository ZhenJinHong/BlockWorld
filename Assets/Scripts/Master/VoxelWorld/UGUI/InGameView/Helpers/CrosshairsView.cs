using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.InputMiao;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class CrosshairsView : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        [SerializeField] TextMiao targetName;
        [SerializeField] TextMiao targetState;
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        void Start()
        {
            if (viewUMCtrBase == null && !TryGetComponent<ViewUMCtrBase>(out viewUMCtrBase))
                viewUMCtrBase = gameObject.AddComponent<CanvasEnableUMCtr>();
            this.ListenEvent();
        }
        void OnDestroy()
        {
            this.RemoveListen();
        }

        GameCassette gameGassette;
        public void Enter(GameCassette data)
        {
            viewUMCtrBase.Open();
            InputManagerMiao.OnCursorLockEvent += CursorEvent;

            EventManagerMiao.GetEvents<PlayerGameStatus.Events>().OnUpdate += PlayerDataUpdate;
            voxelDefinitionDataBase = data.VoxelWorldDataBaseManaged.VoxelDefinitionDataBase;
            this.gameGassette = data;
        }

        public void Exit(GameCassette data)
        {
            viewUMCtrBase.Close();
            InputManagerMiao.OnCursorLockEvent -= CursorEvent;

            EventManagerMiao.GetEvents<PlayerGameStatus.Events>().OnUpdate -= PlayerDataUpdate;
            this.gameGassette = null;
        }

        public void Pause(GameCassette data)
        {
            viewUMCtrBase.Close();
        }

        public void Continue(GameCassette data)
        {
            viewUMCtrBase.Open();
        }
        IVoxelDefinitionDataBase voxelDefinitionDataBase;
        void PlayerDataUpdate(PlayerGameStatus.IDatas datas)
        {
            if (gameGassette != null)
            {
                var rayResult = datas.VoxelRayResult;// 事件里报错竟不会导致程序暂停
                var voxelItemInfo = voxelDefinitionDataBase.GetVoxelItemInfo(rayResult.Target.VoxelTypeIndex);
                if (voxelItemInfo != null)
                {
                    targetName.TranslationKey = voxelItemInfo.VoxelName;
                    targetState.TextValue = Voxel.Water(rayResult.Target.VoxelMaterial) ? "浸润" : string.Empty;
                }
            }
        }
        void CursorEvent(CursorLockMode mode)
        {
            if (gameGassette == null || gameGassette.InPause)
            {
                return;
            }
            if (gameGassette.InGame)
            {
                bool open = mode != CursorLockMode.None;
                if (open)
                    viewUMCtrBase.Open();
                else
                    viewUMCtrBase.Close();
            }
        }


    }
}