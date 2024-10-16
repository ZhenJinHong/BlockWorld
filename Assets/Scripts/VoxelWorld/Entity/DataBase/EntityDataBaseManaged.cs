using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public interface IEntityDataBase
    {
        Entity GetPrefab(Hash128 key);
        void GetPrefab(Hash128[] ids, NativeArray<Entity> array);
        Entity GetPrefab(string name);
        bool TryGetPrefab(Hash128 key, out Entity prefab);
    }
    public class EntityDataBaseManaged : IEntityDataBase
    {
        EntityQuery entityPrefabQuery;
        NativeHashMap<Unity.Entities.Hash128, Entity> PrefabMap;
        NativeHashMap<Unity.Entities.Hash128, BlobAssetReference<EntityData>> EntityDataMap;
        public EntityDataBase EntityDataBase => new EntityDataBase(PrefabMap);
        public void Dispose()
        {
            PrefabMap.Dispose();
            EntityDataMap.Dispose();
        }
        public EntityDataBaseManaged()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAll<VoxelWorldEntity, Prefab>();
            entityPrefabQuery = builder.Build(entityManager);

            int initCapacity = entityPrefabQuery.CalculateEntityCount();
            PrefabMap = new NativeHashMap<Hash128, Entity>(initCapacity, Allocator.Persistent);
            EntityDataMap = new NativeHashMap<Hash128, BlobAssetReference<EntityData>>(initCapacity, Allocator.Persistent);
            NativeArray<Entity> prefabs = entityPrefabQuery.ToEntityArray(Allocator.Temp);
            NativeArray<VoxelWorldEntity> voxelWorldEntities = entityPrefabQuery.ToComponentDataArray<VoxelWorldEntity>(Allocator.Temp);
            for (int i = 0; i < prefabs.Length; i++)
            {
                Entity entity = prefabs[i];
                VoxelWorldEntity voxelWorldEntity = voxelWorldEntities[i];
                var id = voxelWorldEntity.EntityData.Value.ID;
                PrefabMap.Add(id, entity);
                EntityDataMap.Add(id, voxelWorldEntity.EntityData);
            }
        }
        public bool TryGetPrefab(Unity.Entities.Hash128 key, out Entity prefab)
            => PrefabMap.TryGetValue(key, out prefab);
        public Entity GetPrefab(Unity.Entities.Hash128 key)
        {
            if (!PrefabMap.TryGetValue(key, out Entity prefab))
            {

            }
            return prefab;
        }
        public Entity GetPrefab(string name)
        {
            return GetPrefab(AuthoringExtension.Hash128(name));
        }
        public void GetPrefab(Unity.Entities.Hash128[] ids, NativeArray<Entity> array)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (PrefabMap.TryGetValue(ids[i], out Entity prefab))
                    array[i] = prefab;
            }
        }
    }
}
