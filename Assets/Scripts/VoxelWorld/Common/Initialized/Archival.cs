using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public readonly struct Archival : IComponentData
    {
        public struct Data
        {
            public int2 InitialWorldCenterChunkIndex;
        }
        public readonly BlobAssetReference<Data> ArchiveDataRef;
        public Archival(BlobAssetReference<Data> dataRef)
        {
            ArchiveDataRef = dataRef;
        }
    }
}
