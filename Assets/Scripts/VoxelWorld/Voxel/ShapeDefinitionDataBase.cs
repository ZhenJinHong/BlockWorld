using CatFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IShapeDefinitionDataBase
    {
        VoxelShapeColliderAsset VoxelShapeColliderAsset { get; }
        BlobAssetReference<VoxelShapeBlobAsset> VoxelShapeBlobAsset { get; }
        IReadOnlyList<VoxelShapeClassify> VoxelShapeClassifies { get; }

        IVoxelShapeInfo GetDefaultVoxelShapeInfo();
        ushort GetVoxelShapeIndex(string name);
        IVoxelShapeInfo GetVoxelShapeInfo(string name);
    }
    public class VoxelShapeClassify
    {
        public readonly string ClassifyName;
        readonly IVoxelShapeInfo[] shapeDefinitions;
        public IVoxelShapeInfo this[int index] => shapeDefinitions[index];
        public int Length => shapeDefinitions.Length;
        public IReadOnlyList<IVoxelShapeInfo> ShapeDefinitions => shapeDefinitions;
        public VoxelShapeClassify(string classifyName, IVoxelShapeInfo[] shapeDefinitions)
        {
            ClassifyName = classifyName;
            this.shapeDefinitions = shapeDefinitions;
        }
    }
    public interface IVoxelShapeInfo
    {
        public Texture2D Icon { get; }
        public string Name { get; }
        public string ClassifyName { get; }
        public VoxelShapeHeader Header { get; }
        ushort ShapeIndex { get; }
    }
    public class ShapeDefinitionDataBase : IShapeDefinitionDataBase
    {
        public class InternalShapeDefinition : IVoxelShapeInfo
        {
            public Texture2D Icon { get; private set; }
            public string Name { get; private set; }
            public string ClassifyName { get; private set; }
            public VoxelShapeHeader Header { get; private set; }
            public ushort ShapeIndex => Header.ShapeIndex;

            public Texture2D ItemImage => Icon;

            public Texture2D CornerImage => null;

            public string Label => Name;

            public static InternalShapeDefinition Create(IVoxelShapeDefinition voxelShapeDefinition, VoxelShapeHeader voxelShapeHeader)
            {
                InternalShapeDefinition internalShapeDefinition = new InternalShapeDefinition
                {
                    Icon = voxelShapeDefinition.Icon,
                    Name = voxelShapeDefinition.Name,
                    ClassifyName = voxelShapeDefinition.ClassifyName,
                    Header = voxelShapeHeader
                };
                return internalShapeDefinition;
            }
        }
        BlobAssetReference<VoxelShapeBlobAsset> voxelShapeBlobAsset;
        public BlobAssetReference<VoxelShapeBlobAsset> VoxelShapeBlobAsset
            => voxelShapeBlobAsset;
        NativeArray<BlobAssetReference<Unity.Physics.Collider>> solidColliderAsset;
        BlobAssetReference<Unity.Physics.Collider> nonSolidCube;
        public VoxelShapeColliderAsset VoxelShapeColliderAsset => new VoxelShapeColliderAsset(nonSolidCube, solidColliderAsset.AsReadOnly());
        public void Dispose()
        {
            voxelShapeBlobAsset.Dispose();
            solidColliderAsset.Dispose();// 内部Blob引用无法调用dispose,会提示释放了,是自动了?
            nonSolidCube.Dispose();
        }
        VoxelShapeClassify[] voxelShapeClassifies;
        Dictionary<string, InternalShapeDefinition> shapeNameMap;
        public IReadOnlyList<VoxelShapeClassify> VoxelShapeClassifies => voxelShapeClassifies;
        class ShapeDataTempContainer
        {
            readonly string debugInfo;
            internal NativeList<TempVoxelShapeData> shapesTempForJob;
            internal NativeList<int> indexsTempForJob;
            internal NativeList<float3> vertsTempForJob;
            internal NativeList<float2> uvsTempForJob;
            internal NativeList<float3> normalsTempForJob;

            internal NativeList<VoxelFaceData> 有序临时面数据数组;
            internal NativeList<ushort> 有序临时索引数组;
            internal NativeList<float3> 有序临时顶点数组;
            internal NativeList<float2> 有序临时UV数组;
            internal NativeList<float3> 有序临时法向数组;

            NativeHashMap<int, ushort> 旧新顶点索引查找图;
            internal ShapeDataTempContainer(string debugInfo)
            {
                shapesTempForJob = new NativeList<TempVoxelShapeData>(Allocator.TempJob);
                indexsTempForJob = new NativeList<int>(Allocator.TempJob);
                vertsTempForJob = new NativeList<float3>(Allocator.TempJob);
                uvsTempForJob = new NativeList<float2>(Allocator.TempJob);
                normalsTempForJob = new NativeList<float3>(Allocator.TempJob);

                有序临时面数据数组 = new NativeList<VoxelFaceData>(Allocator.TempJob);
                有序临时索引数组 = new NativeList<ushort>(Allocator.TempJob);
                有序临时顶点数组 = new NativeList<float3>(Allocator.TempJob);
                有序临时UV数组 = new NativeList<float2>(Allocator.TempJob);
                有序临时法向数组 = new NativeList<float3>(Allocator.TempJob);

                旧新顶点索引查找图 = new NativeHashMap<int, ushort>(64, Allocator.TempJob);
                this.debugInfo = debugInfo;
            }
            public JobHandle SortJob(JobHandle dependOn = default)
            {
                return new SortVoxelShapeAssetJob()
                {
                    shapesTempForJob = shapesTempForJob,
                    trianglesTempForJob = indexsTempForJob,
                    vertsTempForJob = vertsTempForJob,
                    uvsTempForJob = uvsTempForJob,
                    normalsTempForJob = normalsTempForJob,

                    有序临时面数据数组 = 有序临时面数据数组,
                    有序临时索引数组 = 有序临时索引数组,
                    有序临时顶点数组 = 有序临时顶点数组,
                    有序临时UV数组 = 有序临时UV数组,
                    有序临时法向数组 = 有序临时法向数组,

                    旧新顶点索引查找图 = 旧新顶点索引查找图,
                }.Schedule(dependOn);
            }
            public void Dispose()
            {
                shapesTempForJob.Dispose();
                indexsTempForJob.Dispose();
                vertsTempForJob.Dispose();
                uvsTempForJob.Dispose();
                normalsTempForJob.Dispose();
                有序临时面数据数组.Dispose();
                有序临时索引数组.Dispose();
                有序临时顶点数组.Dispose();
                有序临时UV数组.Dispose();
                有序临时法向数组.Dispose();
                旧新顶点索引查找图.Dispose();
            }
            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(debugInfo);
                stringBuilder.AppendLine($"临时形状数据:{shapesTempForJob.Length}");
                stringBuilder.AppendLine($"临时顶点索引:{indexsTempForJob.Length}");
                stringBuilder.AppendLine($"临时顶点:{vertsTempForJob.Length}");
                stringBuilder.AppendLine($"临时UV:{uvsTempForJob.Length}");
                stringBuilder.AppendLine($"临时法向:{normalsTempForJob.Length}");
                stringBuilder.AppendLine($"有序面数据:{有序临时面数据数组.Length}");
                stringBuilder.AppendLine($"有序顶点索引:{有序临时索引数组.Length}");
                stringBuilder.AppendLine($"有序顶点:{有序临时顶点数组.Length}");
                stringBuilder.AppendLine($"有序UV:{有序临时UV数组.Length}");
                stringBuilder.AppendLine($"有序法向:{有序临时法向数组.Length}");
                return stringBuilder.ToString();
            }
        }
        class ShapeDataReader
        {
            IVoxelShapeDefinition defaultShape;
            VoxelShapeElement defaultElement;
            internal Dictionary<string, (VoxelShapeHeader, IVoxelShapeDefinition)> voxelShapeHeaderMap;
            List<IVoxelShapeDefinition> multiElementDefinitions;
            List<Vector3> verts;
            List<ushort> triangles;
            List<Vector2> uvs;
            List<Vector3> normals;
            public ShapeDataReader(IVoxelShapeDefinition defaultShape)
            {
                this.defaultShape = defaultShape;
                defaultElement = defaultShape.VoxelShapes[0];

                voxelShapeHeaderMap = new Dictionary<string, (VoxelShapeHeader, IVoxelShapeDefinition)>();
                multiElementDefinitions = new List<IVoxelShapeDefinition>();

                verts = new List<Vector3>();
                triangles = new List<ushort>();
                uvs = new List<Vector2>();
                normals = new List<Vector3>();
            }
            public void AddDefault(ShapeDataTempContainer container)
            {
                TryAddShapeCount(defaultShape);
                ReadSingleElement(defaultElement, container);
            }
            // 分类单个形状多方向,与多方向各自形状,并即刻读取单个形状多方向元素
            public void ReadFirstElement(IList<IVoxelShapeDefinition> VoxelShapeDefinitions, ShapeDataTempContainer container)
            {
                foreach (IVoxelShapeDefinition normal in VoxelShapeDefinitions)
                {
                    if (normal == null || normal.VoxelShapes == null || normal.VoxelShapes.Count == 0)
                    {
                        if (ConsoleCat.Enable) ConsoleCat.LogWarning("空的网格与面数据");
                        continue;
                    }
                    if (normal.VoxelShapes.Count == VoxelShapeHeader.ShapeDirectionCount)
                    {
                        multiElementDefinitions.Add(normal);
                        continue;
                    }
                    VoxelShapeElement shapeElement = normal.VoxelShapes[0];
                    if (CheckValid(shapeElement) && TryAddShapeCount(normal))
                    {
                        ReadSingleElement(shapeElement, container);
                    }
                }
            }
            public void FinishReadMultiElement(ShapeDataTempContainer container)// 要求形状里有8个子形状
            {
                foreach (IVoxelShapeDefinition normal in multiElementDefinitions)
                {
                    bool isValid = true;
                    for (int i = 0; i < VoxelShapeHeader.ShapeDirectionCount; i++)
                    {
                        if (!CheckValid(normal.VoxelShapes[i]))
                        {
                            isValid = false;
                            if (ConsoleCat.Enable)
                            {
                                ConsoleCat.LogWarning($"无效的多元素形状:{normal.Name}");
                            }
                            break;
                        }
                    }
                    if (isValid && TryAddShapeCount(normal))
                    {
                        for (int i = 0; i < VoxelShapeHeader.ShapeDirectionCount; i++)
                        {
                            ReadSingleElement(normal.VoxelShapes[i], container);
                        }
                    }
                }
                multiElementDefinitions.Clear();
            }
            bool CheckValid(VoxelShapeElement shapeElement)
            {
                return shapeElement != null && shapeElement.Mesh != null && shapeElement.Mesh.isReadable;
            }
            ushort shapeCount;
            bool TryAddShapeCount(IVoxelShapeDefinition shapeDef)
            {
                VoxelShapeHeader voxelShapeHeader = new VoxelShapeHeader()
                {
                    ShapeIndex = shapeCount,
                    // 每个形状有八个方向变体,每个变体七个面,所以形状句柄在面数据数组的每个开始都是当前形状索引*八个形状乘以7个面
                    //StartIndexInTotalFaceDataArray = VoxelShapeHeader.GetFaceDataStartIndexByShapeStartIndex(shapeCount),
                };
                if (voxelShapeHeaderMap.TryAdd(shapeDef.Name, (voxelShapeHeader, shapeDef)))
                {
                    shapeCount++;
                    return true;
                }
                else if (ConsoleCat.Enable)
                {
                    ConsoleCat.LogWarning($"发现重名的形状定义:{shapeDef.Name}");
                }
                return false;
            }
            void ReadSingleElement(VoxelShapeElement shapeElement, ShapeDataTempContainer container)
            {
                verts.Clear();
                triangles.Clear();
                uvs.Clear();
                normals.Clear();

                Mesh normalMesh = shapeElement.Mesh;
                // 获取模型的数据
                normalMesh.GetVertices(verts);
                normalMesh.GetIndices(triangles, 0, false);
                normalMesh.GetUVs(0, uvs);
                normalMesh.GetNormals(normals);
                // 新的模型的顶点索引从上次结束位置
                // start和end仅用于指定该形状的数据放在了哪一段
                TempVoxelShapeData voxelShapeData = new TempVoxelShapeData
                {
                    IndexStartIndex = container.indexsTempForJob.Length,
                    VertexStartIndex = container.vertsTempForJob.Length,

                    FrontRect = shapeElement.FrontRect,
                    BackRect = shapeElement.BackRect,
                    TopRect = shapeElement.TopRect,
                    BottomRect = shapeElement.BottomRect,
                    RightRect = shapeElement.RightRect,
                    LeftRect = shapeElement.LeftRect,
                };

                for (int i = 0; i != verts.Count; i++)
                {
                    container.vertsTempForJob.Add(verts[i]);
                    container.uvsTempForJob.Add(uvs[i]);
                    container.normalsTempForJob.Add(normals[i]);
                }
                for (int i = 0; i != triangles.Count; i++)
                {
                    container.indexsTempForJob.Add(triangles[i]);// 保持着网格原本的三角形索引数值，不要加，因为在临时面数据里已经放置了IndexStartIndex
                }
                voxelShapeData.IndexEnd = container.indexsTempForJob.Length;
                voxelShapeData.VertexEnd = container.vertsTempForJob.Length;
                container.shapesTempForJob.Add(voxelShapeData);
            }
        }
        class ShapeColliderBuilder
        {
            NativeList<Unity.Physics.CompoundCollider.ColliderBlobInstance> colliderBlobInstances;
            NativeArray<BlobAssetReference<Unity.Physics.Collider>> solidCollider;
            BlobAssetReference<Unity.Physics.Collider> cubeCollider;

            public ShapeColliderBuilder(int count)
            {
                colliderBlobInstances = new(Allocator.Temp);
                solidCollider = new NativeArray<BlobAssetReference<Unity.Physics.Collider>>(count, Allocator.Persistent);
                cubeCollider = VoxelShapeDefinition.Create(ShapePhysicsInfo.Cube);
                solidCollider[0] = cubeCollider;
            }
            public void Add(VoxelShapeHeader voxelShapeHeader, ShapePhysicsInfo[] shapePhysicsInfos)
            {
                BlobAssetReference<Unity.Physics.Collider> temp;
                if (shapePhysicsInfos == null || shapePhysicsInfos.Length == 0)
                {
                    temp = cubeCollider;
                }
                else
                {
                    if (shapePhysicsInfos.Length != 1)
                    {
                        colliderBlobInstances.Clear();
                        for (int i = 0; i < shapePhysicsInfos.Length; i++)
                        {
                            ShapePhysicsInfo shapePhysicsInfo = shapePhysicsInfos[i];
                            Unity.Physics.CompoundCollider.ColliderBlobInstance colliderBlobInstance = new()
                            {
                                Collider = ShapePhysicsInfo.IsValid(shapePhysicsInfo) ? VoxelShapeDefinition.Create(shapePhysicsInfo) : cubeCollider,
                                CompoundFromChild = RigidTransform.identity,
                            };
                            colliderBlobInstances.Add(colliderBlobInstance);
                        }
                        temp = Unity.Physics.CompoundCollider.Create(colliderBlobInstances.AsArray());
                    }
                    else
                    {
                        temp = ShapePhysicsInfo.IsValid(shapePhysicsInfos[0]) ? VoxelShapeDefinition.Create(shapePhysicsInfos[0]) : cubeCollider;
                    }
                }
                solidCollider[voxelShapeHeader.ShapeIndex] = temp;
            }
            public NativeArray<BlobAssetReference<Unity.Physics.Collider>> Build()
            {
                return solidCollider;
            }
        }
        public ShapeDefinitionDataBase(DataBaseDefinition dataBase, IList<IVoxelShapeDefinition> voxelShapeDefinitions)
        {
            VoxelShapeElement defaultElement = dataBase.defaultShape.VoxelShapes[0];
            if (defaultElement == null || defaultElement.Mesh == null || defaultElement.Mesh.isReadable == false)
                throw new Exception("无效的默认形状元素");


            Dictionary<string, List<InternalShapeDefinition>> voxelShapeClassifyMap = new Dictionary<string, List<InternalShapeDefinition>>();


            ShapeDataTempContainer singleShapeDataContainer = new ShapeDataTempContainer("单形状多方向数据:");
            ShapeDataTempContainer multiShapeDataContainer = new ShapeDataTempContainer("多形状数据:");

            ShapeDataReader shapeDataReader = new ShapeDataReader(dataBase.defaultShape);
            shapeDataReader.AddDefault(singleShapeDataContainer);
            shapeDataReader.ReadFirstElement(voxelShapeDefinitions, singleShapeDataContainer);

            JobHandle sortSingleShapeJobHandle = singleShapeDataContainer.SortJob();
            #region 单个形状多方向创建的
            NativeList<VoxelFaceData> 最终面数据 = new NativeList<VoxelFaceData>(Allocator.TempJob);
            NativeList<float3> 最终顶点数据 = new NativeList<float3>(Allocator.TempJob);
            NativeList<ushort> 最终索引数据 = new NativeList<ushort>(Allocator.TempJob);
            NativeList<float2> 最终UV数据 = new NativeList<float2>(Allocator.TempJob);
            NativeList<float3> 最终法向数据 = new NativeList<float3>(Allocator.TempJob);

            CreateVoxelShapeAssetJob createVoxelShapeAssetJob = new CreateVoxelShapeAssetJob()
            {
                有序临时面数据数组 = singleShapeDataContainer.有序临时面数据数组,
                有序临时顶点数组 = singleShapeDataContainer.有序临时顶点数组,
                有序临时索引数组 = singleShapeDataContainer.有序临时索引数组,
                有序临时UV数组 = singleShapeDataContainer.有序临时UV数组,
                有序临时法向数组 = singleShapeDataContainer.有序临时法向数组,

                allfaceDatas = 最终面数据,
                allIndexs = 最终索引数据,
                allverts = 最终顶点数据,
                alluvs = 最终UV数据,
                allnormals = 最终法向数据,
            };
            JobHandle createShapeMultiDirectionJobHandle = createVoxelShapeAssetJob.Schedule(sortSingleShapeJobHandle);
            #endregion
            shapeDataReader.FinishReadMultiElement(multiShapeDataContainer);
            JobHandle sortMultiSingleShapeJobHandle = multiShapeDataContainer.SortJob();
            #region 拷贝形状及索引到内部定义,以及生成形状碰撞体
            ShapeColliderBuilder shapeColliderBuilder = new ShapeColliderBuilder(shapeDataReader.voxelShapeHeaderMap.Count);

            // 可以先把已经计算的指示柄加入,
            shapeNameMap = new Dictionary<string, InternalShapeDefinition>();
            foreach (KeyValuePair<string, (VoxelShapeHeader, IVoxelShapeDefinition)> kv in shapeDataReader.voxelShapeHeaderMap)
            {
                VoxelShapeHeader voxelShapeHeader = kv.Value.Item1;
                IVoxelShapeDefinition voxelShapeDefinition = kv.Value.Item2;

                InternalShapeDefinition internalShapeDefinition = InternalShapeDefinition.Create(voxelShapeDefinition, voxelShapeHeader);
                #region
                shapeNameMap.Add(kv.Key, internalShapeDefinition);
                #endregion
                #region 归类形状
                if (!voxelShapeClassifyMap.TryGetValue(voxelShapeDefinition.ClassifyName, out var classify))
                {
                    classify = new List<InternalShapeDefinition>();
                    voxelShapeClassifyMap[voxelShapeDefinition.ClassifyName] = classify;
                }
                classify.Add(internalShapeDefinition);
                #endregion
                #region 碰撞体部分

                shapeColliderBuilder.Add(voxelShapeHeader, voxelShapeDefinition.PhysicsInfos);
                #endregion
            }
            voxelShapeClassifies = new VoxelShapeClassify[voxelShapeClassifyMap.Count];
           
            // 归类好的形状分类改成列表
            int classIndex = 0;
            foreach (var shapeClassify in voxelShapeClassifyMap)
            {
                VoxelShapeClassify voxelShapeClassify = new VoxelShapeClassify(shapeClassify.Key, shapeClassify.Value.ToArray());
                voxelShapeClassifies[classIndex] = voxelShapeClassify;
                classIndex++;
            }
            #endregion
            // 再完成工作
            sortMultiSingleShapeJobHandle.Complete();
            createShapeMultiDirectionJobHandle.Complete();
            new CopyMultiShapeDataJob()
            {
                有序临时面数据数组 = multiShapeDataContainer.有序临时面数据数组,
                有序临时索引数组 = multiShapeDataContainer.有序临时索引数组,
                有序临时顶点数组 = multiShapeDataContainer.有序临时顶点数组,
                有序临时UV数组 = multiShapeDataContainer.有序临时UV数组,
                有序临时法向数组 = multiShapeDataContainer.有序临时法向数组,

                allfaceDatas = 最终面数据,
                allIndexs = 最终索引数据,
                allverts = 最终顶点数据,
                alluvs = 最终UV数据,
                allnormals = 最终法向数据,
            }.Run();

            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log(singleShapeDataContainer);
                ConsoleCat.Log(multiShapeDataContainer);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("最终获得以下数据");
                stringBuilder.AppendLine($"面数据:{最终面数据.Length}");
                stringBuilder.AppendLine($"顶点索引:{最终索引数据.Length}");
                stringBuilder.AppendLine($"顶点:{最终顶点数据.Length}");
                stringBuilder.AppendLine($"UV:{最终UV数据.Length}");
                stringBuilder.AppendLine($"法向:{最终法向数据.Length}");
                float size;
                unsafe
                {
                    size =
                        最终面数据.Length * sizeof(VoxelFaceData) +
                        最终索引数据.Length * sizeof(ushort) +
                        (最终顶点数据.Length + 最终法向数据.Length) * sizeof(float3) +
                        最终UV数据.Length * sizeof(float2);
                }
                stringBuilder.AppendLine($"预计占用内存{size / 1024f / 1024f}MB");
                ConsoleCat.Log(stringBuilder.ToString());
            }
            BlobBuilder builder = new BlobBuilder(Allocator.Temp);

            ref VoxelShapeBlobAsset voxelShapeBlobAsset = ref builder.ConstructRoot<VoxelShapeBlobAsset>();

            builder.CopyToBlobArray<VoxelFaceData>(ref voxelShapeBlobAsset.FaceDatas, ref 最终面数据);
            builder.CopyToBlobArray<float3>(ref voxelShapeBlobAsset.Verts, ref 最终顶点数据);
            builder.CopyToBlobArray<ushort>(ref voxelShapeBlobAsset.Indexs, ref 最终索引数据);
            builder.CopyToBlobArray<float2>(ref voxelShapeBlobAsset.UVs, ref 最终UV数据);
            builder.CopyToBlobArray<float3>(ref voxelShapeBlobAsset.Normals, ref 最终法向数据);
            VoxelWorld.VoxelShapeBlobAsset.BuildFaceForwardFindMap(ref voxelShapeBlobAsset, ref builder);
            VoxelWorld.VoxelShapeBlobAsset.BuildRotateMatrix(ref voxelShapeBlobAsset, ref builder);
            VoxelWorld.VoxelShapeBlobAsset.BuildCompressWaterFaceCubeAngle(ref voxelShapeBlobAsset, ref builder);

            var result = builder.CreateBlobAssetReference<VoxelShapeBlobAsset>(Allocator.Persistent);

            singleShapeDataContainer.Dispose();
            multiShapeDataContainer.Dispose();

            最终面数据.Dispose();
            最终索引数据.Dispose();
            最终顶点数据.Dispose();
            最终UV数据.Dispose();
            最终法向数据.Dispose();

            builder.Dispose();
            this.voxelShapeBlobAsset = result;
            solidColliderAsset = shapeColliderBuilder.Build();
            nonSolidCube = VoxelShapeDefinition.Create(ShapePhysicsInfo.Cube, false);
        }

        #region 体素形状查询
        public IVoxelShapeInfo GetDefaultVoxelShapeInfo()
        {
            return voxelShapeClassifies[0][0];
        }
        public IVoxelShapeInfo GetVoxelShapeInfo(string name)
        {
            if (shapeNameMap.TryGetValue(name, out var shapeDefinition))
                return shapeDefinition;
            return GetDefaultVoxelShapeInfo();
        }
        public ushort GetVoxelShapeIndex(string name)
        {
            if (shapeNameMap.TryGetValue(name, out var shapeDefinition))
                return shapeDefinition.ShapeIndex;
            return 0;
        }
        #endregion
    }

}
