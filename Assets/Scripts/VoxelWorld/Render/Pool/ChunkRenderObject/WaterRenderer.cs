using CatFramework.Tools;
using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatDOTS.VoxelWorld
{
    public class WaterRendererPoolUnion : ChunkRendererPoolUnion
    {
        Material[] materials;
        public WaterRendererPoolUnion(VoxelWorldDataBaseManaged.IRenderProvider renderProvider) : base(renderProvider)
        {
            materials = new Material[1]
            {
                renderProvider.WaterMaterial,
            };
        }

        protected override void SetChunkRendererSetting(ChunkRenderer chunkRenderer)
        {
            chunkRenderer.MeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        public void SetMeshData(MeshDataContainer container)
        {
            MeshDataContainer.PointMeshData water = container.water;

            if (water.NonEmpty)
            {
                ChunkRenderer chunkRenderer = GetOrClear();
                Mesh mesh = chunkRenderer.Mesh;
                MeshRenderer meshRenderer = chunkRenderer.MeshRenderer;

                NativeArray<float3> waterVerts = water.verts.AsArray();
                NativeArray<ushort> waterIndexs = water.indexs.AsArray();

                MeshUpdateFlags meshUpdateFlags = MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;

                mesh.SetVertices(waterVerts, 0, waterVerts.Length, meshUpdateFlags);
                int totalSubmeshCount = waterIndexs.Length > 65536 ? 2 : 1;

                mesh.subMeshCount = totalSubmeshCount;
                if (materials.Length != totalSubmeshCount)
                {
                    materials = new Material[totalSubmeshCount];
                }
                // 假设索引数组长度65538;
                // 子网格数则2;
                int currentSubMeshIndex = 0;
                for (; currentSubMeshIndex < totalSubmeshCount; currentSubMeshIndex++)
                {
                    materials[currentSubMeshIndex] = renderProvider.WaterMaterial;

                    int start = currentSubMeshIndex * 65536;
                    int length = math.min(waterIndexs.Length - start, 65536);
                    // 第二轮,start 为65536,length 为2 basevertexIndex 为 65536
                    mesh.SetIndices(waterIndexs, start, length, MeshTopology.Points, currentSubMeshIndex, false, start);
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