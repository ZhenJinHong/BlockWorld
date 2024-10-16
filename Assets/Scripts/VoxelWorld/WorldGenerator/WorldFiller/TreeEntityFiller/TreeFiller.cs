using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class TreeFiller : IWorldFiller
    {
        public string Name { get; set; }
        public Voxel Trunk;
        public Voxel Leave;
        public float RangeScale;
        public float PutScale;
        public float RangeThreshold;
        public float PutThreshold;
        public void Dispose()
        {
        }
        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new TreeFillerJob()
            {
                RangeScale = RangeScale,
                PutScale = PutScale,
                RangeThreshold = RangeThreshold,
                PutThreShold = PutThreshold,
                Trunk = Trunk,
                Leave = Leave,

                HeightMap = bigChunkMapContainer.HeightMap,
                RangeNoiseMap = bigChunkMapContainer.RegionNoiseMap,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt,
                Voxels = bigChunkMapContainer.Voxels,
            }.Schedule(dependsOn);
        }
    }
}
