//using System;
//using System.Collections;
//using Unity.Collections;
//using UnityEngine;
//using UnityEngine.VFX;
//using UnityEngine.VFX.Utility;

//namespace Assets.Scripts.Master.Test
//{
//    [RequireComponent(typeof(VisualEffect))]
//    public class VFXSendEventText : MonoBehaviour
//    {
//        NativeArray<Vector3> vector3s;
//        VisualEffect visualEffect;
//        VFXEventAttribute eventAttribute;
//        int playEventID = Shader.PropertyToID("OnPlay");
//        int spawnCoordID = Shader.PropertyToID("SpawnCoord");
//        // Use this for initialization
//        void Start()
//        {
//            vector3s = new NativeArray<Vector3>(128, Allocator.Persistent);
//            Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
//            for (int i = 0; i < vector3s.Length; i++)
//            {
//                vector3s[i] = random.NextFloat3(0f, 100f);
//                //Debug.Log(vector3s[i]);
//            }
//            visualEffect = GetComponent<VisualEffect>();
//            Debug.Log(visualEffect == null);
//            //playEventID = Shader.PropertyToID("OnPlay");
//            //spawnCoordID = Shader.PropertyToID("SpawnCoord");
//            eventAttribute = visualEffect.CreateVFXEventAttribute();
//            Debug.Log("该特效有属性SpawnCoord：" + eventAttribute.HasVector3(spawnCoordID));
//        }
//        private void OnDestroy()
//        {
//            vector3s.Dispose();
//        }
//        // Update is called once per frame
//        void FixedUpdate()
//        {
//            for (int i = 0; i < vector3s.Length; i++)
//            {
//                eventAttribute.SetVector3(spawnCoordID, vector3s[i]);
//                visualEffect.SendEvent(playEventID, eventAttribute);
//            }
//        }
//    }
//}