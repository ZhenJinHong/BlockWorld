using CatFramework;
using CatFramework.Tools;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatDOTS.VoxelWorld
{
    public class ChunkRenderObject : IPoolItem
    {
        readonly BlockRendererPoolUnion blockRendererPoolUnion;
        readonly WaterRendererPoolUnion waterRenderPoolUnion;
        readonly UnderLimitPointPoolUnion grassRendererPoolUnion;
        readonly UnderLimitPointPoolUnion fireRendererPoolUnion;
        public bool Active
        {
            set
            {
                blockRendererPoolUnion.Active = value;
                waterRenderPoolUnion.Active = value;
                grassRendererPoolUnion.Active = value;
                fireRendererPoolUnion.Active = value;
            }
        }
        public void SetChunkIndex(int3 chunkIndex)
        {
            this.chunkIndex = chunkIndex;
            SetPosition(chunkIndex * Settings.SmallChunkSize);
        }
        int3 chunkIndex;
        public int3 ChunkIndex => chunkIndex;
        void SetPosition(float3 position)
        {
            blockRendererPoolUnion.SetPosition(position);
            waterRenderPoolUnion.SetPosition(position);
            grassRendererPoolUnion.SetPosition(position);
            fireRendererPoolUnion.SetPosition(position);
        }
        void IPoolItem.Dispose()
        {
            Reset();
        }
        public ChunkRenderObject(VoxelWorldDataBaseManaged.IRenderProvider renderProvider)
        {
            blockRendererPoolUnion = new BlockRendererPoolUnion(renderProvider)
            {
                RendererName = "Block",
            };
            waterRenderPoolUnion = new WaterRendererPoolUnion(renderProvider)
            {
                RendererName = "Water",
            };

            grassRendererPoolUnion = new UnderLimitPointPoolUnion(renderProvider)
            {
                RendererName = "Grass",
                material = renderProvider.GrassMaterial,
                ShadowCastingMode = ShadowCastingMode.On,
            };
            fireRendererPoolUnion = new UnderLimitPointPoolUnion(renderProvider)
            {
                RendererName = "Fire",
                material = renderProvider.FireMaterial,
                ShadowCastingMode = ShadowCastingMode.Off,
            };

            Active = false;
        }
        public void SetMeshData(MeshDataContainer container)
        {
            blockRendererPoolUnion.SetMeshData(container);
            waterRenderPoolUnion.SetMeshData(container);

            grassRendererPoolUnion.SetMeshData(container.grass, container);
            fireRendererPoolUnion.SetMeshData(container.fire, container);
        }
        public void ClearMeshData()
        {
            Reset();
        }
        public void Reset()
        {
            blockRendererPoolUnion.Repaid();
            waterRenderPoolUnion.Repaid();
            grassRendererPoolUnion.Repaid();
            fireRendererPoolUnion.Repaid();
        }
    }
}