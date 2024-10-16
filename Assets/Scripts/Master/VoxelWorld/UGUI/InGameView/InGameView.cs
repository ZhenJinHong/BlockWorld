//using CatFramework;
//using CatFramework.EventsMiao;
//using CatFramework.UiMiao;
//using System;
//using System.Collections;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    public class InGameView : MonoBehaviour
//    {
//        protected virtual void Awake() 
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
//            gameEvents.OnEnter += Enter;
//            gameEvents.OnExit += Exit;
//            gameEvents.OnContinue += Continue;
//            gameEvents.OnPause += Pause;
//        }
//        protected virtual void Start() { }
//        protected virtual void OnDestroy()
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
//            gameEvents.OnEnter -= Enter;
//            gameEvents.OnExit -= Exit;
//            gameEvents.OnContinue -= Continue;
//            gameEvents.OnPause -= Pause;
//        }
//        bool ingame;
//        protected bool Ingame => ingame;
//        bool pause;
//        protected bool IsPause => pause;
//        protected virtual void Enter(GameCassette.IDataCollection data)
//        {
//            if (ingame)
//            {
//                if (ConsoleCat.Enable) ConsoleCat.LogWarning("InGameView重复设定进入游戏");// 检查Awake是否有那个重复订阅的问题
//            }
//            ingame = true;
//            pause = false;
//        }
//        protected virtual void Exit(GameCassette.IDataCollection data)
//        {
//            ingame = false;
//            pause = false;
//        }
//        protected virtual void Continue(GameCassette.IDataCollection collection)
//        {
//            pause = false;
//        }
//        protected virtual void Pause(GameCassette.IDataCollection collection)
//        {
//            pause = true;
//        }
//    }
//}