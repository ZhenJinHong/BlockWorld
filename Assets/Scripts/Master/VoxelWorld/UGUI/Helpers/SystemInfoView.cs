//using CatDOTS.VoxelWorld;
//using CatFramework;
//using CatFramework.Tools;
//using CatFramework.UiMiao;
//using System;
//using System.Collections;
//using System.Text;
//using Unity.Entities;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace VoxelWorld.UGUICTR
//{
//    public class SystemInfoView : MonoBehaviour
//    {
//        [SerializeField] TextMiao systemInfoText;
//        StringBuilder stringBuilder = new StringBuilder();
//        VoxelWorldChunkSystem voxelWorldChunkSystem;
//        BuildChunkMeshSystem buildChunkMeshSystem;
//        Timer timer;
//        void Update()
//        {
//            if (timer.Ready())
//            {
//                timer.AppendDelay(0.5f);
//                stringBuilder.AppendLine("帧率:" + 1 / Time.deltaTime + " FPS");
//                if (voxelWorldChunkSystem != null)
//                {
//                    voxelWorldChunkSystem.GetInfo(stringBuilder);
//                }
//                else
//                {
//                    voxelWorldChunkSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<VoxelWorldChunkSystem>();
//                }
//                if (buildChunkMeshSystem != null)
//                {
//                    stringBuilder.AppendLine("---");
//                    buildChunkMeshSystem.GetInfo(stringBuilder);
//                }
//                else
//                {
//                    buildChunkMeshSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildChunkMeshSystem>();
//                }
//                systemInfoText.TextValue = stringBuilder.ToString();
//                stringBuilder.Clear();
//            }
//        }
//    }
//}