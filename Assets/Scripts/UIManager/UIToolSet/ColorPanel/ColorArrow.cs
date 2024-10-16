//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//namespace CatFramework
//{
//    public class ColorArrow : MonoBehaviour, IPointerClickHandler
//    {
//        [SerializeField] Slider slider;
//        [SerializeField] Image image;
//        [SerializeField] Image background;
//        IColorArrows ColorArrows;
//        private void Awake()
//        {
//            if (slider != null)
//            {
//                slider.onValueChanged.RemoveAllListeners();
//                slider.onValueChanged.AddListener(SliderCallBack);
//            }
//        }
//        public void AddListener(IColorArrows colorArrows)
//        {
//            ColorArrows = colorArrows;
//        }
//        //public void Select()
//        //{
//        //    if (slider)
//        //        slider.Select();
//        //}
//        public void SetSliderValueWithoutNotify(float value)
//        {
//            if (slider != null)
//                slider.SetValueWithoutNotify(value);
//        }
//        public void SetImageColor(Color color)
//        {
//            if (image != null)
//                image.color = color;
//        }
//        public void SetBackgroundColor(Color color)
//        {
//            if (background != null)
//                background.color = color;
//        }
//        public void SetValueWithoutNotify(float value, Color color/*, Vector2 anchoredPosition*/)
//        {
//            if (slider != null)
//            {
//                slider.SetValueWithoutNotify(value);
//            }
//            if (image != null)
//            {
//                image.color = color;
//            }
//        }
//        void SliderCallBack(float value)
//        {
//            ColorArrows?.SetTime(value, this);
//        }
//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (eventData.button == PointerEventData.InputButton.Left)
//            {
//                ColorArrows?.SelectArrow(this);
//            }
//            else if (eventData.button == PointerEventData.InputButton.Right)
//            {
//                ColorArrows?.DeleteArrow(this);
//            }
//        }
//    }
//}