using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatDOTS.VoxelWorld
{
    public class BlockRendererPoolUnion : ChunkRendererPoolUnion
    {
        Material[] materials;

        public BlockRendererPoolUnion(VoxelWorldDataBaseManaged.IRenderProvider renderProvider) : base(renderProvider)
        {
            materials = new Material[] { renderProvider.OpaqueMaterial };
        }
        protected override void SetChunkRendererSetting(ChunkRenderer chunkRenderer)
        {
            chunkRenderer.MeshRenderer.shadowCastingMode = ShadowCastingMode.On;
        }

        public void SetMeshData(MeshDataContainer container)
        {
            MeshDataContainer.Block block = container.block;
            if (block.NonEmpty)
            {
                ChunkRenderer chunkRenderer = GetOrClear();
                Mesh mesh = chunkRenderer.Mesh;
                MeshRenderer meshRenderer = chunkRenderer.MeshRenderer;
                // 在设置uv和三角形的时候，默认计算包围，但是实际上却也没有计算包围盒
                // 只有设置顶点的时候，默认计算了

                NativeArray<float3> vertices = block.vertices.AsArray();

                NativeArray<float4> uvs = block.uvs.AsArray();
                NativeArray<SubMeshRange> subMeshRanges = block.subMeshRanges.AsArray();
                NativeArray<ushort> triangles = block.triangles.AsArray();
                NativeArray<float3> normals = block.normals.AsArray();

                MeshUpdateFlags meshUpdateFlags = MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;

                mesh.SetVertices<float3>(vertices, 0, vertices.Length, meshUpdateFlags);
                mesh.SetUVs<float4>(0, uvs, 0, uvs.Length, meshUpdateFlags);
                mesh.SetNormals<float3>(normals);
                mesh.subMeshCount = subMeshRanges.Length;
                //if (renderer.materials.Length != subMeshRanges.Length)// 读取会导致材质实例化,所以应该选择直接复制
                //{

                //}
                if (materials.Length != subMeshRanges.Length)
                    materials = new Material[subMeshRanges.Length];

                for (int b = 0; b < subMeshRanges.Length; b++)
                {
                    SubMeshRange subMeshRange = subMeshRanges[b];
                    materials[b] = subMeshRange.RenderType switch
                    {
                        VoxelRenderType.OpaqueBlock => renderProvider.OpaqueMaterial,
                        VoxelRenderType.TransparentBlock => renderProvider.TransparentMaterial,
                        _ => renderProvider.OpaqueMaterial
                    };

                    int start = subMeshRange.TriangleStartIndex;
                    int length = subMeshRange.TriangleRangeLength;
                    int baseVertexIndex = subMeshRange.BaseVertexIndex;

                    mesh.SetIndices<ushort>(triangles, start, length, MeshTopology.Triangles, b, false, baseVertexIndex);
                }
                meshRenderer.sharedMaterials = materials;
                mesh.bounds = container.aabb.Value.ToBounds();
                mesh.MarkModified();
            }
            else
            {
                Repaid();
            }
        }
    }
}