using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    public struct WorldTopography
    {
        public float BaseHeight;
        public float TopographicHeightDiff;
        public override readonly string ToString()
        {
            return "基础高度 : " + BaseHeight + "\n" + "地势高差 : " + TopographicHeightDiff;
        }
    }
    public class WorldGenerator : IWorldGenerator
    {
        //public float BaseHeight;
        public WorldTopography WorldTopography;
        public IWorldRegionGenarator WorldRegionGenarator;
        public ITerrainGenerator[] TerrainGenerators;
        public IWorldFiller[] WorldFillerGenarators;
        public IEntityFiller[] EntityFillers;
        public void Dispose()
        {
            WorldRegionGenarator?.Dispose();
            DisposeGenarator(TerrainGenerators);
            DisposeGenarator(WorldFillerGenarators);
            DisposeGenarator(EntityFillers);
        }
        static void DisposeGenarator(IDisposable[] disposables)
        {
            for (int i = 0; i < disposables.Length; i++)
                disposables[i].Dispose();
        }
        public JobHandle ScheduleGenerateBigChunkMap(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            if (WorldRegionGenarator != null)
                dependsOn = WorldRegionGenarator.ScheduleRegionJob(bigChunkMapContainer, dependsOn);
            for (int i = 0; i < TerrainGenerators.Length; i++)
            {
                dependsOn = TerrainGenerators[i].ScheduleGenerateHeightMap(bigChunkMapContainer, WorldTopography, dependsOn);
            }
            return dependsOn;
        }

        public JobHandle ScheduleFillBigChunk(IBigChunkMapContainer bigChunkMapContainer, JobHandle dependsOn)
        {
            for (int i = 0; i < WorldFillerGenarators.Length; i++)
            {
                dependsOn = WorldFillerGenarators[i].ScheduleFillerJob(bigChunkMapContainer, dependsOn);
            }
            return dependsOn;
        }
        public JobHandle ScheduleFillEntity(VoxelWorldMap.BigChunkSliceReadOnly bigChunkSlice, EntityCommandBufferSystem entityCommandBufferSystem, JobHandle dependsOn)
        {
            for (int i = 0; i < EntityFillers.Length; i++)
            {
                dependsOn = EntityFillers[i].Schedule(bigChunkSlice, entityCommandBufferSystem.CreateCommandBuffer(), dependsOn);
            }
            return dependsOn;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"世界地势 : {WorldTopography}");
            stringBuilder.AppendLine("区域 : ");
            stringBuilder.AppendLine(WorldRegionGenarator?.ToString());
            stringBuilder.AppendLine("地形 : ");
            ToString(stringBuilder, TerrainGenerators);
            stringBuilder.AppendLine("填充物 : ");
            ToString(stringBuilder, WorldFillerGenarators);
            return stringBuilder.ToString();
        }
        static void ToString(StringBuilder stringBuilder, object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                stringBuilder.AppendLine(objects[i].ToString());
            }
        }
    }
}
