using CatFramework.Magics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 后续可以按需要分多种的,
    /// </summary>
    [CreateAssetMenu(fileName = "New VoxelWorldDataBase", menuName = "ECSVoxelWorld/DataBase/VoxelWorld")]
    public class DataBaseDefinition : ScriptableObject
    {
        public GameObject blockObjectPrefab;
        public Material opaqueMaterial;
        public Material transparentMaterial;
        public Material waterMaterial;
        public Material fireMaterial;
        public Material grassMaterial;
        public VoxelShapeDefinition defaultShape;
        public BixelDataBaseDefinition bixelDataBaseDefinition;
    }
    /// <summary>
    /// 数据将在结束后释放
    /// </summary>
    public class TempDataBase
    {
        public IList<IVoxelDefinition> VoxelDefinitions;
        public IList<IVoxelShapeDefinition> VoxelShapeDefinitions;
        public IList<IMagicWandDefinition> MagicWandDefinitions;
    }
}
