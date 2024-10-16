using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.CatMath;
using CatFramework.EventsMiao;
using CatFramework.Tools;
using CatFramework.UiMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class BeginGameView : MonoBehaviour
    {
        [SerializeField] UIntInputField seed;
        [SerializeField] DropdownMiao constructorDropdown;
        VoxelWorldManager voxelWorldManager;
        GameCassette gameCassette;
        private void Start()
        {
            // 这个游戏对象是从预制体实例化的,所以可以在Start -> Get
            voxelWorldManager = EventManagerMiao.GetLazyObject<VoxelWorldManager>(nameof(VoxelWorldManager));
            gameCassette = voxelWorldManager.VoxelWorldGameCassette;
            ListUtility.ListConverter<IWorldDefinition, string>(gameCassette.WorldDefinitions, constructorDropdown.OptionsData, (c) => c.Name);
            constructorDropdown.SetValueWithoutNotify(0);
        }
        void UpdateData()
        {
            Archival.Data archivalData = new Archival.Data()
            {
                InitialWorldCenterChunkIndex = VoxelMath.PlayerChunkIndex(new float3(0f, 250f, 0f))
            };
            ArchivalData archivalManaged = gameCassette.ArchivalData;
            archivalManaged.ArchivalDataRef = BlobAssetReference<Archival.Data>.Create(archivalData);

            uint seedValue = this.seed.GetValue() == 0 ? MathC.NextUInt() : seed.GetValue();
            seedValue = seedValue == 0 ? 111154154 : seedValue;

            archivalManaged.WorldGenerator =
                gameCassette.WorldDefinitions[constructorDropdown.GetValue()].
                Create(seedValue, gameCassette.VoxelWorldDataBaseManaged);

            if (ConsoleCat.IsDebug)
                ConsoleCat.DebugInfo($"种子 : {seedValue};世界生成器数据 : {archivalManaged.WorldGenerator}");
        }
        public void EnterGame()
        {
            UpdateData();
            voxelWorldManager.EnterGame();
        }
    }
}