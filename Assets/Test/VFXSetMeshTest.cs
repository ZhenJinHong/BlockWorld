//using System;
//using System.Collections;
//using Unity.Collections;
//using Unity.Mathematics;
//using UnityEngine;
//using UnityEngine.VFX;

//namespace Assets.Scripts.Master.Test
//{
//    public class VFXSetMeshTest : MonoBehaviour
//    {
//        [SerializeField]
//        float3 rangeMin = new float3(-50, 0f, -50f);
//        [SerializeField]
//        float3 rangeMax = new float3(50f, 1f, 50);
//        public float maxColor = 1f;
//        public int spawnCount = 128;
//#if UNITY_EDITOR
//        public bool printCount;
//#endif
//        Mesh mesh;
//        VisualEffect visualEffect;
//        Transform main;
//        NativeArray<float3> coords;
//        Color[] colors;
//        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
//        int playEventID = Shader.PropertyToID("OnPlay");
//        int mapID = Shader.PropertyToID("Map");

//        void Start()
//        {
//            visualEffect = GetComponent<VisualEffect>();
//            main = Camera.main.transform;

//            mesh = new Mesh();
//            coords = new NativeArray<float3>(spawnCount, Allocator.Persistent);
//            float4 min = 0.0f;
//            float4 max = maxColor;
//            colors = new Color[spawnCount];
//            for (int i = 0; i < spawnCount; i++)
//            {
//                colors[i] = Float4ToColor(random.NextFloat4(min, max));
//            }
//            mesh.SetVertices<float3>(coords);
//            mesh.colors = colors;
//            visualEffect.SetMesh(mapID, mesh);
//        }
//        Color Float4ToColor(float4 color) => new Color(color.x, color.y, color.z, color.w);
//        private void OnDestroy()
//        {
//            coords.Dispose();
//        }
//        private void Update()
//        {
//            transform.position = main.position;
//        }
//        // Update is called once per frame
//        void FixedUpdate()
//        {
//            for (int i = 0; i < spawnCount; i++)
//            {
//                coords[i] = random.NextFloat3(rangeMin, rangeMax);
//            }
//            mesh.SetVertices<float3>(coords);//不重新传入的话，是不会更新的
//            visualEffect.SendEvent(playEventID);
//#if UNITY_EDITOR
//            if (printCount)
//            {
//                Debug.Log("该粒子数量：" + visualEffect.aliveParticleCount);
//            }
//#endif
//        }
//    }
//}