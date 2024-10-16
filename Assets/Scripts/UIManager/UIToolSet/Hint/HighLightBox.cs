//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace CatFramework
//{
//    [RequireComponent(typeof(CanvasGroup))]
//    public class HighLightBox : UiImage
//    {
//        [SerializeField] CanvasGroup canvasGroup;
//        RectTransform rect;
//        private void Awake()
//        {
//            if (canvasGroup == null) { canvasGroup = GetComponent<CanvasGroup>(); }
//        }
//        protected override void Start()
//        {
//            base.Start();
//            rect = GetComponent<RectTransform>();
//            Close();
//            //Entry.UiManager.CallHighLightBox += SetHighLight;
//        }
//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//            //Entry.UiManager.CallHighLightBox -= SetHighLight;
//        }
//        public void SetHighLight(IHighLightBoxCaller highLightBoxCaller)
//        {
//            if (highLightBoxCaller != null)
//            {
//                RectTransform Rect = highLightBoxCaller.RectTransform;
//                if (Rect != null)
//                {
//                    rect.position = Rect.position;
//                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Rect.rect.width);
//                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Rect.rect.height);
//                    Open();
//                }
//            }
//            else
//            {
//                Close();
//            }
//        }
//        public virtual void Open()
//        {
//            //canvasGroup.interactable = true;
//            //canvasGroup.blocksRaycasts = true;
//            canvasGroup.alpha = 1;
//        }
//        public virtual void Close()
//        {
//            //canvasGroup.interactable = false;
//            //canvasGroup.blocksRaycasts = false;
//            canvasGroup.alpha = 0;
//        }
//    }
//}