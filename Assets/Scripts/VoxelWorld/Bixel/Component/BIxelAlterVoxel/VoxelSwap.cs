using Unity.Entities;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelSwap : IComponentData
    {
        public Voxel CheckMask;
        public Voxel SwapMask;
        public Voxel Last;
        //public int w;
        //public ushort www;
        public BixelCheckPoint Point1;
        public BixelCheckPoint Point2;
        public static void CheckIsValid(VoxelSwap voxelSwap)
        {
            if (voxelSwap.CheckMask == 0ul || voxelSwap.SwapMask == 0ul || voxelSwap.Point1 == voxelSwap.Point2)
            {
#if UNITY_EDITOR
                Debug.LogWarning("无效的体素交换组件");
#endif
            }
        }
    }
}
