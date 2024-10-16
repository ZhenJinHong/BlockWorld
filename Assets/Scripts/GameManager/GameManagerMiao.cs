using CatFramework.EventsMiao;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.GameManager
{
    //public class GameStatusManager
    //{
    //    GameStatus status;
    //    protected GameStatus GameStatus => status;
    //    public virtual bool TargetIsValid => status != null;
    //    public void SetTarget(GameStatus status)
    //    {
    //        this.status = status;
    //    }
    //    public void RemoveTarget(GameStatus status)
    //    {
    //        if (this.status == status)
    //        {
    //            GameManagerMiao.ExitStatus(status);
    //            this.status = null;
    //        }
    //    }
    //    public void EnterGame()
    //    {
    //        if (TargetIsValid)
    //        {
    //            if (status.InGame)
    //            {
    //                ConsoleCat.LogError("重复调度进入游戏");
    //                return;
    //            }
    //            GameManagerMiao.CurrentStatus = status;
    //        }
    //    }
    //    public void ReturnHome()
    //    {
    //        if (TargetIsValid)
    //        {
    //            GameManagerMiao.ExitStatus(status);
    //        }
    //    }
    //    public void Pause()
    //    {
    //        if (TargetIsValid)
    //            status?.Pause();
    //    }
    //    public void Continue()
    //    {
    //        if (TargetIsValid)
    //            status?.Continue();
    //    }
    //}
    public static class GameManagerMiao
    {

        //static GameStatus currentStatus;
        //public static GameStatus CurrentStatus
        //{
        //    set
        //    {
        //        if (currentStatus != value)
        //        {
        //            if (value == null)
        //            {
        //                if (ConsoleCat.Enable)
        //                    ConsoleCat.LogWarning("要执行的状态为空,如果需要退出状态,请使用状态退出函数");
        //            }
        //            else
        //            {
        //                currentStatus?.Exit();
        //                currentStatus = value;
        //                currentStatus.Enter();
        //            }
        //        }
        //        else
        //        {
        //            if (ConsoleCat.Enable)
        //                ConsoleCat.LogWarning("设置了相同的状态!");
        //        }
        //    }
        //}
        //public static void ExitStatus(GameStatus gameStatus)
        //{
        //    if (gameStatus == null) return;
        //    if (gameStatus == currentStatus)
        //    {
        //        currentStatus.Exit();
        //        currentStatus = null;
        //    }
        //}
        //public static bool StatusEquals(GameStatus gameStatus) => currentStatus == gameStatus;
        static GameManagerMiao()
        {
        }
        //public static T GetGameStatuEvents<T>() where T : class, IUniqueEvents, new()
        //{
        //    return EventManagerMiao.GetEvents<T>();
        //}
        //public static T GetGameStatuController<T>() where T : class, IUniquePublisher, new()
        //{
        //    return EventManagerMiao.GetPulisher<T>();
        //}
        public static void Start()
        {

        }
        public static void ShutDown()
        {
            //ExitStatus(currentStatus);
        }
        //#region 扩展
        //public static void ListenEvent<GameStatusEvents, GameData>(this IGameStatusEventListen<GameStatusEvents, GameData> eventListen)
        //    where GameData : class
        //    where GameStatusEvents : class, IGameStatusEvents<GameData>, new()
        //{
        //    var gameEvents = EventManagerMiao.GetEvents<GameStatusEvents>();
        //    gameEvents.OnEnter += eventListen.Enter;
        //    gameEvents.OnExit += eventListen.Exit;
        //    gameEvents.OnContinue += eventListen.Continue;
        //    gameEvents.OnPause += eventListen.Pause;
        //}
        //public static void RemoveEvent<GameStatusEvents, GameData>(this IGameStatusEventListen<GameStatusEvents, GameData> eventListen)
        //    where GameData : class
        //    where GameStatusEvents : class, IGameStatusEvents<GameData>, new()
        //{
        //    var gameEvents = EventManagerMiao.GetEvents<GameStatusEvents>();
        //    gameEvents.OnEnter -= eventListen.Enter;
        //    gameEvents.OnExit -= eventListen.Exit;
        //    gameEvents.OnContinue -= eventListen.Continue;
        //    gameEvents.OnPause -= eventListen.Pause;
        //}
        //#endregion
    }
}
