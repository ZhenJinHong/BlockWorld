using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public static class BixelFunction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BixelIsReady(ref Bixel bixel, in float curTime)
        {
            if (bixel.TargetTime < curTime)
            {
                bixel.TargetTime = curTime + bixel.Delay;
                return true;
            }
            return false;
        }
    }
}
