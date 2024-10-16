//using CatDOTS.VoxelWorld;
//using CatFramework;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Collections;
//using Unity.Mathematics;
//using UnityEngine;
//using static CatDOTS.VoxelWorld.VoxelWorldDataBaseManaged;

//namespace Assets.Test
//{
//    public class SortVoxelShapeAssetTest : MonoBehaviour
//    {
//        //[SerializeField] DataBaseDefinition dataBase;
//        //[SerializeField] Material material;
//        //[SerializeField] GameObject frontO;
//        //[SerializeField] GameObject backO;
//        //List<TempVoxelShapeData> shapesTempForJob = new List<TempVoxelShapeData>();
//        //List<float3> vertsTempForJob = new List<float3>();
//        //List<int> trianglesTempForJob = new List<int>();
//        //List<float2> uvsTempForJob = new List<float2>();
//        //List<float3> normalsTempForJob = new List<float3>();

//        //List<VoxelFaceData> 有序临时面数据数组 = new List<VoxelFaceData>();
//        //List<float3> 有序临时顶点数组 = new List<float3>();
//        //List<ushort> 有序临时索引数组 = new List<ushort>();
//        //List<float2> 有序临时UV数组 = new List<float2>();
//        //List<float3> 有序临时法向数组 = new List<float3>();

//        //Dictionary<int, ushort> 旧新顶点索引查找图 = new Dictionary<int, ushort>();

//        //const float minThreshold = -0.48f;
//        //const float maxThreshold = 0.48f;
//        //public void Execute()
//        //{
//        //    VoxelShapeDefinition[] voxelShapeDefinitions =new VoxelShapeDefinition[0];
//        //    if (voxelShapeDefinitions.Length == 0) return;

//        //    List<Vector3> verts = new List<Vector3>();
//        //    List<ushort> triangles = new List<ushort>();
//        //    List<Vector2> uvs = new List<Vector2>();
//        //    List<Vector3> normals = new List<Vector3>();
//        //    Read(voxelShapeDefinitions[0]);
//        //    Read(voxelShapeDefinitions[1]);
//        //    void Read(VoxelShapeDefinition normal)
//        //    {
//        //        Mesh normalmesh = normal.Mesh;
//        //        // 获取模型的数据
//        //        normalmesh.GetVertices(verts);
//        //        normalmesh.GetIndices(triangles, 0);
//        //        normalmesh.GetUVs(0, uvs);
//        //        normalmesh.GetNormals(normals);
//        //        // 新的模型的顶点索引从上次结束位置
//        //        TempVoxelShapeData voxelShapeData = new TempVoxelShapeData
//        //        {
//        //            IndexStartIndex = trianglesTempForJob.Count,
//        //            VertexStartIndex = vertsTempForJob.Count,

//        //            FrontRect = normal.FrontRect,
//        //            BackRect = normal.BackRect,
//        //            TopRect = normal.TopRect,
//        //            BottomRect = normal.BottomRect,
//        //            RightRect = normal.RightRect,
//        //            LeftRect = normal.LeftRect,
//        //        };

//        //        for (int i = 0; i != verts.Count; i++)
//        //        {
//        //            vertsTempForJob.Add(verts[i]);
//        //            uvsTempForJob.Add(uvs[i]);
//        //            normalsTempForJob.Add(normals[i]);
//        //        }
//        //        for (int i = 0; i != triangles.Count; i++)
//        //        {
//        //            trianglesTempForJob.Add(triangles[i]);// 保持着网格原本的三角形索引数值，不要加，因为在临时面数据里已经放置了基础顶点索引
//        //        }
//        //        voxelShapeData.IndexEnd = trianglesTempForJob.Count;
//        //        voxelShapeData.VertexEnd = vertsTempForJob.Count;
//        //        shapesTempForJob.Add(voxelShapeData);

//        //        verts.Clear();
//        //        triangles.Clear();
//        //        uvs.Clear();
//        //        normals.Clear();
//        //    }
//        //    List<int> left = new List<int>();
//        //    List<int> right = new List<int>();
//        //    List<int> top = new List<int>();
//        //    List<int> bottom = new List<int>();
//        //    List<int> front = new List<int>();
//        //    List<int> back = new List<int>();
//        //    List<int> notFit = new List<int>();

