using UnityEngine;
namespace CatFramework.Tools
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// 单例都是全局不销毁的
        /// </summary>
        protected static T instance;
        public static T Instance => instance;
        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = (T)this;
        }
        public static bool IsUsing { get { return instance != null; } }
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}

