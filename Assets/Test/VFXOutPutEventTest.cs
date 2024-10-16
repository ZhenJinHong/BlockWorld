//using System.Collections;
//using UnityEngine;
//using UnityEngine.VFX;
//using UnityEngine.VFX.Utility;

//namespace Assets.Scripts.Master.Test
//{
//    public class VFXOutPutEventTest : VFXOutputEventAbstractHandler
//    {
//        int spawnCoordID = Shader.PropertyToID("SpawnCoord");
//        public override bool canExecuteInEditor => false;

//        public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
//        {
//            Debug.Log("SpawnCoord:" + eventAttribute.GetVector3(spawnCoordID));
//        }
//    }
//}