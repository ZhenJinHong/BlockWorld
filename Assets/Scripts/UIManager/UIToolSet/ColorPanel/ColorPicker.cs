//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//namespace CatFramework
//{
//    [DisallowMultipleComponent]
//    public sealed class ColorPicker : MonoBehaviour
//    {
//        private Color32 originalColor;
//        private Color32 modifiedColor;
//        private HSV modifiedHsv = new();
//        private bool interact = true;
//        [SerializeField] Palette palette;

//        [SerializeField] Slider mainComponent;
//        [SerializeField] SliderInputField rComponent;
//        [SerializeField] SliderInputField gComponent;
//        [SerializeField] SliderInputField bComponent;
//        [SerializeField] SliderInputField aComponent;
//        [SerializeField] SliderInputField hdrComponent;
//        [SerializeField] StringInputField hexaComponent;
//        [SerializeField] C_Button decided;
//        [SerializeField] C_Button cancel;
//        [SerializeField] C_Button close;
//        IColorReceiver colorReceiver;
//        private void Awake()
//        {
//            mainComponent.onValueChanged.RemoveAllListeners();
//            mainComponent.onValueChanged.AddListener(SetMain);
//            palette.AddListener(SetHsv);
//            rComponent.AddListener(SetR);
//            rComponent.SetValueRange(0, 255);
//            gComponent.AddListener(SetG);
//            gComponent.SetValueRange(0, 255);
//            bComponent.AddListener(SetB);
//            bComponent.SetValueRange(0, 255);
//            aComponent.AddListener(SetA);
//            aComponent.SetValueRange(0, 255);
//            hdrComponent.SetValueRange(-100, 100);
//            hexaComponent.AddListener(SetHexa);
//            decided.AddListener(Decided);
//            cancel.AddListener(Cancel);
//            close.AddListener(Close);
//        }
//        private void Start()
//        {
//            //Entry.UiManager.CallColorPicker += AddListener;
//            gameObject.SetActive(false);
//        }
//        private void OnDestroy()
//        {
//            //Entry.UiManager.CallColorPicker -= AddListener;
//        }
//        public void AddListener(IColorReceiver colorReceiver)
//        {
//            this.colorReceiver = colorReceiver;
//            if (colorReceiver != null && !colorReceiver.IsDestroy)
//            {
//                originalColor = colorReceiver.OriginalColor;
//                modifiedColor = originalColor;

//                gameObject.SetActive(true);
//                aComponent.gameObject.SetActive(colorReceiver.UseAlpha);
//                hdrComponent.gameObject.SetActive(colorReceiver.UseHDR);
//                decided.gameObject.SetActive(!colorReceiver.RealTimeUpdata);
//                RecalculateMenu(true);
//                //hexaComponent.Placeholder = "RRGGBB" + (useA ? "AA" : "");
//            }
//        }

//        private void RecalculateMenu(bool recalculateHSV)
//        {
//            if (colorReceiver == null || colorReceiver.IsDestroy) { return; }
//            interact = false;
//            if (recalculateHSV)
//            {
//                modifiedHsv = new HSV(modifiedColor);
//            }
//            else
//            {
//                modifiedColor = modifiedHsv.ToColor();
//            }
//            rComponent.SetValueWithoutNotify(modifiedColor.r);
//            rComponent.SetColor(new Color32(255, modifiedColor.g, modifiedColor.b, 255), new Color32(0, modifiedColor.g, modifiedColor.b, 255));
//            gComponent.SetValueWithoutNotify(modifiedColor.g);
//            gComponent.SetColor(new Color32(modifiedColor.r, 255, modifiedColor.b, 255), new Color32(modifiedColor.r, 0, modifiedColor.b, 255));
//            bComponent.SetValueWithoutNotify(modifiedColor.b);
//            bComponent.SetColor(new Color32(modifiedColor.r, modifiedColor.g, 255, 255), new Color32(modifiedColor.r, modifiedColor.g, 0, 255));
//            if (colorReceiver.UseAlpha)
//            {
//                aComponent.SetValueWithoutNotify(modifiedColor.a);
//                aComponent.SetColor(Color.white, new Color32(modifiedColor.r, modifiedColor.g, modifiedColor.b, 255));
//            }
//            palette.SetValueWithoutNotify(new HSV(modifiedHsv.H, 1f, 1f).ToColor(), new Vector2(modifiedHsv.S, modifiedHsv.V));

//            hexaComponent.SetValueWithoutNotify(colorReceiver.UseAlpha ? ColorUtility.ToHtmlStringRGBA(modifiedColor) : ColorUtility.ToHtmlStringRGB(modifiedColor));
//            if (colorReceiver.RealTimeUpdata)
//            {
//                SendColor();
//            }
//            interact = true;
//        }

