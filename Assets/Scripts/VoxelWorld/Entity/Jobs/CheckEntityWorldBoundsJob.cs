using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct CheckEntityWorldBoundsJob : IJob
    {
        [ReadOnly] public NativeHashSet<int3>.ReadOnly ActiveBigChunks;
        public NativeList<int3> WaitingUnloads;
        public int2 PlayerChunkIndex;
        public int UnloadingDistance;

        readonly bool CheckOutOfUnloadingDistance(int2 bigChunkIndex)
            => VoxelMath.OutOfDistance(bigChunkIndex, PlayerChunkIndex, UnloadingDistance);
        public void Execute()
        {
            #region 检查是否有需要回收的
            if (WaitingUnloads.Length == 0)
            {
                // 首先检查是否有需要移除渲染的，加入队列后，后续从渲染图移除
                var enumerator = ActiveBigChunks.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int3 bigChunkIndex = enumerator.Current;
                    if (CheckOutOfUnloadingDistance(bigChunkIndex.As2D()))
                    {
                        WaitingUnloads.Add(bigChunkIndex);
                    }
                }
            }
            #endregion
        }
    }
}
