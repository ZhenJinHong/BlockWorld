using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Physics;
namespace CatFramework_TestDOTS
{
    [DisableAutoCreation]
    [BurstCompile]
    internal partial struct NativeArrayTestSystem : ISystem
    {
        NativeArray<ushort> _1;
        NativeArray<ushort> _2;
        //NativeArray<ushort> _3;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _1 = new NativeArray<ushort>(16777216, Allocator.Persistent);
            _2 = new NativeArray<ushort>(16777216, Allocator.Persistent);

            for (int i = 0; i < _1.Length; i++)
            {
                _1[i] = (ushort)i;
            }
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            NativeArray<ushort> temp = _1;
            _1 = _2;
            _2 = temp;
            state.Dependency = new CopyArrayTest()
            {
                _1 = _1,
                _2 = _2
            }.ScheduleBatch(_1.Length, _1.Length / 8, state.Dependency);
        }
    }
    [BurstCompile]
    struct CopyArrayTest : IJobParallelForBatch
    {
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<ushort> _1;
        [NativeDisableParallelForRestriction]
        public NativeArray<ushort> _2;
        public void Execute(int startIndex, int count)
        {
            var _1s = _1.GetSubArray(startIndex, count);
            var _2s = _2.GetSubArray(startIndex, count);
            _1s.CopyTo(_2s);
        }
    }
}
