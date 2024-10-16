using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    [RequireComponent(typeof(RectTransform))]
    public class CanvasPrefabUMCtr : ViewUMCtrBase
    {
        [SerializeField] RectTransform parent;
        [SerializeField] GameObject prefab;
        GameObject instance;

        public override bool IsVisual => instance != null;
        public override void Enable()
        {
            Show();
        }
        public override void Disable()
        {
            Hide();
        }
        public override void Show()
        {
            if (prefab == null)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning($"空的预制体!");
                return;
            }
            if (instance == null)
            {
                instance = Instantiate(prefab, parent == null ? transform : parent);
            }
        }
        public override void Hide()
        {
            if (instance != null)
                Destroy(instance);
            instance = null;
        }
    }
}