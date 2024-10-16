namespace CatDOTS
{
    [System.Serializable]
    public struct VATClip
    {
        public AnimationClipID AnimationClipID;
        public bool Loop;
        /// <summary>
        /// 实际时长
        /// </summary>
        public float Length;
        public float FPS;
        public float EventPoint;
        /// <summary>
        /// 总帧数，帧需要从0开始，帧数是数，从1开始，如果有最后一帧为17序号的，则总帧数18帧；
        /// </summary>
        public float Frames;
        /// <summary>
        /// 如果是第一个动画，则0开头
        /// </summary>
        public float StartFrame;
        /// <summary>
        /// 比帧数少一，否则导致最后一帧实际也是第一帧
        /// </summary>
        public float EndFrame;
    }
}