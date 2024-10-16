using CatFramework.EventsMiao;

namespace CatFramework.GameManager
{
    public static class GameCassetteFunction
    {
        //public static Events GetEvents<Events>() where Events : class, IUniqueEvents, new()
        //{
        //    return EventManagerMiao.GetEvents<Events>();
        //}
        //public static void ListenEvent<Events, Data>(this IGameCassetteEventsListen<Events, Data> eventListen)
        //    where Data : class
        //    where Events : class, IGameCassetteEvents<Data>, new()
        //{
        //    var gameEvents = EventManagerMiao.GetEvents<Events>();
        //    gameEvents.OnEnter += eventListen.Enter;
        //    gameEvents.OnExit += eventListen.Exit;
        //    gameEvents.OnContinue += eventListen.Continue;
        //    gameEvents.OnPause += eventListen.Pause;
        //}
        //public static void RemoveEvent<Events, Data>(this IGameCassetteEventsListen<Events, Data> eventListen)
        //    where Data : class
        //    where Events : class, IGameCassetteEvents<Data>, new()
        //{
        //    var gameEvents = EventManagerMiao.GetEvents<Events>();
        //    gameEvents.OnEnter -= eventListen.Enter;
        //    gameEvents.OnExit -= eventListen.Exit;
        //    gameEvents.OnContinue -= eventListen.Continue;
        //    gameEvents.OnPause -= eventListen.Pause;
        //}
    }
}
