using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New WorldDefinition", menuName = "ECSVoxelWorld/WorldDefinition")]
    public class WorldDefinition : ScriptableObject, IWorldDefinition
    {
        [SerializeField] WorldGeneratorBuilder builder = new WorldGeneratorBuilder();
        public BaseWorldRegionDefinition WorldRegionDefinition;
        public BaseTerrainDefinition[] TerrainDefinitions;
        public BaseWorldFillerDefinition[] WorldFillerDefinitions;
        public BaseEntityFillerDefinition[] EntityFillerDefinitions;
        public string Name => name;

        public IWorldGenerator Create(uint seed, VoxelWorldDataBaseManaged dataBase)
        {
            builder.Name = name;
            builder.WorldRegionDefinition = WorldRegionDefinition;
            builder.TerrainDefinitions = TerrainDefinitions;
            builder.WorldFillerDefinitions = this.WorldFillerDefinitions;
            builder.EntityFillerDefinitions = this.EntityFillerDefinitions;
            return builder.Create(seed, dataBase);
        }
    }
}
