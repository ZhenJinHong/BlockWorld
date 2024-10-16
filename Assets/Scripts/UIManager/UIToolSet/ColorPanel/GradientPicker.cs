//using CatFramework;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using System;
//public interface IGradientPicker
//{
//    void CreateNewKey(float time, bool isColorArrow);
//    int KeyCount(bool isColorArrow);
//    void SelectKey(int index, bool isColorArrow);
//    void RemoveKey(int index, bool isColorArrow);
//    void SetTime(float time, bool isColorArrow, int index);
//}
//[DisallowMultipleComponent]
//public class GradientPicker : MonoBehaviour, IGradientPicker
//{
//    [SerializeField] ColorArrows ColorArrows;
//    [SerializeField] ColorArrows AlphaArrows;
//    [SerializeField] Image GradientImage;
//    [SerializeField] SelectColor selectColor;//用以调出调色板
//    [SerializeField] SliderInputField alphaSlider;//用以更改当前颜色透明度
//    [SerializeField] CatFramework.C_Button apply;
//    [SerializeField] CatFramework.C_Button close;

//    IGradientReceiver gradientReceiver;

//    private Gradient modifiedGradient;
//    const int GradientImageWidth = 325;
//    readonly Color32[] gradientColor = new Color32[GradientImageWidth * GradientImageHight];
//    const int GradientImageHight = 1;
//    const int keyLimit = 8;

//    private readonly List<GradientColorKey> colorKeys = new();
//    private readonly List<GradientAlphaKey> alphaKeys = new();
//    private void Awake()
//    {
//        Texture2D texture = new(GradientImageWidth, GradientImageHight) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };

//        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, GradientImageWidth, GradientImageHight), new Vector2());

//        GradientImage.sprite = sprite;

//        alphaSlider.AddListener(SetAlpha);
//        alphaSlider.SetValueRange(0, 1);
//        selectColor.AddListener(OnValueChange);
//        apply.AddListener(Apply);
//        close.AddListener(Close);
//    }
//    private void Start()
//    {
//        //Entry.UiManager.CallGradientPicker += AddListener;
//        gameObject.SetActive(false);
//    }
//    private void OnDestroy()
//    {
//        //Entry.UiManager.CallGradientPicker -= AddListener;
//    }
//    public void AddListener(IGradientReceiver gradientReceiver)
//    {
//        this.gradientReceiver = gradientReceiver;
//        if (gradientReceiver != null && !gradientReceiver.IsDestroy)
//        {
//            ColorArrows.Init(keyLimit, this, true);//可能由于监听时Awake没执行，有出现提示索引溢出问题
//            AlphaArrows.Init(keyLimit, this, false);

//            modifiedGradient = gradientReceiver.Gradient;
//            gameObject.SetActive(true);
//            colorKeys.Clear();
//            alphaKeys.Clear();
//            ColorArrows.DisableArrows();
//            AlphaArrows.DisableArrows();
//            foreach (GradientColorKey k in modifiedGradient.colorKeys)
//            {
//                ColorArrows.EnableArrow(k.time, k.color);
//            }
//            foreach (GradientAlphaKey k in modifiedGradient.alphaKeys)
//            {
//                AlphaArrows.EnableArrow(k.time, Color.white);
//            }
//            CalculateTexture();
//        }
//    }
//    #region Arrows调用
//    public int KeyCount(bool isColorArrow)
//    {
//        return isColorArrow ? colorKeys.Count : alphaKeys.Count;
//    }
//    public void CreateNewKey(float time, bool isColorArrow)//Arrows使用新增箭头对应的键
//    {
//        if (isColorArrow)
//        {
//            Color color = modifiedGradient.Evaluate(time);
//            ColorArrows.SetArrowColor(colorKeys.Count, new Color(color.r, color.g, color.b, 1f));
//            colorKeys.Add(new GradientColorKey(color, time));
//        }
//        else
//            alphaKeys.Add(new GradientAlphaKey(modifiedGradient.Evaluate(time).a, time));
//    }
//    public void RemoveKey(int index, bool isColorArrow)//箭头移除时，移除对应的键
//    {
//        if (isColorArrow)
//            colorKeys.RemoveAt(index);
//        else
//            alphaKeys.RemoveAt(index);
//        modifiedGradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
//        CalculateTexture();
//    }
//    public void SelectKey(int index, bool isColorArrow)//当选择箭头时，更新箭头应显示的信息
//    {
//        if (isColorArrow)
//        {
//            selectColor.SetValueWithoutNotify(colorKeys[index].color);
//        }
//        else
//        {
//            alphaSlider.SetValueWithoutNotify(alphaKeys[index].alpha);
//        }
//    }
//    public void SetTime(float time, bool isColorArrow, int index)//当箭头拖动时，重置当前箭头的键值
//    {
//        if (isColorArrow)
//            colorKeys[index] = new GradientColorKey(colorKeys[index].color, time);
//        else
//            alphaKeys[index] = new GradientAlphaKey(alphaKeys[index].alpha, time);
//        modifiedGradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
//        CalculateTexture();
//    }
//    #endregion
//    #region 透明度拖条和颜色面板打开按钮使用
//    public void SetAlpha(float value)//透明度拖条使用
//    {
//        int index = AlphaArrows.CurrentIndex;
//        if (index < alphaKeys.Count && index != -1)
//        {
//            alphaKeys[index] = new GradientAlphaKey(value, alphaKeys[index].time);
//            modifiedGradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
//            CalculateTexture();
//        }
//    }
//    void OnValueChange(Color color)
//    {
//        int index = ColorArrows.CurrentIndex;
//        if (index < colorKeys.Count && index != -1)
//        {
//            colorKeys[index] = new GradientColorKey(color, colorKeys[index].time);
//            ColorArrows.SetArrowColor(color);
//            modifiedGradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
//            CalculateTexture();
//        }
//    }
//    #endregion
//    private void CalculateTexture()
//    {
//        for (int i = 0; i < gradientColor.Length; i++)
//        {
//            gradientColor[i] = modifiedGradient.Evaluate(i / (float)gradientColor.Length);
//        }
//        GradientImage.sprite.texture.SetPixels32(gradientColor);
//        GradientImage.sprite.texture.Apply();
//        if (gradientReceiver != null && !gradientReceiver.IsDestroy)
//        {
//            gradientReceiver.GradientChanged();
//        }
//    }
//    public void Apply()
//    {
//        if (gradientReceiver != null && !gradientReceiver.IsDestroy)
//        {
//            gradientReceiver.GradientSubmit();
//        }
//    }
//    public void Close()
//    {
//        gameObject.SetActive(false);
//        gradientReceiver = null;
//    }
//}
