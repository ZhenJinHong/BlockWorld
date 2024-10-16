using CatDOTS.VoxelWorld.Player;
using CatFramework;
using CatFramework.Magics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CatDOTS.VoxelWorld.Magics
{
    public abstract class BasePutBixel : Magic
    {
        public float Delay = 1f;
        IBixelDataBase bixelDataBase;
        ISyncECBCreate syncECBCreate;
        public BasePutBixel(IBixelDataBase bixelDataBase, ISyncECBCreate syncECBCreate)
        {
            this.bixelDataBase = bixelDataBase;
            this.syncECBCreate = syncECBCreate;
            if (bixelDataBase == null || syncECBCreate == null)
                throw new System.NullReferenceException("空的乙素数据库或命令缓冲创建");
        }
        public override bool Fire()
        {
            if (!ActionCompoentDataIsValid()) return false;
            if (Parent.User is IVoxelPlayer voxelPlayer && voxelPlayer.Enable)
            {
                var rayResult = voxelPlayer.VoxelRayResult;
                if (rayResult.Hit)
                {
                    var ecb = syncECBCreate.CreateECB();
                    var entity = ecb.Instantiate(bixelDataBase.BixelPrefab);
                    ecb.SetComponent(entity, new LocalTransform()
                    {
                        Position = math.floor(rayResult.Position) + 0.5f,
                        Rotation = quaternion.identity,
                        Scale = 1.0f,
                    });
                    Bixel bixel = new Bixel()
                    {
                        Delay = Delay < 0.05f ? 0.05f : Delay,
                    };
                    ecb.SetComponent(entity, bixel);
                    AddBixelActionCompoent(ecb, entity);
                    Parent.AppendDelay(bixelDataBase.PutDelay);
                    return true;
                }
            }
            return false;
        }
        protected abstract bool ActionCompoentDataIsValid();
        protected abstract void AddBixelActionCompoent(EntityCommandBuffer ecb, Entity entity);// 多态或者走判断?
    }
}
