using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    public interface IWorldDefinition
    {
        string Name { get; }
        IWorldGenerator Create(uint seed, VoxelWorldDataBaseManaged dataBase);
    }
}
