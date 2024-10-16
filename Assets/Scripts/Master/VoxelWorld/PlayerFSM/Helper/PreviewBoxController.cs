using CatFramework;
using CatFramework.EventsMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelWorld.PlayerHelper
{
    public class PreviewBoxController : MonoBehaviour
    {
        private void Start()
        {
            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
            gameEvents.OnEnter += EnterGame;
            gameEvents.OnExit += ExitGame;
        }
        private void OnDestroy()
        {
            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
            gameEvents.OnEnter -= EnterGame;
            gameEvents.OnExit -= ExitGame;
        }
        void EnterGame(GameCassette data)
        {
            var playerEvents = EventManagerMiao.GetEvents<PlayerGameStatus.Events>();
            playerEvents.OnUpdate += PlayerDataUpdate;
        }
        void ExitGame(GameCassette data)
        {
            var playerEvents = EventManagerMiao.GetEvents<PlayerGameStatus.Events>();
            playerEvents.OnUpdate -= PlayerDataUpdate;
        }
        void PlayerDataUpdate(PlayerGameStatus.IDatas datas)
        {
            transform.position = new float3(datas.VoxelRayResult.TargetIndex);
        }
    }
}
