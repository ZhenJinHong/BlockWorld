//using CatDOTS.VoxelWorld;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.AddressableAssets;

//namespace Assets.Test
//{
//    public class LoadAddressableAssetTest : MonoBehaviour
//    {
//        [SerializeField] AssetReferenceT<VoxelDefinition> assetReference;
//        //public void Load()
//        //{
//        //    var handle = Addressables.LoadAssetsAsync<VoxelDefinition>("VoxelDefinition", Call);
//        //    Addressables.Release(handle);
//        //}
//        //void Call(VoxelDefinition def)
//        //{
//        //    Debug.Log(def.name);
//        //}
//        public void Print()
//        {
//            Debug.Log(assetReference.SubObjectName);
//            Debug.Log(assetReference.RuntimeKey);
//            Debug.Log(assetReference.AssetGUID);
//            Debug.Log(assetReference.Asset);
//        }
//    }
//}