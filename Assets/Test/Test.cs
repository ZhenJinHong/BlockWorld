//using System;
//using Unity.Burst.Intrinsics;
//using Unity.Burst;
//using Unity.Collections.LowLevel.Unsafe;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Physics;
//using Unity.Physics.Systems;
//using Unity.Transforms;
//using Math = Unity.Physics.Math;
//using Unity.Jobs;

//namespace Master
//{
//    internal class Test
//    {
//        #region 拷贝
//        [BurstCompile]
//        internal struct CreateRigidBodies : IJobChunk
//        {
//            [ReadOnly] public EntityTypeHandle EntityType;
//            [ReadOnly] public ComponentTypeHandle<LocalToWorld> LocalToWorldType;
//            [ReadOnly] public ComponentTypeHandle<Parent> ParentType;
//            [ReadOnly] public ComponentTypeHandle<LocalTransform> LocalTransformType;
//            [ReadOnly] public ComponentTypeHandle<PostTransformMatrix> PostTransformMatrixType;
//            [ReadOnly] public ComponentTypeHandle<PhysicsCollider> PhysicsColliderType;
//            [ReadOnly] public ComponentTypeHandle<PhysicsCustomTags> PhysicsCustomTagsType;
//            [ReadOnly] public int FirstBodyIndex;

//            [NativeDisableContainerSafetyRestriction] public NativeArray<RigidBody> RigidBodies;
//            [NativeDisableContainerSafetyRestriction] public NativeParallelHashMap<Entity, int>.ParallelWriter EntityBodyIndexMap;
//            [ReadOnly] public NativeArray<int> ChunkBaseEntityIndices;

//            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
//            {
//                //当前区块中首个实体在查询中的索引//当提前计算里查询数量的情况下，就可以将这个索引对应到数组中
//                int firstEntityIndexInQuery = ChunkBaseEntityIndices[unfilteredChunkIndex];
//                NativeArray<PhysicsCollider> chunkColliders = chunk.GetNativeArray(ref PhysicsColliderType);
//                NativeArray<LocalToWorld> chunkLocalToWorlds = chunk.GetNativeArray(ref LocalToWorldType);
//                NativeArray<LocalTransform> chunkLocalTransforms = chunk.GetNativeArray(ref LocalTransformType);
//                NativeArray<Entity> chunkEntities = chunk.GetNativeArray(EntityType);
//                NativeArray<PhysicsCustomTags> chunkCustomTags = chunk.GetNativeArray(ref PhysicsCustomTagsType);

//                bool hasChunkPhysicsColliderType = chunkColliders.IsCreated;
//                bool hasChunkPhysicsCustomTagsType = chunk.Has(ref PhysicsCustomTagsType);
//                bool hasChunkParentType = chunk.Has(ref ParentType);
//                bool hasChunkLocalToWorldType = chunkLocalToWorlds.IsCreated;
//                bool hasChunkLocalTransformType = chunkLocalTransforms.IsCreated;
//                bool hasPostTransformMatrixType = chunk.Has(ref PostTransformMatrixType);

//                RigidTransform worldFromBody = RigidTransform.identity;
//                var entityEnumerator =
//                    new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
//                while (entityEnumerator.NextEntityIndex(out int i))
//                {
//                    int rbIndex = FirstBodyIndex + firstEntityIndexInQuery + i;//遍历区块里的实体，当区块首个实体的索引加区块中第N个
//                                                                               // if entities are in a transform hierarchy then LocalTransform is in the space of their parents
//                                                                               // in that case, LocalToWorld is the only common denominator for world space
//                    if (hasChunkParentType)
//                    {
//                        if (hasChunkLocalToWorldType)
//                        {
//                            var localToWorld = chunkLocalToWorlds[i];
//                            worldFromBody = Math.DecomposeRigidBodyTransform(localToWorld.Value);
//                        }
//                    }
//                    else
//                    {
//                        if (hasChunkLocalTransformType)
//                        {
//                            worldFromBody.pos = chunkLocalTransforms[i].Position;
//                            worldFromBody.rot = chunkLocalTransforms[i].Rotation;
//                        }
//                        else if (hasChunkLocalToWorldType)
//                        {
//                            worldFromBody.pos = chunkLocalToWorlds[i].Position;
//                            worldFromBody.rot = Math.DecomposeRigidBodyOrientation(chunkLocalToWorlds[i].Value);
//                        }
//                    }

//                    // GameObjects with non-identity scale have their scale baked into their collision shape and mass, so
//                    // the entity's transform scale (if any) should not be applied again here. Entities that did not go
//                    // through baking should apply their uniform scale value to the rigid body.
//                    // Baking also adds a PostTransformMatrix component to apply the GameObject's authored scale in the
//                    // rendering code, so we test for that component to determine whether the entity's current scale
//                    // should be applied or ignored.
//                    // (DOTS-7098): More robust check here?
//                    float scale = 1.0f;

//                    if (!hasPostTransformMatrixType && hasChunkLocalTransformType)
//                    {
//                        scale = chunkLocalTransforms[i].Scale;
//                    }

