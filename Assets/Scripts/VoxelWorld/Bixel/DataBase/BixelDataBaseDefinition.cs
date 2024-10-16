using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [CreateAssetMenu(fileName = "New BixelDataBase", menuName = "ECSVoxelWorld/DataBase/Bixel")]
    public class BixelDataBaseDefinition : ScriptableObject
    {
        public Material OpaueVoxelMaterial;
        public Material GrassVoxelMaterial;
        public Material TransparentMaterial;
        public Mesh BixelMesh;
    }
}