//        //used by EventTrigger to calculate the chosen value in color box
//        private void SetHsv(Vector2 localpoint)
//        {
//            modifiedHsv.S = localpoint.x;
//            modifiedHsv.V = localpoint.y;
//            RecalculateMenu(false);
//        }

//        //gets main Slider value
//        private void SetMain(float value)
//        {
//            if (interact)
//            {
//                modifiedHsv.H = value;//直接色相修改，其他不变
//                RecalculateMenu(false);//所以这里是false，将由hsv转color
//            }
//        }

//        //gets r Slider value
//        private void SetR(float value)
//        {
//            if (interact)
//            {
//                modifiedColor.r = System.Convert.ToByte(value);
//                RecalculateMenu(true);
//            }
//        }
//        //gets g Slider value
//        private void SetG(float value)
//        {
//            if (interact)
//            {
//                modifiedColor.g = System.Convert.ToByte(value);
//                RecalculateMenu(true);
//            }
//        }
//        //gets b Slider value
//        private void SetB(float value)
//        {
//            if (interact)
//            {
//                modifiedColor.b = System.Convert.ToByte(value);
//                RecalculateMenu(true);
//            }
//        }
//        //gets a Slider value
//        private void SetA(float value)
//        {
//            if (interact)
//            {
//                modifiedHsv.A = System.Convert.ToByte(value);
//                RecalculateMenu(false);
//            }
//        }
//        //gets hexa InputField value
//        private void SetHexa(string value)
//        {
//            if (interact)
//            {
//                if (ColorUtility.TryParseHtmlString("#" + value, out Color c))
//                {
//                    if (!colorReceiver.UseAlpha) c.a = 1;
//                    modifiedColor = c;
//                    RecalculateMenu(true);
//                }
//                else
//                {
//                    hexaComponent.SetValueWithoutNotify(colorReceiver.UseAlpha ? ColorUtility.ToHtmlStringRGBA(modifiedColor) : ColorUtility.ToHtmlStringRGB(modifiedColor));
//                }
//            }
//        }
//        private void Decided()
//        {
//            if (colorReceiver != null && !colorReceiver.IsDestroy)
//            {
//                SendColor();
//            }
//        }
//        private void SendColor()
//        {
//            if (colorReceiver.UseHDR)
//            {
//                colorReceiver.CallBack((Color)modifiedColor * (hdrComponent.GetValue()));
//            }
//            else
//            {
//                colorReceiver.CallBack(modifiedColor);
//            }
//        }
//        private void Cancel()
//        {
//            if (colorReceiver != null && !colorReceiver.IsDestroy)
//            {
//                colorReceiver.CallBack(originalColor);
//            }
//        }
//        private void Close()
//        {
//            gameObject.SetActive(false);
//            colorReceiver = null;
//        }
//        //HSV helper class
//        private struct HSV
//        {
//            public float H /*= 0*/, S /*= 1*/, V/* = 1*/;
//            public byte A /*= 255*/;
//            //public HSV() { }
//            public HSV(float h, float s, float v)
//            {
//                H = h;
//                S = s;
//                V = v;
//                A = 255;
//            }
//            public HSV(Color color)
//            {
//                float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
//                float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

//                float hue = /*H*/0f;
//                if (min != max)//此处避免了除以0的情况
//                {
//                    if (max == color.r)
//                    {
//                        hue = (color.g - color.b) / (max - min);

//                    }
//                    else if (max == color.g)
//                    {
//                        hue = 2f + (color.b - color.r) / (max - min);

//                    }
//                    else
//                    {
//                        hue = 4f + (color.r - color.g) / (max - min);
//                    }

//                    hue *= 60;
//                    if (hue < 0) hue += 360;
//                }

//                H = hue;
//                S = (max == 0) ? 0 : 1f - (min / max);
//                V = max;
//                A = (byte)(color.a * 255);
//            }
//            public Color32 ToColor()
//            {
//                int hi = Mathf.FloorToInt(H / 60) % 6;
//                float f = H / 60 - Mathf.Floor(H / 60);

//                float value = V * 255;
//                byte v = (byte)System.Convert.ToInt32(value);
//                byte p = (byte)System.Convert.ToInt32(value * (1 - S));
//                byte q = (byte)System.Convert.ToInt32(value * (1 - f * S));
//                byte t = (byte)System.Convert.ToInt32(value * (1 - (1 - f) * S));

//                return hi switch
//                {
//                    0 => new Color32(v, t, p, A),
//                    1 => new Color32(q, v, p, A),
//                    2 => new Color32(p, v, t, A),
//                    3 => new Color32(p, q, v, A),
//                    4 => new Color32(t, p, v, A),
//                    5 => new Color32(v, p, q, A),
//                    _ => new Color32(),
//                };
//            }
//        }
//    }
//}
