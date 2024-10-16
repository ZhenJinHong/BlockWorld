//using System.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using UnityEngine;
//using Unity.Entities.Graphics;
//using Unity.Rendering;

//namespace CatFramework_TestDOTS
//{
//    [DisableAutoCreation]
//    public partial class ChunkMeshSpawnTest : SystemBase
//    {
//        protected override void OnCreate()
//        {
//            base.OnCreate();

//            for (int i = 0; i < 10; i++)
//            {
//                for (int j = 0; j < 10; j++)
//                {
//                    CreateChunk(new float3(i, 0, j));
//                }
//            }
//        }
//        void CreateChunk(float3 chunkCoord)
//        {
//            Entity entity = EntityManager.CreateEntity();
//            Mesh mesh = new Mesh();

//            var entitiesGraphicSystem = World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
//            //entitiesGraphicSystem.
//            //UnityEngine.Graphics.
//            entitiesGraphicSystem.RegisterMesh(mesh);
//            RenderMeshArray renderMeshArray = new RenderMeshArray(); renderMeshArray.ResetHash128();
//            RenderMeshDescription renderMeshDescription = new RenderMeshDescription(UnityEngine.Rendering.ShadowCastingMode.Off, false);

//            //RenderMeshUtility.AddComponents(entity,EntityManager,in renderMeshDescription,)
//        }
//        protected override void OnUpdate()
//        {

//        }
//    }
//}