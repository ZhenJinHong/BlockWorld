//using CatFramework.MapGenerate;
//using System.Collections;
//using UnityEngine;

//namespace Assets.Test
//{
//    [RequireComponent(typeof(MeshFilter))]
//    public class SquareGridChunkTest : MonoBehaviour
//    {
//        [SerializeField] VoxelWorld voxelWorld;
//        Mesh mesh;
//        void Start()
//        {
//            MeshFilter meshFilter = GetComponent<MeshFilter>();
//            mesh = new Mesh();
//            new SquareGridChunk(mesh, voxelWorld).SquareGrid();
//            meshFilter.mesh = mesh;
//        }
//        private void OnDestroy()
//        {
//            GetComponent<MeshFilter>().mesh = null;
//            mesh.Clear();
            
//        }
//    }
//}