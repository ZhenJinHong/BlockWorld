using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatFramework.UiTK
{
    public class MessagePopup : VisualElementController
    {
        readonly Button ok;
        readonly Button cancel;
        readonly Label message;
        readonly Label title;
        public MessagePopup() : base(CreateContainer())
        {
            AddToClassList(viewAbsFullClass);
            AddToClassList(viewChildCenterCenterClass);

            VisualElement container = new VisualElement();
            Add(container);

            title = new Label().Default();
            title.AddToClassList(h3Class);
            title.AddToClassList(colorBgSqueakyOpacityClass);
            container.Add(title);
            message = new Label().Default();
            message.AddToClassList(h4Class);
            message.AddToClassList(colorBgSqueakyOpacityClass);
            container.Add(message);
            VisualElement btnContainer = new VisualElement();
            btnContainer.AddToClassList(viewRowClass);
            container.Add(btnContainer);

            ok = CreateButton(OKDown, "OK");
            btnContainer.Add(ok);
            cancel = CreateButton(CancelDown, "Back");
            btnContainer.Add(cancel);

            //root.RegisterCallback<MouseDownEvent>(RayBlock);// 根不可以监听Down事件否则优先触发根的Down事件
            // ok和back改用了button 优先触发了ok或back
            // 并且这里依旧必须使用down，因为别的控件比如列表是使用down触发的操作（而这个操作打开了消息框）
            // 如果使用up，会因为down刚打开消息框，结果下一帧up，就立刻关闭了消息框
            Target.RegisterCallback<MouseDownEvent>(RayBlock);
        }
        Action<MessageBoxData> OKAction;
        Action<MessageBoxData> CancelAction;
        Action<MessageBoxData> RayBlockAction;

        void OKDown()
        {
            //e.StopPropagation();// 停止同类事件的传播
            //e.StopImmediatePropagation();// 停止包括其他事件的传递？
            MessageBoxData data = new MessageBoxData();
            OKAction?.Invoke(data);
            if (data.Stop)
            {
                Stop();
            }
        }
        void CancelDown()
        {
            MessageBoxData data = new MessageBoxData();
            CancelAction?.Invoke(data);
            if (data.Stop)
            {
                Stop();
            }
        }
        void RayBlock(MouseDownEvent e)
        {
            e.StopPropagation();//不需要停止传播，这个已经是最顶层了
            MessageBoxData data = new MessageBoxData();
            RayBlockAction?.Invoke(data);
#if UNITY_EDITOR
            Debug.Log("拦截了输入");
#endif
            if (data.Stop)
            {
                Stop();
            }
        }
        public void Stop()
        {
            Close();
            OKAction = null;
            CancelAction = null;
            RayBlockAction = null;
        }

        public void AddListener(Action<MessageBoxData> okAction, Action<MessageBoxData> cancelAction, Action<MessageBoxData> rayBlockAction)
        {
            this.OKAction = okAction;
            this.CancelAction = cancelAction;
            this.RayBlockAction = rayBlockAction;
            ok.Visible_C(okAction != null);
            cancel.Visible_C(cancelAction != null);
        }
        public void ShowMessage(string message, string title = null, bool blockRay = true)
        {
            Open();
            this.message.text = message;
            if (title == null)
            {
                this.title.Display_C(false);
            }
            Target.pickingMode = blockRay ? PickingMode.Position : PickingMode.Ignore;
        }
    }
}
