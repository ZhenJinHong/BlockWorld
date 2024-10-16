using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New PowOctavePerlinTerrain", menuName = "ECSVoxelWorld/Terrain/PowOctavePerlin")]
    public class PowOctavePerlinTerrainDefinition : BaseTerrainDefinition<PowOctavePerlinTerrainBuilder>
    {
    }
}