//using System;
//using System.Collections.Generic;

//namespace CatFramework.Magics
//{
//    public class BluePrintProductionModuleCenter
//    {
//        List<IBluePrintProductionModule> modules;
//        public void AddModule(IBluePrintProductionModule module)
//        {
//            if (module == null) return;
//            modules.Add(module);
//        }
//        public void RemoveMoudule(IBluePrintProductionModule module)
//        {
//            modules.Remove(module);
//        }
//        public void Set(IBluePrint bluePrint)
//        {
//            bool h = false;
//            for (int i = 0; i < modules.Count; i++)
//            {
//                IBluePrintProductionModule module = modules[i];
//                if (h)
//                    module.Hide();
//                else if (module.IsUsable && module.Set(bluePrint))
//                {
//                    h = true;
//                }
//            }
//        }
//    }
//    public interface IBluePrintProductionModule : IModule
//    {
//        bool Set(IBluePrint bluePrint);
//        void Hide();
//    }
//    public class BluePrintInventory
//    {

//    }
//    public interface IBluePrint
//    {
//    }
//}
