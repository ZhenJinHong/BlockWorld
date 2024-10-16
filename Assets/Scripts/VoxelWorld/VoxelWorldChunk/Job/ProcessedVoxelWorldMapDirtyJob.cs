using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 从图中获取上帧被修改过的小区块索引,并追加至等待队列中
    /// </summary>
    [BurstCompile]
    public struct ProcessedVoxelWorldMapDirtyJob : IJob
    {
        public WaitingRebuildSmallChunkMeshQueueData WaitingRebuildSmallChunkMeshQueueData;
        public NativeList<int3> SmallChunkVariationList;
        public VoxelWorldMapData VoxelWorldMapData;
        public int ProcessCount;
        public void Execute()
        {
            //// 必须有个临时的,以获取当前集合已经需要被移除的元素;
            //NativeArray<int3> dirtySmallTempArray =
            //    new NativeArray<int3>(math.min(VoxelWorldMapData.DirtySmallChunkSet.Count, ProcessCount), Allocator.Temp);

            //var dirtySmallChunkEnumerator = VoxelWorldMapData.DirtySmallChunkSet.GetEnumerator();
            //while (dirtySmallChunkEnumerator.MoveNext() && ProcessCount > 0)
            //{
            //    ProcessCount--;
            //    int3 smallChunkIndex = dirtySmallChunkEnumerator.Current;
            //    dirtySmallTempArray[ProcessCount] = smallChunkIndex;
            //    int3 bigChunkIndex = new int3(smallChunkIndex.x, 0, smallChunkIndex.z);
            //    // 大区块数据标记脏,需要在卸载时保存
            //    if (VoxelWorldMapData.TryGetValue(bigChunkIndex, out bool isDirtyData))
            //    {
            //        // 小区块脏需要重构网格
            //        WaitingUpdateMeshQueueData.AddSmallChunk(smallChunkIndex);

            //        if (!isDirtyData)
            //            VoxelWorldMapData.SetBigChunkDataDirty(bigChunkIndex);
            //    }
            //}
            //#region 将已经标记完成脏的小区块从集合移除
            //var dirtyMeshEnumerator = dirtySmallTempArray.GetEnumerator();
            //while (dirtyMeshEnumerator.MoveNext())
            //{
            //    VoxelWorldMapData.DirtySmallChunkSet.Remove(dirtyMeshEnumerator.Current);
            //}
            //#endregion
            NativeArray<int3> dirtyMeshs = VoxelWorldMapData.ProcessedChunkDirty(ProcessCount);
            var enumerator = dirtyMeshs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WaitingRebuildSmallChunkMeshQueueData.AddSmallChunk(enumerator.Current);
                SmallChunkVariationList.Add(enumerator.Current);
            }
            enumerator.Dispose();
        }
    }
}
