using Unity.Entities;

namespace CatDOTS.VoxelWorld.Magics
{
    // 直接构造对应的乙素的类魔法,还是魔法里再套弹药?那样套太深,不好修改其内部字段
    // 不要管魔杖的生产,那是另外的.现在需要有蓝图去构建这个乙素魔法,放入库存,允许这个魔法拆卸装载
    public class PutVoxelReplaceBixel : BasePutBixel
    {
        public VoxelReplace VoxelReplace;
        public PutVoxelReplaceBixel(IBixelDataBase bixelDataBase, ISyncECBCreate syncECBCreate) : base(bixelDataBase, syncECBCreate) { }
        protected override bool ActionCompoentDataIsValid()
        {
            return true;
        }
        protected override void AddBixelActionCompoent(EntityCommandBuffer ecb, Entity entity)
        {
            ecb.AddComponent(entity, VoxelReplace);
        }
    }
}
