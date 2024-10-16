using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    public class VoxelWorldSettings
    {
        public VoxelWorldDataBaseManaged.IDebugSetting DebugSetting { get; set; }
        public VoxelWorldDataBaseManaged.IVoxelWorldSetting VoxelWorldSetting { get; set; }
        //public WorldChunkSetting WorldChunkSetting { get; set; }
    }
}