//        //    for (int i = 0; i < shapesTempForJob.Count; i++)
//        //    {
//        //        TempVoxelShapeData normalShapeData = shapesTempForJob[i];
//        //        // 要注意的是这里计算索引，本身是基于原本网格的
//        //        CalculateShapeData(in normalShapeData, front, back, top, bottom, right, left, notFit);
//        //    }


//        //    int x = 0; int y = 0;
//        //    for (int i = 0; i < 有序临时面数据数组.Count; i += VoxelFaceData.FaceCountInSingleShape)
//        //    {
//        //        Debug.Log("实例化了");
//        //        GameObject testQube = UnityEngine.Object.Instantiate(dataBase.chunkObjectPrefab);
//        //        testQube.transform.position = new Vector3(x, y, 0);
//        //        MeshFilter meshFilter = testQube.GetComponent<MeshFilter>();
//        //        MeshRenderer meshRenderer = testQube.GetComponent<MeshRenderer>();
//        //        Mesh mesh = new Mesh();
//        //        meshFilter.mesh = mesh;
//        //        meshRenderer.material = material;

//        //        for (int j = 0; j < VoxelFaceData.FaceCountInSingleShape; j++)
//        //        {
//        //            int vertexIndex = verts.Count;
//        //            VoxelFaceData voxelFaceData = 有序临时面数据数组[j + i];
//        //            int indexStartindex = voxelFaceData.IndexStartIndex;
//        //            int indexEnd = voxelFaceData.IndexEnd;
//        //            int vertexStartIndex = voxelFaceData.VertexStartIndex;
//        //            int vertexEnd = voxelFaceData.VertexEnd;
//        //            for (; indexStartindex < indexEnd; indexStartindex++)
//        //            {
//        //                triangles.Add((ushort)(vertexIndex + 有序临时索引数组[indexStartindex]));
//        //            }
//        //            for (; vertexStartIndex < vertexEnd; vertexStartIndex++)
//        //            {
//        //                verts.Add(有序临时顶点数组[vertexStartIndex]);
//        //                float2 uv = 有序临时UV数组[vertexStartIndex];
//        //                uvs.Add(uv);
//        //                normals.Add(有序临时法向数组[vertexStartIndex]);
//        //            }
//        //        }
//        //        mesh.SetVertices(verts);
//        //        mesh.SetTriangles(triangles, 0);
//        //        mesh.SetUVs(0, uvs);
//        //        mesh.SetNormals(normals);
//        //        mesh.RecalculateBounds();

//        //        verts.Clear();
//        //        uvs.Clear();
//        //        normals.Clear();
//        //        triangles.Clear();

//        //        x++;
//        //        if (x == 10)
//        //        {
//        //            x = 0; y++;
//        //        }
//        //    }
//        //}
//        //void Instantiate(float3 pos, GameObject prefab, int index)
//        //{
//        //    GameObject gameObject = Instantiate(prefab);
//        //    gameObject.transform.position = pos;
//        //    gameObject.name = prefab.name + index;
//        //}
//        ///// <summary>
//        ///// 计算正向/左上角形状，将网格数据划分各面中
//        ///// </summary>
//        //void CalculateShapeData(in TempVoxelShapeData normal, List<int> front, List<int> back, List<int> top, List<int> bottom, List<int> right, List<int> left, List<int> notFit)
//        //{
//        //    front.Clear(); back.Clear(); top.Clear(); bottom.Clear(); right.Clear(); left.Clear(); notFit.Clear();
//        //    int baseVertexIndex = normal.VertexStartIndex;
//        //    Debug.Log($"基础顶点索引:{baseVertexIndex}");
//        //    Debug.Log($"索引的索引:{normal.IndexStartIndex}");
//        //    // 将已列入索引数据的三角形索引，重新按照归属面排序
//        //    for (int startIndex = normal.IndexStartIndex; startIndex < normal.IndexEnd; startIndex += 3)
//        //    {
//        //        float3 v1 = vertsTempForJob[trianglesTempForJob[startIndex] + baseVertexIndex];
//        //        float3 v2 = vertsTempForJob[trianglesTempForJob[startIndex + 1] + baseVertexIndex];
//        //        float3 v3 = vertsTempForJob[trianglesTempForJob[startIndex + 2] + baseVertexIndex];
//        //        float3 xf = new float3(v1.x, v2.x, v3.x);
//        //        float3 yf = new float3(v1.y, v2.y, v3.y);
//        //        float3 zf = new float3(v1.z, v2.z, v3.z);

