//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CatFramework.UiMiao
//{
//    public class ModuleCenter : MonoBehaviour
//    {
//        List<IModule> dataInteractionModules = new List<IModule>();
//        IEnableIterateModule enableIterateModule;
//        IEnableIterateModule EnableIterateModule
//        {
//            set
//            {
//                enabled = value != null;
//                if (enableIterateModule != null && enableIterateModule.IsUsable)
//                {
//                    enableIterateModule.EndUpdate();
//                }
//                enableIterateModule = value;
//            }
//        }
//        private void Update()
//        {
//            if (enableIterateModule != null)
//            {
//                if (!enableIterateModule.IsUsable || !enableIterateModule.Update())
//                {
//                    EnableIterateModule = null;
//                }
//            }
//        }
//    }
//}
