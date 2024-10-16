using CatDOTS.VoxelWorld.Player;
using CatFramework.Magics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld.Magics
{
    public class VoxelMaterialSetMagic : Magic
    {
        IVoxelCommandPools voxelCommandPools;
        float putDelay;
        public VoxelMaterial VoxelMaterial;
        public VoxelMaterialSetMagic(IVoxelCommandPools voxelCommandPools)
        {
            this.voxelCommandPools = voxelCommandPools;
            putDelay = 0.15f;
        }
        public override bool Fire()
        {
            if (Parent.User is IVoxelPlayer voxelPlayer)
            {
                if (voxelPlayer.Enable)
                {
                    var rayResult = voxelPlayer.VoxelRayResult;
                    if (rayResult.Hit)
                    {
                        //0 ^ 1 = 1;方块无水
                        //1 ^ 0 = 1;
                        //1 ^ 1 = 0;方块有水
                        //0 ^ 0 = 0;
                        Voxel voxel = rayResult.Target;
                        if ((voxel.VoxelMaterial & VoxelMaterial) != VoxelMaterial)
                        {
                            voxel.VoxelMaterial |= VoxelMaterial;
                            SingleVoxelCommand singleVoxelCommand = voxelCommandPools.GetSingleVoxelCommand();
                            singleVoxelCommand.TargetVoxelIndexInWorld = rayResult.TargetIndex;
                            singleVoxelCommand.Voxel = voxel;
                            SyncAlterVoxelSystem.AddCommand(singleVoxelCommand);

                            Parent.AppendDelay(putDelay);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
