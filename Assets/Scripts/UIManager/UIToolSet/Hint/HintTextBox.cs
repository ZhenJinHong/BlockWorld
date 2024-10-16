//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;

//namespace CatFramework
//{
//    [RequireComponent(typeof(CanvasGroup))]
//    public class HintTextBox : MonoBehaviour
//    {
//        [SerializeField] Text text;
//        [SerializeField] RectTransform rectTrans;
//        [SerializeField] CanvasGroup canvasGroup;
//        float timer;
//        void Start()
//        {
//            if (rectTrans == null)
//                rectTrans = GetComponent<RectTransform>();
//            hasRectTrans = rectTrans;
//            if (text == null)
//                ConsoleCat.NullError();
//            Close();
//            //Entry.UiManager.CallHintTextBox += Hint;
//        }
//        private void OnDestroy()
//        {
//            //Entry.UiManager.CallHintTextBox -= Hint;
//        }
//        bool hasRectTrans;
//        void FixedUpdate()
//        {
//            if (hasRectTrans)
//            {
//                timer -= Time.deltaTime;
//                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, Mouse.current.position.ReadValue(), null, out Vector3 pos);
//                rectTrans.position = pos;
//                if (timer < 0)
//                {
//                    Close();
//                }
//            }
//        }
//        public void Hint(IHintTextBoxCaller hintTextBoxCaller)
//        {
//            if (hintTextBoxCaller != null && !hintTextBoxCaller.IsDestory)
//            {
//                text.text = hintTextBoxCaller.HintText;
//                timer = hintTextBoxCaller.HintText == null ? 0 : hintTextBoxCaller.HintText.Length * 0.1f;
//                if (timer < 1f) timer = 1f;
//                else if (timer > 3f) timer = 3f;
//                Open();
//            }
//            else
//            {
//                Close();
//            }
//        }
//        void Open()
//        {
//            canvasGroup.interactable = true;
//            canvasGroup.blocksRaycasts = true;
//            canvasGroup.alpha = 1;
//            enabled = true;
//        }
//        void Close()
//        {
//            canvasGroup.interactable = false;
//            canvasGroup.blocksRaycasts = false;
//            canvasGroup.alpha = 0;
//            enabled = false;
//        }
//    }
//}