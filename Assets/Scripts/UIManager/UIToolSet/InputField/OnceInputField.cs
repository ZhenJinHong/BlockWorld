using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CatFramework.CatMath;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    public sealed class OnceInputField : MonoBehaviour, IScrollHandler
    {
        [SerializeField] InputField inputField;
        //RectTransform originalParent;
        RectTransform rectTransform;

        ITextReceiver textReceiver;
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            if (inputField == null)
                inputField = GetComponent<InputField>();
            //originalParent = rectTransform.parent as RectTransform;
            //if (originalParent == null)
            //{
            //    ConsoleCat.NullError();
            //}
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnValueChange);
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnSubmit);
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(OnEndEdit);
            gameObject.SetActive(false);

            //Entry.UiManager.CallOnceInputField += AddListener;
            if (UiManagerMiao.OnceInputField == null) UiManagerMiao.OnceInputField = this;
        }
        private void OnDestroy()
        {
            //Entry.UiManager.CallOnceInputField -= AddListener;
            if (UiManagerMiao.OnceInputField == this) UiManagerMiao.OnceInputField = null;
        }
        /// <summary>
        /// 特殊字符已去除
        /// </summary>
        public void Call(ITextReceiver textReceiver)
        {
            if (rectTransform == null) return;
            this.textReceiver = textReceiver;
            if (textReceiver != null)
            {
                if (!textReceiver.IsDestroy)
                {
                    RectTransform rect = textReceiver.CoverageRect;
                    if (rect != null)
                    {
                        //rectTransform.SetParent(rect);
                        rectTransform.position = rect.position;
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width);
                        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.rect.height);
                        //设定默认值会导致输入字段失活
                        inputField.SetTextWithoutNotify(textReceiver.OriginalText);
                        gameObject.SetActive(true);
                        inputField.ActivateInputField();
                    }
                }
            }
        }
        void OnValueChange(string Value)
        {
            if (textReceiver != null)
            {
                if (!textReceiver.IsDestroy)
                {
                    if (textReceiver.OneKeyShoot)
                    {
                        Recover();
                    }
                    else
                    {
                        textReceiver.OnValueChange(Value);
                    }
                }
                else
                {
                    Recover();
                }
            }
        }
        void OnSubmit(string v)
        {
            if (textReceiver != null)
            {
                if (!textReceiver.IsDestroy)
                {
                    if (MathC.CheackTextIsError(v))
                    {
                        v = MathC.ProcessedText(v);
                    }
                    textReceiver.OnSubmit(v);
                }
                else
                {
                    Recover();
                }
            }
        }
        void OnEndEdit(string v)
        {
            if (textReceiver != null)
            {
                if (!textReceiver.IsDestroy)
                {
                    if (MathC.CheackTextIsError(v))
                    {
                        v = MathC.ProcessedText(v);
                    }
                    textReceiver.OnEndEdit(v);
                }
                Recover();
            }
        }
        void Recover()
        {
            gameObject.SetActive(false);
            //rectTransform.SetParent(originalParent);
            textReceiver = null;
        }

        public void OnScroll(PointerEventData eventData)
        {
            Recover();
        }
    }
}