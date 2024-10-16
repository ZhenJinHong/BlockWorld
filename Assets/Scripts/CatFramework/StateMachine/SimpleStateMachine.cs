using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public interface ISimpleState<T>
        where T : class
    {
        void Enter(SimpleStateMachine<T> ssm);
        void Exit(SimpleStateMachine<T> ssm);
        void Update(SimpleStateMachine<T> ssm);
        void LateUpdate(SimpleStateMachine<T> ssm);
    }
    public class SimpleStateMachine<T> where T : class
    {
        T owner;
        public T Owner => owner;
        public SimpleStateMachine(T owner)
        {
            this.owner = owner;
        }
        ISimpleState<T> current;

        public void Start(ISimpleState<T> state)
        {
            if (current != state)
            {
                current?.Exit(this);
                current = state;
                current?.Enter(this);
            }
        }
        public void Update()
        {
            current?.Update(this);
        }
        public void LateUpdate()
        {
            current?.LateUpdate(this);
        }
    }
}
