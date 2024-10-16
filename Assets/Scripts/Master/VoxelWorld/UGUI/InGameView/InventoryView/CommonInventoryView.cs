using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class CommonInventoryView : MonoBehaviour,IGameEventsListener<GameCassette.Events,GameCassette>
    {
        [SerializeField] UInventoryViewTransfer commonInventoryView;

        private void Start()
        {
            this.ListenEvent();
        }
        private void OnDestroy()
        {
            this.RemoveListen();
        }
        public void Enter(GameCassette data)
        {
            commonInventoryView.Items = data.CommonInventoryInLattice;
            commonInventoryView.Open();
        }

        public void Exit(GameCassette data)
        {
            commonInventoryView.Items = null;
            commonInventoryView.Close();
        }
        public void Continue(GameCassette data)
        {
        }

        public void Pause(GameCassette data)
        {
        }
    }
}