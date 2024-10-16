using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    [Obsolete]
    public struct VoxelMeshFindingMap : IComponentData
    {
        public BlobAssetReference<CubeMeshFindingMapAsset> VoxelDataMap;
    }
}
