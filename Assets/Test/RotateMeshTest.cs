//using CatDOTS.VoxelWorld;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;

//namespace Assets.Test
//{
//    public class RotateMeshTest : MonoBehaviour
//    {
//        [SerializeField] GameObject prefab;
//        [SerializeField] Mesh mesh;
//        [SerializeField] EE E;
//        enum EE : uint
//        {
//            w,
//            a,
//        }
//        //MeshRenderer meshRenderer;
//        //MeshFilter meshFilter;
//        //[SerializeField] Vector3 center;
//        [SerializeField] Vector3 dir;
//        //Mesh mesh2;
//        List<Vector3> verts = new List<Vector3>();
//        List<ushort> triangles = new List<ushort>();
//        List<Vector2> uvs = new List<Vector2>();
//        List<Vector3> normals = new List<Vector3>();

//        List<Vector3> verts1 = new List<Vector3>();
//        List<Vector3> normals1 = new List<Vector3>();
//        private void Start()
//        {
//            //meshRenderer = GetComponent<MeshRenderer>();
//            //meshFilter = GetComponent<MeshFilter>();
//            //mesh2 = new Mesh();
//            mesh.GetVertices(verts);
//            mesh.GetIndices(triangles, 0, false);
//            mesh.GetUVs(0, uvs);
//            mesh.GetNormals(normals);
//        }
//        public void R()
//        {
//            //verts1.Clear();
//            //normals1.Clear();
//            //Matrix4x4 matrix4X4 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(dir), new Vector3(1f, 1f, 1f));
//            ////Vector4 offset = matrix4X4 * center;
//            ////Quaternion q = Quaternion.Euler(dir);
//            //for (int i = 0; i < verts.Count; i++)
//            //{
//            //    //MeshRenderer.
//            //    //verts1.Add(q * verts[i]);
//            //    //normals1.Add(q * normals[i]);
//            //    verts1.Add(matrix4X4 * verts[i] + new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
//            //    normals1.Add(matrix4X4 * normals[i]);
//            //}
//            //mesh2.SetVertices(verts1);
//            //mesh2.SetIndices(triangles, MeshTopology.Triangles, 0);
//            //mesh2.SetUVs(0, uvs);
//            //mesh2.SetNormals(normals1);
//            //mesh2.RecalculateBounds();
//            //mesh2.RecalculateTangents();
//            //mesh2.RecalculateNormals();
//            //meshFilter.mesh = mesh2;
//        }
//        public void RR()
//        {
//            int y = 0;
//            for (; y != 4; y++)
//            {
//                Create(new Vector3(0, y, 0));
//                Create(new Vector3(2, y, 0));
//                //Create(new Vector3(0, y, 2));
//            }
//            Create(new Vector3(0, 0, 1));// 竖起绕Y轴
//            Create(new Vector3(0, 1, 1));
//            Create(new Vector3(2, 0, 1));
//            Create(new Vector3(2, 1, 1));
//            //for (; x != 4; x++)
//            //{
//            //    Create(new Vector3(x, y, z));
//            //}
//            //for (; z != 4; z++)
//            //{
//            //    Create(new Vector3(x, y, z));
//            //}
//        }
//        void Create(Vector3 vector3)
//        {
//            verts1.Clear();
//            normals1.Clear();
//            Matrix4x4 matrix4X4 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(vector3 * 90f), new Vector3(1f, 1f, 1f));
//            float4x4 matrix = float4x4.EulerZXY(math.radians(vector3 * 90f));
//            //Vector4 offset = matrix4X4 * center;
//            //Quaternion q = Quaternion.Euler(dir);
//            for (int i = 0; i < verts.Count; i++)
//            {
//                //MeshRenderer.
//                //verts1.Add(q * verts[i]);
//                //normals1.Add(q * normals[i]);
//                //verts1.Add(matrix4X4 * verts[i] + new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
//                //normals1.Add(matrix4X4 * normals[i]);
//                verts1.Add(math.rotate(matrix, verts[i]) + new float3(0.5f, 0.5f, 0.5f));
//                normals1.Add(math.rotate(matrix, normals[i]));
//            }
//            GameObject gameObject = Instantiate(prefab);

//            Mesh mesh = new Mesh();
//            mesh.SetVertices(verts1);
//            mesh.SetIndices(triangles, MeshTopology.Triangles, 0);
//            mesh.SetUVs(0, uvs);
//            mesh.SetNormals(normals1);
//            mesh.RecalculateBounds();

//            gameObject.GetComponent<MeshFilter>().mesh = mesh;
//            gameObject.transform.position = vector3 * 4f;
//        }
//    }
//}