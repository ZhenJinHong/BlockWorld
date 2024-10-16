using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public struct BixelDataBase : IComponentData
    {
        public Entity BixelPrefab;
    }
    public interface IBixelDefinition
    {
        string Name { get; }
    }
    public interface IBixelPrefab
    {
        string Name { get; }
        public Entity Prefab { get; }
    }

    public abstract class BixelDefinition : ScriptableObject, IBixelDefinition
    {
        public string Name => name;
        public abstract ComponentType GetComponentType();
    }
    public class BixelVoxelReplaceDefinition
    {

    }
}