using CatDOTS.VoxelWorld.Player;
using CatFramework;
using CatFramework.Magics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld.Magics
{
    public class DestoryVoxel : Magic
    {
        readonly IVoxelCommandPools voxelCommandPools;
        readonly float destroyDelay;
        public DestoryVoxel(IVoxelCommandPools voxelCommandPools)
        {
            this.voxelCommandPools = voxelCommandPools;
            destroyDelay = 0.15f;
        }
        public override bool Fire()
        {
            if (Parent.User is IVoxelPlayer voxelPlayer && voxelPlayer.Enable)
            {
                var rayResult = voxelPlayer.VoxelRayResult;
                if (rayResult.Hit)
                {
                    if (rayResult.CanDestroyTargetFaceFroward())
                    {
                        Voxel voxel = Voxel.Empty;
                        voxel.VoxelMaterial |= rayResult.TargetFaceFroward.VoxelMaterial & VoxelMaterial.Water;
                        voxelPlayer.AddVoxel(rayResult.TargetFaceFroward);
                        AddCommand(voxel, rayResult.TargetFaceForwardIndex);
                    }
                    else if (rayResult.CanDestroyTarget())
                    {
                        Voxel voxel = Voxel.Empty;
                        voxel.VoxelMaterial |= rayResult.Target.VoxelMaterial & VoxelMaterial.Water;
                        voxelPlayer.AddVoxel(rayResult.Target);
                        AddCommand(voxel, rayResult.TargetIndex);
                    }
                }
            }
            return false;
        }
        void AddCommand(Voxel voxel, int3 index)
        {
            SingleVoxelCommand singleVoxelCommand = voxelCommandPools.GetSingleVoxelCommand();
            singleVoxelCommand.Voxel = voxel;
            singleVoxelCommand.TargetVoxelIndexInWorld = index;

            SyncAlterVoxelSystem.AddCommand(singleVoxelCommand);
            Parent.AppendDelay(destroyDelay);
        }
    }
}
