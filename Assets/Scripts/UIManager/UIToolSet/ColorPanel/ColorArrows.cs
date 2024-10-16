//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//namespace CatFramework
//{
//    public interface IColorArrows
//    {
//        void DeleteArrow(ColorArrow colorArrow);
//        void SelectArrow(ColorArrow colorArrow);
//        void SetTime(float time, ColorArrow colorArrow);
//    }
//    [DisallowMultipleComponent]
//    public class ColorArrows : MonoBehaviour, IPointerClickHandler, IColorArrows
//    {
//        [SerializeField] ColorArrow prefab;

//        RectTransform rectTransform;
//        List<ColorArrow> arrowList;
//        bool enbleInteract = true;
//        bool isColorArrow;
//        IGradientPicker gradientPicker;
//        int currentIndex = -1;
//        public int CurrentIndex => currentIndex;
//        bool isInit;
//        private void OnDestroy()
//        {
//            arrowList = null;
//        }
//        #region 梯度拾取面板使用
//        public void Init(int count, IGradientPicker gradientPicker, bool isColorArrow)
//        {
//            if (isInit) return;
//            isInit = true;
//            if (prefab == null)
//            {
//                ConsoleCat.NullError();
//                return;
//            }
//            if (arrowList != null && arrowList.Count > 0)
//            {
//                for (int i = 0; i < arrowList.Count; i++)
//                {
//                    Destroy(arrowList[i]);
//                }
//                arrowList.Clear();
//            }
//            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
//            arrowList = new List<ColorArrow>(count);
//            this.gradientPicker = gradientPicker;
//            this.isColorArrow = isColorArrow;
//            for (int i = 0; i < count; i++)
//            {
//                ColorArrow colorArrow = Instantiate(prefab, rectTransform);
//                colorArrow.AddListener(this);
//                arrowList.Add(colorArrow);
//            }
//        }
//        /// <summary>
//        /// 禁用全部
//        /// </summary>
//        public void DisableArrows()
//        {
//            for (int i = 0; i < arrowList.Count; i++)
//            {
//                arrowList[i].gameObject.SetActive(false);
//            }
//        }
//        /// <summary>
//        /// 从当前键的数量，重新启用对应索引的箭头，并生成一个新的键
//        /// </summary>
//        public void EnableArrow(float time, Color color)
//        {
//            if (enbleInteract && gradientPicker != null)
//            {
//                enbleInteract = false;
//                int keyCount = gradientPicker.KeyCount(isColorArrow);
//                if (keyCount < arrowList.Count)//键的数量最多与箭头相等
//                {
//                    arrowList[keyCount].gameObject.SetActive(true);
//                    arrowList[keyCount].SetValueWithoutNotify(time, color);
//                    if (currentIndex != -1 && currentIndex < arrowList.Count)
//                    {
//                        arrowList[currentIndex].SetBackgroundColor(Color.gray);
//                    }
//                    currentIndex = keyCount;//转到新生成的键
//                    gradientPicker.CreateNewKey(time, isColorArrow);
//                }
//                enbleInteract = true;
//            }
//        }
//        /// <summary>
//        /// 设置当前的箭头的颜色
//        /// </summary>
//        public void SetArrowColor(Color color)
//        {
//            if (currentIndex != -1 && currentIndex < arrowList.Count)
//            {
//                arrowList[currentIndex].SetImageColor(color);
//            }
//        }
//        /// <summary>
//        /// 设置指定索引的箭头的颜色
//        /// </summary>
//        public void SetArrowColor(int index, Color color)
//        {
//            if (index < arrowList.Count)
//            {
//                arrowList[index].SetImageColor(color);
//            }
//        }
//        #endregion
//        #region 箭头调用
//        void IColorArrows.SelectArrow(ColorArrow colorArrow)
//        {
//            if (gradientPicker != null)
//            {
//                enbleInteract = false;
//                int index = arrowList.IndexOf(colorArrow);

//                if (index != -1 && index < gradientPicker.KeyCount(isColorArrow))
//                {
//                    if (currentIndex != -1 && currentIndex < arrowList.Count)
//                    {
//                        arrowList[currentIndex].SetBackgroundColor(Color.gray);
//                    }
//                    currentIndex = index;//更新当前索引
//                    colorArrow.SetBackgroundColor(Color.green);
//                    gradientPicker.SelectKey(index, isColorArrow);
//                }
//                enbleInteract = true;
//            }
//        }
//        void IColorArrows.SetTime(float time, ColorArrow colorArrow)
//        {
//            if (enbleInteract)
//            {
//                if (gradientPicker != null)
//                {
//                    enbleInteract = false;
//                    int index = arrowList.IndexOf(colorArrow);
//                    currentIndex = index;
//                    gradientPicker.SetTime(time, isColorArrow, index);
//                    enbleInteract = true;
//                }
//            }
//        }
//        void IColorArrows.DeleteArrow(ColorArrow colorArrow)
//        {
//            if (enbleInteract && gradientPicker != null)
//            {
//                enbleInteract = false;
//                int keyCount = gradientPicker.KeyCount(isColorArrow);
//                if (keyCount > 2)
//                {
//                    int index = arrowList.IndexOf(colorArrow);
//                    if (index != -1 && index < keyCount)
//                    {
//                        arrowList.RemoveAt(index);
//                        arrowList.Add(colorArrow);

//                        if (currentIndex == index)//和被移动至末尾的相同时（暂时禁用）
//                        {
//                            currentIndex = -1;//当前的箭标取消
//                        }
//                        colorArrow.gameObject.SetActive(false);
//                        gradientPicker.RemoveKey(index, isColorArrow);
//                    }
//                }
//                enbleInteract = true;
//            }
//        }
//        #endregion
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            if (rectTransform && gradientPicker != null)
//            {
//                //获得的点，以该rectTransform.localPosition为原点，计算的坐标
//                if (eventData.button == PointerEventData.InputButton.Left)
//                {
//                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
//                    {
//                        float di = localPoint.x;
//                        di -= rectTransform.rect.xMin;
//                        di /= rectTransform.rect.width;
//                        EnableArrow(di, Color.white);
//                    }
//                }
//            }
//        }
//    }
//}