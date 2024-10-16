using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public abstract class PageListViewTransfer<VisualItem, Collection, ListViewController> : MonoBehaviour, IScrollHandler, IPageListViewTransfer
        where VisualItem : Component, IUiItem
        where Collection : class
        where ListViewController : PageListViewUMCtr<VisualItem, Collection>, new()
    {
        [SerializeField] protected int maxPageSize = -1;
        [SerializeField] protected bool destroyItemWhenClose;
        [SerializeField] protected VisualItem prefab;
        [Header("内容父级")]
        [SerializeField] protected RectTransform content;
        protected ListViewController viewController;
        public ListViewController ViewController => viewController;
        public IPageListViewController PageListViewController => viewController;
        public Collection Items
        {
            get => viewController.Items;
            set => viewController.Items = value;
        }
        protected virtual void Awake()
        {
            viewController = new ListViewController()
            {
                Prefab = prefab,
                Parent = content,
                IsUsable = true,
                destroyItemWhenClose = destroyItemWhenClose,
                PageSizeProvider = CreatePageSizeProvider(),
            };

            viewController.PageSizeProvider.MaxPageSize = maxPageSize;
            PopulateOtherViewArgs(viewController);
        }
        protected virtual void Start()
        {

        }
        protected virtual void OnDestroy()
        {
            viewController.Items = null;// 解除视图控制器对集合事件的订阅
        }
        protected virtual IPageSizeProvider CreatePageSizeProvider()
        {
            return UGUIUtility.CreatePageSizeProvider(prefab.GetComponent<RectTransform>(), content);
        }
        protected virtual void PopulateOtherViewArgs(ListViewController listView) { }
        public void SetPageNum(int targetPage) => viewController.SetPageNum(targetPage);
        public virtual void Open() => viewController.Open();
        public virtual void Close() => viewController.Close();
        public virtual void OnScroll(PointerEventData eventData)
           => viewController.OnScroll(eventData.scrollDelta.y > 0 ? -1 : 1);
    }
}