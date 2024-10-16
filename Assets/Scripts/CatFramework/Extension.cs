using System;
using System.Runtime.CompilerServices;

namespace CatFramework
{
    public static class Extension
    {
        //public static void SaveToFile(this ISetting setting)
        //{
        //    DataCollection.SaveSetting(setting);
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUsableSafe(this IModule module)
            => module != null && module.IsUsable;
    }
}
