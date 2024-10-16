//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;

//namespace CatDOTS.VoxelWorld
//{
//    [BurstCompile]
//    public struct TreeEntityFillJob : IJob
//    {
//        public Entity Toput;
//        [ReadOnly] public VoxelWorldMap.BigChunkSliceReadOnly BigChunkSlice;
//        public EntityCommandBuffer ECB;
//        public float RangeScale;
//        public float PutScale;
//        public float RangeThreshold;
//        public float PutThreShold;
//        public void Execute()
//        {
//            int2 BigChunkPos = BigChunkSlice.BigChunkPosInt.As2D();
//            for (int z = 0; z < Settings.SmallChunkSize; z++)
//            {
//                for (int x = 0; x < Settings.SmallChunkSize; x++)
//                {
//                    float2 pos = new float2(x, z) + BigChunkPos;
//                    if (Noise.GetPerlinNoise(pos * RangeScale) > RangeThreshold && Noise.GetPerlinNoise(pos * PutScale) > PutThreShold)
//                    {
//                        for (int y = Settings.WorldHeightInVoxel - 8; y > 0; y--)
//                        {
//                            Voxel voxel = BigChunkSlice[VoxelMath.LocalVoxelArrayIndexInBigChunk(x, y, z)];
//                            if (Voxel.NonAir(voxel.VoxelTypeIndex))
//                            {
//                                if (Voxel.Water(voxel.VoxelMaterial))
//                                {
//                                    break;
//                                }
//                                Entity tree = ECB.Instantiate(Toput);
//                                ECB.SetComponent<LocalTransform>(tree, new LocalTransform()
//                                {
//                                    Position = new float3(x + BigChunkPos.x, y, z + BigChunkPos.y),
//                                    Rotation = quaternion.RotateY(x + y - z),
//                                    Scale = 1f,
//                                });
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
