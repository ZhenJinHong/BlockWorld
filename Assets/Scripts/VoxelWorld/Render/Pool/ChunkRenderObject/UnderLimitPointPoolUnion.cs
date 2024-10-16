using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatDOTS.VoxelWorld
{
    public class UnderLimitPointPoolUnion : ChunkRendererPoolUnion
    {
        public Material material;
        public ShadowCastingMode ShadowCastingMode;
        public UnderLimitPointPoolUnion(VoxelWorldDataBaseManaged.IRenderProvider renderProvider) : base(renderProvider)
        {
        }
        public void SetMeshData(MeshDataContainer.PointMeshData pointMeshData, MeshDataContainer container)
        {
            if (pointMeshData.NonEmpty)
            {
                ChunkRenderer chunkRenderer = GetOrClear();
                Mesh mesh = chunkRenderer.Mesh;
                NativeArray<float3> verts = pointMeshData.verts.AsArray();
                NativeArray<ushort> indexs = pointMeshData.indexs.AsArray();
                MeshUpdateFlags meshUpdateFlags = MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;
                mesh.SetVertices<float3>(verts, 0, verts.Length, meshUpdateFlags);
                mesh.SetIndices<ushort>(indexs, MeshTopology.Points, 0, false, 0);
                mesh.bounds = container.aabb.Value.ToBounds();
                mesh.MarkModified();
            }
            else
            {
                Repaid();
            }
        }

        protected override void SetChunkRendererSetting(ChunkRenderer chunkRenderer)
        {
            MeshRenderer meshRenderer = chunkRenderer.MeshRenderer;
            meshRenderer.sharedMaterial = material;
            meshRenderer.shadowCastingMode = ShadowCastingMode;
        }
    }
}