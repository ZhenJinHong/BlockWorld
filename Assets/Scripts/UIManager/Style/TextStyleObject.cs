using CatFramework.EventsMiao;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    [CreateAssetMenu(fileName = "New TextStyle", menuName = "UGUIStyle/TextStyle")]
    public class TextStyleObject : StyleObject
    {
        public TextStyleData StyleData { get => styleData; }
        public TextStyleData StyleDataOnHover { get => styleDataOnHover; }

        [SerializeField] TextStyleData styleData;
        [SerializeField] TextStyleData styleDataOnHover;
        public void Apply(TextMiao textMiao)
        {
            var text = textMiao.Text;
            if (text != null)
                styleData.Apply(text);
        }
        public void ApplyOnHover(TextMiao textMiao)
        {
            var text = textMiao.Text;
            if (text != null)
                styleDataOnHover.Apply(text);
        }
        public override void Initialized(Style style)
        {
            base.Initialized(style);
            styleData.Initialized(style);
            styleDataOnHover.Initialized(style);
        }
        public override IDataEventLinked AsDataEventLinked()
        {
            return new DataEventLinked<TextStyleObject>(this);
        }
    }
    [Serializable]
    public class TextStyleData
    {
        [SerializeField] private string fontName;
        [SerializeField] private FontStyle fontStyle;
        [SerializeField] private int fontSize;
        [SerializeField] private float lineSpacing;
        [SerializeField] private bool richText;
        [SerializeField] private TextAnchor textAnchor;
        [SerializeField] private bool alignByGeometry;
        [SerializeField] private HorizontalWrapMode horizontalWrapMode;
        [SerializeField] private VerticalWrapMode verticalWrapMode;
        [SerializeField] private bool bestFit;
        [SerializeField] private Color color;
        [SerializeField] private bool raycastTarget;
        [SerializeField] private bool maskable;

        [NonSerialized] public Font font;
        public void Initialized(Style style)
        {
            font = style.GetFont(fontName);
        }
        public string FontName { get => fontName; set => fontName = value; }
        public FontStyle FontStyle { get => fontStyle; set => fontStyle = value; }
        public int FontSize { get => fontSize; set => fontSize = value; }
        public float LineSpacing { get => lineSpacing; set => lineSpacing = value; }
        public bool RichText { get => richText; set => richText = value; }
        public TextAnchor TextAnchor { get => textAnchor; set => textAnchor = value; }
        public bool AlignByGeometry { get => alignByGeometry; set => alignByGeometry = value; }
        public HorizontalWrapMode HorizontalWrapMode { get => horizontalWrapMode; set => horizontalWrapMode = value; }
        public VerticalWrapMode VerticalWrapMode { get => verticalWrapMode; set => verticalWrapMode = value; }
        public bool BestFit { get => bestFit; set => bestFit = value; }
        public Color Color { get => color; set => color = value; }
        public bool RaycastTarget { get => raycastTarget; set => raycastTarget = value; }
        public bool Maskable { get => maskable; set => maskable = value; }
        public void Apply(Text text)
        {
            if (text == null) return;
            if (font != null)
                text.font = font;
            text.fontStyle = fontStyle;
            text.fontSize = fontSize;
            text.lineSpacing = lineSpacing;
            text.supportRichText = richText;
            text.alignment = textAnchor;
            text.alignByGeometry = alignByGeometry;
            text.horizontalOverflow = horizontalWrapMode;
            text.verticalOverflow = verticalWrapMode;
            text.resizeTextForBestFit = bestFit;
            text.color = color;
            text.raycastTarget = raycastTarget;
            text.maskable = maskable;
        }
    }
}