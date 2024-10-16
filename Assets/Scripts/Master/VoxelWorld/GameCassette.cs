using CatFramework;
using CatDOTS;
using CatDOTS.VoxelWorld;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using CatFramework.Magics;
using CatDOTS.VoxelWorld.Magics;
using CatFramework.DataMiao;
using CatFramework.EventsMiao;
using CatFramework.InputMiao;

namespace VoxelWorld
{
    public class GameCassette : CatFramework.GameManager.GameCassette<VoxelWorldManager, GameCassette.Events, GameCassette>
    {
        public UiCallDataFuctions UiCallDataFuctions { get; private set; }
        public CommonInventory CommonInventoryInBar { get; private set; }
        public CommonInventory CommonInventoryInLattice { get; private set; }
        public VoxelShapeInventory ShapeInventory { get; private set; }
        public MagicWandInventory MagicWandInventory { get; private set; }
        public VoxelItemInventory TotalVoxelItemInventory { get; private set; }
        public VoxelWorldDataBaseManaged VoxelWorldDataBaseManaged { get; private set; }
        public ArchivalData ArchivalData { get; private set; }
        public IList<IWorldDefinition> WorldDefinitions { get; private set; }
        public SyncECBCreator<BeginSimulationEntityCommandBufferSystem> SyncECBCreator { get; private set; }
        public class Events : GameEvents<GameCassette>
        {
        }

