//using CatFramework.EventsMiao;
//using CatFramework.UiMiao;
//using System;
//using System.Collections;
//using UnityEngine;

//namespace VoxelWorld.UGUICTR
//{
//    public class ViewWhenInGame : MonoBehaviour
//    {
//        [SerializeField] ViewUMCtrBase viewUMCtrBase;
//        private void Awake()
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
//            gameEvents.OnEnter += Enter;
//            gameEvents.OnExit += Exit;
//        }
//        private void OnDestroy()
//        {
//            var gameEvents = EventManagerMiao.GetEvents<GameCassette.Events>();
//            gameEvents.OnEnter -= Enter;
//            gameEvents.OnExit -= Exit;
//        }
//        void Enter(GameCassette.IDataCollection data)
//        {
//            viewUMCtrBase.Open();
//        }
//        void Exit(GameCassette.IDataCollection data)
//        {
//            viewUMCtrBase.Close();
//        }
//    }
//}