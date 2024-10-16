using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CatFramework.UiMiao
{
    public class EnquireViewUMCtr : CanvasEnableUMCtr
    {
        [SerializeField] TextMiao tipInfo;
        [SerializeField] ButtonMiao ok;
        [SerializeField] ButtonMiao cancel;
        protected override void Awake()
        {
            base.Awake();
            ok.OnClick += Close;
            cancel.OnClick += Close;
        }
        public string TipInfoNoTranslate
        {
            get { if (tipInfo != null) return tipInfo.TextValue; return string.Empty; }
            set { if (tipInfo != null) tipInfo.TextValue = value; }
        }
        public event Action<UiClick> OnOk
        {
            add { if (ok != null) ok.OnClick += value; }
            remove { if (ok != null) ok.OnClick -= value; }
        }
        public event Action<UiClick> OnCancel
        {
            add { if (cancel != null) cancel.OnClick += value; }
            remove { if (cancel != null) cancel.OnClick -= value; }
        }
        public void Close(UiClick uiClick) => Close();
    }
}