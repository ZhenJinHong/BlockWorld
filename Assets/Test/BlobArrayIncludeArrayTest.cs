//using System.Collections;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using UnityEngine;

//namespace Assets.Test
//{
//    public class BlobArrayIncludeArrayTest : MonoBehaviour
//    {
//        //// 鲍勃资产是允许数组套数组的
//        //public void Test()
//        //{
//        //    BlobBuilder builder = new BlobBuilder(Allocator.Temp);
//        //    ref TTT ttt = ref builder.ConstructRoot<TTT>();
//        //    BlobBuilderArray<TT> tts = builder.Allocate<TT>(ref ttt.blobArray, 4);
//        //    for (int i = 0; i < tts.Length; i++)
//        //    {
//        //        ref TT tt = ref tts[i];
//        //        BlobBuilderArray<int> blobBuilderArray = builder.Allocate<int>(ref tt.blobArray, 4);
//        //        for (int j = 0; j < blobBuilderArray.Length; j++)
//        //        {
//        //            blobBuilderArray[j] = j * 4;
//        //        }
//        //    }
//        //    var result = builder.CreateBlobAssetReference<TTT>(Allocator.Temp);
//        //    builder.Dispose();
//        //    ref TTT aTTT = ref result.Value;
//        //    ref BlobArray<TT> aTT = ref aTTT.blobArray;
//        //    for (int i = 0; i < aTT.Length; i++)
//        //    {
//        //        ref BlobArray<int> att = ref aTT[i].blobArray;
//        //        for (int j = 0; j < att.Length; j++)
//        //        {
//        //            Debug.Log($"第{i}个TT里的数组里的第{j}个数字：{att[j]}");
//        //        }
//        //    }
//        //}
//        //struct TT
//        //{
//        //    public BlobArray<int> blobArray;
//        //}
//        //struct TTT
//        //{
//        //    public BlobArray<TT> blobArray;
//        //}
//        NativeArray<BlobAssetReference<int>> ints;
//        // 数组可以套鲍勃引用
//        public void CreateBlob()
//        {
//            ints = new NativeArray<BlobAssetReference<int>>(1, Allocator.Persistent);
//            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
//            ref int i = ref blobBuilder.ConstructRoot<int>();
//            i = 2;
//            var result = blobBuilder.CreateBlobAssetReference<int>(Allocator.Persistent);
//            ints[0] = result;
//            blobBuilder.Dispose();
//        }
//        public void Schedule()
//        {
//            new TestNativeArrayIncluderBlobJob()
//            {
//                ints = ints
//            }.Schedule().Complete();
//        }
//        [BurstCompile]
//        struct TestNativeArrayIncluderBlobJob : IJob
//        {
//            public NativeArray<BlobAssetReference<int>> ints;
//            public void Execute()
//            {
//                BlobAssetReference<int> blobReference = ints[0];
//                Debug.Log($"{blobReference.Value}");
//            }
//        }
//        private void OnDestroy()
//        {
//            if (ints.IsCreated)
//            {
//                ints[0].Dispose();
//                ints.Dispose();
//            }
//        }
//    }
//}