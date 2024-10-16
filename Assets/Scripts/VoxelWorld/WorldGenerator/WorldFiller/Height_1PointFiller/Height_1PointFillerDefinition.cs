using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New Height_1PointFiller", menuName = "ECSVoxelWorld/Filler/Height_1Point")]
    public class Height_1PointFillerDefinition : BaseWorldFillerDefinition
    {
        [SerializeField] Height_1PointFillerBuilder builder = new Height_1PointFillerBuilder();
        [SerializeField] VoxelDefinition toPut;
        public override IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = dataBase.VoxelDefinitionDataBase;
            builder.Name = name;
            builder.Toput = voxelDefinitionDataBase.GetVoxel(toPut);
            return builder.Create(seed, baseHeight, dataBase);
        }
    }
}
