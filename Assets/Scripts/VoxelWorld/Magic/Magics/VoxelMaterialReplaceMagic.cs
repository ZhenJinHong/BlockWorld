using CatDOTS.VoxelWorld.Player;
using CatFramework.Magics;

namespace CatDOTS.VoxelWorld.Magics
{
    public class VoxelMaterialReplaceMagic : Magic
    {
        IVoxelCommandPools voxelCommandPools;
        float putDelay;
        public VoxelMaterial VoxelMaterial;
        public VoxelMaterialReplaceMagic(IVoxelCommandPools voxelCommandPools)
        {
            this.voxelCommandPools = voxelCommandPools;
            putDelay = 0.15f;
        }
        public override bool Fire()
        {
            if (Parent.User is IVoxelPlayer voxelPlayer && voxelPlayer.Enable)
            {
                var rayResult = voxelPlayer.VoxelRayResult;
                if (rayResult.Hit)
                {
                    Voxel voxel = rayResult.Target;
                    voxel.VoxelMaterial ^= VoxelMaterial;
                    SingleVoxelCommand singleVoxelCommand = voxelCommandPools.GetSingleVoxelCommand();
                    singleVoxelCommand.TargetVoxelIndexInWorld = rayResult.TargetIndex;
                    singleVoxelCommand.Voxel = voxel;
                    SyncAlterVoxelSystem.AddCommand(singleVoxelCommand);

                    Parent.AppendDelay(putDelay);
                    return true;
                }
            }
            return false;
        }
    }
}
