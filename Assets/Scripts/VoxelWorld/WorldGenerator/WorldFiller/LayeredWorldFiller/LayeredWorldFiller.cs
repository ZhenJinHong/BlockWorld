using CatFramework.SLMiao;
using System.Text;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public class LayeredWorldFiller : IWorldFiller
    {
        public string Name { get; set; }
        BlobAssetReference<LayeredWorldFilling> Filler;
        public void Dispose()
        {
            Filler.Dispose();
        }
        public LayeredWorldFiller(BlobAssetReference<LayeredWorldFilling> filler)
        {
            this.Filler = filler;
        }
        public JobHandle ScheduleFillerJob(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            return new LayeredWorldFillerJob()
            {
                WorldFilling = Filler,
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
            Serialization.ObjectFieldToString(Filler.Value, stringBuilder);
            return stringBuilder.ToString();
        }
    }
}
