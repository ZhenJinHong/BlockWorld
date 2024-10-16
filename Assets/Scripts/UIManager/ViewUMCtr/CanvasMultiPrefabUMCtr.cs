using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    /// <summary>
    /// 同时实例化所有的预制体
    /// </summary>
    public class CanvasMultiPrefabUMCtr : ViewUMCtrBase
    {
        [SerializeField] RectTransform parent;
        [SerializeField] GameObject[] prefabs;
        GameObject[] instances;
        public override bool IsVisual => instances != null;
        public override void Disable()
        {
        }
        public override void Enable()
        {
        }
        public override void Hide()
        {
            if (instances != null)
            {
                for (int i = 0; i < instances.Length; i++)
                {
                    if (instances[i] != null)
                        Destroy(instances[i]);
                }
                instances = null;
            }
        }
        public override void Show()
        {
            if (instances != null) return;
            RectTransform rect = parent == null ? transform as RectTransform : parent;
            instances = new GameObject[prefabs.Length];
            for (int i = 0; i < prefabs.Length; i++)
            {
                var prefab = prefabs[i];
                if (prefab != null)
                    instances[i] = Instantiate(prefab, rect);
                else
                    if (ConsoleCat.Enable) ConsoleCat.LogWarning("空预制体");
            }
        }
    }
}