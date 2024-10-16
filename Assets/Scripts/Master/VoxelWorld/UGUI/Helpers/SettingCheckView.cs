//using CatFramework.DataMiao;
//using CatFramework.SLMiao;
//using CatFramework.UiMiao;
//using System.Collections;
//using System.Text;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    public class SettingCheckView : MonoBehaviour
//    {
//        [SerializeField] RectTransform btnsContent;
//        [SerializeField] ButtonMiao optionPrefab;
//        [SerializeField] TextMiao infoText;
//        StringBuilder stringBuilder = new StringBuilder();
//        void Start()
//        {
//            var enumerator = DataManagerMiao.GetSettingEnumerator();
//            while (enumerator.MoveNext())
//            {
//                ButtonMiao option = Instantiate(optionPrefab, btnsContent);
//                option.Label = enumerator.Current.Key.Name;
//                option.useData = enumerator.Current.Value;
//                option.OnClick += ReadSettings;
//            }
//        }
//        public void ReadSettings(UiClick uiClick)
//        {
//            if (uiClick.ButtonMiao.useData is ISetting setting)
//            {
//                Serialization.ObjectPropertyToString(setting, stringBuilder);
//                infoText.TextValue = stringBuilder.ToString();
//                stringBuilder.Clear();
//            }
//        }
//    }
//}