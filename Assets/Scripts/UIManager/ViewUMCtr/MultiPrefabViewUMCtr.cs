using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    /// <summary>
    /// 根据预制体数量生成同样数量的选项,选中的时候只实例化对应的预制体
    /// </summary>
    public class MultiPrefabViewUMCtr : MonoBehaviour
    {
        GameObject currentView;
        GameObject CurrentView
        {
            set
            {
                if (currentView != null) Destroy(currentView);
                currentView = value;
            }
        }
        [SerializeField] RectTransform btnsContent;
        [SerializeField] Transform content;
        [SerializeField] ButtonMiao optionPrefab;
        [SerializeField] GameObject[] viewPrefab;
        private void Start()
        {
            foreach (GameObject prefab in viewPrefab)
            {
                if (prefab == null)
                {
                    if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning($"存在空的设定预制体");
                    continue;
                }
                ButtonMiao buttonMiao = Instantiate(optionPrefab, btnsContent);
                buttonMiao.Label = prefab.name;
                buttonMiao.OnClick += OptionClick;
                buttonMiao.useData = prefab;
            }
        }
        void OptionClick(UiClick uiClick)
        {
            if (uiClick.ButtonMiao.useData is GameObject prefab)
            {
                CurrentView = Instantiate(prefab, content);
            }
        }
    }
}