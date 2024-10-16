using System;
using System.Collections.Generic;

namespace CatFramework.UiTK
{
    public class UIFSM
    {
        protected IUIState lastState;
        public bool HasState=> lastState != null;
        public UIFSM() { }
        /// <summary>
        /// 这个方法应当由用户的操作触发比如点击了按钮或鼠标按键触发设置状态，继而再退出上个状态
        /// </summary>
        public virtual void SetState(IUIState state)
        {
            if (lastState != state)
            {
                lastState?.OnExit();         // 上个状态退出
                lastState = state;           // 先赋值
                state?.OnEnter();            // 进入下个状态
            }
        }
        public void Update() => lastState?.OnUpdate();
        public void LateUpdate() => lastState?.OnLateUpdate();
    }
    /// <summary>
    /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
    /// </summary>
    public interface IUIState
    {
        /// <summary>
        /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
        /// </summary>
        void OnEnter();
        void OnUpdate();
        void OnLateUpdate();
        /// <summary>
        /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
        /// </summary>
        void OnExit();
    }
    /// <summary>
    /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
    /// </summary>
    public abstract class UIState : IUIState
    {
        /// <summary>
        /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
        /// </summary>
        public abstract void OnEnter();
        /// <summary>
        /// Enter和Exit在逻辑上应当成对相反，并且在这两个方法里不应该FSM->SetState
        /// </summary>
        public abstract void OnExit();
        public virtual void OnLateUpdate() { }
        public virtual void OnUpdate() { }
    }
}
