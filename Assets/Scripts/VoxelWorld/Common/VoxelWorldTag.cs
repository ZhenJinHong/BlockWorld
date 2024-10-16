using System;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 这个是关键，所有体素系统都要添加为此更新
    /// </summary>
    public struct VoxelWorldTag : IComponentData// TODO修改成世界观察者
    {
    }
}
