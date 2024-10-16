using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    public interface IWorldFillerDefinition
    {
        IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase);
    }
}
