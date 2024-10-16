using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatDOTS
{
    [CreateAssetMenu(fileName = "new VATAsset", menuName = "CatFramework/VAT")]
    public class VATAsset : ScriptableObject
    {
        /// <summary>
        /// 帧在贴图中的占比
        /// </summary>
        public float FrameHeightInMap;
        public List<VATClip> VATDatas = new List<VATClip>();
        public bool IsValid
        {
            get
            {
                return VATDatas.Count > 0;
            }
        }
    }
}