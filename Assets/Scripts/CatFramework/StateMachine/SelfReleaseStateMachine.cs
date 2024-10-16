using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.StateMachine
{
    public class SelfReleaseStateMachine<Owner>
    {
        ISelfReleaseState<Owner> current;
        public ISelfReleaseState<Owner> Current
            => current;
        public bool HasState
            => current != null;
        public bool IsCurrentState(ISelfReleaseState<Owner> state)
            => current == state;
        public void Set(ISelfReleaseState<Owner> state)
        {
            if (current != state)
            {
                current?.Exit();
                current = state;
                current?.Enter();
            }
        }
        public void Update(Owner input)
        {
            if (current != null)
            {
                if (!current.Update(input))
                {
                    current = null;
                }
            }
        }
    }
    public interface ISelfReleaseState<T>
    {
        void Enter();
        /// <summary>
        /// 允许在循环中返回flase,则状态机清除该状态
        /// </summary>
        /// <returns></returns>
        bool Update(T input);
        void Exit();
    }
    public class SelfReleaseStateMachine
    {
        ISelfReleaseState current;
        public ISelfReleaseState Current 
            => current;
        public bool HasState
            => current != null;
        public bool IsCurrentState(ISelfReleaseState state) 
            => current == state;
        public void Set(ISelfReleaseState state)
        {
            if (current != state)
            {
                current?.Exit();
                current = state;
                current?.Enter();
            }
        }
        public void Update(float deltaTime)
        {
            if (current != null)
            {
                if (!current.Update(deltaTime))
                {
                    current = null;
                }
            }
        }
    }
    public interface ISelfReleaseState
    {
        void Enter();
        /// <summary>
        /// 允许在循环中返回flase,则状态机清除该状态
        /// </summary>
        /// <returns></returns>
        bool Update(float deltaTime);
        void Exit();
    }
}
