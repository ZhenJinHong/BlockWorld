using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelWorldDataBase : IComponentData
    {
        public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
        public BlobAssetReference<VoxelShapeBlobAsset> VoxelShapeBlobAsset;
        public VoxelWorldDataBase(BlobAssetReference<VoxelTypeAsset> voxelTypeDataBase, BlobAssetReference<VoxelShapeBlobAsset> voxelShapeBlobAsset)
        {
            VoxelTypeDataBase = voxelTypeDataBase;
            VoxelShapeBlobAsset = voxelShapeBlobAsset;
        }
    }
}
