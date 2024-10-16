//using CatFramework.Tools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    public interface IWorldEntityFiller
//    {

//    }
//    public class WorldEntityFiller : IWorldEntityFiller
//    {
//        IEntityFiller[] entityFillers;
//        public int Length => entityFillers.Length;
//        public IEntityFiller this[int index] => entityFillers[index];
//        public WorldEntityFiller(IEntityFiller[] entityFillers)
//        {
//            this.entityFillers = entityFillers;
//        }
//    }
//    public class WorldEntityFillerDefinition : ScriptableObject
//    {
//        [SerializeField] BaseEntityFillerDefinition[] baseEntityFillerDefinitions;
//        public IWorldEntityFiller Create(uint seed, VoxelWorldDataBaseManaged dataBaseManaged)
//        {
//            var entityFillers = ListUtility.ArrayConverter(baseEntityFillerDefinitions, (s) => s.Create(seed, dataBaseManaged));
//            return new WorldEntityFiller(entityFillers);
//        }
//    }
//}
