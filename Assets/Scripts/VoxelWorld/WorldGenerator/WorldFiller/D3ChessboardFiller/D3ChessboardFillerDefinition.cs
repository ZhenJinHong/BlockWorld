using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New D3ChessboardFiller", menuName = "ECSVoxelWorld/Filler/D3Chessboard")]
    public class D3ChessboardFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] D3ChessboardFillerBuilder builder = new D3ChessboardFillerBuilder();
        [SerializeField] VoxelVariantDefinition voxel1;
        [SerializeField] VoxelVariantDefinition voxel2;
        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = dataBase.VoxelDefinitionDataBase;
            builder.Name = name;
            builder.Voxel1 = voxelDefinitionDataBase.GetVoxel(voxel1);
            builder.Voxel2 = voxelDefinitionDataBase.GetVoxel(voxel2);
            return builder.Create(seed, baseHeight, dataBase);
        }
    }
}