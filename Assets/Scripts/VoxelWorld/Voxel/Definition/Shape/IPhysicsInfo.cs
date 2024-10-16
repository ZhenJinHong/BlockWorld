using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IPhysicsInfo
    {
        float BevelRadius { get; }
        Vector3 Center { get; }
        Vector3 Size { get; }
        Vector3 Angle { get; }
        ColliderType ColliderType { get; }
    }
}