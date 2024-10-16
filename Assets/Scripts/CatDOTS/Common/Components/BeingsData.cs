using CatDOTS;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS
{
    /// <summary>
    /// 组件
    /// </summary>
    public struct BeingsData : IComponentData
    {
        public BlobAssetReference<BeingsDataBlobAsset> Data;
    }
    public struct BeingsDataBlobAsset
    {
        public Hash128 ID;//这个比较的时候是按uint==uint，如果用fixedstring判等，查看到的代码要复杂的多有不少的循环
        public FixedString32Bytes Name;
        public float Hp;
        public float Attack;
        public float Defense;
        public float WalkSpeed;
        public float RunSpeed;
        public float RotateChangeRate;
        public float JumpVelocity;
        public float SpeedChangeRate;
        public float OverLoadFactor;
        public float GravityFactor;
        //public int3 GridSize;
        public int2 FloorSpace;
        public int Height;
        public static BlobAssetReference<BeingsDataBlobAsset> Create(BasicBlobAssetAuthoring authoring)
        {
            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
            ref BeingsDataBlobAsset beingsAsset = ref blobBuilder.ConstructRoot<BeingsDataBlobAsset>();

            beingsAsset.ID = authoring.GetHash128();
            beingsAsset.Name = authoring.Name;
            beingsAsset.Hp = authoring.Hp;
            beingsAsset.Attack = authoring.Attack;
            beingsAsset.Defense = authoring.Defense;
            beingsAsset.WalkSpeed = authoring.WalkSpeed;
            beingsAsset.RunSpeed = authoring.RunSpeed;
            beingsAsset.RotateChangeRate = authoring.RotateChangeRate;
            beingsAsset.JumpVelocity = authoring.JumpVelocity;
            beingsAsset.SpeedChangeRate = authoring.SpeedChangeRate;
            beingsAsset.OverLoadFactor = authoring.OverLoadFactor;
            beingsAsset.GravityFactor = authoring.GravityFactor;
            int3 gridSize = authoring.GridSize;
            //beingsAsset.GridSize = authoring.GridSize;
            beingsAsset.FloorSpace = new int2(gridSize.x, gridSize.z);
            beingsAsset.Height = gridSize.y;
            var result = blobBuilder.CreateBlobAssetReference<BeingsDataBlobAsset>(Allocator.Persistent);
            blobBuilder.Dispose();
            return result;
        }
    }
}