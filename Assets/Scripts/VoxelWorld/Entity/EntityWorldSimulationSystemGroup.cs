//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Entities;
//using Unity.Physics.Systems;
//using UnityEngine.Scripting;

//namespace CatDOTS.VoxelWorld
//{
//    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
//    public partial class EntityWorldSimulationSystemGroup : ComponentSystemGroup
//    {
//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            //World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PhysicsSystemGroup>().Enabled = false;
//        }
//        [Preserve]
//        public EntityWorldSimulationSystemGroup() { }
//    }
//}
