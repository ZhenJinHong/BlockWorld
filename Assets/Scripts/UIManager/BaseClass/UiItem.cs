//using System;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace CatFramework
//{
//    public class UiItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHintTextBoxCaller, IHighLightBoxCaller
//    {
//        [SerializeField] bool highLight = true;
//        [SerializeField] Image image;
//        [SerializeField] ImageType imageType;
//        [SerializeField] Text text;
//        [SerializeField] TextType textType;
//        [SerializeField] bool translate;
//        [SerializeField] string translationKey;
//        [SerializeField] string hintTextKey;
//        UISettingCollection m_UISettingCollection;
//        protected UISettingCollection UISettingCollection
//        {
//            get
//            {
//                m_UISettingCollection ??= DataManagerMiao.GetDataCollection<UISettingCollection>();
//                return m_UISettingCollection;
//            }
//        }
//        protected ImageColorData ImageColorData => UISettingCollection.ImageColorData;
//        protected FontTypeData FontTypeData => UISettingCollection.FontTypeData;
//        protected FontSizeData FontSizeData => UISettingCollection.FontSizeData;
//        protected FontColorData FontColorData => UISettingCollection.FontColorData;
//        protected TextSettingData TextSettingData => UISettingCollection.TextSettingData;
//        ILanguage language;
//        protected ILanguage Language
//        { get { language ??= LanguageCollection.GetLanguage(translationKey); return language; } }
//        protected LanguageCollection LanguageCollection => DataManagerMiao.GetDataCollection<LanguageCollection>();
//        string hintText;
//        object useData;
//        public bool HighLight { get => highLight; set => highLight = value; }
//        public object UseData { get => useData; set => useData = value; }
//        public bool IsDestroy { get => this == null; }
//        public Image C_Image { get => image; }
//        public Color ImageColor
//        {
//            get { if (image) return image.color; return Color.white; }
//            set { if (image) image.color = value; }
//        }
//        public Color TextColor
//        {
//            get { if (text) return text.color; return Color.black; }
//            set { if (text) text.color = value; }
//        }
//        public Text C_Text { get => text; }
//        #region 文本组件的控制
//        /// <summary>
//        /// 设定是否允许翻译（将订阅或退订翻译事件）
//        /// </summary>
//        public bool Translate
//        {
//            get { return translate; }
//            set
//            {
//                //在前后的值不相等的情况下才需要调整
//                if (translate != value)
//                {
//                    translate = value;
//                    //如果是真，则原本未监听事件
//                    if (translate)
//                    {
//                        LanguageCollection.Changed += GoTranslate;
//                    }
//                    else
//                    {
//                        LanguageCollection.Changed -= GoTranslate;
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// 设定翻译键，如果允许翻译将翻译，否则为直接赋值文本
//        /// </summary>
//        public string TranslationKey
//        {
//            get { return translationKey; }
//            set
//            {
//                if (translationKey != value)
//                {
//                    translationKey = value;
//                    if (text)
//                    {
//                        text.text = translate ? Language.Translation : value;
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// 设定文本，不翻译
//        /// </summary>
//        public string Text
//        {
//            get { if (text) return text.text; return "null"; }
//            set { if (text) text.text = value; }
//        }
//        /// <summary>
//        /// 设置提示文本键，并翻译
//        /// </summary>
//        public string HintTextKey
//        {
//            get => hintTextKey;
//            set
//            {
//                if (hintTextKey != value)
//                {
//                    hintTextKey = value;//不可以判空，直接赋值即可，以便enter，exit 确定是否需要提示
//                    HintText = Language.Tip;
//                }
//            }
//        }
//        /// <summary>
//        /// 调整文本类型，并重调文本
//        /// </summary>
//        public TextType TextType
//        {
//            get { return textType; }
//            set
//            {
//                if (textType != value)
//                {
//                    textType = value;
//                    if (text)
//                    {
//                        FixTextSetting(TextSettingData);
//                        FixFontSize(FontSizeData);
//                        FixFontColor(FontColorData);
//                        FixFont(FontTypeData);
//                    }
//                }
//            }
//        }
//        public string HintText { get => hintText; set => hintText = value; }
//        public Color HintColor { get { if (text) return text.color; return Color.black; } }
//        #endregion
//        public bool IsRecycle { get; set; }
//        public GameObject GameObject => gameObject;
//        public Transform Transform => transform;
//        public bool IsDestory => this == null;

