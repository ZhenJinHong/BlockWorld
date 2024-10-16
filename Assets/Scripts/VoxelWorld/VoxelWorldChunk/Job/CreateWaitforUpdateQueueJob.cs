//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;

//namespace CatDOTS.VoxelWorld
//{
//    [BurstCompile]
//    public struct CreateWaitforUpdateQueueJob : IJob
//    {
//        public NativeHashSet<int3> DirtyChunkSet;
//        public NativeList<int3> WaitingQueue;
//        public int WaitingCount;
//        public void Execute()
//        {
//            WaitingQueue.Clear();
//            var enumerator = DirtyChunkSet.GetEnumerator();
//            while (WaitingCount > 0 && enumerator.MoveNext())
//            {
//                WaitingQueue.Add(enumerator.Current);
//            }
//            var enumerator2 = WaitingQueue.GetEnumerator();
//            while (enumerator2.MoveNext())
//            {
//                DirtyChunkSet.Remove(enumerator2.Current);
//            }
//        }
//    }
//}
