using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CatFramework.UiMiao
{
    [DisallowMultipleComponent]
    public abstract class ViewUMCtrBase : MonoBehaviour
    {
        public event Action<ViewUMCtrBase> OnOpen;
        public event Action<ViewUMCtrBase> OnClose;
        [SerializeField] bool closeWhenStart = true;
        public abstract bool IsVisual { get; }
        protected virtual void Awake()
        {
        }
        protected virtual IEnumerator Start()
        {
            yield return null;
            if (closeWhenStart)
                Hide();
        }
        protected virtual void OnDestroy()
        {

        }
        public void OpenOrClose()
        {
            if (IsVisual) Close();
            else Open();
        }
        public void OpenOrClose(bool open)
        {
            if (open) Open();
            else Close();
        }
        // 不做当前是否可视的判断,以便可以重复打开重复发布事件
        public virtual void Open()
        {
            OnOpen?.Invoke(this);
            Show();
        }
        public virtual void Close()
        {
            OnClose?.Invoke(this);
            Hide();
        }
        public abstract void Enable();
        public abstract void Disable();
        public abstract void Show();
        public abstract void Hide();
    }
}