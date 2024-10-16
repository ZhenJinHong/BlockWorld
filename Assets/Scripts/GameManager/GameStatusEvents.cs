//using CatFramework.EventsMiao;
//using System;
//using UnityEngine;

//namespace CatFramework.GameManager
//{
//    [Obsolete]
//    public interface IGameStatusEventListen<GameStatusEvents, GameData>
//    {
//        void Enter(GameData data);
//        void Exit(GameData data);
//        void Pause(GameData data);
//        void Continue(GameData data);
//    }
//    [Obsolete]
//    public abstract class GameStatusEventListen<GameStatusEvents, GameData> : MonoBehaviour
//        where GameData : class
//        where GameStatusEvents : class, IGameStatusEvents<GameData>, new()
//    {
//        protected virtual void Awake()
//        {

//        }
//        protected virtual void Start()
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameStatusEvents>();
//            gameEvents.OnEnter += Enter;
//            gameEvents.OnExit += Exit;
//            gameEvents.OnContinue += Continue;
//            gameEvents.OnPause += Pause;
//        }
//        protected virtual void OnDestroy()
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameStatusEvents>();
//            gameEvents.OnEnter -= Enter;
//            gameEvents.OnExit -= Exit;
//            gameEvents.OnContinue -= Continue;
//            gameEvents.OnPause -= Pause;
//        }
//        bool ingame;
//        protected bool Ingame => ingame;
//        bool pause;
//        protected bool IsPause => pause;
//        protected virtual void Enter(GameData data)
//        {
//            if (ingame)
//            {
//                if (ConsoleCat.Enable) ConsoleCat.LogWarning("InGameView重复设定进入游戏");// 检查Awake是否有那个重复订阅的问题
//            }
//            ingame = true;
//            pause = false;
//        }
//        protected virtual void Exit(GameData data)
//        {
//            ingame = false;
//            pause = false;
//        }
//        protected virtual void Continue(GameData data)
//        {
//            pause = false;
//        }
//        protected virtual void Pause(GameData data)
//        {
//            pause = true;
//        }
//    }
//    public abstract class GameStatusEvents<GameData> : IGameStatusEvents<GameData> where GameData : class
//    {
//        public event Action<GameData> OnEnter;
//        public event Action<GameData> OnPause;
//        public event Action<GameData> OnContinue;
//        public event Action<GameData> OnExit;
//        public GameData Data { get; private set; }
//        public GameStatusEvents()
//        {
//        }
//        void IGameStatusEvents<GameData>.Enter(GameData gameStatusData)
//        {
//            this.Data = gameStatusData;
//            OnEnter?.Invoke(gameStatusData);
//        }
//        void IGameStatusEvents<GameData>.Exit(GameData gameStatusData)
//        {
//            this.Data = gameStatusData;
//            OnExit?.Invoke(gameStatusData);
//        }
//        void IGameStatusEvents<GameData>.Pause(GameData gameStatusData)
//        {
//            this.Data = gameStatusData;
//            OnPause?.Invoke(gameStatusData);
//        }
//        void IGameStatusEvents<GameData>.Continue(GameData gameStatusData)
//        {
//            this.Data = gameStatusData;
//            OnContinue?.Invoke(gameStatusData);
//        }
//    }
//}
