using Unity.Entities;

namespace CatDOTS.VoxelWorld.Magics
{
    public class PutSwapVoxelBixel : BasePutBixel
    {
        public VoxelSwap VoxelSwap;
        public PutSwapVoxelBixel(IBixelDataBase bixelDataBase, ISyncECBCreate syncECBCreate) : base(bixelDataBase, syncECBCreate)
        {
        }

        protected override bool ActionCompoentDataIsValid()
        {
            return true;
        }

        protected override void AddBixelActionCompoent(EntityCommandBuffer ecb, Entity entity)
        {
            ecb.AddComponent(entity, VoxelSwap);
        }
    }
}
