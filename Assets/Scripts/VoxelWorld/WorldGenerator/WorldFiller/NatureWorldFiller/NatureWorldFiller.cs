using CatFramework.SLMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class NatureWorldFiller : IWorldFiller
    {
        public string Name { get; set; }
        BlobAssetReference<NatureWorldFilling> Filling;
        public void Dispose()
        {
            Filling.Dispose();
        }
        public NatureWorldFiller(BlobAssetReference<NatureWorldFilling> filling)
        {
            this.Filling = filling;
        }
        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new NatureWorldFillerJob()
            {
                WorldFilling = Filling,

                HeightMap = bigChunkMapContainer.HeightMap,
                RangeNoiseMap = bigChunkMapContainer.RegionNoiseMap,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt,
                Voxels = bigChunkMapContainer.Voxels,
            }.Schedule(Settings.VoxelCapacityInBigChunk, Settings.VoxelCapacityInSmallChunk, dependsOn);
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Name}--种子:");
            Serialization.ObjectFieldToString(Filling.Value, stringBuilder);
            return stringBuilder.ToString();
        }
    }
}
