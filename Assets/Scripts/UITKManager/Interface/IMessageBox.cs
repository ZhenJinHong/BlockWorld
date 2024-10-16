using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.UiTK
{
    public interface IMessageBox
    {
        void AddListener(Action<MessageBoxData> okAction, Action<MessageBoxData> cancelAction, Action<MessageBoxData> rayBlockAction);
        void ShowMessage(string message, string title = null, bool blockRay = true);
        void Stop();
    }
    public class EmptyMessageBox : IMessageBox
    {
        void DebugNull()
        {
#if UNITY_EDITOR
            Debug.LogWarning("缺失消息盒");
#endif
        }
        public void AddListener(Action<MessageBoxData> okAction, Action<MessageBoxData> cancelAction, Action<MessageBoxData> rayBlockAction) => DebugNull();
        public void ShowMessage(string message, string title = null, bool blockRay = true) => DebugNull();
        public void Stop() => DebugNull();
    }
    /// <summary>
    /// 默认在发送了消息后关闭消息框
    /// </summary>
    public class MessageBoxData
    {
        bool show;
        public bool Stop => !show;
        public void StopHide()
        {
            show = true;
        }
    }
}