//                    RigidBodies[rbIndex] = new RigidBody
//                    {
//                        WorldFromBody = new RigidTransform(worldFromBody.rot, worldFromBody.pos),
//                        Scale = scale,
//                        Collider = hasChunkPhysicsColliderType ? chunkColliders[i].Value : default,
//                        Entity = chunkEntities[i],
//                        CustomTags = hasChunkPhysicsCustomTagsType ? chunkCustomTags[i].Value : (byte)0
//                    };

//                    EntityBodyIndexMap.TryAdd(chunkEntities[i], rbIndex);
//                }
//            }
//        }
//        // Reads broadphase data from dynamic rigid bodies
//        [BurstCompile]
//        struct PrepareDynamicBodyDataJob : IJobParallelFor
//        {
//            [ReadOnly] public NativeArray<RigidBody> RigidBodies;
//            [ReadOnly] public NativeArray<MotionVelocity> MotionVelocities;
//            [ReadOnly] public float TimeStep;
//            [ReadOnly] public float3 Gravity;
//            [ReadOnly] public float AabbMargin;

//            public NativeArray<PointAndIndex> Points;
//            public NativeArray<Aabb> Aabbs;
//            public NativeArray<CollisionFilter> FiltersOut;
//            public NativeArray<bool> RespondsToCollisionOut;

//            public unsafe void Execute(int index)
//            {
//                ExecuteImpl(index, AabbMargin, Gravity, TimeStep, RigidBodies, MotionVelocities, Aabbs, Points, FiltersOut, RespondsToCollisionOut);
//            }

//            internal static unsafe void ExecuteImpl(int index, float aabbMargin, float3 gravity, float timeStep,
//                NativeArray<RigidBody> rigidBodies, NativeArray<MotionVelocity> motionVelocities,
//                NativeArray<Aabb> aabbs, NativeArray<PointAndIndex> points,
//                NativeArray<CollisionFilter> filtersOut, NativeArray<bool> respondsToCollisionOut)
//            {
//                RigidBody body = rigidBodies[index];

//                Aabb aabb;
//                if (body.Collider.IsCreated)
//                {
//                    var mv = motionVelocities[index];

//                    // Apply gravity only on a copy to get proper expansion for the AABB,
//                    // actual applying of gravity will be done later in the physics step
//                    mv.LinearVelocity += gravity * timeStep * mv.GravityFactor;

//                    MotionExpansion expansion = mv.CalculateExpansion(timeStep);
//                    aabb = expansion.ExpandAabb(body.CalculateAabb());
//                    aabb.Expand(aabbMargin);

//                    filtersOut[index] = body.Collider.Value.GetCollisionFilter();
//                    respondsToCollisionOut[index] = body.Collider.Value.RespondsToCollision;
//                }
//                else
//                {
//                    aabb.Min = body.WorldFromBody.pos;
//                    aabb.Max = body.WorldFromBody.pos;

//                    filtersOut[index] = CollisionFilter.Zero;
//                    respondsToCollisionOut[index] = false;
//                }

//                aabbs[index] = aabb;
//                points[index] = new BoundingVolumeHierarchy.PointAndIndex
//                {
//                    Position = aabb.Center,
//                    Index = index
//                };
//            }
//        }
//        #endregion
//    }
//}
//using static Unity.Physics.Math;
//using Unity.Collections;
//using Unity.Mathematics;
//using Unity.Collections.LowLevel.Unsafe;
//using Unity.Physics;

//namespace w
//{
//    public class S
//    {
//        internal unsafe void BuildFirstNLevels(
//               NativeArray<PointAndIndex> points,
//               NativeArray<Builder.Range> branchRanges, NativeArray<int> branchNodeOffset,
//               int threadCount, out int branchCount)
//        {
//            //首先要知道的是前面已经提前设置两个节点？
//            //两个层级设置64个范围（分支）
//            Builder.Range* level0 = stackalloc Builder.Range[Constants.MaxNumTreeBranches];
//            Builder.Range* level1 = stackalloc Builder.Range[Constants.MaxNumTreeBranches];
//            int level0Size = 1;
//            int level1Size = 0;

//            Aabb aabb = new Aabb();
//            //预计是用来计算所有物体的总包围盒的
//            SetAabbFromPoints(ref aabb, (float4*)points.GetUnsafePtr(), points.Length);
//            //因为提前放里两个节点所以level0的根是1？
//            level0[0] = new Builder.Range(0, points.Length, 1, aabb);
//            //根层级的所有物体数除以线程数，与最小范围大小，之间取大值，获取最大允许范围长度
//            //假设线程16，根级有50000人，则根级范围3125
//            int largestAllowedRange = math.max(level0[0].Length / threadCount, Constants.SmallRangeSize);
//            //则195.3125
//            int smallRangeThreshold = math.max(largestAllowedRange / threadCount, Constants.SmallRangeSize);
//            //上一级的最大范围
//            int largestRangeInLastLevel;
//            //最大分支数减去一个拆分
//            int maxNumBranchesMinusOneSplit = Constants.MaxNumTreeBranches - 3;
//            //因为提前放里两个节点所以空余节点从第3个开始？
//            int freeNodeIndex = 2;

