//using CatDOTS.VoxelWorld;
//using CatDOTS;
//using System.Collections;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Physics;
//using UnityEngine;
//using Unity.Mathematics;
//using Material = Unity.Physics.Material;
//using Collider = Unity.Physics.Collider;
//using System.Collections.Generic;

//namespace Assets.Test
//{
//    public class EntityColliderLimitTest : MonoBehaviour
//    {
//        //[SerializeField] MeshFilter meshFilter;
//        //public void Create()
//        //{
//        //    Mesh mesh = meshFilter.sharedMesh;


//        //}
//        [SerializeField] MeshFilter meshFilter;
//        [SerializeField] int meshSize;
//        private void OnValidate()
//        {
//            meshSize = meshSize / 4 * 4;
//        }
//        NativeReference<BlobAssetReference<Collider>> C;
//        public NativeArray<float3> vertexs;
//        public NativeArray<int3> indexs;
//        public NativeArray<int> index;
//        private void OnDestroy()
//        {
//            if (C.IsCreated) C.Dispose();
//            if (vertexs.IsCreated) vertexs.Dispose();
//            if (indexs.IsCreated) indexs.Dispose();
//            if (index.IsCreated) index.Dispose();
//        }
//        JobHandle lastJob;
//        public void SetToMesh()
//        {
//            if (meshFilter != null)
//            {
//                meshFilter.sharedMesh = new Mesh();
//                meshFilter.sharedMesh.SetVertices<float3>(vertexs);
//                meshFilter.sharedMesh.SetIndices<int>(index, MeshTopology.Triangles, 0);
//            }
//        }
//        public void Create()
//        {
//            if (!C.IsCreated)
//                C = new NativeReference<BlobAssetReference<Collider>>(Allocator.Persistent);
//            if (!vertexs.IsCreated)
//                vertexs = new NativeArray<float3>(meshSize, Allocator.Persistent);
//            if (!indexs.IsCreated)
//                indexs = new NativeArray<int3>(meshSize / 2, Allocator.Persistent);
//            if (!index.IsCreated)
//                index = new NativeArray<int>(meshSize / 2 * 3, Allocator.Persistent);
//            lastJob.Complete();
//            if (C.Value.IsCreated)
//                C.Value.Dispose();

//            lastJob = new MeshTEst()
//            {
//                vertexs = vertexs,
//                indexs = indexs,
//                index = index,
//            }.Schedule();
//            lastJob = new CreateMeshColliderTest()
//            {
//                C = C,
//                indexs = indexs,
//                vertexs = vertexs,
//            }.Schedule(lastJob);
//        }
//        [BurstCompile]
//        struct MeshTEst : IJob
//        {
//            public NativeArray<float3> vertexs;
//            public NativeArray<int3> indexs;
//            public NativeArray<int> index;
//            public void Execute()
//            {
//                int index = 0;
//                int index2 = 0;
//                int x = 0, y = 0, z = 0;
//                for (int i = 0; i < vertexs.Length; i += 4)
//                {
//                    float3 offset = new float3(x, y, z);
//                    vertexs[i] = new float3() + offset;
//                    vertexs[i + 1] = new float3(0f, 0f, 1f) + offset;
//                    vertexs[i + 2] = new float3(1f, 0f, 1f) + offset;
//                    vertexs[i + 3] = new float3(1f, 0f, 0f) + offset;

//                    indexs[index] = new int3(i, i + 1, i + 2);
//                    indexs[index + 1] = new int3(i + 2, i + 3, i);

//                    this.index[index2] = i;
//                    this.index[index2 + 1] = i + 1;
//                    this.index[index2 + 2] = i + 2;
//                    this.index[index2 + 3] = i + 2;
//                    this.index[index2 + 4] = i + 3;
//                    this.index[index2 + 5] = i;

//                    index2 += 6;

