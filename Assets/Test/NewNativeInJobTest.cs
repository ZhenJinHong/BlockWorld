//using System.Collections;
//using Unity.Collections;
//using Unity.Jobs;
//using UnityEngine;

//namespace Assets.Test
//{
//    public class NewNativeInJobTest : MonoBehaviour
//    {
//        public void C()
//        {
//            new QJob().Schedule().Complete();
//        }
//        struct QJob : IJob
//        {
//            public void Execute()
//            {
//                NativeList<int> ints = new NativeList<int>(Allocator.Temp);
//                ints.Add(0);
//                ints.Add(1);
//                for (int i = 0; i < ints.Length; i++)
//                {
//                    Debug.Log($"{ints[i]}");
//                }
//            }
//        }
//    }
//}