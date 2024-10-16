//using CatFramework;
//using CatFramework.UiMiao;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    public class SettingsView : MonoBehaviour
//    {
//        GameObject currentView;
//        GameObject CurrentView
//        {
//            set
//            {
//                if (currentView != null) Destroy(currentView);
//                currentView = value;
//            }
//        }
//        [SerializeField] RectTransform btnsContent;
//        [SerializeField] Transform content;
//        [SerializeField] ButtonMiao optionPrefab;
//        [SerializeField] GameObject[] viewPrefab;
//        private void Start()
//        {
//            foreach (GameObject gameObject in viewPrefab)
//            {
//                if (gameObject == null)
//                {
//                    if (ConsoleCat.Enable)
//                        ConsoleCat.LogWarning($"存在空的设定预制体");
//                    continue;
//                }
//                ButtonMiao buttonMiao = Instantiate(optionPrefab, btnsContent);
//                buttonMiao.Label = gameObject.name;
//                buttonMiao.OnClick += OptionClick;
//                buttonMiao.useData = gameObject;
//            }
//        }
//        void OptionClick(UiClick uiClick)
//        {
//            if (uiClick.ButtonMiao.useData is GameObject gameObject)
//            {
//                CurrentView = Instantiate(gameObject, content);
//            }
//        }
//    }
//}