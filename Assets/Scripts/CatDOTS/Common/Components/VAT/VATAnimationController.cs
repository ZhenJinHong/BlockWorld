using Unity.Collections;
using Unity.Entities;

namespace CatDOTS
{
    public struct VATAnimationController
    {
        /// <summary>
        /// 帧在贴图中的占比
        /// </summary>
        public float FrameHeightInMap;
        public BlobArray<VATClip> Clips;
        public BlobPtr<VATClip> IdleClipPtr;
        public BlobPtr<VATClip> WalkClipPtr;
        public BlobPtr<VATClip> RunClipPtr;
        public BlobPtr<VATClip> WatchClipPtr;
        public BlobPtr<VATClip> AttackClipPtr;
        public static BlobAssetReference<VATAnimationController> Create(VATAsset asset)
        {
            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
            ref VATAnimationController animationController = ref blobBuilder.ConstructRoot<VATAnimationController>();

            animationController.FrameHeightInMap = asset.FrameHeightInMap;
            BlobBuilderArray<VATClip> clipsBuilder = blobBuilder.Allocate(ref animationController.Clips, asset.VATDatas.Count);
            bool hasIdle = false;
            bool hasWatch = false;
            bool hasWalk = false;
            bool hasRun = false;
            bool hasAttack = false;
            int walkIndex = 0;
            for (int i = 0; i < asset.VATDatas.Count; i++)
            {
                clipsBuilder[i] = asset.VATDatas[i];
                switch (asset.VATDatas[i].AnimationClipID)
                {
                    case AnimationClipID.Idle:
                        blobBuilder.SetPointer<VATClip>(ref animationController.IdleClipPtr, ref clipsBuilder[i]);
                        hasIdle = true;
                        break;
                    case AnimationClipID.Watch:
                        blobBuilder.SetPointer<VATClip>(ref animationController.WatchClipPtr, ref clipsBuilder[i]);
                        hasWatch = true;
                        break;
                    case AnimationClipID.Walk:
                        blobBuilder.SetPointer<VATClip>(ref animationController.WalkClipPtr, ref clipsBuilder[i]);
                        walkIndex = i;
                        hasWalk = true;
                        break;
                    case AnimationClipID.Run:
                        blobBuilder.SetPointer<VATClip>(ref animationController.RunClipPtr, ref clipsBuilder[i]);
                        hasRun = true;
                        break;
                    case AnimationClipID.Attack:
                        blobBuilder.SetPointer<VATClip>(ref animationController.AttackClipPtr, ref clipsBuilder[i]);
                        hasAttack = true;
                        break;
                }
            }
            //不能直接把未完成构建的资产内的指针（此时的指针并未有指向具体内容）赋值给其他指针
            if (!hasWalk) blobBuilder.SetPointer<VATClip>(ref animationController.WalkClipPtr, ref clipsBuilder[walkIndex]);
            if (!hasIdle) blobBuilder.SetPointer<VATClip>(ref animationController.IdleClipPtr, ref clipsBuilder[walkIndex]);
            if (!hasWatch) blobBuilder.SetPointer<VATClip>(ref animationController.WatchClipPtr, ref clipsBuilder[walkIndex]);
            if (!hasRun) blobBuilder.SetPointer<VATClip>(ref animationController.RunClipPtr, ref clipsBuilder[walkIndex]);
            if (!hasAttack) blobBuilder.SetPointer<VATClip>(ref animationController.AttackClipPtr, ref clipsBuilder[walkIndex]);


            var result = blobBuilder.CreateBlobAssetReference<VATAnimationController>(Allocator.Persistent);
            blobBuilder.Dispose();
            return result;
        }
    }
}