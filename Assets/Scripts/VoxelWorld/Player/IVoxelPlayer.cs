using CatFramework.CatEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld.Player
{
    public interface IVoxelPlayer : IComponent
    {
        bool Enable { get; }
        IVoxelRayResult VoxelRayResult { get; }
        void AddVoxel(Voxel voxel);
    }
}
