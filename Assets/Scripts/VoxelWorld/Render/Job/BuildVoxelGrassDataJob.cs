using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct BuildVoxelGrassDataJob : IJob
    {
        [ReadOnly] public VoxelWorldMap.SmallChunkSliceReadyOnly SmallChunkSlice;
        public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
        public NativeReference<AABB> aabb;
        public NativeList<float3> verts;
        public NativeList<ushort> indexs;
        public NativeList<float3> fireVerts;
        public NativeList<ushort> fireIndexs;
        public void Execute()
        {
            ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;
            int3 min = 0, max = 0;
            // 首先这个是索引
            int x = 0, y = 0, z = 0;// 需要从指定的小区块位置开始
            for (int voxelArrayIndex = 0; voxelArrayIndex < Settings.VoxelCapacityInSmallChunk; voxelArrayIndex++)
            {
                Voxel voxel = SmallChunkSlice[voxelArrayIndex];
                if (voxel != Voxel.Null)
                {
                    int3 voxelPosInSmallChunk = new int3(x, y, z);
                    min = math.min(min, voxelPosInSmallChunk);
                    max = math.max(max, voxelPosInSmallChunk);
                }
                if (Voxel.NonAir(voxel.VoxelTypeIndex))
                {
                    ref VoxelType voxelType = ref voxelTypes[voxel.VoxelTypeIndex];
                    if (voxelType.VoxelRenderType == VoxelRenderType.Grass)
                    {
                        indexs.Add((ushort)(verts.Length));
                        verts.Add(new float3(VectorCompress.CompressByte3ToFloat(x, y, z), voxelType.TextureIndex, z));
                    }
                }
                if (Voxel.Fire(voxel.VoxelMaterial))
                {
                    fireIndexs.Add((ushort)fireVerts.Length);
                    fireVerts.Add(new float3(VectorCompress.CompressByte3ToFloat(x, y, z), 0.0f, 0.0f));
                }
                z++;
                if (z == Settings.SmallChunkSize)
                {
                    z = 0;
                    x++;
                    if (x == Settings.SmallChunkSize)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
            max += 1;// max以体素原点计算，此时需要加多1
            float3 extents = new float3(max - min) * 0.5f;
            AABB aabb = new AABB()
            {
                Center = min + extents,
                Extents = extents,
            };
            this.aabb.Value = aabb;
        }
    }
}
