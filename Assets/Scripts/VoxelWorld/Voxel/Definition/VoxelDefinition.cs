using CatFramework.Magics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelDefinition
    {
        string ClassifyName { get; }
        string VoxelName { get; }
        VoxelMaterial VoxelMaterial { get; }
        VoxelRenderType VoxelRenderType { get; }
        Texture2D Icon { get; }
        Texture2D Texture { get; }
        byte Energy { get; }
        byte Resistance { get; }
    }
    [CreateAssetMenu(fileName = "New BlockVoxelDefinition", menuName = "ECSVoxelWorld/VoxelDefinition/Block")]
    public class VoxelDefinition : ScriptableObject, IVoxelDefinition
    {
        [SerializeField] string classifyName = "方块";
        public string ClassifyName => classifyName;
        public string VoxelName => name;

        [SerializeField] VoxelMaterial voxelMaterial;
        [SerializeField] VoxelRenderType renderType = VoxelRenderType.OpaqueBlock;
        //[SerializeField, Range(0.0f, 0.5f)] float grassOffset;
        [SerializeField] Texture2D icon;
        [SerializeField] Texture2D texture;
        [SerializeField] byte energy;
        [SerializeField] byte resistance = 15;
        public VoxelMaterial VoxelMaterial => voxelMaterial;
        public VoxelRenderType VoxelRenderType => renderType;
        //public float GrassOffset => grassOffset;
        public Texture2D Texture => texture;
        public Texture2D Icon => icon;
        public byte Energy => energy;
        public byte Resistance => resistance;
    }
}
