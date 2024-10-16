using System;

namespace CatDOTS
{
    [Flags]
    public enum PlayerState
    {
        Init = 1 << 0,
        /// <summary>
        /// 未有玩家角色时
        /// </summary>
        Idle = 1 << 1,
        Sprint = 1 << 2,
        Jump = 1 << 3,
        Crouch = 1 << 4,
        Flying = 1 << 5,
    }
}
