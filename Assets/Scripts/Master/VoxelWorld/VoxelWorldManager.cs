using CatFramework;
using CatDOTS.VoxelWorld;
using Master;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using VoxelWorld.UGUICTR;
using CatFramework.UiMiao;
using CatFramework.EventsMiao;
using CatFramework.InputMiao;
using CatFramework.GameManager;

namespace VoxelWorld
{

    public class VoxelWorldManager : BaseGameManager
    {
        [SerializeField] DataBaseDefinition dataBaseDefinition;
        GameCassetteManager<VoxelWorldManager> GameCassetteManager;
        public GameCassette VoxelWorldGameCassette { get; private set; }
        protected override void Initialize()
        {
            base.Initialize();
            VoxelWorldGameCassette = GameCassette.Create(dataBaseDefinition);
            GameCassetteManager = new GameCassetteManager<VoxelWorldManager>(this, VoxelWorldGameCassette);
            EventManagerMiao.RegisterLazyObject(nameof(VoxelWorldManager), this);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameCassetteManager.Dispose();
        }
        public void EnterGame()
        {
            GameCassetteManager.Enter<GameCassette>();
        }
        public void ReturnHome()
        {
            GameCassetteManager.ExitCurrent();
        }
        public void Continue()
        {
            GameCassetteManager.Continue();
        }
        public void Cursor(InputAction.CallbackContext context)
        {
            if (context.started) ToggleCursor();
        }
        public void MainMenu(InputAction.CallbackContext context)
        {
            if (context.started) ToggleMainMenu();
        }
        public void ToggleCursor()
        {
            if (GameCassetteManager.InPause) return;
            InputManagerMiao.CursorState = InputManagerMiao.CursorState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        public void ToggleMainMenu()
        {
            if (GameCassetteManager.InGame)
            {
                if (GameCassetteManager.InPause)
                    GameCassetteManager.Continue();
                else
                    GameCassetteManager.Pause();
            }
        }
        private void OnApplicationQuit()
        {
            if (ConsoleCat.Enable)
                ConsoleCat.DebugInfo("退出游戏");
        }
    }
    public class ArchivalData : IVoxelWorldGameArchivalData
    {
        public bool IsValid => WorldGenerator != null;
        public IWorldGenerator WorldGenerator
        {
            get => worldGenerator;
            set
            {
                DisposeGenerator();
                worldGenerator = value;
            }
        }
        private IWorldGenerator worldGenerator;
        public void Dispose()
        {
            if (archivalDataRef.IsCreated) archivalDataRef.Dispose();
            DisposeGenerator();
        }
        void DisposeGenerator()
        {
            if (worldGenerator is IDisposable disposable)
            {
                if (ConsoleCat.Enable) ConsoleCat.Log($"释放世界生成器 : {worldGenerator}");
                disposable.Dispose();
            }
        }

        BlobAssetReference<Archival.Data> archivalDataRef;


        public BlobAssetReference<Archival.Data> ArchivalDataRef
        {
            set { if (archivalDataRef.IsCreated) archivalDataRef.Dispose(); archivalDataRef = value; }
        }
        public Archival Archival => new Archival(archivalDataRef);
    }
    public class UiCallDataFuctions
    {
        List<IToolStripContent> toolStripContentforProductInv;
        List<IToolStripContent> toolStripContentforBlueprintInv;
        public List<IToolStripContent> ToolStripContentforProductInv => toolStripContentforProductInv;
        public List<IToolStripContent> ToolStripContentforBlueprintInv => toolStripContentforBlueprintInv;
        GameCassette gameCassette;
        public UiCallDataFuctions(GameCassette gameCassette)
        {
            this.gameCassette = gameCassette;
            toolStripContentforProductInv = new List<IToolStripContent>();
            toolStripContentforProductInv.Add(new ToolStripContentForInventory()
            {
                Label = "材料",
                GetInventory = GetVoxelInv,
            });
            toolStripContentforProductInv.Add(new ToolStripContentForInventory()
            {
                Label = "形状",
                GetInventory = GetShapeInv
            });
            toolStripContentforProductInv.Add(new ToolStripContentForInventory()
            {
                Label = "乙素",
                GetInventory = GetBixelInv,
            });
            toolStripContentforProductInv.Add(new ToolStripContentForInventory()
            {
                Label = "魔杖",
                GetInventory = GetMagicWandInv
            });

            toolStripContentforBlueprintInv = new List<IToolStripContent>();
            toolStripContentforBlueprintInv.Add(new ToolStripContentForInventory()
            {
                Label = "材料",
            });
            toolStripContentforBlueprintInv.Add(new ToolStripContentForInventory()
            {
                Label = "魔杖",
            });
            toolStripContentforBlueprintInv.Add(new ToolStripContentForInventory()
            {
                Label = "乙素",
            });
        }
        IReadonlyItemStorageCollection<IUlatticeItemStorage> GetVoxelInv() => gameCassette?.TotalVoxelItemInventory;
        IReadonlyItemStorageCollection<IUlatticeItemStorage> GetBixelInv() => null;
        IReadonlyItemStorageCollection<IUlatticeItemStorage> GetShapeInv() => gameCassette?.ShapeInventory;
        IReadonlyItemStorageCollection<IUlatticeItemStorage> GetMagicWandInv() => gameCassette?.MagicWandInventory;
    }
}