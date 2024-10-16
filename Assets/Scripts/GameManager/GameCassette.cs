using CatFramework.EventsMiao;
using System;

namespace CatFramework.GameManager
{
    public interface IGameCassette : IDisposable
    {
        bool InGame { get; }
        bool InPause { get; }
    }
    /// <summary>
    /// 如果需要跳转游戏卡,找持有者去处理
    /// </summary>
    /// <typeparam name="T">持有者,持有者包含可能共同使用的数据</typeparam>
    public interface IGameCassette<T> : IGameCassette
         where T : class
    {
        T Owner { get; }

        void Enter(T owner);
        void Exit(T owner);
        void Pause(T owner);
        void Continue(T owner);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">持有者,持有者包含可能共同使用的数据</typeparam>
    public abstract class GameCassette<T, Events, Cassette> : IGameCassette<T>
        where T : class
        where Cassette : class, IGameCassette<T>
        where Events : class, IGameEvents<Cassette>, new()
    {
        bool ingame;
        bool inPause;
        T owner;
        public T Owner => owner;
        protected Events events;
        public bool InGame => ingame;
        public bool InPause => inPause;
        void IGameCassette<T>.Enter(T owner)
        {
            this.owner = owner;
            InternalEnter(owner);
            if (ConsoleCat.Enable && ingame)
                ConsoleCat.LogWarning("重复进入游戏!");
            ingame = true;
            inPause = false;
            if (ConsoleCat.IsDebug)
                ConsoleCat.DebugInfo($"进入游戏:{this}");
            events = EventManagerMiao.GetEvents<Events>();
            events.Enter(this as Cassette);
        }
        void IGameCassette<T>.Exit(T owner)
        {
            this.owner = owner;
            InternalExit(owner);
            if (ConsoleCat.Enable && (!ingame))
                ConsoleCat.LogWarning("重复退出游戏!");
            ingame = false;
            inPause = false;
            if (ConsoleCat.IsDebug)
                ConsoleCat.DebugInfo($"退出游戏:{this}");
            events.Exit(this as Cassette);
            events = null;
        }
        void IGameCassette<T>.Pause(T owner)
        {
            this.owner = owner;
            InternalPause(owner);
            if (inPause && ConsoleCat.Enable)
                ConsoleCat.LogWarning("重复暂停");
            inPause = true;
            if (ConsoleCat.IsDebug)
                ConsoleCat.DebugInfo($"暂停游戏:{this}");
            events.Pause(this as Cassette);
        }
        void IGameCassette<T>.Continue(T owner)
        {
            this.owner = owner;
            InternalContinue(owner);
            if ((!inPause) && ConsoleCat.Enable)
                ConsoleCat.LogWarning("重复继续");
            inPause = false;
            if (ConsoleCat.IsDebug)
                ConsoleCat.DebugInfo($"继续游戏:{this}");
            events.Continue(this as Cassette);
        }
        protected abstract void InternalEnter(T owner);
        protected abstract void InternalExit(T owner);
        protected abstract void InternalPause(T owner);
        protected abstract void InternalContinue(T owner);
        public virtual void Dispose() { }
    }
}
