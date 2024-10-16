//using CatDOTS;
//using System.Collections;
//using Unity.Mathematics;
//using Unity.Physics.Systems;
//using UnityEditor.Rendering;
//using UnityEngine;

//namespace Assets.Scripts.Master.Test
//{
//    public class AnimationClipSampleTest : MonoBehaviour
//    {
//        [SerializeField] private AnimationClip clip;
//        [SerializeField, Range(0, 1)] float time;
//        Material material;
//        public float frames;
//        int frameID = Shader.PropertyToID("_AnimationTime");
//        public VATAsset vATAsset;
//        public int targetClipIndex;
//        public float AnimationTime;
//        public bool Auto;
//        private void Start()
//        {
//            material = GetComponent<MeshRenderer>().material;
//            //frames = clip.length / material.GetFloat(frameHeightID);
//        }
//        // Update is called once per frame
//        float currentF;
//        void Update()
//        {
//            if (vATAsset != null)
//            {
//                targetClipIndex = targetClipIndex < vATAsset.VATDatas.Count ? targetClipIndex : vATAsset.VATDatas.Count - 1;
//                VATClip clip = vATAsset.VATDatas[targetClipIndex];
//                //采样时按照的舍去，计算帧数；1.25秒15FPS有18.75帧，实际有18帧（采样的时候按照0到17各采样一帧），当第18帧时该帧与0帧重合
//                //要考虑同贴图的其他动画
//                if (Auto)
//                    AnimationTime += Time.deltaTime;
//                //循环播放
//                AnimationTime = AnimationTime > clip.Length
//                    ? (clip.Loop ? AnimationTime - clip.Length : clip.Length)
//                    : AnimationTime;
//                //进度时间乘以帧率获取已进行的帧数
//                float frame = AnimationTime * clip.FPS;
//                //此处为Floor即不到1的为0
//                float IntFrame = math.floor(frame);

//                float frameProgress = frame - IntFrame;

//                IntFrame = (IntFrame < clip.Frames)
//                    ? IntFrame
//                    : (clip.Loop ? clip.StartFrame : clip.EndFrame);

//                float timeInMap = IntFrame * vATAsset.FrameHeightInMap;
//                float nextTimeInMap = IntFrame + 1.0f < clip.Frames//此处可能有问题的出现，假设IntFrame是17（总帧数18）那么加1后是18的值
//                    ? timeInMap + vATAsset.FrameHeightInMap
//                    : (clip.Loop ? clip.StartFrame * vATAsset.FrameHeightInMap : timeInMap);
//                material.SetVector(frameID, new Vector4(timeInMap, nextTimeInMap, frameProgress));
//            }
//        }
//    }
//}