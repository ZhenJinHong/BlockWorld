//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace CatFramework.UiMiao
//{
//    [Obsolete]
//    public class MutexViewUMCtr : MonoBehaviour
//    {
//        [SerializeField] List<ViewUMCtrBase> viewUMCtrs;
//        public ViewUMCtrBase Current { get; private set; }
//        /// <summary>
//        /// 传递的是现在新打开的视窗
//        /// </summary>
//        public Action<ViewUMCtrBase> OnTabContentOpen;
//        /// <summary>
//        /// 传递的是现在刚关闭的视窗
//        /// </summary>
//        public Action<ViewUMCtrBase> OnTabContentClose;
//        private void Start()
//        {
//            if (viewUMCtrs == null)
//                viewUMCtrs = new List<ViewUMCtrBase>();
//            else
//            {
//                for (int i = 0; i < viewUMCtrs.Count; i++)
//                {
//                    ViewUMCtrBase viewUMCtr = viewUMCtrs[i];
//                    RegisterView(viewUMCtr);
//                }
//            }
//        }
//        public void RegisterView(ViewUMCtrBase viewUMCtr)
//        {
//            if (viewUMCtr == null || viewUMCtrs.Contains(viewUMCtr)) return;
//            if (viewUMCtr.IsVisual)
//            {
//                if (Current != null)
//                    Current.Close();
//                Current = viewUMCtr;
//            }
//            viewUMCtr.OnOpen += WhenTabContentOpen;
//            viewUMCtr.OnClose += WhenTabContentClose;
//        }
//        public void UnregisterView(ViewUMCtrBase viewUMCtr)
//        {
//            if (viewUMCtrs.Remove(viewUMCtr))
//            {
//                viewUMCtr.OnOpen -= WhenTabContentOpen;
//                viewUMCtr.OnClose -= WhenTabContentClose;
//            }
//        }
//        void WhenTabContentOpen(ViewUMCtrBase viewUMCtr)
//        {
//            if (Current != viewUMCtr)
//            {
//                if (Current != null)
//                    Current.Close();
//                if (Current != null && ConsoleCat.Enable)
//                {
//                    ConsoleCat.DebugInfo("上个标签未正常关闭，逻辑有误？");
//                }
//            }
//            Current = viewUMCtr;
//            OnTabContentOpen?.Invoke(viewUMCtr);
//        }
//        void WhenTabContentClose(ViewUMCtrBase viewUMCtr)
//        {
//            if (Current == viewUMCtr)
//            {
//                Current = null;
//            }
//            else
//            {
//                if (ConsoleCat.Enable)
//                    ConsoleCat.DebugInfo("关闭的窗口与当前窗口不匹配");
//            }
//            OnTabContentClose?.Invoke(viewUMCtr);
//        }
//    }
//}