//        public RectTransform RectTransform => transform as RectTransform;

//        protected virtual void Start()
//        {
//            if (translate)
//            {
//                LanguageCollection.Changed += GoTranslate;
//                GoTranslate();
//            }
//            if (text)
//            {
//                //TextSettingData.HasChanged += FixTextSetting;
//                //FontSizeData.HasChanged += FixFontSize;
//                //FontColorData.HasChanged += FixFontColor;
//                //FontTypeData.HasChanged += FixFont;
//                FixTextSetting(TextSettingData);
//                FixFontSize(FontSizeData);
//                FixFontColor(FontColorData);
//                FixFont(FontTypeData);
//            }
//            if (image)
//            {
//                //ImageColorData.HasChanged += FixImage;
//                FixImage(ImageColorData);
//            }
//        }
//        protected virtual void OnDestroy()
//        {
//            if (translate)
//            {
//                LanguageCollection.Changed -= GoTranslate;
//                GoTranslate();
//            }
//            if (text)
//            {
//                //TextSettingData.HasChanged -= FixTextSetting;
//                //FontSizeData.HasChanged -= FixFontSize;
//                //FontColorData.HasChanged -= FixFontColor;
//                //FontTypeData.HasChanged -= FixFont;
//                FixTextSetting(TextSettingData);
//                FixFontSize(FontSizeData);
//                FixFontColor(FontColorData);
//                FixFont(FontTypeData);
//            }
//            if (image)
//            {
//                //ImageColorData.HasChanged -= FixImage;
//                FixImage(ImageColorData);
//            }
//        }
//        #region 图像调整函数
//        public void FixImage(ImageColorData imageColorData)
//        {
//            if (image)
//            {
//                image.color = imageColorData.GetImageColor(imageType);
//            }
//        }
//        #endregion
//        #region 文本组件调整函数
//        public void GoTranslate()
//        {
//            if (text)
//            {
//                text.text = Language.Translation;
//            }
//            if (!string.IsNullOrEmpty(hintTextKey))
//            {
//                HintText = Language.Tip;
//            }
//        }
//        public void FixTextSetting(TextSettingData textSettingData)
//        {
//            if (text)
//            {
//                Value3<bool> data = textSettingData.GetFontSetting(textType);
//                text.resizeTextForBestFit = data.x;
//                text.verticalOverflow = data.y ? VerticalWrapMode.Overflow : VerticalWrapMode.Truncate;
//                text.horizontalOverflow = data.z ? HorizontalWrapMode.Overflow : HorizontalWrapMode.Wrap;
//            }
//        }
//        public void FixFontSize(FontSizeData fontSizeData)
//        {
//            if (text)
//            {
//                Value3<int> data = fontSizeData.GetFontSize(textType);
//                text.fontSize = data.x;
//                text.resizeTextMinSize = data.y;
//                text.resizeTextMaxSize = data.z;
//            }
//        }
//        public void FixFontColor(FontColorData fontColorData)
//        {
//            if (text)
//            {
//                text.color = fontColorData.GetFontColor(textType);
//            }
//        }
//        public void FixFont(FontTypeData fontTypeData)
//        {
//        }
//        #endregion
//        public virtual void OnPointerEnter(PointerEventData eventData)
//        {
//            //if (!string.IsNullOrEmpty(hintTextKey))
//            //    Entry.UiManager.HintTextBox(this);
//            //if (highLight)
//            //    Entry.UiManager.HighLightBox(this);
//        }
//        public virtual void OnPointerExit(PointerEventData eventData)
//        {
//            //if (!string.IsNullOrEmpty(hintTextKey))
//            //    Entry.UiManager.HintTextBox(null);
//            //if (highLight)
//            //    Entry.UiManager.HighLightBox(null);
//        }
//    }
//}