using CatFramework.DataMiao;
using CatFramework.Localized;
using CatFramework.UiTK;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CatFramework
{
    public sealed class LanguageListViewController : VisualViewController
    {
        readonly ListView languageListView;
        readonly List<string> languageNames;
        LanguageCollection LanguageCollection => LocalizedManagerMiao.LanguageCollection;
        public LanguageListViewController() : base(CreateContainer())
        {
            AddToClassList(viewFullClass);

            languageNames = new List<string>();
            LanguageCollection.GetAllLanguageNames(languageNames);

            languageListView = new ListView(languageNames, 40f, MakeLanguageItem, BindLanguageItem)
            {
                unbindItem = UnBindLanguageItem,
                destroyItem = DestroyLanguageItem
            };
            languageListView.selectionChanged += SelectedLanguage;//如果使用懒惰初始化，必须注意事件的订阅与退订

            Add(languageListView);
        }
        VisualElement MakeLanguageItem()
        {
            Label label = CreateTextLine();
            label.pickingMode = PickingMode.Position;
            return label;
        }
        void BindLanguageItem(VisualElement element, int index)
        {
            if (element is Label label && index > -1 && index < languageNames.Count)
            {
                label.text = languageNames[index];
            }
        }
        void UnBindLanguageItem(VisualElement element, int index)// 列表在刷新的时候会解绑// 取消显示时解绑
        {
            if (element is Label label)
            {
                label.text = "无";
            }
        }
        void DestroyLanguageItem(VisualElement element)
        {
            //element.DisposeUseData();//实际上不用释放，并没有绑定usedata
        }
        void SelectedLanguage(IEnumerable<object> l)
        {
            if (languageListView.selectedItem is string languageName)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.DebugInfo("切换语言：" + languageName);
                LocalizedManagerMiao.SwitchLanguage(languageName);
            }
        }
    }
}