using CatFramework.Localized;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    [RequireComponent(typeof(Text))]
    public class TextMiao : UiItem<TextStyleObject>
    {
        [SerializeField] Text text;
        public Text Text => text;
        /// <summary>
        /// 直接设置文本或获取文本
        /// </summary>
        public string TextValue
        {
            get { return text.text; }
            set { text.text = value; }
        }
        [SerializeField] bool listenLanguageChange;
        [SerializeField] string original;
        public bool ListenLanguageChange
        {
            get => listenLanguageChange;
            set
            {
                if (listenLanguageChange != value)
                {
                    if (listenLanguageChange)
                        LocalizedManagerMiao.OnLanguageChange -= LanguageChange;
                    else
                        LocalizedManagerMiao.OnLanguageChange += LanguageChange;
                    listenLanguageChange = value;
                }
            }
        }
        /// <summary>
        /// 设置原文或获取原文
        /// </summary>
        public string TranslationKey
        {
            get => original;
            set
            {
                if (original == value) return;
                original = value;
                if (listenLanguageChange)
                {
                    language = LocalizedManagerMiao.LanguageCollection.GetLanguage(original);
                    text.text = language.Translation;
                }
                else
                {
                    TextValue = original;
                }
            }
        }
        ILanguage language;
        protected override void Awake()
        {
            base.Awake();
            if (text == null)
                text = GetComponent<Text>();
            if (listenLanguageChange)
            {
                language = LocalizedManagerMiao.LanguageCollection.GetLanguage(original);
                LocalizedManagerMiao.OnLanguageChange += LanguageChange;
                text.text = language.Translation;
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //if (listenLanguageChange)
            LocalizedManagerMiao.OnLanguageChange -= LanguageChange;
        }
        void LanguageChange()
        {
            //Debug.Log("修改语言");
            text.text = language.Translation;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (text == null)
                text = GetComponent<Text>();
            text.text = original;
        }
#endif
        public override void ApplyStyle(TextStyleObject styleData)
        {
            styleData.Apply(this);
        }
        public override void ApplyHoverStyle(TextStyleObject styleData)
        {
            styleData.ApplyOnHover(this);
        }
    }
}