        Entity voxelWorldEntity;
        List<AsyncOperationHandle> asyncOperationHandles;
        GameCassette()
        {
           
        }
        // 为了后续的异步?,那么new的时候也可以放在一个方法里异步?
        public static GameCassette Create(DataBaseDefinition dataBaseDefinition)
        {
            var DefHandle = Addressables.LoadAssetsAsync<ScriptableObject>("Definition", null);

            List<IVoxelDefinition> voxelDefinitions = new List<IVoxelDefinition>();
            List<IVoxelShapeDefinition> voxelShapeDefinitions = new List<IVoxelShapeDefinition>();
            List<IMagicWandDefinition> magicWandDefinitions = new List<IMagicWandDefinition>();
            List<IWorldDefinition> worldDefinitions = new List<IWorldDefinition>();

            var defs = DefHandle.WaitForCompletion();
            foreach (ScriptableObject scriptableObject in defs)
            {
                switch (scriptableObject)
                {
                    case IVoxelDefinition voxelDefinition:
                        voxelDefinitions.Add(voxelDefinition);
                        break;
                    case IVoxelShapeDefinition voxelShapeDefinition:
                        voxelShapeDefinitions.Add(voxelShapeDefinition);
                        break;
                    case IMagicWandDefinition magicWandDefinition:
                        magicWandDefinitions.Add(magicWandDefinition);
                        break;
                    case IWorldDefinition worldDefinition:
                        worldDefinitions.Add(worldDefinition);
                        break;
                }
            }

            TempDataBase tempDataBase = new TempDataBase
            {
                VoxelDefinitions = voxelDefinitions,
                VoxelShapeDefinitions = voxelShapeDefinitions,
                MagicWandDefinitions = magicWandDefinitions,
            };

            VoxelWorldSettings voxelWorldSettings = new VoxelWorldSettings()
            {
                VoxelWorldSetting = DataManagerMiao.LoadOrCreateSetting<QualitySettingsData>(),
                DebugSetting = DataManagerMiao.LoadOrCreateSetting<DebugSetting>(),
                //WorldChunkSetting = new WorldChunkSetting(24, 288),
            };
            ArchivalData archivalData = new ArchivalData()
            {

            };
            VoxelWorldDataBaseManaged dataBaseManaged = new VoxelWorldDataBaseManaged(dataBaseDefinition, archivalData, voxelWorldSettings, tempDataBase);

            //List<IWorldGenerator> worldGenerators = new List<IWorldGenerator>();
            //if (worldDefinitions.Count != 0)
            //{
            //    for (int i = 0; i < worldDefinitions.Count; i++)
            //    {
            //        worldGenerators.Add(worldDefinitions[i].GetWorldGenerator(dataBaseManaged.VoxelDefinitionDataBase));
            //    }
            //}

            List<AsyncOperationHandle> asyncOperationHandles = new List<AsyncOperationHandle>
            {
                DefHandle
            };

            World world = World.DefaultGameObjectInjectionWorld;

            IShapeDefinitionDataBase shapeDefinitionDataBase = dataBaseManaged.ShapeDefinitionDataBase;

            EntityManager entityManager = world.EntityManager;
            Entity entity = entityManager.CreateSingleton<VoxelWorldDataBase>(dataBaseManaged.VoxelWorldDataBase);

            entityManager.AddComponentObject(entity, dataBaseManaged);
            entityManager.AddComponentData<VoxelShapeColliderAsset>(entity, shapeDefinitionDataBase.VoxelShapeColliderAsset);


            GameCassette voxelWorldGameCassette = new GameCassette()
            {
                voxelWorldEntity = entity,
                asyncOperationHandles = asyncOperationHandles,
                VoxelWorldDataBaseManaged = dataBaseManaged,
                ArchivalData = archivalData,
                WorldDefinitions = worldDefinitions,
                ShapeInventory = new VoxelShapeInventory(shapeDefinitionDataBase.VoxelShapeClassifies),
                TotalVoxelItemInventory = new VoxelItemInventory(dataBaseManaged.VoxelDefinitionDataBase, shapeDefinitionDataBase),
                MagicWandInventory = new MagicWandInventory(dataBaseManaged.MagicWandDataBase),
                CommonInventoryInLattice = new CommonInventory(4, 9),
                CommonInventoryInBar = new CommonInventory(1, 9),
                SyncECBCreator = new(),
            };
            voxelWorldGameCassette.UiCallDataFuctions = new UiCallDataFuctions(voxelWorldGameCassette);
            return voxelWorldGameCassette;
        }
        public override void Dispose()
        {
            base.Dispose();
            //VoxelWorldDataBaseManaged.Dispose();// 其释放有ECS执行
            for (int i = 0; i < asyncOperationHandles.Count; i++)
            {
                Addressables.Release(asyncOperationHandles[i]);
            }
            ArchivalData.Dispose();
        }
        void LoadArchive()
        {

            IVoxelDefinitionDataBase voxelDefinitionDataBase = VoxelWorldDataBaseManaged.VoxelDefinitionDataBase;
            IShapeDefinitionDataBase shapeDefinitionDataBase = VoxelWorldDataBaseManaged.ShapeDefinitionDataBase;
            IMagicWandDataBase magicWandDataBase = VoxelWorldDataBaseManaged.MagicWandDataBase;

            // 数据的读取,可以在Enter之前处理,然后再设置状态
#if UNITY_EDITOR
            IVoxelShapeInfo defaultShape = shapeDefinitionDataBase.GetDefaultVoxelShapeInfo();
            IReadOnlyList<IVoxelItemInfo> voxelItemInfos = voxelDefinitionDataBase.GetClassifyVoxelItemInfos("方块");
            if (voxelItemInfos != null)
            {
                for (int i = 0; i < voxelItemInfos.Count && i < CommonInventoryInLattice.Count; i++)
                {
                    var def = voxelItemInfos[i];
                    CommonInventoryInLattice.SetItem(new VoxelItemStorage() { VoxelItemInfo = def, VoxelShapeInfo = defaultShape, itemCount = 100000 }, i);
                    TotalVoxelItemInventory.Add(def.ID, 100000);
                }
                for (int i = 0; i < voxelItemInfos.Count && i < CommonInventoryInBar.Count; i++)
                {
                    var def = voxelItemInfos[i];
                    CommonInventoryInBar.SetItem(new VoxelItemStorage() { VoxelItemInfo = def, VoxelShapeInfo = defaultShape, itemCount = 100000 }, i);
                }
            }
            var voxelCommandPools = MagicWandInventory.VoxelCommandPools;

            VoxelMaterialSetMagic water = new VoxelMaterialSetMagic(voxelCommandPools) { VoxelMaterial = VoxelMaterial.Water };
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "水魔杖") { [0] = water }));

            VoxelMaterialSetMagic fire = new VoxelMaterialSetMagic(voxelCommandPools) { VoxelMaterial = VoxelMaterial.Blaze };
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "火魔杖") { [0] = fire }));

            VoxelMaterialReplaceMagic waterBucket = new VoxelMaterialReplaceMagic(voxelCommandPools) { VoxelMaterial = VoxelMaterial.Water };
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "水桶") { [0] = waterBucket, }));

            VoxelMaterialReplaceMagic fireBucket = new VoxelMaterialReplaceMagic(voxelCommandPools) { VoxelMaterial = VoxelMaterial.Blaze };
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "火桶") { [0] = fireBucket, }));

            PutVoxelReplaceBixel putVoxelReplaceBixel = new PutVoxelReplaceBixel(VoxelWorldDataBaseManaged.BixelDataBase, SyncECBCreator)
            {
                Delay = 1f,
                VoxelReplace = new VoxelReplace()
                {

                },
            };
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "体素置换") { [0] = putVoxelReplaceBixel }));

            PutSwapVoxelBixel putSwapVoxelBixel = new PutSwapVoxelBixel(VoxelWorldDataBaseManaged.BixelDataBase, SyncECBCreator)
            {
                Delay = 1f,
                VoxelSwap = new VoxelSwap()
                {
                    CheckMask = VoxelMath.VoxelMask(VoxelMaskTag.Material),
                    SwapMask = VoxelMath.VoxelMask(VoxelMaskTag.ShapeDiretion),
                    Point1 = BixelCheckPoint.Front,
                    Point2 = BixelCheckPoint.Back,
                }
            };
            //ConsoleCat.Log("voxelswap" + (putSwapVoxelBixel.VoxelSwap.CheckMask & new Voxel(0, VoxelMaterial.Blaze)));
            MagicWandInventory.AddItem(new MagicWandStorage(new MagicWand(1, magicWandDataBase[0], "体素交换") { [0] = putSwapVoxelBixel }));
#endif
        }
        #region 游戏流程事件
        protected override void InternalEnter(VoxelWorldManager owner)
        {
            LoadArchive();

            var ECB = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BeginPresentationEntityCommandBufferSystem>().CreateCommandBuffer();
            ECB.AddComponent<VoxelWorldTag>(voxelWorldEntity, new VoxelWorldTag());
            ECB.AddComponent<Archival>(voxelWorldEntity, ArchivalData.Archival);

            InputManagerMiao.CursorState = CursorLockMode.Locked;
        }
        protected override void InternalExit(VoxelWorldManager owner)
        {
            var ECB = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BeginPresentationEntityCommandBufferSystem>().CreateCommandBuffer();
            ECB.RemoveComponent<VoxelWorldTag>(voxelWorldEntity);
            ECB.RemoveComponent<Archival>(voxelWorldEntity);

            InputManagerMiao.CursorState = CursorLockMode.None;
        }
        protected override void InternalPause(VoxelWorldManager owner)
        {
            InputManagerMiao.CursorState = CursorLockMode.None;
        }
        protected override void InternalContinue(VoxelWorldManager owner)
        {
            InputManagerMiao.CursorState = CursorLockMode.Locked;
        }
        #endregion
    }
}