//using CatDOTS.AssetCollection;
//using CatDOTS;
//using Master;
//using System.Collections;
//using UnityEngine;
//using System.Linq;
//using Unity.Entities;
//using Unity.Physics;
//using Unity.Transforms;
//using Unity.Collections;

//namespace Assets.Test
//{
//    public class SpawnTest : MonoBehaviour
//    {
//        PrefabData[] prefabDatas;
//        PrefabData spawnData;
//        // Use this for initialization
//        void Start()
//        {
//            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
//            prefabDatas = ECSEntry.PrefabCategoryCollection.PrefabDataSet.Values.ToArray();
//            for (int i = 0; i < prefabDatas.Length; i++)
//            {
//                if (entityManager.HasComponent<PhysicsCollider>(prefabDatas[i].Prefab))
//                {
//                    spawnData = prefabDatas[i];
//                    break;
//                }
//            }
//            spawnData ??= prefabDatas[0];
//        }
//        int x;
//        int y;
//        int count = 128 * 128;
//        // Update is called once per frame
//        void LateUpdate()
//        {
//            if (spawnData == null)
//            {
//                enabled = false;
//                return;
//            }
//            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
//            NativeArray<Entity> entities = new NativeArray<Entity>(32, Allocator.Temp);
//            entityManager.Instantiate(spawnData.Prefab, entities);
//            for (int i = 0; i < entities.Length; i++)
//            {
//                Entity entity = entities[i];
//                entityManager.SetComponentData<LocalTransform>(entity, new LocalTransform()
//                {
//                    Position = new Unity.Mathematics.float3(x, 0, y),
//                    Rotation = Quaternion.identity,
//                    Scale = 1f,
//                });
//                x++;
//                if (x == 128)
//                {
//                    x = 0;
//                    y++;
//                }
//                count--;
//                if (count <= 0)
//                {
//                    enabled = false;
//                    break;
//                }
//            }
//        }
//    }
//}