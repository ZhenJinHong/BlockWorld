using System;
using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class ShareEnquireView : MonoBehaviour
    {
        [SerializeField] EnquireViewUMCtr enquireViewUMCtr;
        private void Awake()
        {
            if (UiManagerMiao.shareEnquireView == null)
                UiManagerMiao.shareEnquireView = this;
        }
        private void Start()
        {
            enquireViewUMCtr.OnOk += OK;
            enquireViewUMCtr.OnCancel += Cancel;
        }
        private void OnDestroy()
        {
            if (UiManagerMiao.shareEnquireView == this)
                UiManagerMiao.shareEnquireView = null;
        }
        Action onOk, onCancel;
        public void Show(string tip, Action onOk, Action onCancel = null)
        {
            this.onOk = onOk;
            this.onCancel = onCancel;
            enquireViewUMCtr.TipInfoNoTranslate = tip;
            enquireViewUMCtr.Open();
        }
        void OK(UiClick uiClick)
        {
            onOk?.Invoke();
            Clear();
        }
        void Cancel(UiClick uiClick)
        {
            onCancel?.Invoke();
            Clear();
        }
        void Clear() { onOk = null; onCancel = null; }
    }
}