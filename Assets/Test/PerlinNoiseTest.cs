//using CatFramework.CatMath;
//using CatDOTS.VoxelWorld;
//using System;
//using System.Collections;
//using Unity.Mathematics;
//using UnityEngine;

//namespace Assets.Test
//{
//    public class PerlinNoiseTest : MonoBehaviour
//    {
//        void Update()
//        {
//            //switch (noiseType)
//            //{
//            //    case Noise.CNoise:
//            //        Debug.Log(noise.cnoise(new float2(x, y)));
//            //        break;
//            //    case Noise.SNoise:
//            //        Debug.Log(noise.snoise(new float2(x, y)));
//            //        break;
//            //    case Noise.MathFNoise:
//            //        Debug.Log(Mathf.PerlinNoise(x, y));
//            //        break;
//            //}

//            //x += Time.deltaTime;
//            //y += Time.deltaTime;
//            float w = (byte)DateTime.Now.Ticks;
//            Debug.Log(CatDOTS.VoxelWorld.Noise.GetPerlinNoise(w, 0, 2));
//            Debug.Log(w);
//        }
//        void EE()
//        {

//        }
//    }
//}