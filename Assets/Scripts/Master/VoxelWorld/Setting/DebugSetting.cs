using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.DataMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace VoxelWorld
{
    internal class DebugSetting : Setting, VoxelWorldDataBaseManaged.IDebugSetting
    {
        public override string Name => "调试";
        [SerializeField]
        private bool showNewDirtyChunkInfo;
        public bool ShowNewDirtyChunkInfo { get => showNewDirtyChunkInfo; set => showNewDirtyChunkInfo = value; }
        public override void ResetSetting()
        {
#if UNITY_EDITOR
            ShowNewDirtyChunkInfo = true;
#endif
        }
    }
}
