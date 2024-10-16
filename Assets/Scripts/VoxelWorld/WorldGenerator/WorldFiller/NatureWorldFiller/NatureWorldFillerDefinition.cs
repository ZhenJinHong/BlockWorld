using System.Collections;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New NatureWorldFiller", menuName = "ECSVoxelWorld/Filler/NatureWorld")]
    public class NatureWorldFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] NatureWorldFillerBuilder builder = new NatureWorldFillerBuilder();
        [SerializeField] VoxelDefinition[] blocks;
        [SerializeField] VoxelDefinition[] underWaterBlocks;
        [SerializeField] VoxelDefinition[] surfaceBlocks;
        [SerializeField] VoxelDefinition[] grasss;
        [SerializeField] VoxelDefinition[] flowers;
        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = dataBase.VoxelDefinitionDataBase;
            builder.Name = name;
            builder.Blocks = voxelDefinitionDataBase.VoxelDefToVoxel(blocks);
            builder.UnderWaterBlocks = voxelDefinitionDataBase.VoxelDefToVoxel(underWaterBlocks);
            builder.SurfaceBlocks = voxelDefinitionDataBase.VoxelDefToVoxel(surfaceBlocks);
            builder.Grasss = voxelDefinitionDataBase.VoxelDefToVoxel(grasss);
            builder.Flowers = voxelDefinitionDataBase.VoxelDefToVoxel(flowers);

            return builder.Create(seed, baseHeight);
        }
    }
}