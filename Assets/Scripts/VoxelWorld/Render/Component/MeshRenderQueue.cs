//using System;
//using System.Collections.Generic;
//using Unity.Entities;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    /// <summary>
//    /// 创建系统时创建
//    /// </summary>
//    public class MeshRenderQueue : IComponentData
//    {
//        readonly HashSet<int3> waitForBuildMeshSet;
//        readonly Queue<int3> queue;
//        public MeshRenderQueue()// 组件需要默认构造函数
//        {
//        }
//        public MeshRenderQueue(HashSet<int3> waitForBuildMeshSet, Queue<int3> queue)
//        {
//            this.waitForBuildMeshSet = waitForBuildMeshSet;
//            this.queue = queue;
//        }
//        #region 追加
//        void Add(int3 smallChunkIndex)
//        {
//            if (Settings.ValidChunkHeightIndex(smallChunkIndex.y) && waitForBuildMeshSet.Add(smallChunkIndex))
//            {
//                queue.Enqueue(smallChunkIndex);
//            }
//        }
//        public void AddDirtyMeshRecord(int3 smallChunkIndex)
//        {
//            Add(smallChunkIndex);
//        }
//        // 首先遍历每个记录更改的区块坐标的周围区块，
//        // 查看是否这个区块本身就是标记变更了
//        // 否则将这个新的需要变更的记录进新需要变更
//        void AddDirtyMeshRecordAndCheckNear(int3 smallChunkIndex)
//        {
//            Add(smallChunkIndex);
//            Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z + 1));// 检查周围六个区块
//            Add(new int3(smallChunkIndex.x, smallChunkIndex.y, smallChunkIndex.z - 1));
//            Add(new int3(smallChunkIndex.x, smallChunkIndex.y + 1, smallChunkIndex.z));
//            Add(new int3(smallChunkIndex.x, smallChunkIndex.y - 1, smallChunkIndex.z));
//            Add(new int3(smallChunkIndex.x + 1, smallChunkIndex.y, smallChunkIndex.z));
//            Add(new int3(smallChunkIndex.x - 1, smallChunkIndex.y, smallChunkIndex.z));
//        }
//        public void AddWaitForBuildMeshAndCheckNear(int3 smallChunkIndex)
//        {
//            AddDirtyMeshRecordAndCheckNear(smallChunkIndex);
//        }
//        public void AddWaitForBuildMeshAndCheckNear(List<int3> chunks)
//        {
//            foreach (var smallChunkIndex in chunks)
//            {
//                AddDirtyMeshRecordAndCheckNear(smallChunkIndex);
//            }
//            chunks.Clear();
//        }
//        public void AddWaitForBuildMeshAndCheckNear(Queue<int3> chunks)
//        {
//            foreach (var smallChunkIndex in chunks)
//            {
//                AddDirtyMeshRecordAndCheckNear(smallChunkIndex);
//            }
//            chunks.Clear();
//        }
//        #endregion
//    }
//}
