using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New OctavePerlinTerrain", menuName = "ECSVoxelWorld/Terrain/OctavePerlin")]
    public class OctavePerlinTerrainDefinition : BaseTerrainDefinition<OctavePerlinTerrainBuilder>
    {
    }
}