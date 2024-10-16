using System;
using Unity.Burst;
using Unity.Entities;

namespace CatDOTS
{
    /// <summary>
    /// 使用销毁工作的原因，如果有工作后于销毁工作对被执行里销毁的实体操作
    /// </summary>
    /// <remarks>
    /// 错误，没有必要使用销毁工作，如果有工作后于销毁工作操作实体，就算使用销毁工作，也一定报错；而先于销毁工作操作实体，会因为播放命令的顺序先操作的再销毁
    /// 是不会报错的
    /// </remarks>
    [BurstCompile, Obsolete]
    public partial struct DestroyEntityJob
    {
        public EntityCommandBuffer ECB;
        [BurstCompile]
        public void Execute(in Entity entity)
        {
            ECB.DestroyEntity(entity);
        }
    }
}
