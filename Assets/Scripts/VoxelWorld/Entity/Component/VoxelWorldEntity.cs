using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public struct EntityData
    {
        public Unity.Entities.Hash128 ID;
        public static BlobAssetReference<EntityData> Create(MonoBehaviour monoBehaviour)
        {
            return BlobAssetReference<EntityData>.Create(new EntityData()
            {
                ID = AuthoringExtension.Hash128(monoBehaviour.name),
            });
        }
    }
    public struct VoxelWorldEntity : IComponentData
    {
        public BlobAssetReference<EntityData> EntityData;
    }
}
