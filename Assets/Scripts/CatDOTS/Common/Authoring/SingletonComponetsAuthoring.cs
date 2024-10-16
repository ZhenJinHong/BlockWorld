using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS
{
    public sealed class SingletonComponetsAuthoring : MonoBehaviour
    {
        sealed class SingletonComponentsBaker : Baker<SingletonComponetsAuthoring>
        {
            public override void Bake(SingletonComponetsAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent<SingletonSystemEntity>(entity);
            }
        }
    }
}