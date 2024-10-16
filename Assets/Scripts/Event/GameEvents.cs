using System;

namespace CatFramework.EventsMiao
{
    public interface IGameEventsListener<Events, Data>
         where Data : class
         where Events : class, IGameEvents<Data>, new()
    {
        void Enter(Data data);
        void Exit(Data data);
        void Pause(Data data);
        void Continue(Data data);
    }
    public interface IGameEvents<T> : IUniqueEvents
    {
        event Action<T> OnEnter;
        event Action<T> OnPause;
        event Action<T> OnContinue;
        event Action<T> OnExit;
        void Enter(T data);
        void Exit(T data);
        void Pause(T data);
        void Continue(T data);
    }
    public class GameEvents<T> : IGameEvents<T>
    {
        public event Action<T> OnEnter;
        public event Action<T> OnPause;
        public event Action<T> OnContinue;
        public event Action<T> OnExit;
        public T Data { get; private set; }


        void IGameEvents<T>.Enter(T data)
        {
            this.Data = data;
            OnEnter?.Invoke(data);
        }

        void IGameEvents<T>.Exit(T data)
        {
            this.Data = data;
            OnExit?.Invoke(data);
        }

        void IGameEvents<T>.Pause(T data)
        {
            this.Data = data;
            OnPause?.Invoke(data);
        }
        void IGameEvents<T>.Continue(T data)
        {
            this.Data = data;
            OnContinue?.Invoke(data);
        }
    }
}