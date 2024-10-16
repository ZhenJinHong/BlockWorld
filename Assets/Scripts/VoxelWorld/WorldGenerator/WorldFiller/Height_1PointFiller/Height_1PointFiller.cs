using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class Height_1PointFiller : IWorldFiller
    {
        public string Name { get; set; }
        public Voxel Toput;
        public float RangeScale;
        public float PutScale;
        public float RangeThreshold;
        public float PutThreshold;
        public void Dispose()
        {
        }
        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new Height_1PointFillerJob()
            {
                RangeScale = RangeScale,
                PutScale = PutScale,
                RangeThreshold = RangeThreshold,
                PutThreShold = PutThreshold,
                Toput = Toput,

                HeightMap = bigChunkMapContainer.HeightMap,
                RangeNoiseMap = bigChunkMapContainer.RegionNoiseMap,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt,
                Voxels = bigChunkMapContainer.Voxels,
            }.Schedule(dependsOn);
        }
    }
}
