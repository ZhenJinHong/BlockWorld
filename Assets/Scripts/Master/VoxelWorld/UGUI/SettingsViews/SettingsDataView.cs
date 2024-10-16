using CatFramework;
using CatFramework.DataMiao;
using CatFramework.SLMiao;
using CatFramework.Tools;
using CatFramework.UiMiao;
using System.Collections;
using System.Text;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class SettingsDataView : MonoBehaviour
    {
        [SerializeField] RectTransform btnsContent;
        [SerializeField] ButtonMiao optionPrefab;
        [SerializeField] RectTransform content;
        private void Start()
        {
            foreach (var v in DataManagerMiao.GetReadonly().Values)
            {
                if (v.Value is ISetting setting)
                {
                    ButtonMiao option = Instantiate(optionPrefab, btnsContent);
                    option.Label = setting.Name;
                    option.useData = setting;
                    option.OnClick += ReadSettings;
                }
            };
        }
        void ResetUI()
        {
            UnityUtility.DestroyAllChild(content);
            if (setting == null) return;
            CommonUIPrefabs commonUIPrefabs = UiManagerMiao.commonUIPrefabs;
            if (commonUIPrefabs != null)
            {
                commonUIPrefabs.InstantiateField(content, setting);
            }
        }
        public void ReadSettings(UiClick uiClick)
        {
            if (uiClick.ButtonMiao.useData is ISetting setting)
            {
                this.setting = setting;
                ResetUI();
            }
        }
        ISetting setting;
        public void ApplySetting()
        {
            if (setting == null) return;
            setting.SaveSetting();
            setting.ApplySetting();
            setting.NotifySettingChange();
        }
        public void ResetSetting()
        {
            if (setting == null) return;
            setting.ResetSetting();
            ResetUI();
        }
    }
}