//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace CatFramework
//{
//    public class UiImage : MonoBehaviour
//    {
//        [SerializeField] Image image;
//        [SerializeField] ImageType imageType;
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
//        public Image C_Image { get => image; }
//        public Color Color
//        {
//            get
//            {
//                return image.color;
//            }
//            set
//            {
//                image.color = value;
//            }
//        }
//        protected virtual void Start()
//        {
//            //ImageColorData.HasChanged += FixImage;
//            FixImage(ImageColorData);
//        }
//        protected virtual void OnDestroy()
//        {
//            //ImageColorData.HasChanged -= FixImage;
//            FixImage(ImageColorData);
//        }
//        #region 图像调整函数
//        public void FixImage(ImageColorData imageColorData)
//        {
//            image.color = imageColorData.GetImageColor(imageType);
//        }
//        #endregion
//    }
//}