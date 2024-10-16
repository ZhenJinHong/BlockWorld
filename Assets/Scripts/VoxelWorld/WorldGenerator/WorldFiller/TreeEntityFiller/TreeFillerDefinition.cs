using CatFramework.Tools;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New TreeFiller", menuName = "ECSVoxelWorld/Filler/Tree")]
    public class TreeFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] TreeFillerBuilder builder = new TreeFillerBuilder();

        [SerializeField] VoxelDefinition trunk;
        [SerializeField] VoxelShapeDefinition trunkShape;
        [SerializeField] VoxelDefinition leave;
        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = dataBase.VoxelDefinitionDataBase;
            IShapeDefinitionDataBase shapeDefinitionDataBase = dataBase.ShapeDefinitionDataBase;
            builder.Name = name;
            builder.Trunk = voxelDefinitionDataBase.GetVoxel(trunk.VoxelName);
            builder.Trunk.ShapeIndex = shapeDefinitionDataBase.GetVoxelShapeIndex(trunkShape.Name);
            builder.Leave = voxelDefinitionDataBase.GetVoxel(leave.VoxelName);
            return builder.Create(seed, baseHeight, dataBase);
        }
    }
}
