//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Physics;
//using Unity.Physics.Extensions;
//using Unity.Physics.Systems;
//using UnityEngine.UIElements;
//using UnityEngine.VFX;

//namespace Assets.Scripts.Master.Test
//{
//    public class VFXListTest
//    {
//        public float x = 0.2f;
//        public float y = 0.2f;
//        // Use this for initialization
//        void Start()
//        {
//        }
//        void E()
//        {
//            CollisionWorld collisionWorld = new CollisionWorld();
//            collisionWorld.CastRay(default); ;
//            BuildPhysicsWorld buildPhysicsWorld = new BuildPhysicsWorld();
//            buildPhysicsWorld.Equals(buildPhysicsWorld);
//            PhysicsColliderKeyEntityPair physicsColliderKeyEntityPair = new PhysicsColliderKeyEntityPair();
//            physicsColliderKeyEntityPair.Entity = Entity.Null;
//            BlobAssetReference<Collider> boxCollider = BoxCollider.Create(new BoxGeometry()
//            {

//            });
//            UnityEngine.Hash128.Compute(new List<int>());
//            PhysicsCollider physicsCollider = new PhysicsCollider();
//            physicsCollider.Value.Value.SetCollisionFilter(new CollisionFilter());
//            CompoundCollider.ColliderBlobInstance colliderBlobInstance = new CompoundCollider.ColliderBlobInstance()
//            {
//                CompoundFromChild = RigidTransform.identity,
//                Collider = boxCollider,
//            };
//            BlobAssetReference<Collider> compoundCollider = CompoundCollider.Create(new NativeArray<CompoundCollider.ColliderBlobInstance>()
//            {
//                [0] = colliderBlobInstance,
//            });
//            //VisualElementStyleSheetSet visualElementStyleSheetSet = new VisualElementStyleSheetSet();
//            //StyleSheet styleSheet = new StyleSheet();
//            //VisualElement visualElement = new VisualElement();
//            //visualElement.style.backgroundImage = new StyleBackground();
//        }
//        //public unsafe bool Raycast<TProcessor, TCollector>(RaycastInput input, ref TProcessor leafProcessor, ref TCollector collector)
//        //    where TProcessor : struct, IRaycastLeafProcessor
//        //    where TCollector : struct, ICollector<RaycastHit>
//        //{
//        //    bool hadHit = false;
//        //    int* stack = stackalloc int[Constants.UnaryStackSize], top = stack;
//        //    *top++ = 1;// 加-是为了第一次执行能从第一个索引位置开始
//        //    do
//        //    {
//        // 按照上次循环获取的已击中的节点，倒退一索引获取最末尾的节点，继续检测该节点是否可继续拆分，
//        //        Node* node = m_Nodes/*节点指针数组*/ + *(--top);//第一次执行，则此处减去一为0，即获取到第一个数组元素
//        //        bool4 hitMask = node->Bounds.Raycast(input.Ray, collector.MaxFraction, out float4 hitFractions);//获知是否击中了物体
//        //        int4 hitData;
//        //        int hitCount = math.compress((int*)(&hitData), 0, node->Data, hitMask);// 通过hitmask获取具体击中了多少个物体
//        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//        //public static unsafe int compress(int* output, int index, int4 val, bool4 mask)
//        //{
//        //    if (mask.x)
//        //        output[index++] = val.x;
//        //    if (mask.y)
//        //        output[index++] = val.y;
//        //    if (mask.z)
//        //        output[index++] = val.z;
//        //    if (mask.w)
//        //        output[index++] = val.w;

//        //    return index;
//        //}

//        //        if (node->IsLeaf)
//        //        {
//        //            for (int i = 0; i < hitCount; i++)
//        //            {
//        //                hadHit |= leafProcessor.RayLeaf(input, hitData[i], ref collector);
//        //                if (collector.EarlyOutOnFirstHit && hadHit)
//        //                {
//        //                    return true;
//        //                }
//        //            }
//        //        }
//        //        else
//        //        {
//        //            *((int4*)top) = hitData;// 当前元素位置放置获取到的击中数据（实际为子节点的索引）
//        //            top += hitCount;// 如果不是叶子，则增加索引数，下次循环会--，从末尾位置继续解析节点
//        //        }
//        //    }
//        //    while (top > stack);

//        //    return hadHit;
//        //}
//        //// Update is called once per frame
//        //void FixedUpdate()
//        //{
//        //    for (int i = 0; i < effects.Count; i++)
//        //    {
//        //        effects[i].SendEvent(playEventID);
//        //    }
//        //}
//    }
//}