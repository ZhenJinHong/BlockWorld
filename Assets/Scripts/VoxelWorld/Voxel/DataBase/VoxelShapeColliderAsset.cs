using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace CatDOTS.VoxelWorld
{
    public readonly struct VoxelShapeColliderAsset : IComponentData
    {
        public readonly NativeArray<BlobAssetReference<Collider>>.ReadOnly SolidColliderAsset;
        public readonly BlobAssetReference<Collider> NonSolidCube;
        public VoxelShapeColliderAsset(BlobAssetReference<Collider> nonSolidCube, NativeArray<BlobAssetReference<Collider>>.ReadOnly solidColliderAsset)
        {
            NonSolidCube = nonSolidCube;
            SolidColliderAsset = solidColliderAsset;
        }
    }
}
