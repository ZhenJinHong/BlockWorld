using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New FlatFiller", menuName = "ECSVoxelWorld/Filler/Flat")]
    public class FlatFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] FlatFillerBuilder builder = new FlatFillerBuilder();
        [SerializeField] VoxelDefinition[] layerVoxels; 
        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            builder.Name = name;
            builder.LayerVoxels = dataBase.VoxelDefinitionDataBase.VoxelDefToVoxel(layerVoxels);
            return builder.Create(seed, baseHeight, dataBase);
        }
    }
}