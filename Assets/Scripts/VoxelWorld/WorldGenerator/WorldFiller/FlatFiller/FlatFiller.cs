using Unity.Collections;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class FlatFiller : IWorldFiller
    {
        public string Name { get; set; }
        NativeArray<Voxel> LayerVoxels;
        public void Dispose()
        {
            LayerVoxels.Dispose();
        }
        public FlatFiller(NativeArray<Voxel> voxels)
        {
            LayerVoxels = voxels;
        }
        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new FlatFillerJob()
            {
                LayerVoxels = LayerVoxels,
                Voxels = bigChunkMapContainer.Voxels,
            }.Schedule(dependsOn);
        }
    }
}