//                    index += 2;
//                    z++;
//                    if (z == 24)
//                    {
//                        z = 0;
//                        x++;
//                        if (x == 24)
//                        {
//                            x = 0;
//                            y++;
//                        }
//                    }
//                }
//            }
//        }
//        [BurstCompile]
//        struct CreateMeshColliderTest : IJob
//        {
//            public NativeArray<float3> vertexs;
//            public NativeArray<int3> indexs;
//            public NativeReference<BlobAssetReference<Collider>> C;
//            public void Execute()
//            {
//                CollisionFilter collisionFilter = new CollisionFilter()
//                {
//                    BelongsTo = DOTSLayer.SolidVoxel,
//                    CollidesWith = DOTSLayer.AllDynamic,
//                    GroupIndex = 0,
//                };
//                Material material = new Material()
//                {
//                    Friction = 0.05f,// 摩擦力
//                    Restitution = 0f,// 弹力
//                    CollisionResponse = CollisionResponsePolicy.Collide,// 碰撞反应,是否碰撞,或是触发器等
//                    CustomTags = 0,// 可以用来标记这个碰撞体是什么,体素不需要这样的标记?
//                    EnableSurfaceVelocity = false,   // 就是物体移动,上面的物体也移动?
//                    EnableMassFactors = false,// 重力系数组件是否可以应用?
//                    FrictionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
//                    RestitutionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
//                };
//                C.Value = Unity.Physics.MeshCollider.Create(vertexs, indexs, collisionFilter, material);
//            }
//        }
//        //    [SerializeField] int Count = 10000;
//        //    NativeArray<BlobAssetReference<Unity.Physics.Collider>> Colliders;
//        //    NativeReference<BlobAssetReference<Collider>> Result;
//        //    NativeArray<CompoundCollider.ColliderBlobInstance> ColliderBlobInstances;
//        //    JobHandle lastJob;
//        //    private void OnDestroy()
//        //    {
//        //        if (Colliders.IsCreated) Colliders.Dispose();
//        //        if (Result.IsCreated)
//        //        {
//        //            if (Result.Value.IsCreated) Result.Value.Dispose();
//        //            Result.Dispose();
//        //        }
//        //        if (ColliderBlobInstances.IsCreated)
//        //            ColliderBlobInstances.Dispose();
//        //    }
//        //    public void Create()
//        //    {
//        //        lastJob.Complete();
//        //        if (!Colliders.IsCreated)
//        //        {
//        //            Colliders = new NativeArray<BlobAssetReference<Collider>>(Count, Allocator.Persistent);
//        //            lastJob = new CreateColliderJobTest()
//        //            {
//        //                Colliders = Colliders,
//        //            }.Schedule();
//        //        }
//        //        if (!ColliderBlobInstances.IsCreated)
//        //            ColliderBlobInstances = new NativeArray<CompoundCollider.ColliderBlobInstance>(Count, Allocator.Persistent);
//        //        if (!Result.IsCreated)
//        //            Result = new NativeReference<BlobAssetReference<Collider>>(Allocator.Persistent);
//        //        if (Result.Value.IsCreated)
//        //            Result.Value.Dispose();
//        //        lastJob = new CreateCompoundColliderTest()
//        //        {
//        //            Colliders = Colliders,
//        //            ColliderBlobInstances = ColliderBlobInstances,
//        //            Result = Result,
//        //        }.Schedule(lastJob);
//        //    }
//        //}
//        //[BurstCompile]
//        //public struct CreateColliderJobTest : IJob
//        //{
//        //    public NativeArray<BlobAssetReference<Unity.Physics.Collider>> Colliders;
//        //    public void Execute()
//        //    {
//        //        BoxGeometry boxGeometry = new BoxGeometry()
//        //        {
//        //            BevelRadius = 0f,
//        //            Center = 0f,
//        //            Size = new Unity.Mathematics.float3(1f),
//        //            Orientation = quaternion.identity,
//        //        };
//        //        CollisionFilter collisionFilter = new CollisionFilter()
//        //        {
//        //            BelongsTo = DOTSLayer.SolidVoxel,
//        //            CollidesWith = DOTSLayer.AllDynamic,
//        //            GroupIndex = 0,
//        //        };
//        //        Material material = new Material()
//        //        {
//        //            Friction = 0.05f,// 摩擦力
//        //            Restitution = 0f,// 弹力
//        //            CollisionResponse = CollisionResponsePolicy.Collide,// 碰撞反应,是否碰撞,或是触发器等
//        //            CustomTags = 0,// 可以用来标记这个碰撞体是什么,体素不需要这样的标记?
//        //            EnableSurfaceVelocity = false,   // 就是物体移动,上面的物体也移动?
//        //            EnableMassFactors = false,// 重力系数组件是否可以应用?
//        //            FrictionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
//        //            RestitutionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
//        //        };

//        //        for (int i = 0; i < Colliders.Length; i++)
//        //        {
//        //            boxGeometry.Orientation = quaternion.RotateY(i);
//        //            BlobAssetReference<Collider> cubeSolid = Unity.Physics.BoxCollider.Create(boxGeometry, collisionFilter, material);
//        //            Colliders[i] = cubeSolid;
//        //        }
//        //    }
//        //}
//        //[BurstCompile]
//        //public struct CreateCompoundColliderTest : IJob
//        //{
//        //    [ReadOnly] public NativeArray<BlobAssetReference<Unity.Physics.Collider>> Colliders;
//        //    public NativeReference<BlobAssetReference<Collider>> Result;
//        //    public NativeArray<CompoundCollider.ColliderBlobInstance> ColliderBlobInstances;
//        //    public void Execute()
//        //    {
//        //        float3 pos = 0f;
//        //        int x = 0, y = 0, z = 0;
//        //        for (int i = 0; i < ColliderBlobInstances.Length; i++)
//        //        {
//        //            ColliderBlobInstances[i] = new CompoundCollider.ColliderBlobInstance()
//        //            {
//        //                Collider = Colliders[i],
//        //                CompoundFromChild = new RigidTransform(quaternion.identity, pos),
//        //            };
//        //            pos += new float3(x, y, z);
//        //            z++;
//        //            if (z == 24)
//        //            {
//        //                z = 0;
//        //                x++;
//        //                if (x == 24)
//        //                {
//        //                    x = 0;
//        //                    y++;
//        //                }
//        //            }
//        //        }
//        //        Result.Value = CompoundCollider.Create(ColliderBlobInstances);
//        //    }
//    }
//}