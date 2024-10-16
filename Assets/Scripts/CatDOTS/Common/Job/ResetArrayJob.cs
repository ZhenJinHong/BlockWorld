using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CatDOTS
{
    [BurstCompile]
    public struct ResetArrayJob<T> : IJob where T : unmanaged
    {
        public NativeArray<T> Array;
        public T Value;
        public void Execute()
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = Value;
            }
        }
    }
    [BurstCompile]
    public struct ResetArrayJobParallelForBatch<T> : IJobParallelForBatch where T : unmanaged
    {
        public NativeArray<T> Array;
        public T Value;
        public void Execute(int startIndex, int count)
        {
            int end = startIndex + count;
            for (; startIndex < end; startIndex++)
            {
                Array[startIndex] = Value;
            }
        }
    }
    [BurstCompile]
    public struct ResetArrayJobParallelFor<T> : IJobParallelFor where T : unmanaged
    {
        public NativeArray<T> Array;
        public T Value;
        public void Execute(int index)
        {
            Array[index] = Value;
        }
    }
}
