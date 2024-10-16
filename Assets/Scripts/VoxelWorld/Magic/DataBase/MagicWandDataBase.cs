using CatFramework.Magics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld.Magics
{
    public interface IMagicWandDataBase
    {
        IMagicWandDefinition this[int index] { get; }
    }
    public class MagicWandDataBase : IMagicWandDataBase
    {
        List<IMagicWandDefinition> magicWandDefinitions;
        public IMagicWandDefinition this[int index] => magicWandDefinitions[index];
        public MagicWandDataBase(IList<IMagicWandDefinition> magicWandDefinitions)
        {


            this.magicWandDefinitions = new List<IMagicWandDefinition>();
            for (int i = 0; i < magicWandDefinitions.Count; i++)
            {
                this.magicWandDefinitions.Add(magicWandDefinitions[i]);
            }
        }
        public void Dispose()
        {

        }
    }
}
