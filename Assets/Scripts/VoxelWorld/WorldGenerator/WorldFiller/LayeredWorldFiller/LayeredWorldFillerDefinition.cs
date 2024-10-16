using CatFramework.Tools;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New LayeredWorldFiller", menuName = "ECSVoxelWorld/Filler/LayeredWorld")]
    public class LayeredWorldFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] LayeredWorldFillerBuilder builder = new LayeredWorldFillerBuilder();

        [SerializeField] VoxelDefinition[] blocks;
        //[SerializeField] VoxelDefinition[] underWaterBlocks;
        [SerializeField] VoxelDefinition[] surfaces;
        [SerializeField] VoxelDefinition[] grasss;
        [SerializeField] VoxelDefinition[] flowers;

        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = dataBase.VoxelDefinitionDataBase;
            builder.Name = name;
            builder.Blocks = voxelDefinitionDataBase.VoxelDefToVoxel(blocks);
            //builder.UnderWaterBlocks = voxelDefinitionDataBase.VoxelDefToVoxel(underWaterBlocks);
            builder.SurfaceBlocks = voxelDefinitionDataBase.VoxelDefToVoxel(surfaces);
            builder.Grasss = voxelDefinitionDataBase.VoxelDefToVoxel(grasss);
            builder.Flowers = voxelDefinitionDataBase.VoxelDefToVoxel(flowers);
            return builder.Create(seed, baseHeight);
        }
    }
}