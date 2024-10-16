using System.Collections;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelVariantDefinition
    {
        VoxelDefinition VoxelDef { get; }
        VoxelMaterial VoxelMaterial { get; }
        byte Energy { get; }
    }
    [CreateAssetMenu(fileName = "New VoxelVariantDefinition", menuName = "ECSVoxelWorld/Variant/Voxel")]
    public class VoxelVariantDefinition : ScriptableObject, IVoxelVariantDefinition
    {
        [SerializeField] VoxelDefinition voxel;
        [SerializeField] VoxelMaterial voxelMaterial;
        [SerializeField] byte energy;
        public VoxelDefinition VoxelDef => voxel;
        public VoxelMaterial VoxelMaterial => voxelMaterial;
        public byte Energy => energy;
    }
}