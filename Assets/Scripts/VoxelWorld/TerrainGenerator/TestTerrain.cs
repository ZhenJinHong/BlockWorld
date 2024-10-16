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
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    [CreateAssetMenu(fileName = "New TestTerrain", menuName = "ECSVoxelWorld/TestTerrain")]
//    public class TestTerrain : TerrainBaseClass
//    {
//        [SerializeField] VoxelDefinition One;
//        [SerializeField] VoxelDefinition Two;
//        [SerializeField] VoxelDefinition Three;
//        Voxel OneVoxel;
//        Voxel TwoVoxel;
//        Voxel ThreeVoxel;
//        public override void Initialize(VoxelWorldDataBaseManaged manager)
//        {
//            OneVoxel = manager.GetVoxel(One.VoxelName);
//            TwoVoxel = manager.GetVoxel(Two.VoxelName);
//            ThreeVoxel = manager.GetVoxel(Three.VoxelName);
//        }

//        public override JobHandle ScheduleGenerateHeightMapJob(BlobAssetReference<SeedData> seedData, BigChunkDataContainer bigChunkDataContainer)
//        {
//            return default;
//        }

//        public override JobHandle ScheduleGenerateVoxelJob(BlobAssetReference<SeedData> seedData, NativeArray<byte> HeightMap, SmallChunkDataContainer smallChunkDataContainer, JobHandle dependOn)
//        {
//            ref SeedData blobSeedData = ref seedData.Value;
//            if (smallChunkDataContainer.SmallChunkIndex.y == 0)
//            {
//                return new TestTerrainJob()
//                {
//                    Slice = smallChunkDataContainer.Voxels,
//                    SmallChunkPosInt = smallChunkDataContainer.SmallChunkPosInt,
//                    OneVoxel = OneVoxel,
//                    TwoVoxel = TwoVoxel,
//                    ThreeVoxel = ThreeVoxel,
//                    noiseOffset = blobSeedData.noiseOffset,
//                    noiseScale = blobSeedData.noiseScale
//                }.Schedule(dependOn);
//            }
//            return default;
//        }
//    }
//    [BurstCompile]
//    struct TestTerrainJob : IJob
//    {
//        public NativeArray<Voxel> Slice;
//        public int3 SmallChunkPosInt;
//        public Voxel OneVoxel;
//        public Voxel TwoVoxel;
//        public Voxel ThreeVoxel;
//        public float noiseOffset;
//        public float noiseScale;
//        public void Execute()
//        {
//            int x = 0, y = 0, z = 0;
//            for (int i = 0; i < Settings.VoxelCapacityInSmallChunk; i++)
//            {
//                int3 voxelPos = new int3(x, y, z);
//                Slice[VoxelMath.LocalVoxelArrayIndexInSmallChunk(voxelPos)] = GetVoxel(voxelPos + SmallChunkPosInt);
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
//        }
//        Voxel GetVoxel(int3 voxelPos)
//        {
//            if (math.all((voxelPos & 1) == 1))
//            {
//                float v = Noise.Get3DPerlinNoise(voxelPos, noiseOffset, noiseScale);
//                if (v < 0.3f)
//                    return OneVoxel;
//                if (v < 0.6f)
//                    return TwoVoxel;
//                return ThreeVoxel;
//            }

//            return Voxel.Empty;
//        }
//    }
//    [BurstCompile]
//    struct TestTerrainBuildJob : IJobParallelForBatch
//    {
//        public NativeSlice<Voxel> Slice;
//        public int3 bigChunkPosInt;
//        public Voxel OneVoxel;
//        public Voxel TwoVoxel;
//        public Voxel ThreeVoxel;
//        public float noiseOffset;
//        public float noiseScale;
//        public void Execute(int startIndex, int count)
//        {
//            int end = startIndex + count;
//            if (startIndex > 0)// 只要一层测试
//            {
//                return;
//            }
//            // 比如开始索引为8192，数量为4096，则除后为2，乘以小区块大小，为最终高度
//            int x = 0, y = startIndex / count * Settings.SmallChunkSize, z = 0;// 高度已经偏移到目标小区块
//            for (int i = startIndex; i < end; i++)
//            {
//                Slice[VoxelMath.LocalVoxelArrayIndexInBigChunk(x, y, z)] = GetVoxel(new int3(x, y, z) + bigChunkPosInt);
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
//        }
//        Voxel GetVoxel(int3 voxelPos)
//        {
//            if (math.all((voxelPos & 1) == 1))
//            {
//                float v = Noise.Get3DPerlinNoise(voxelPos, noiseOffset, noiseScale);
//                if (v < 0.3f)
//                    return OneVoxel;
//                if (v < 0.6f)
//                    return TwoVoxel;
//                return ThreeVoxel;
//            }

//            return Voxel.Empty;
//        }
//    }
//}
