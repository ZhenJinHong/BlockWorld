
using CatFramework.DataMiao;
using CatFramework.UiMiao;
using UnityEngine;

namespace CatFramework
{
    public class FrameSetting : Setting
    {
        [SerializeField]
        bool debug;
        public bool Debug
        {
            get => debug;
            set => debug = value;
        }
        [SerializeField]
        bool console;
        public bool Console
        {
            get => console;
            set => console = value;
        }
        [SerializeField]
        private int frameRate;
        public int FrameRate
        {
            get => frameRate;
            set
            {
                if (value < -1 || (value > -1 && value < 4)) value = 4;
                frameRate = value;
            }
        }
        // 往后加个自定义的布尔值，来判断自定义等级
        [SerializeField]
        int qualityLevel;
        [Dropdown(nameof(GetAllQualityLevel))]
        public int QualityLevel
        {
            get => qualityLevel;
            set => qualityLevel = value;
        }
        public string[] GetAllQualityLevel()
        {
            return QualitySettings.names;
        }
        int defaultQualitylevel;
        protected override void ReadDefaultValue()
        {
            base.ReadDefaultValue();
            defaultQualitylevel = QualitySettings.GetQualityLevel();
        }
        public override void ResetSetting()
        {
            debug = true;
            console = true;
            frameRate = 60;
#if UNITY_EDITOR
            frameRate = -1;
#endif
            qualityLevel = defaultQualitylevel;
        }
        public override void ApplySetting()
        {
            base.ApplySetting();
            ApplySetting(this);
        }
        public static void ApplySetting(FrameSetting frameSetting)
        {
            ConsoleCat.Enable = frameSetting.console;
            ConsoleCat.EnableDebug(frameSetting.debug);
            Application.targetFrameRate = frameSetting.FrameRate;
            int QualityLevel = frameSetting.QualityLevel;
            if (QualityLevel != QualitySettings.GetQualityLevel())// 如果变更了质量按照质量来,//直接放后面覆盖?
            {
                QualitySettings.SetQualityLevel(QualityLevel);// TODO要考虑首次设置的时候,质量大概率不等于打包时的质量
            }
        }
    }
}
