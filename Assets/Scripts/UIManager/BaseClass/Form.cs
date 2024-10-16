//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using System;
//namespace CatFramework
//{
//    /// <summary>
//    /// 面板在Initialize里已经调用AddToMenuStrip
//    /// </summary>
//    [RequireComponent(typeof(CanvasGroup))]
//    public abstract class Form : MonoBehaviour, IForm
//    {
//        public abstract string AssetName { get; }
//        public int Order => order;
//        public bool IsActivated => canvasGroup.interactable;
//        public string MenuItemID => menuItemID;

//        [SerializeField] protected int order;
//        [SerializeField] string menuItemID;
//        [SerializeField, Tooltip("多个父级以'/'划分")] string parentMenustrip;
//        [SerializeField] Image formImage;
//        [SerializeField] protected CanvasGroup canvasGroup;
//        bool IsInitialize;
//        public void Initialize()
//        {
//            if (IsInitialize) return; IsInitialize = true;
//            if (canvasGroup == null)
//                canvasGroup = GetComponent<CanvasGroup>();
//            if (!string.IsNullOrWhiteSpace(parentMenustrip))
//            {
//                //Entry.UiManager.AddToMenuStrip(this, parentMenustrip);
//                parentMenustrip = null;
//            }
//            InternalInitialize();
//            Hide();
//        }
//        protected virtual void OnDestroy()
//        {
//            if (formImage != null)
//            {
//                //UISettingCollection uiSettingCollection = DataManagerCat.GetDataCollection<UISettingCollection>();
//                //uiSettingCollection.ImageColorData.HasChanged -= FixImage;
//            }
//        }
//        protected abstract void InternalInitialize();
//        public abstract void UpdateForm();
//        public void Sort(int order)
//        {
//            transform.SetSiblingIndex(order);
//        }
//        /// <summary>
//        /// 专项专用，只有完全打开时调用
//        /// </summary>
//        public event Action<Form> OpenFormevent;
//        /// <summary>
//        /// 专项专用，只有完全关闭时调用
//        /// </summary>
//        public event Action<Form> CloseFormevent;
//        /// <summary>
//        /// 除了面板的打开事件，不要在里面抛别的事件，开关现在是交给MenuStrip管理的
//        /// </summary>
//        public virtual void Open()
//        {
//            Show();
//            OpenFormevent?.Invoke(this);

//        }
//        /// <summary>
//        /// 除了面板的关闭事件，不要在里面抛别的事件，开关现在是交给MenuStrip管理的
//        /// </summary>
//        public virtual void Close()
//        {
//            Hide();
//            CloseFormevent?.Invoke(this);
//        }
//        public virtual void EnableForm()
//        {
//            canvasGroup.interactable = true;
//            canvasGroup.blocksRaycasts = true;
//            canvasGroup.alpha = 1;
//        }
//        public virtual void DisableForm()
//        {
//            canvasGroup.interactable = false;
//            canvasGroup.blocksRaycasts = false;
//            canvasGroup.alpha = 0.1f;
//        }
//        public void Show()
//        {
//            canvasGroup.interactable = true;
//            canvasGroup.blocksRaycasts = true;
//            canvasGroup.alpha = 1;
//        }
//        public void Hide()
//        {
//            canvasGroup.interactable = false;
//            canvasGroup.blocksRaycasts = false;
//            canvasGroup.alpha = 0;
//        }
//    }
//}