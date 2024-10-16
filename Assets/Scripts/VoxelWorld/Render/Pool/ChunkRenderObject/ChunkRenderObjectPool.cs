using CatFramework;
using CatFramework.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public class ChunkRenderObjectPool : ForcedPool<ChunkRenderObject>
    {
        readonly VoxelWorldDataBaseManaged.IRenderProvider renderProvider;
        public ChunkRenderObjectPool(VoxelWorldDataBaseManaged.IRenderProvider renderProvider, int initCapacity, int maxCapacity) : base(initCapacity, maxCapacity)
        {
            this.renderProvider = renderProvider;
        }
        protected override ChunkRenderObject CreateElement()
        {
            return new ChunkRenderObject(renderProvider);
        }
    }
}