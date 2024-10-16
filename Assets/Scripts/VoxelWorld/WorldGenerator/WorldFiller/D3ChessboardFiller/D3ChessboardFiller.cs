using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class D3ChessboardFiller : IWorldFiller
    {
        public Voxel Voxel1;
        public Voxel Voxel2;
        public int FloorCount;

        public string Name { get; set; }
        public void Dispose()
        {
        }

        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            //int2 r = (bigChunkMapContainer.BigChunkPosInt.xz / Settings.SmallChunkSize) & 1;
            //if (r.x == r.y) return dependsOn;
            return new D3ChessboardFillerJob()
            {
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt,
                Voxels = bigChunkMapContainer.Voxels,
                Voxel1 = Voxel1,
                Voxel2 = Voxel2,
                FloorCount = FloorCount,
            }.Schedule(dependsOn);
        }
    }
}