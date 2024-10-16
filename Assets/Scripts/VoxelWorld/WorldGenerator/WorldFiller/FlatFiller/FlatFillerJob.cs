using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct FlatFillerJob : IJob
    {
        [ReadOnly] public NativeArray<Voxel> LayerVoxels;
        public NativeArray<Voxel> Voxels;
        public void Execute()
        {
            //for (int i = 0; i < Voxels.Length; i++)
            //{
            //    Voxels[i] = Voxel.Empty;
            //}// 不适合直接放在这里,如果需要清空,单独给世界生成器放一个清空工作
            int x = 0, y = 0, z = 0;
            int end = Settings.VoxelCountInFloor * math.min(LayerVoxels.Length, Settings.WorldHeightInVoxel);
            for (int voxelIndex = 0; voxelIndex < end; voxelIndex++)
            {
                Voxels[voxelIndex] = LayerVoxels[y];
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
        }
    }
}