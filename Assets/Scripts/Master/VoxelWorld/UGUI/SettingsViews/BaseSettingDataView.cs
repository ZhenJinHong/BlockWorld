//using CatFramework.DataMiao;
//using CatFramework.Tools;
//using CatFramework.UiMiao;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    public abstract class BaseSettingDataView<Setting> : MonoBehaviour where Setting : CatFramework.DataMiao.Setting, new()
//    {
//        [SerializeField] protected RectTransform content;
//        protected Setting setting;
//        protected virtual void Start()
//        {
//            setting = DataManagerMiao.LoadOrCreateSetting<Setting>();
//            ResetUI();
//        }
//        void ResetUI()
//        {
//            UnityUtility.DestroyAllChild(content);
//            CommonUIPrefabs commonUIPrefabs = UiManagerMiao.commonUIPrefabs;
//            if (commonUIPrefabs != null)
//            {
//                SetOtherUI(commonUIPrefabs);
//                commonUIPrefabs.InstantiateField(content, setting);
//            }
//        }
//        protected virtual void SetOtherUI(CommonUIPrefabs commonUIPrefabs)
//        {

//        }
//        public void ApplySetting()
//        {
//            setting.SaveSetting();
//            setting.NotifyChange();
//        }
//        public void ResetSetting()
//        {
//            setting.SetDefaultValue();
//            ResetUI();
//        }
//    }
//}