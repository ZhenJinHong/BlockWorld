using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld
{
    public class InGameViewController : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        [SerializeField] ViewUMCtrBase viewUMCtrBase;
        void Start()
        {
            this.ListenEvent();
        }
        private void OnDestroy()
        {
            this.RemoveListen();
        }

        public void Enter(GameCassette data)
        {
            viewUMCtrBase.Open();
        }

        public void Exit(GameCassette data)
        {
            viewUMCtrBase.Close();
        }

        public void Pause(GameCassette data)
        {
            viewUMCtrBase.Close();
        }

        public void Continue(GameCassette data)
        {
            viewUMCtrBase.Open();
        }
    }
}