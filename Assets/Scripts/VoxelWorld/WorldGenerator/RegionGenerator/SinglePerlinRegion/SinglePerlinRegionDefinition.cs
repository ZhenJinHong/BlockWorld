using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New SinglePerlinRegion", menuName = "ECSVoxelWorld/Region/SinglePerlin")]
    public class SinglePerlinRegionDefinition : BaseWorldRegionDefinition<SinglePerlinRegionBuiler>
    {
    }
}