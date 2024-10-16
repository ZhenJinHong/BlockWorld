using CatFramework;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using CatDOTS;
using CatDOTS.VoxelWorld;
using CatFramework.Tools;
using Unity.Collections;
using CatFramework.CatEntity;
using CatDOTS.VoxelWorld.Player;
using System;
using CatFramework.Magics;
using CatFramework.EventsMiao;
using CatFramework.DataMiao;
using CatDOTS.VoxelWorld.Magics;
using CatFramework.UiMiao;
using CatFramework.InputMiao;
using CatFramework.PlayerManager;

namespace VoxelWorld
{
    [System.Serializable]
    public class PlayerGameInputProvider : InputActionWrapper, FirstPersonPlayerSystem.IInputProvider
    {
        FirstPersonInput inputActions;
        FirstPersonInput InputActions { get { inputActions ??= InputManagerMiao.GetShapeInputAsset<FirstPersonInput>(); return inputActions; } }
        protected override bool Active => InputActions.PlayerInWorld.enabled;
        public Vector3 defaultPosition = new Vector3(0f, 200f, 0f);
        public float3 DefaultPosition => defaultPosition;
        #region 输入的读取
        public bool LeftPress { get; set; }
        public bool RightPress { get; set; }
        public PlayerState State { get; set; }
        private Vector2 look;
        public float2 Look
        {
            get
            {
                var temp = look;// 应该平滑下?依旧置空,但下次追加look是否应该根据上次的look平滑?
                look = Vector2.zero;
                return temp;
            }
            private set => look = value;
        }
        public float2 Move { get; private set; }
        #endregion
        bool enableInput;
        public bool EnableInput
        {
            get => enableInput;
            set
            {
                if (enableInput != value)
                {
                    enableInput = value;
                    SetActive(value);
                }
            }
        }
        public override void Enable()
        {
            InputActions.PlayerInWorld.Enable();
        }
        public override void Disable()
        {
            InputActions.PlayerInWorld.Disable();
        }
        protected override void InternalRegister()
        {
            if (ConsoleCat.Enable)// 保留,可以用来判定是否重复进入状态,但是状态机已判定,不应该重复的
                ConsoleCat.Log("登记玩家在世界中的输入行为");
            var playeraction = InputActions.PlayerInWorld;

            playeraction.PrimaryAction.started += LeftBtn;
            playeraction.PrimaryAction.canceled += LeftBtn;
            playeraction.SecondAction.started += RightBtn;
            playeraction.SecondAction.canceled += RightBtn;

            playeraction.Look.started += PointerMove;
            playeraction.Look.canceled += PointerMove;
            playeraction.Move.performed += WASD;
            playeraction.Move.canceled += WASD;

            playeraction.Fly.started += Flying;
            playeraction.JumpOrElevated.started += Jump;
            playeraction.JumpOrElevated.canceled += NoJump;
            playeraction.CrouchOrFalling.started += Crouch;
            playeraction.CrouchOrFalling.canceled += NoCrouch;

            playeraction.Sprint.started += Shift;
        }
        protected override void InternalUnregister()
        {
            var playeraction = InputActions.PlayerInWorld;


            playeraction.PrimaryAction.started -= LeftBtn;
            playeraction.PrimaryAction.canceled -= LeftBtn;
            playeraction.SecondAction.started -= RightBtn;
            playeraction.SecondAction.canceled -= RightBtn;

            playeraction.Look.started -= PointerMove;
            playeraction.Look.canceled -= PointerMove;
            playeraction.Move.performed -= WASD;
            playeraction.Move.canceled -= WASD;

            playeraction.Fly.started -= Flying;
            playeraction.JumpOrElevated.started -= Jump;
            playeraction.JumpOrElevated.canceled -= NoJump;
            playeraction.CrouchOrFalling.started -= Crouch;
            playeraction.CrouchOrFalling.canceled -= NoCrouch;

            playeraction.Sprint.started -= Shift;
        }
        #region 输入
        void LeftBtn(InputAction.CallbackContext context) => LeftPress = context.started;
        void RightBtn(InputAction.CallbackContext context) => RightPress = context.started;
        void PointerMove(InputAction.CallbackContext context) => look += context.ReadValue<Vector2>();
        void Flying(InputAction.CallbackContext context) => State = (State & PlayerState.Flying) == PlayerState.Flying ?
                State ^ PlayerState.Flying : State | PlayerState.Flying;
        //速度提升
        void Jump(InputAction.CallbackContext context) => State |= PlayerState.Jump;
        void NoJump(InputAction.CallbackContext context) => State ^= PlayerState.Jump;
        void Crouch(InputAction.CallbackContext context) => State |= PlayerState.Crouch;
        void NoCrouch(InputAction.CallbackContext context) => State ^= PlayerState.Crouch;
        void WASD(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
        void Shift(InputAction.CallbackContext context)
            => State = (State & PlayerState.Sprint) == PlayerState.Sprint ?
                State ^ PlayerState.Sprint : State | PlayerState.Sprint;
        #endregion
    }
    // TODO封装输入结果,传递出去,完成输入的系统也要封装一个射线投射结果,以便可以按照实体执行不同操作
    public class PlayerGameStatus : PlayerStatus, ICatEntity, FinishPlyerInputSystem.IOutPutReceiver, IPlayerInWorldActionData, IVoxelPlayer, IMagicWandHolder
    {
        public interface IDatas
        {
            IPlayerInWorldActionData PlayerInWorldActionData { get; }
            IVoxelRayResult VoxelRayResult { get; }
        }
        class Datas : IDatas
        {
            public PlayerGameStatus PlayerGameStatus { get; set; }
            public IPlayerInWorldActionData PlayerInWorldActionData => PlayerGameStatus;
            public IVoxelRayResult VoxelRayResult => PlayerGameStatus.VoxelRayResult;
        }
        interface IEvents : IUniqueEvents
        {
            void FinishInput(IDatas data);
            void UiUpdate(IDatas data);
            void Update(IDatas data);
        }
        public class Events : IEvents
        {
            /// <summary>
            /// 数据是上一帧的数据
            /// </summary>
            public event Action<IDatas> OnUpdate;
            /// <summary>
            /// 当帧的操作数据
            /// </summary>
            public event Action<IDatas> OnFinishInput;
            public event Action<IDatas> OnUiUpdate;
            void IEvents.Update(IDatas data)
            {
                if (data != null)
                    OnUpdate?.Invoke(data);
            }
            void IEvents.FinishInput(IDatas data)
            {
                if (data != null)
                    OnFinishInput?.Invoke(data);
            }
            void IEvents.UiUpdate(IDatas data)
            {
                if (data != null)
                    OnUiUpdate?.Invoke(data);
            }
        }
        public override bool EnableInput
        {
            get => provider.EnableInput;
            set => provider.EnableInput = value;
        }
        public float3 EyeOffset { get; set; }
        #region 输入的完成
        public float3 Position
        {
            get => position; set
            {
                position = value;
                playerTrans.position = value;
                carmeraTrans.position = value + EyeOffset;
                PlayerManagerMiao.PlayerPosition = value;
            }
        }
        public quaternion CameraRotation
        {
            get => cameraRotation; set
            {
                cameraRotation = value;
                carmeraTrans.rotation = value;
            }
        }
        public quaternion PlayerRotation
        {
            get => playerRotation; set
            {
                playerRotation = value;
                playerTrans.rotation = value;
            }
        }