//            var builder = new Builder { Bvh = this, Points = points, UseSah = false };

//            Builder.Range* subRanges = stackalloc Builder.Range[4];

//            do
//            {
//                largestRangeInLastLevel = 0;
//                //level0Size初始值为1，预估这个是用来判断当前分支已用节点数
//                for (int i = 0; i < level0Size; ++i)
//                {
//                    if (level0[i].Length > smallRangeThreshold && freeNodeIndex < maxNumBranchesMinusOneSplit)
//                    {
//                        // Split range in up to 4 sub-ranges.
//                        // 当前0层分支拆分成
//                        builder.ProcessLargeRange(level0[i], subRanges);
////public void ProcessLargeRange(Range range, Range* subRanges)
////{
////    if (!UseSah)
////    {
////        ComputeAxisAndPivot(ref range, out int axis, out float pivot);

////        Range* temps = stackalloc Range[2];
////        Segregate(axis, pivot, range, 2, ref temps[0], ref temps[1]);

////        ComputeAxisAndPivot(ref temps[0], out int lAxis, out float lPivot);
////        Segregate(lAxis, lPivot, temps[0], 1, ref subRanges[0], ref subRanges[1]);

////        ComputeAxisAndPivot(ref temps[1], out int rAxis, out float rPivot);
////        Segregate(rAxis, rPivot, temps[1], 1, ref subRanges[2], ref subRanges[3]);
////    }
////    else
////    {
////        Range* temps = stackalloc Range[2];
////        SegregateSah3(range, 2, ref temps[0], ref temps[1]);

////        SegregateSah3(temps[0], 1, ref subRanges[0], ref subRanges[1]);
////        SegregateSah3(temps[1], 1, ref subRanges[2], ref subRanges[3]);
////    }
////}
//                        largestRangeInLastLevel = math.max(largestRangeInLastLevel, subRanges[0].Length);
//                        largestRangeInLastLevel = math.max(largestRangeInLastLevel, subRanges[1].Length);
//                        largestRangeInLastLevel = math.max(largestRangeInLastLevel, subRanges[2].Length);
//                        largestRangeInLastLevel = math.max(largestRangeInLastLevel, subRanges[3].Length);
//                        // 为子范围创建节点并附加级别 1 子范围。
//                        // Create nodes for the sub-ranges and append level 1 sub-ranges.
//                        builder.CreateInternalNodes(subRanges, 4, level0[i].Root, level1, ref level1Size, ref freeNodeIndex);
//                         //                         子范围数组，子范围数组长度      子范围数组的根   
////public void CreateInternalNodes(Range* subRanges, int numSubRanges, int root, Range* rangeStack, ref int stackSize, ref int freeNodeIndex)
////{
////    int4 rootData = int4.zero;

////    for (int i = 0; i < numSubRanges; ++i)
////    {
////        rootData[i] = freeNodeIndex++;
////        rangeStack[stackSize] = subRanges[i];
////        rangeStack[stackSize++].Root = rootData[i];
////    }

////    Node* rootNode = GetNode(root);
////    rootNode->Data = rootData;
////    rootNode->IsInternal = true;
////}
//                    }
//                    else
//                    {
//                        // Too small, ignore.
//                        level1[level1Size++] = level0[i];
//                    }
//                }

//                Builder.Range* tmp = level0;
//                level0 = level1;
//                level1 = tmp;

//                level0Size = level1Size;
//                level1Size = 0;
//                smallRangeThreshold = largestAllowedRange;
//            }//  遍历里全部的根级分支？                        预计为如果上一级的最大范围超过允许的最大范围，继续循环拆分
//            while (level0Size < Constants.MaxNumTreeBranches && largestRangeInLastLevel > largestAllowedRange);

//            RangeSizeAndIndex* rangeMapBySize = stackalloc RangeSizeAndIndex[Constants.MaxNumTreeBranches];

//            int nodeOffset = freeNodeIndex;
//            for (int i = 0; i < level0Size; i++)
//            {
//                rangeMapBySize[i] = new RangeSizeAndIndex { RangeIndex = i, RangeSize = level0[i].Length, RangeFirstNodeOffset = nodeOffset };
//                nodeOffset += level0[i].Length;
//            }

//            SortRangeMap(rangeMapBySize, level0Size);

//            for (int i = 0; i < level0Size; i++)
//            {
//                branchRanges[i] = level0[rangeMapBySize[i].RangeIndex];
//                branchNodeOffset[i] = rangeMapBySize[i].RangeFirstNodeOffset;
//            }

//            for (int i = level0Size; i < Constants.MaxNumTreeBranches; i++)
//            {
//                branchNodeOffset[i] = -1;
//            }

//            branchCount = level0Size;

//            m_Nodes[0] = Node.Empty;
//        }
//    }


//}
//using Unity.Entities;
//using UnityEngine;

//namespace Master
//{
//    public class Test : MonoBehaviour
//    {
//        public void Create()
//        {
//            //World world = World.DefaultGameObjectInjectionWorld;
//            //EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder();
//            //EntityQuery query =world.EntityManager.CreateEntityQuery();
//        }
//    }
//}
