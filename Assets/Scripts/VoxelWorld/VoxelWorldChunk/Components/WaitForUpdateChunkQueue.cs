//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    public struct WaitForUpdateChunkQueue : IComponentData
//    {
//        NativeList<int3> WaitingQueue;
//        public WaitForUpdateChunkQueue(NativeList<int3> waitingQueue)
//        {
//            WaitingQueue = waitingQueue;
//        }
//        public NativeArray<int3>.ReadOnly ReadOnly()
//        {
//            return WaitingQueue.AsParallelReader();
//        }
//    }
//    public struct WaitForUpdateChunkQueueData
//    {
//        public NativeList<int3> WaitingQueue;
//        public WaitForUpdateChunkQueueData(int initCapacity)
//        {
//            WaitingQueue = new NativeList<int3>(initCapacity, Allocator.Persistent);
//        }
//        public WaitForUpdateChunkQueue Get()
//        {
//            return new WaitForUpdateChunkQueue(WaitingQueue);
//        }
//        public void Dispose()
//        {
//            WaitingQueue.Dispose();
//        }
//        public void Clear()
//        {
//            WaitingQueue.Clear();
//        }
//    }
//}
