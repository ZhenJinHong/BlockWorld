using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS
{
    public class PrefabAuthoring : MonoBehaviour
    {
        class PrefabBaker : Baker<PrefabAuthoring>
        {
            public override void Bake(PrefabAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent<Prefab>(entity);
            }
        }
    }
}