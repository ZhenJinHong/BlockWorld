using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.CatMath;
using CatFramework.DataMiao;
using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VoxelWorld
{
    public enum AntiAliasingLevel
    {
        None = 0,
        X2 = 2,
        X4 = 4,
        X8 = 8,
    }
    /// <summary>
    /// 定义渲染距离即为渲染范围的一半
    /// </summary>
    public class QualitySettingsData : Setting, VoxelWorldDataBaseManaged.IVoxelWorldSetting
    {
        public override string Name => "质量";
        //[SerializeField]
        //private int frameRate;
        //public int FrameRate
        //{
        //    get => frameRate;
        //    set
        //    {
        //        if (value < -1 || (value > -1 && value < 4)) value = 4;
        //        frameRate = value;
        //    }
        //}
        // 往后加个自定义的布尔值，来判断自定义等级
        //[SerializeField]
        //int qualityLevel;
        //[Dropdown(nameof(GetAllQualityLevel))]
        //public int QualityLevel
        //{
        //    get => qualityLevel;
        //    set => qualityLevel = value;
        //}
        [SerializeField] int loadBigChunkPerFrame;
        public int LoadBigChunkPerFrame
        {
            get => loadBigChunkPerFrame;
            set => loadBigChunkPerFrame = MathC.Clamp(value, 1, 16);
        }
        [SerializeField]
        int preloadingDistance;
        public int PreloadingDistance
        {
            get => preloadingDistance;
            set => preloadingDistance = Settings.ClampWorldDistance(Math.Max(4, value));
        }
        [SerializeField]
        int renderDistance;
        public int RenderDistance
        {
            get => ClampRenderDistance(renderDistance);// 获取的时候如果不钳制,会导致因为只改了预加载距离,而渲染距离未变的问题
            set => renderDistance = ClampRenderDistance(value);
        }
        int ClampRenderDistance(int value)
        {
            return value > preloadingDistance - 2 ? preloadingDistance - 2 : value;
        }

        public int LoadingDistance => RenderDistance + 1;
        public int UnloadingDistance => Math.Max(PreloadingDistance, RenderDistance + 2);// 最低为渲染距离加2
        public int UnloadingRange => UnloadingDistance * 2;

        public int ChunkRenderObjectPoolCapacity
        {
            get
            {
                int renderRange = RenderDistance * 2;
                return renderRange * renderRange * 2;  // 一般情况下也就两层需要渲染显示的
            }
        }
        public int MaxMemoryMB => 1024;
        [SerializeField] int updateMeshPerFrame;
        public int UpdateMeshPerFrame
        {
            get => updateMeshPerFrame;
            set => updateMeshPerFrame = value;
        }
        [SerializeField] int voxelChunkBehaviorPerFrame;
        public int VoxelChunkBehaviorPerFrame
        {
            get => voxelChunkBehaviorPerFrame;
            set => voxelChunkBehaviorPerFrame = value;
        }

        [SerializeField] bool fog;
        public bool Fog
        {
            get => fog; set => fog = value;
        }
        public string[] GetAllQualityLevel()
        {
            return QualitySettings.names;
        }
        protected override void ReadDefaultValue()
        {
            base.ReadDefaultValue();
            //defaultQualitylevel = QualitySettings.GetQualityLevel();
        }
        //int defaultQualitylevel;
        public override void ApplySetting()
        {
            base.ApplySetting();
            QualitySettingChange(this);
        }
        public override void ResetSetting()
        {
//            frameRate = 60;
//#if UNITY_EDITOR
//            frameRate = -1;
//#endif
            //qualityLevel = defaultQualitylevel;
            fog = false;
            loadBigChunkPerFrame = 2;
            PreloadingDistance = 8;
            RenderDistance = 6;
            UpdateMeshPerFrame = 1;// 这个只有一的情况下,如果修改的边界的方块,邻近区块等待下一帧更新,会出现空
            VoxelChunkBehaviorPerFrame = 1;
        }
        public static void QualitySettingChange(QualitySettingsData qualitySettings)
        {
            //int FrameRate = qualitySettings.FrameRate;
            //int QualityLevel = qualitySettings.QualityLevel;
            //AntiAliasingLevel antiAliasingLevel = qualitySettings.MSAA_AntiAliasing;
            //bool fog = qualitySettings.Fog;
            //int HalfRenderRange = qualitySettings.RenderDistance;
            //if (FrameRate != Application.targetFrameRate)
            //    Application.targetFrameRate = FrameRate;
            ////if (QualitySettings.antiAliasing != (int)antiAliasingLevel)
            ////{
            ////    QualitySettings.antiAliasing = (int)antiAliasingLevel;
            ////}
            //if (QualityLevel != QualitySettings.GetQualityLevel())// 如果变更了质量按照质量来,//直接放后面覆盖?
            //{
            //    QualitySettings.SetQualityLevel(QualityLevel);// TODO要考虑首次设置的时候,质量大概率不等于打包时的质量
            //}
            //if (RenderSettings.fog != fog)
            //{
            //    RenderSettings.fog = fog;
            //}
            //if (fog)
            //{
            //    int endDistance = HalfRenderRange * Settings.SmallChunkSize;
            //    int startDistance = endDistance - Settings.SmallChunkSize;
            //    RenderSettings.fogStartDistance = startDistance;
            //    RenderSettings.fogEndDistance = endDistance;
            //}
        }
    }
}
