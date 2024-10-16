using CatFramework.SLMiao;
using System.Collections;
using System.Text;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatDOTS.VoxelWorld
{
    public class PowOctavePerlinTerrain : ITerrainGenerator
    {
        public string Name { get; set; }
        PowOctavePerlinSeed Seed;
        public PowOctavePerlinTerrain(PowOctavePerlinSeed seed)
        {
            Seed = seed;
        }
        public void Dispose()
        {
        }

        public JobHandle ScheduleGenerateHeightMap(IBigChunkMapContainer bigChunkMapContainer, WorldTopography worldTopography, JobHandle dependsOn)
        {
            return new PowOctavePerlinTerrainJob()
            {
                WorldTopography = worldTopography,
                Seed = Seed,
                BigChunkPos = bigChunkMapContainer.BigChunkPosInt.As2D(),
                RegionMap = bigChunkMapContainer.RegionNoiseMap,
                HeightMap = bigChunkMapContainer.HeightMap,
            }.Schedule(dependsOn);
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Name}--种子值:");
            Serialization.ObjectFieldToString(Seed, stringBuilder);
            return stringBuilder.ToString();
        }
    }
}