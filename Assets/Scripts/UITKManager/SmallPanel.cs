using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static CatFramework.UiTK.VisualElementController;
namespace CatFramework.UiTK
{
    [DisallowMultipleComponent, RequireComponent(typeof(UIDocument))]
    public class SmallPanel : MonoBehaviour
    {
        protected UIDocument UIDocument => GetComponent<UIDocument>();
        public bool IsVisual => !root.ClassListContains(noDisplayClass);
        VisualElement root;
        public VisualElement Root => root;
        protected virtual void Awake()
        {
            root = UIDocument.rootVisualElement;
        }
        protected virtual void Start()
        {
        }
        protected virtual void OnDestroy()
        {
        }
    }
}