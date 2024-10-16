using CatDOTS.VoxelWorld.Magics;
using CatFramework;
using CatFramework.Magics;
using CatFramework.SLMiao;
using CatFramework.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelItemStorage : IMagicEnergy
    {
        long ItemCount { get; }
        Voxel Voxel { get; }
        VoxelType VoxelType { get; }
        IVoxelItemInfo VoxelItemInfo { get; }
        IVoxelShapeInfo VoxelShapeInfo { get; }

        bool CountEnough(long need);
        void Decrease(long count);
        void Increase(long count);
    }
    public class VoxelWorldDataBaseManaged : IDisposable, IComponentData
    {
        class WorldSetting : IVoxelWorldSetting
        {
            public int MaxMemoryMB { get; set; }
            public int LoadBigChunkPerFrame { get; set; }
            public int UpdateMeshPerFrame { get; set; }
            public int RenderDistance { get; set; }
            public int LoadingDistance { get; set; }
            public int UnloadingDistance { get; set; }
            public int UnloadingRange { get; set; }
            public int ChunkRenderObjectPoolCapacity { get; set; }
        }
        /// <summary>
        /// 范围为全长,距离为一半
        /// </summary>
        public interface IVoxelWorldSetting
        {
            int MaxMemoryMB { get; }
            int LoadBigChunkPerFrame { get; }
            int UpdateMeshPerFrame { get; }
            int RenderDistance { get; }
            int LoadingDistance { get; }
            int UnloadingDistance { get; }
            int UnloadingRange { get; }
            int ChunkRenderObjectPoolCapacity { get; }
        }
        public interface IDebugSetting
        {
            bool ShowNewDirtyChunkInfo { get; }
        }
        public interface IRenderProvider
        {
            Transform ChunkParent { get; }
            Material OpaqueMaterial { get; }
            Material TransparentMaterial { get; }
            Material WaterMaterial { get; }
            Material FireMaterial { get; }
            Material GrassMaterial { get; }
            Pool<ChunkRenderer> ChunkRenderPool { get; }
        }
        class RenderProvider : IRenderProvider
        {
            public override string ToString()
            {
                return $"区块父级其子级数量 : {chunkParent.childCount} ; \n {ChunkRenderPool}";
            }
            readonly Transform chunkParent;
            public Material opaqueMaterial;
            public Material transparentMaterial;
            public Material waterMaterial;
            public Material fireMaterial;
            public Material grassMaterial;
            public Transform ChunkParent => chunkParent;
            public Material OpaqueMaterial => opaqueMaterial;
            public Material TransparentMaterial => transparentMaterial;
            public Material WaterMaterial => waterMaterial;
            public Material FireMaterial => fireMaterial;
            public Material GrassMaterial => grassMaterial;
            public Pool<ChunkRenderer> ChunkRenderPool { get; private set; }
            public RenderProvider(GameObject chunkPrefab)
            {
                GameObject ChunkParent = new GameObject("ChunkParent");
                UnityEngine.Object.DontDestroyOnLoad(ChunkParent);
                chunkParent = ChunkParent.transform;

                ChunkRenderPool = new Pool<ChunkRenderer>(new ChunkRendererProvider(chunkPrefab, chunkParent), 128, 4096);
            }
        }
        public void GetInfo(StringBuilder stringBuilder)
        {
            Serialization.ObjectPropertyToString(VoxelWorldSetting, stringBuilder);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(VoxelWorldRenderProvider.ToString());
        }
        public IRenderProvider VoxelWorldRenderProvider { get; }
        WorldSetting worldSetting;
        IVoxelWorldSetting voxelWorldSetting;
        public IVoxelWorldSetting VoxelWorldSetting
        {
            get
            {
                if (worldSetting == null)
                {
                    worldSetting = new WorldSetting();
                    Serialization.CopyPropertyWithObjectTypeDifferent(voxelWorldSetting, worldSetting);
                }
                return worldSetting;
            }
        }
        public IDebugSetting DebugSetting { get; }
        public IVoxelWorldGameArchivalData VoxelWorldGameArchivalData { get; }
        //public WorldChunkSetting WorldChunkSetting { get; }
        private EntityDataBaseManaged entityDataBaseManaged;
        private VoxelDefinitionDataBase voxelDefinitionDataBase;
        private ShapeDefinitionDataBase shapeDefinitionDataBase;
        private MagicWandDataBase magicWandDataBase;
        private BixelDataBaseManaged bixelDataBaseManaged;
        public void Dispose()
        {
            entityDataBaseManaged.Dispose();
            shapeDefinitionDataBase.Dispose();
            voxelDefinitionDataBase.Dispose();
            magicWandDataBase.Dispose();
            if (ConsoleCat.Enable)
                ConsoleCat.Log($"释放了体素世界数据库");
        }
        public IEntityDataBase EntityDataBase => entityDataBaseManaged;
        public IVoxelDefinitionDataBase VoxelDefinitionDataBase => voxelDefinitionDataBase;
        public IShapeDefinitionDataBase ShapeDefinitionDataBase => shapeDefinitionDataBase;
        public IMagicWandDataBase MagicWandDataBase => magicWandDataBase;
        public IBixelDataBase BixelDataBase => bixelDataBaseManaged;
        public VoxelWorldDataBase VoxelWorldDataBase
            => new VoxelWorldDataBase(voxelDefinitionDataBase.VoxelTypeDataBase, shapeDefinitionDataBase.VoxelShapeBlobAsset);
        public VoxelWorldDataBaseManaged() { }
        public VoxelWorldDataBaseManaged(DataBaseDefinition dataBase, IVoxelWorldGameArchivalData voxelWorldGameArchivalData, VoxelWorldSettings voxelWorldSettings, TempDataBase tempDataBase)
        {
            VoxelWorldRenderProvider = new RenderProvider(dataBase.blockObjectPrefab)
            {
                opaqueMaterial = dataBase.opaqueMaterial,
                transparentMaterial = dataBase.transparentMaterial,
                waterMaterial = dataBase.waterMaterial,
                fireMaterial = dataBase.fireMaterial,
                grassMaterial = dataBase.grassMaterial,
            };

            voxelWorldSetting = voxelWorldSettings.VoxelWorldSetting;
            DebugSetting = voxelWorldSettings.DebugSetting;
            //WorldChunkSetting = voxelWorldSettings.WorldChunkSetting;

            VoxelWorldGameArchivalData = voxelWorldGameArchivalData;

            entityDataBaseManaged = new EntityDataBaseManaged();
            shapeDefinitionDataBase = new ShapeDefinitionDataBase(dataBase, tempDataBase.VoxelShapeDefinitions);
            voxelDefinitionDataBase = new VoxelDefinitionDataBase(dataBase, tempDataBase.VoxelDefinitions);
            magicWandDataBase = new MagicWandDataBase(tempDataBase.MagicWandDefinitions);
            bixelDataBaseManaged = new BixelDataBaseManaged(dataBase.bixelDataBaseDefinition, voxelDefinitionDataBase);
        }
    }

}
