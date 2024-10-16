//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    [CreateAssetMenu(fileName = "New TreeEntityFiller", menuName = "ECSVoxelWorld/EntityFiller/TreeEntity")]
//    public class TreeEntityFillerDefinition : BaseEntityFillerDefinition
//    {
//        [SerializeField] TreeEntityFillerBuilder builder = new TreeEntityFillerBuilder();
//        [SerializeField] string toPut;
//        public override IEntityFiller Create(uint seed, VoxelWorldDataBaseManaged dataBase)
//        {
//            IEntityDataBase entityDataBase = dataBase.EntityDataBase;
//            builder.Name = Name;
//            builder.Toput = entityDataBase.GetPrefab(toPut);
//            return builder.Create(seed, dataBase);
//        }
//    }
//}
