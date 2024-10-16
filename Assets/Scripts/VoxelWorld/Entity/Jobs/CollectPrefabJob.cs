//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;

//namespace CatDOTS.VoxelWorld
//{
//    [BurstCompile]
//    public partial struct CollectPrefabJob : IJobEntity
//    {
//        public NativeHashMap<Unity.Entities.Hash128, Entity> PrefabMap;
//        public NativeHashMap<Unity.Entities.Hash128, BlobAssetReference<EntityData>> EntityDataMap;
//        public void Execute(in Entity entity, in VoxelWorldEntity voxelWorldEntity, in Prefab prefab)
//        {
//            var id = voxelWorldEntity.EntityData.Value.ID;
//            PrefabMap.Add(id, entity);
//            EntityDataMap.Add(id, voxelWorldEntity.EntityData);
//        }
//    }
//}
