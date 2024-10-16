using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public abstract class ViewModule : MonoBehaviour, IViewModule
    {
        [SerializeField] bool closeWhenStart = true;
        public virtual bool IsUsable => this != null;
        protected virtual void Awake() { }
        protected virtual void Start()
        {
            if (closeWhenStart)
                Close();
        }
        protected virtual void OnDestroy() { }
        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            //if (ConsoleCat.Enable) ConsoleCat.Log($"打开视图模块{this.GetType()}");// 一些必要的提示
        }

        public abstract bool Show(object content);
    }
}
