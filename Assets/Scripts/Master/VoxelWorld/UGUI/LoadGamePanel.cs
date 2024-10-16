using CatFramework.UiMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    public class LoadGamePanel : MonoBehaviour
    {
        [SerializeField] ButtonMiao ready;

        private void Start()
        {
            ready.OnClick += Ready;
            transform.SetAsLastSibling();
        }
        void Ready(UiClick uiClick)
        {
            Destroy(this.gameObject);
        }
    }
}