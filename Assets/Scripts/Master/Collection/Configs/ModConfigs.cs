using CatFramework.DataMiao;
using UnityEngine;

namespace CatFramework
{
    [System.Serializable]
    public class ModConfigs : IConfig
    {
        [SerializeField] string modName;
        [SerializeField] string modVersion;
        [SerializeField] string modAuthor;
        public string Name => modName;
        public string Version => modVersion;
        public string Author => modAuthor;
    }
}
