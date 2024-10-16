//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace CatFramework
//{
//    public sealed class SliderInputField : TextInputField<float>
//    {
//        [SerializeField] Slider colorSlider;
//        [SerializeField] Image sliderImage;
//        [SerializeField] Image background;
//        protected override void Start()
//        {
//            base.Start();
//            if (colorSlider != null)
//            {
//                colorSlider.onValueChanged.RemoveAllListeners();
//                colorSlider.onValueChanged.AddListener(SliderCall);
//            }

//        }
//        public override void SetValueRange(float min, float max)
//        {
//            if (colorSlider != null)
//            {

//                colorSlider.minValue = min;
//                colorSlider.maxValue = max;
//            }
//        }
//        public override void SetValueWithoutNotify(float value)
//        {
//            base.SetValueWithoutNotify(value);
//            if (colorSlider != null)
//                colorSlider.SetValueWithoutNotify(value);
//        }
//        public void SetColor(Color sliderColor, Color background)
//        {
//            if (sliderImage)
//                sliderImage.color = sliderColor;
//            if (this.background)
//                this.background.color = background;
//        }
//        void SliderCall(float value)
//        {
//            this.value = value;
//            if (valueText)
//                valueText.Text = value.ToString();//直接用setValuewithoutnotify会导致再次设置拖条的值，
//            onSubmit?.Invoke(value);
//        }
//        public override float ProcessText(string text)
//        {
//            if (float.TryParse(text, out float value))//通过文本输入的，还需要设置拖条的值
//            {
//                //if (colorSlider != null)
//                //    value = ToolsSet.Clamp(value, colorSlider.minValue, colorSlider.maxValue);
//                //return value;//通过输入框输入的，允许溢出
//            }
//            return value;
//        }
//    }
//}