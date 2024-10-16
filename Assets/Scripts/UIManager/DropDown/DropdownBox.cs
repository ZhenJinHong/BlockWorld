//using CatFramework.Localized;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace CatFramework.UiMiao
//{
//    public sealed class DropdownBox : MonoBehaviour, IPointerClickHandler
//    {
//        [SerializeField] ButtonMiao optionPrefab;
//        [SerializeField] TextMiao Label;
//        [SerializeField] RectTransform content;
//        [SerializeField] GameObject viewport;
//        readonly List<string> optionDatas = new List<string>();
//        readonly List<ButtonMiao> buttons = new List<ButtonMiao>();
//        Action<int> onValueChange;
//        string value;
//        public string Value => value;
//        public void SetValueWithoutNotify(string value)
//        {
//            for (int i = 0; i < optionDatas.Count; i++)
//            {
//                if (optionDatas[i] == value)
//                {
//                    InternalSetValueWithoutNotify(i);
//                    break;
//                }
//            }
//        }
//        public void SetValueWithoutNotify(int value)
//        {
//            InternalSetValueWithoutNotify(value);
//        }
//        void InternalSetValueWithoutNotify(int value)
//        {
//            if (value > -1 && value < optionDatas.Count)
//            {
//                m_Index = value;
//                this.value = optionDatas[value];
//                if (Label != null)
//                    Label.TextValue = optionDatas[m_Index];
//            }
//        }
//        int m_Index;
//        public void AddListener(Action<int> onValueChange)
//        {
//            this.onValueChange = onValueChange;
//        }
//        public void AddOptions(string[] options)
//        {
//            if (options == null) return;
//            for (int i = 0; i < options.Length; i++)
//            {
//                this.optionDatas.Add(options[i]);
//            }
//        }
//        public void AddOptions(List<string> options)
//        {
//            if (options == null) return;
//            for (int i = 0; i < options.Count; i++)
//            {
//                this.optionDatas.Add(options[i]);
//            }
//        }
//        public void AddOption(string option)
//        {
//            if (string.IsNullOrEmpty(option)) return;
//            optionDatas.Add(option);
//        }
//        void ShowBox()
//        {
//            if (viewport.activeSelf || optionDatas.Count == 0) return;
//            if (optionPrefab == null || content == null)
//            {
//                ConsoleCat.NullError();
//                return;
//            }
//            for (int i = 0; i < optionDatas.Count; i++)
//            {
//                ButtonMiao button = Instantiate(optionPrefab, content);
//                button.gameObject.SetActive(true);
//                button.useData = i;
//                button.OnClick += OptionCallBack;
//                button.Label = optionDatas[i];
//                buttons.Add(button);
//            }
//            if (UiManagerMiao.RayBlocker != null)
//                UiManagerMiao.RayBlocker.Call(HideBox, 2999);
//            //Entry.UiManager.Blocker(this);
//            viewport.SetActive(true);
//        }
//        void HideBox()
//        {
//            if (viewport.activeSelf)
//            {
//                for (int i = 0; i < buttons.Count; i++)
//                {
//                    Destroy(buttons[i].gameObject);
//                }
//                buttons.Clear();
//                if (UiManagerMiao.RayBlocker != null)
//                    UiManagerMiao.RayBlocker.Call(null, 2999);
//                viewport.SetActive(false);
//            }
//        }
//        void OptionCallBack(UiClick uiClick)
//        {
//            if (uiClick.ButtonMiao.useData is int index && index != m_Index)
//            {
//                InternalSetValueWithoutNotify(index);
//                onValueChange?.Invoke(m_Index);
//            }
//            HideBox();
//        }
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            if (!viewport.activeSelf) ShowBox();
//        }

//        public void Intercept()
//        {
//            HideBox();
//        }
//    }
//}