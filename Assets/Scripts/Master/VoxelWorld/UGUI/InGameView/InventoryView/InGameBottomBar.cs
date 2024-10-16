using CatFramework;
using CatFramework.EventsMiao;
using CatFramework.InputMiao;
using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class InGameBottomBar : MonoBehaviour, IGameEventsListener<GameCassette.Events, GameCassette>
    {
        [SerializeField] InputActionAsset asset;
        ItemSlotInputAction itemSlotInputAction;
        [SerializeField] UInventoryViewTransfer barItemView;
        GameCassette gameCassette;
        void Start()
        {
            itemSlotInputAction = new ItemSlotInputAction(asset)
            {
                OnScroll = SwitchBarItem,
                OnSelected = SelectBarItem,
            };
            itemSlotInputAction.RegisterAction();
            this.ListenEvent();
        }
        void OnDestroy()
        {
            itemSlotInputAction.UnregisterAction();
            this.RemoveListen();
        }
        public void Enter(GameCassette data)
        {
            gameCassette = data;
            barItemView.Items = data.CommonInventoryInBar;
            barItemView.Open();

            InputManagerMiao.OnCursorLockEvent += CursorEvent;
            itemSlotInputAction.Enable();
        }

        public void Exit(GameCassette data)
        {
            gameCassette = null;
            barItemView.Close();
            barItemView.Items = null;

            InputManagerMiao.OnCursorLockEvent -= CursorEvent;
            itemSlotInputAction.Disable();
        }

        public void Pause(GameCassette data)
        {
            barItemView.Close();
            itemSlotInputAction.Disable();
        }

        public void Continue(GameCassette data)
        {
            barItemView.Open();
            itemSlotInputAction.Enable();
        }
        void CursorEvent(CursorLockMode mode)
        {
            if (gameCassette == null || gameCassette.InPause) return;
            itemSlotInputAction.SetActive(!(mode == CursorLockMode.None));
        }
        #region 切换快捷栏
        private void SwitchBarItem(int v) => barItemView.SwitchLattice(v);
        private void SelectBarItem(int v) => barItemView.SelectedLattice(v);
        #endregion
    }
}