        public IVoxelRayResult VoxelRayResult => voxelRayResultManaged;
        public bool Enable => provider.EnableInput;
        public ICatEntity CatEntity => this;
        VoxelMatchTool voxelMatchTool = new VoxelMatchTool();
        public void AddVoxel(Voxel voxel)
        {
            voxelMatchTool.SetVoxel(voxel);
            int index = CommonInventoryInBar.FindIndex(voxelMatchTool.match);
            if (index != -1)
            {
                voxelMatchTool.SetVoxelToInventory(CommonInventoryInBar, gameCassette, index);
            }
            else if ((index = gameCassette.CommonInventoryInLattice.FindIndex(voxelMatchTool.match)) != -1)
            {
                voxelMatchTool.SetVoxelToInventory(gameCassette.CommonInventoryInLattice, gameCassette, index);
            }
            else
            {
                gameCassette.TotalVoxelItemInventory.Add(voxel.VoxelTypeIndex, 1);
                gameCassette.TotalVoxelItemInventory.NotifyChange();
            }

        }
        class VoxelMatchTool
        {
            Voxel voxel;
            public Predicate<IUlatticeItemStorage> match { get; }
            public VoxelMatchTool()
            {
                match = M;
            }
            public void SetVoxel(Voxel voxel) { this.voxel = voxel; }
            bool M(IUlatticeItemStorage item)
            {
                return item == null || (item is VoxelItemStorage storage && storage.VoxelID == voxel.VoxelTypeIndex);
            }
            public void SetVoxelToInventory(CommonInventory commonInventory, GameCassette dataCollection, int index)
            {
                var item = commonInventory.GetItem(index);
                if (item == null)
                {
                    var voxelDefinitionDataBase = dataCollection.VoxelWorldDataBaseManaged.VoxelDefinitionDataBase;
                    var shapeDefinitionDataBase = dataCollection.VoxelWorldDataBaseManaged.ShapeDefinitionDataBase;
                    commonInventory.SetItem(new VoxelItemStorage() { VoxelItemInfo = voxelDefinitionDataBase.GetVoxelItemInfo(voxel.VoxelTypeIndex), VoxelShapeInfo = shapeDefinitionDataBase.GetDefaultVoxelShapeInfo(), itemCount = 1 }, index);
                }
                else if (item is VoxelItemStorage itemStorage)
                {
                    itemStorage.Increase(1);
                }
                commonInventory.NotifyChange();
            }
        }

