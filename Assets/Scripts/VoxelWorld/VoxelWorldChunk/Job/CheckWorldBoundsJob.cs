using CatFramework;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct CheckWorldBoundsJob : IJob
    {
        [ReadOnly] public NativeHashMap<int3, bool>.ReadOnly ActiveBigChunks;
        public NativeList<int3> WaitingGenerates;
        public NativeList<int3> WaitingUnloads;
        public int2 PlayerChunkIndex;
        public int UnloadingDistance;
        public int LoadingDistance;

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
                    int3 bigChunkIndex = enumerator.Current.Key;
                    if (CheckOutOfUnloadingDistance(bigChunkIndex.As2D()))
                    {
                        WaitingUnloads.Add(bigChunkIndex);
                    }
                }
            }
            #endregion
            #region 检查是否需要有生成的
            if (WaitingGenerates.Length == 0)
            {
                int min = -LoadingDistance;
                int max = LoadingDistance;// 这里是距离而非索引,导致大了一圈
                //float sqLength = 1.47f * HalfSimulationRange;
                // 开始检查周围所有需要渲染的区块
                for (int x = min; x <= max; x++)
                {
                    for (int z = min; z <= max; z++)
                    {
                        int3 bigChunkIndex = new int3(x + PlayerChunkIndex.x, 0, z + PlayerChunkIndex.y);
                        if (!ActiveBigChunks.ContainsKey(bigChunkIndex))// 否则检查是否在已渲染的图中，如果不在，则加入等待生成
                        {
                            WaitingGenerates.Add(bigChunkIndex);// 并放入待生成
                        }
                    }
                }
                //WaitingGenerates.Sort(new ComparerNearToPlayer(PlayerChunkIndex));
            }
            #endregion
        }
        //[BurstCompile]
        //struct ComparerNearToPlayer : IComparer<int3>
        //{
        //    int2 PlayerPosInChunk;
        //    public ComparerNearToPlayer(int2 PlayerPosInChunk)
        //    {
        //        this.PlayerPosInChunk = PlayerPosInChunk;
        //    }
        //    public int Compare(int3 x, int3 y)
        //    {
        //        int2 xDis = math.abs(x).As2D() - PlayerPosInChunk;
        //        int2 yDis = math.abs(y).As2D() - PlayerPosInChunk;

        //        if (math.all(xDis == yDis))
        //        {
        //            return 0;
        //        }
        //        else if (math.all(xDis > yDis))
        //        {
        //            return 1;
        //        }
        //        return -1;
        //    }
        //}
    }
}
