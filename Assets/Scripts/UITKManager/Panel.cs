using CatFramework.Localized;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using static CatFramework.UiTK.VisualElementController;
namespace CatFramework.UiTK
{
    /// <summary>
    /// 面板实际为逻辑处理！！！，由此，面板将处理更新迭代，所以一开始必须非活动状态
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(UIDocument))]
    public abstract class Panel : MonoBehaviour
    {
        protected static VisualElement CreateFullVisualElement()
        {
            var visualElement = new VisualElement();
            visualElement.AddToClassList(viewFullClass);
            return visualElement;
        }
        protected UIDocument UIDocument => GetComponent<UIDocument>();
        public bool IsVisual => !root.ClassListContains(noDisplayClass);
        VisualElement root;
        public VisualElement Root => root;
        bool IsInit;
        protected virtual void Awake()
        {
            root = UIDocument.rootVisualElement;
            root.AddToClassList(noDisplayClass);
            //UITKManagerCat.RegisterWaitForInit(this);
        }
        protected virtual void Start()
        {
            LocalizedManagerMiao.OnLanguageChange += LanguageChange;
            enabled = false;// 需要Start能够执行了
        }
        protected virtual void OnDestroy()
        {
            LocalizedManagerMiao.OnLanguageChange -= LanguageChange;
        }
        public void Initialized()
        {
            if (!IsInit)
            {
                InternalInitialized();
                IsInit = true;
            }
        }
        protected virtual void InternalInitialized()
        {

        }
        void LanguageChange()
        {
            UQueryBuilder<TextElement> uQuery = root.Query<TextElement>();
            uQuery.ForEach(ForEach);

            static void ForEach(TextElement element)
            {
                element.UpdateText();
            }
        }
        public void Add(VisualElement visualElement)
        {
            root.Add(visualElement);
        }
        public void AddToClassList(string className)
        {
            root.AddToClassList(className);
        }
        public void OpenOrClose()
        {
            if (root.ClassListContains(noDisplayClass))
                Open();
            else
                Close();
        }
        public virtual void Open()
        {
            root.RemoveFromClassList(noDisplayClass);
            enabled = true;
        }
        public virtual void Close()
        {
            root.AddToClassList(noDisplayClass);
            enabled = false;
        }
        public void Show()
        {
            root.RemoveFromClassList(noDisplayClass);
        }
        public void Hide()
        {
            //if (!IsInit)
            //    Initialized();
            root.AddToClassList(noDisplayClass);
        }
    }
}