//        //        if (math.all(zf > maxThreshold))// 在前面 顺Z轴
//        //        {
//        //            Debug.Log($"{v1};{v2};{v3};在前面");
//        //            Instantiate(v1, frontO, startIndex);
//        //            Instantiate(v2, frontO, startIndex + 1);
//        //            Instantiate(v3, frontO, startIndex + 2);
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref front, startIndex, baseVertexIndex);
//        //        }
//        //        else if (math.all(zf < minThreshold))// 在背面
//        //        {
//        //            Debug.Log($"{v1};{v2};{v3};在背面");
//        //            Instantiate(v1, backO, startIndex);
//        //            Instantiate(v2, backO, startIndex + 1);
//        //            Instantiate(v3, backO, startIndex + 2);
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref back, startIndex, baseVertexIndex);
//        //        }
//        //        else if (math.all(yf > maxThreshold))// 在上面
//        //        {
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref top, startIndex, baseVertexIndex);
//        //        }
//        //        else if (math.all(yf < minThreshold))// 在下面
//        //        {
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref bottom, startIndex, baseVertexIndex);
//        //        }
//        //        else if (math.all(xf > maxThreshold))// 在右面
//        //        {
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref right, startIndex, baseVertexIndex);
//        //        }
//        //        else if (math.all(xf < minThreshold))// 在左面
//        //        {
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref left, startIndex, baseVertexIndex);
//        //        }
//        //        else
//        //        {
//        //            AddTriangleToTempFaceList(in trianglesTempForJob, ref notFit, startIndex, baseVertexIndex);
//        //        }
//        //    }
//        //    // 每个面的三角形索引需要从0开始
//        //    // 三角面索引是不能排序的，每三个排序呢？
//        //    SortTriangle(in front, in normal.FrontRect);
//        //    SortTriangle(in back, in normal.BackRect);
//        //    SortTriangle(in top, in normal.TopRect);
//        //    SortTriangle(in bottom, in normal.BottomRect);
//        //    SortTriangle(in right, in normal.RightRect);
//        //    SortTriangle(in left, in normal.LeftRect);
//        //    SortTriangle(in notFit, FaceRect.None);
//        //}
//        //static void AddTriangleToTempFaceList(in List<int> ori, ref List<int> faceVertexIndexList, int start, int baseVertexIndex)
//        //{
//        //    faceVertexIndexList.Add(ori[start] + baseVertexIndex);
//        //    faceVertexIndexList.Add(ori[start + 1] + baseVertexIndex);
//        //    faceVertexIndexList.Add(ori[start + 2] + baseVertexIndex);
//        //}
//        //// 
//        //void SortTriangle(in List<int> faceTriangle, in FaceRect faceRect)
//        //{
//        //    旧新顶点索引查找图.Clear();
//        //    ushort 新相对顶点索引 = 0;
//        //    VoxelFaceData voxelFaceData = new VoxelFaceData()
//        //    {
//        //        FaceRect = faceRect,
//        //        IndexStartIndex = 有序临时索引数组.Count,
//        //        VertexStartIndex = 有序临时顶点数组.Count,
//        //    };
//        //    for (int i = 0; i < faceTriangle.Count; i++)
//        //    {
//        //        int 旧顶点索引 = faceTriangle[i];
//        //        if (!旧新顶点索引查找图.TryGetValue(旧顶点索引, out ushort 已有的新索引))
//        //        {
//        //            已有的新索引 = 新相对顶点索引;
//        //            有序临时顶点数组.Add(vertsTempForJob[旧顶点索引]);
//        //            有序临时UV数组.Add(uvsTempForJob[旧顶点索引]);
//        //            有序临时法向数组.Add(normalsTempForJob[旧顶点索引]);
//        //            旧新顶点索引查找图[旧顶点索引] = 已有的新索引;
//        //            新相对顶点索引++;
//        //        }
//        //        有序临时索引数组.Add(已有的新索引);
//        //    }
//        //    voxelFaceData.IndexEnd = 有序临时索引数组.Count;
//        //    voxelFaceData.VertexEnd = 有序临时顶点数组.Count;
//        //    有序临时面数据数组.Add(voxelFaceData);
//        //    //for (int i = voxelFaceData.IndexStartIndex; i < voxelFaceData.IndexEnd; i++)
//        //    //{
//        //    //    Debug.Log($"排序后的索引:{有序临时索引数组[i]}"); //指向: { 有序临时顶点数组[有序临时索引数组[i] +]}
//        //    //}
//        //}
//    }
//}