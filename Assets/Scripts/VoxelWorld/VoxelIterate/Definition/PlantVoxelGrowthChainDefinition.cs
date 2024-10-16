using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    //public interface IPlantVoxelDefinition : IVoxelDefinition
    //{

    //}
    //[CreateAssetMenu(fileName = "New PlantVoxelDefinition", menuName = "ECSVoxelWorld/VoxelDefinition/Plant")]
    //public class PlantVoxelDefinition : VoxelDefinition, IPlantVoxelDefinition
    //{
    //    [SerializeField] VoxelDefinition nextStage;
    //    [SerializeField] int growthRate;
    //}
    public interface IPlantVoxelGrowthChain
    {
        VoxelDefinition[] Stages { get; }
        sbyte LightingRequirements { get; }
    }
    [CreateAssetMenu(fileName = "New PlantVoxelGrowthChain", menuName = "ECSVoxelWorld/PlantVoxelGrowthChain")]
    public class PlantVoxelGrowthChainDefinition : ScriptableObject, IPlantVoxelGrowthChain
    {
        [SerializeField] VoxelDefinition[] stages;
        [SerializeField] sbyte lightingRequirements;
        public VoxelDefinition[] Stages => stages;
        public sbyte LightingRequirements => lightingRequirements;
    }
    //[System.Serializable]
    //public class PlantVoxelGrowthChainNode
    //{
    //    public VoxelDefinition VoxelDefinition;
    //    public byte LightingRequirements;
    //}
}
