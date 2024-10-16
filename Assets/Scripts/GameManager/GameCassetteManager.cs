using CatFramework.EventsMiao;
using System;
using UnityEngine.Playables;

namespace CatFramework.GameManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">持有者,持有者包含数据</typeparam>
    public class GameCassetteManager<T> : IDisposable
        where T : class
    {
        T owner;
        public T Owner => owner;
        IGameCassette<T> current;
        public bool HasCassette => current != null;
        public bool InGame => current.InGame;
        public bool InPause => current.InPause;// 不判空,如果空直接报错出去
        IGameCassette<T>[] gameCassettes;
        public void Dispose()
        {
            if (gameCassettes == null) return;
            for (int i = 0; i < gameCassettes.Length; i++)
            {
                gameCassettes[i].Dispose();
            }
        }
        public GameCassetteManager(T owner)
        {
            this.owner = owner;
        }
        public GameCassetteManager(T owner, params IGameCassette<T>[] gameCassettes)
        {
            this.owner = owner;
            this.gameCassettes = gameCassettes;
        }
        public void Enter<GameCassette>() where GameCassette : IGameCassette<T>
        {
            if (Assert.IsNull(gameCassettes)) return;
            for (int i = 0; i < gameCassettes.Length; i++)
            {
                if (gameCassettes[i] is GameCassette Tgc)
                {
                    Enter(Tgc);
                    return;
                }
            }
           throw new NullReferenceException("无对应的游戏卡 :" + typeof(GameCassette));
        }
        public void Enter(IGameCassette<T> gameCassette)
        {
            Assert.IsNull(gameCassette, "插入的卡带为空");
            if (gameCassette != current)
            {
                current?.Exit(owner);
                current = gameCassette;
                current?.Enter(owner);
            }
        }
        public void Exit(IGameCassette<T> gameCassette)
        {
            if (gameCassette != current)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("要退出的卡带不一致");
            }
            current?.Exit(owner);
            current = null;
        }
        public void ExitCurrent()
        {
            Exit(current);
        }
        public void Pause()
        {
            if (Assert.IsNull(current)) return;
            current.Pause(owner);
        }
        public void Continue()
        {
            if (Assert.IsNull(current)) return;
            current.Continue(owner);
        }
    }
}
