using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New MessyTerrain", menuName = "ECSVoxelWorld/Terrain/Messy")]
    public class MessyTerrainDefinition : BaseTerrainDefinition<MessyTerrainBuilder>
    {
    }
}