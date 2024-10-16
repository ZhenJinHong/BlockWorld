using CatDOTS.VoxelWorld.Player;
using CatFramework;
using CatFramework.Magics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld.Magics
{
    public class PutVoxel : Magic
    {
        readonly IVoxelCommandPools voxelCommandPools;
        readonly float putDelay;
        public PutVoxel(IVoxelCommandPools voxelCommandPools)
        {
            this.voxelCommandPools = voxelCommandPools;
            putDelay = 0.15f;
        }
        public PutVoxel(IVoxelCommandPools voxelCommandPools, int putDelay)
        {
            this.voxelCommandPools = voxelCommandPools;
            this.putDelay = putDelay;
        }
        public override bool Fire()
        {
            // 通过使用者通知库存?
            if (Parent.MagicEnergy is IVoxelItemStorage voxelItemStorage && Parent.User is IVoxelPlayer voxelPlayer)
            {
                if (voxelPlayer.Enable && voxelItemStorage.CountEnough(1))
                {
                    Voxel voxel = voxelItemStorage.Voxel;
                    if (Voxel.NonAir(voxel.VoxelTypeIndex))
                    {
                        var rayResult = voxelPlayer.VoxelRayResult;
                        if (rayResult.Hit)
                        {
                            if (rayResult.CanPutToTarget(voxel))
                            {
                                voxel.VoxelMaterial |= (rayResult.Target.VoxelMaterial & VoxelMaterial.Water);
                                AddCommand(voxelItemStorage, voxel, rayResult, rayResult.TargetIndex);
                                return true;
                            }
                            else if (rayResult.CanPutToTargetFaceForward(voxel))
                            {
                                voxel.VoxelMaterial |= (rayResult.TargetFaceFroward.VoxelMaterial & VoxelMaterial.Water);
                                AddCommand(voxelItemStorage, voxel, rayResult, rayResult.TargetFaceForwardIndex);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        void AddCommand(IVoxelItemStorage voxelItemStorage, Voxel voxel, IVoxelRayResult rayResult, int3 index)
        {
            voxel.ShapeDirection = rayResult.ShapeDirection;
            if (voxelItemStorage.VoxelShapeInfo != null)
                voxel.ShapeIndex = voxelItemStorage.VoxelShapeInfo.ShapeIndex;

            SingleVoxelCommand singleVoxelCommand = voxelCommandPools.GetSingleVoxelCommand();
            singleVoxelCommand.TargetVoxelIndexInWorld = index;
            singleVoxelCommand.Voxel = voxel;

            SyncAlterVoxelSystem.AddCommand(singleVoxelCommand);
            Parent.AppendDelay(putDelay);
            voxelItemStorage.Decrease(1);
        }
    }
}
