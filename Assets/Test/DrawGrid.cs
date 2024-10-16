//using System.Collections;
//using UnityEngine;

//namespace Assets.Test
//{
//    public class DrawGrid : MonoBehaviour
//    {
//        public int count = 128;
//        private void OnDrawGizmos()
//        {
//            for (int i = 0; i < count; i++)
//            {
//                for (int x = 0; x < count; x++)
//                {
//                    Debug.DrawLine(new Vector3(x, 0f, i * count), new Vector3(x + 1, 0f, i * count));
//                    Debug.DrawLine(new Vector3(x, 0f, i * count), new Vector3(x, 0f, i * count + 1));
//                }
//            }
//        }
//    }
//}