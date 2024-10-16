using CatDOTS.VoxelWorld;
using CatFramework.EventsMiao;
using VoxelWorld.UGUICTR;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld
{
    [RequireComponent(typeof(Volume))]
    public class UnderWaterCtr : MonoBehaviour
    {
        Volume volume;
        void Start()
        {
            volume = GetComponent<Volume>();
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
        public void Enter(GameCassette data)
        {
            EventManagerMiao.GetEvents<PlayerGameStatus.Events>().OnUpdate += PlayerDataUpdate;
        }
        public void Exit(GameCassette data)
        {
            EventManagerMiao.GetEvents<PlayerGameStatus.Events>().OnUpdate -= PlayerDataUpdate;
            volume.enabled = false;
        }
        void PlayerDataUpdate(PlayerGameStatus.IDatas datas)
        {
            var rayResult = datas.VoxelRayResult;
            if (Voxel.Water(rayResult.VoxelInEyePos.VoxelMaterial))
            {
                volume.enabled = true;
            }
            else
            {
                volume.enabled = false;
            }
        }


    }
}