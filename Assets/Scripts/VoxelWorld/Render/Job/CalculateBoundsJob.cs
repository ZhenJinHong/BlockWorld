//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    [BurstCompile]
//    public struct CalculateBoundsJob : IJob
//    {
//        [ReadOnly] public VoxelWorldMap.SmallChunkSliceReadyOnly SmallChunkSlice;
//        public NativeReference<AABB> aabb;
//        public void Execute()
//        {
//            int3 min = 0, max = 0;
//            // 首先这个是索引
//            int x = 0, y = 0, z = 0;// 需要从指定的小区块位置开始

//            for (int voxelArrayIndex = 0; voxelArrayIndex < Settings.VoxelCapacityInSmallChunk; voxelArrayIndex++)
//            {
//                int3 voxelPosInSmallChunk = new int3(x, y, z);// 在当前大区块内的相对位置

//                Voxel voxel = SmallChunkSlice[voxelArrayIndex];
//                if (voxel != Voxel.Null)
//                {
//                    min = math.min(min, voxelPosInSmallChunk);
//                    max = math.max(max, voxelPosInSmallChunk);
//                }
//                z++;
//                if (z == Settings.SmallChunkSize)
//                {
//                    z = 0;
//                    x++;
//                    if (x == Settings.SmallChunkSize)
//                    {
//                        x = 0;
//                        y++;
//                    }
//                }
//            }
//            max += 1;// max以体素原点计算，此时需要加多1
//            float3 extents = new float3(max - min) * 0.5f;
//            AABB aabb = new AABB()
//            {
//                Center = min + extents,
//                Extents = extents,
//            };
//            this.aabb.Value = aabb;
//        }
//    }
//}
