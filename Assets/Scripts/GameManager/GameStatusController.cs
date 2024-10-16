//using CatFramework.EventsMiao;
//using System;

//namespace CatFramework.GameManager
//{
//    [Obsolete]
//    public abstract class GameStatusController<Status> : IUniquePublisher
//        where Status : GameStatus
//    {
//        Status status;
//        protected Status GameStatus => status;
//        public virtual bool TargetIsValid => status != null;
//        public void SetTarget(Status status)
//        {
//            this.status = status;
//        }
//        public void RemoveTarget(Status status)
//        {
//            if (this.status == status)
//            {
//                GameManagerMiao.ExitStatus(status);
//                this.status = null;
//            }
//        }
//        public void EnterGame()
//        {
//            if (TargetIsValid)
//            {
//                if (status.InGame)
//                {
//                    ConsoleCat.LogError("重复调度进入游戏");
//                    return;
//                }
//                GameManagerMiao.CurrentStatus = status;
//            }
//        }
//        public void ReturnHome()
//        {
//            if (TargetIsValid)
//            {
//                GameManagerMiao.ExitStatus(status);
//            }
//        }
//        public void Pause()
//        {
//            if (TargetIsValid)
//                status?.Pause();
//        }
//        public void Continue()
//        {
//            if (TargetIsValid)
//                status?.Continue();
//        }
//    }
//}
