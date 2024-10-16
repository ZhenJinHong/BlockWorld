using CatFramework.Tools;
using CatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;
using CatFramework.DataMiao;
using CatFramework.InputMiao;

namespace VoxelWorld
{
    public class FreeViewStatusRegister : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] SpectatorInputProvider flyFirstPersonIPAW = new SpectatorInputProvider();
        [SerializeField] PlayerSync playerSync;
        FreeViewStatus freeViewStatus;

        private void Start()
        {
            freeViewStatus =
                new FreeViewStatus(flyFirstPersonIPAW, DataManagerMiao.LoadOrCreateSetting<PlayerFreeViewSetting>(), new TransformTarget(target));

            flyFirstPersonIPAW.RegisterAction();
        }
        private void OnDestroy()
        {
            flyFirstPersonIPAW.UnregisterAction();
        }
        public void FreeView(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (playerSync.IsCurrentStatus(freeViewStatus))
                playerSync.SetDefaultStatus();
            else
                playerSync.SetStatus(freeViewStatus);
        }
    }
    public class FreeViewStatus : PlayerStatus
    {
        bool enableInput;
        SpectatorInputProvider provider;
        PlayerFreeViewSetting setting;
        TransformTarget target;
        public FreeViewStatus(SpectatorInputProvider provider, PlayerFreeViewSetting setting, TransformTarget target)
        {
            this.provider = provider;
            this.setting = setting;
            this.target = target;
        }
        public override bool EnableInput
        {
            get { return enableInput; }
            set
            {
                if (enableInput != value)
                {
                    enableInput = value;
                    provider.SetActive(value);
                }
            }
        }
        public override void OnEnter(GameCassette data)
        {
        }
        public override void OnExit(GameCassette data)
        {
        }
        public override void Update()
        {
            base.Update();
            CatFramework.InputMiao.InputProcessFunction.FirstPersonFreeLook(provider, setting, target, Time.deltaTime);
        }
    }
}
