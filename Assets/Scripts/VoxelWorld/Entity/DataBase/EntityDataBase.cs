using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public struct EntityDataBase : IComponentData
    {
        NativeHashMap<Unity.Entities.Hash128, Entity> prefabMap;
        public EntityDataBase(NativeHashMap<Unity.Entities.Hash128, Entity> prefabMap)
        {
            this.prefabMap = prefabMap;
        }
        public bool TryGetPrefab(Unity.Entities.Hash128 hash128, out Entity prefab)
        {
            return prefabMap.TryGetValue(hash128, out prefab);
        }
    }
}
