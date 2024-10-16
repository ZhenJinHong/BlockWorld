using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct D3ChessboardFillerJob : IJob
    {
        public NativeArray<Voxel> Voxels;
        public Voxel Voxel1;
        public Voxel Voxel2;
        public int FloorCount;
        public int3 BigChunkPos;
        public void Execute()
        {
            int x = 0, y = 0, z = 0;
            int end = Settings.VoxelCountInFloor * math.clamp(FloorCount, 1, Settings.SmallChunkSize);
            for (int voxelIndex = 0; voxelIndex < end; voxelIndex++)
            {
                Voxels[voxelIndex] = ((y & 1) == 1 ? (z & 1) == (x & 1) : (z & 1) != (x & 1)) ? Voxel1 : Voxel2;
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