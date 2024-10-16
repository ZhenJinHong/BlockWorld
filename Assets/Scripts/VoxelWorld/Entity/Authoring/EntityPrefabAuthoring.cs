//using System;
//using System.Collections;
//using Unity.Entities;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    [Obsolete]
//    public class EntityPrefabAuthoring : MonoBehaviour
//    {
//        class EntityPrefabBaker : Baker<EntityPrefabAuthoring>
//        {
//            public override void Bake(EntityPrefabAuthoring authoring)
//            {
//                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
//                AddComponent<Prefab>(entity);
//                BlobAssetReference<EntityData> entityData = EntityData.Create(authoring);
//                AddBlobAsset(ref entityData, out _);
//                AddComponent<VoxelWorldEntity>(entity, new VoxelWorldEntity()
//                {
//                    EntityData = entityData,
//                });
//            }
//        }
//    }
//}