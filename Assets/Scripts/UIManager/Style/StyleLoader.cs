using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CatFramework.UiMiao
{
    public class StyleLoader : MonoBehaviour
    {
        public string assetLabel = "UGUIStyle";
        [SerializeField] Font[] fonts;
        // Use this for initialization
        void Start()
        {
            //var ilist = Addressables.LoadAssetsAsync<StyleObject>(assetLabel, null).WaitForCompletion();
            UiManagerMiao.SetStyle(new Style(fonts/*, ilist*/));
        }
        private void OnDestroy()
        {
            UiManagerMiao.ReleaseStyle();
        }
    }
}