        #endregion
        private Transform playerTrans, carmeraTrans;
        private quaternion cameraRotation, playerRotation;
        private float3 position;
        private Datas datas;
        private IEvents events;
        private GameCassette gameCassette;
        private MagicWandSwitch magicWandSwitch;
        private VoxelRayResultManaged voxelRayResultManaged;
        private PlayerGameInputProvider provider;
        public PlayerGameStatus(PlayerGameInputProvider firstPersonInputValue, Transform playerTrans, Transform carmeraTrans)
        {
            this.playerTrans = playerTrans;
            this.carmeraTrans = carmeraTrans;

            this.provider = firstPersonInputValue;

            EyeOffset = new float3(0f, 1.7f, 0f);

            datas = new Datas()
            {
                PlayerGameStatus = this,
            };
            events = EventManagerMiao.GetEvents<Events>();

            magicWandSwitch = new MagicWandSwitch(this);
            voxelRayResultManaged = new VoxelRayResultManaged();
        }
        public T GetComponent<T>() where T : class, IComponent
        {
            return this as T;
        }

        public bool TryGetComponent<T>(out T component) where T : class, IComponent
        {
            component = this as T;
            return component != null;
        }
        public override void OnEnter(GameCassette data)
        {
            gameCassette = data;
            World world = World.DefaultGameObjectInjectionWorld;
            var playerInputSystem = world.GetExistingSystemManaged<FirstPersonPlayerSystem>();
            var finishPlyerInputSystem = world.GetExistingSystemManaged<FinishPlyerInputSystem>();
            playerInputSystem.inputProvider = provider;
            playerInputSystem.setting = DataManagerMiao.LoadOrCreateSetting<PlayerSettingData>();
            finishPlyerInputSystem.outPutReceiver = this;
        }
        public override void Update()
        {
            base.Update();
            events.Update(datas);
        }
        public override void OnExit(GameCassette data)
        {
            gameCassette = null;
            World world = World.DefaultGameObjectInjectionWorld;
            var playerInputSystem = world.GetExistingSystemManaged<FirstPersonPlayerSystem>();
            var finishPlyerInputSystem = world.GetExistingSystemManaged<FinishPlyerInputSystem>();
            playerInputSystem.inputProvider = null;
            playerInputSystem.setting = null;
            finishPlyerInputSystem.outPutReceiver = null;
        }
        #region 用以简洁的属性
        CommonInventory CommonInventoryInBar => gameCassette.CommonInventoryInBar;
        MagicWandInventory MagicWandInventory => gameCassette.MagicWandInventory;
        #endregion
        public void FinishInput(NativeReference<VoxelRayResult>.ReadOnly voxelRayResult)
        {
            voxelRayResultManaged.Update(voxelRayResult);

            if (gameCassette != null)
            {
                if (provider.LeftPress || provider.RightPress)
                {
                    IUlatticeItemStorage current = CommonInventoryInBar.Current;
                    if (current is IVoxelItemStorage || current == null)
                    {
                        var voxelMagicWand =
                           provider.LeftPress || current == null ? MagicWandInventory.DestoryVoxelMagicWand : MagicWandInventory.PutVoxelMagicWand;

                        voxelMagicWand.SetMagicEnergy(current as IMagicEnergy);
                        magicWandSwitch.Hold(voxelMagicWand);
                        if (magicWandSwitch.Fire())
                        {
                            CommonInventoryInBar.NotifyChange();
                        }
                    }
                    else if (current is MagicWandStorage magicWandStorage)
                    {
                        magicWandSwitch.Hold(magicWandStorage.GetMagicWand());
                        if (magicWandSwitch.Fire())
                            CommonInventoryInBar.NotifyChange();
                    }
                }
            }
            events.FinishInput(datas);
        }
    }
}