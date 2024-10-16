

using Unity.Transforms;

namespace CatDOTS
{
    public struct Constant
    {
        public const float ZoneSize = 100f;
        public const float HalfZoneSize = ZoneSize / 2;
        public const int OverWorldSideZoneCount = 3;
        public const float OverWorldHalfSize = (OverWorldSideZoneCount / 2 + 0.5f) * ZoneSize;
        /// <summary>
        /// 开始销毁时的Y坐标大小
        /// </summary>
        public const float PerishCoord = -500f;
    }
    public enum PhysicsLayer : uint
    {
        /// <summary>
        /// 静态环境，无法破坏的
        /// </summary>
        ImmortalStatic = 1 << 6,
        /// <summary>
        /// 基岩，必须保留检测区块时需要用到
        /// </summary>
        Bedrock = 1 << 7,
        /// <summary>
        /// 静态陆军
        /// </summary>
        StaticArmy = 1 << 8,
        /// <summary>
        /// 静态空军
        /// </summary>
        StaticAirForce = 1 << 9,
        /// <summary>
        /// 静态海军
        /// </summary>
        StaticNavy = 1 << 10,
        /// <summary>
        /// 静态物
        /// </summary>
        Static = StaticArmy | StaticAirForce | StaticNavy,
        /// <summary>
        /// 友方陆军
        /// </summary>
        FriendlyArmy = 1 << 11,
        /// <summary>
        /// 友方空军
        /// </summary>
        FriendlyAirForce = 1 << 12,
        /// <summary>
        /// 友方海军
        /// </summary>
        FriendlyNavy = 1 << 13,
        /// <summary>
        /// 友军
        /// </summary>
        Friendly = FriendlyArmy | FriendlyAirForce | FriendlyNavy,
        /// <summary>
        /// 中立陆军
        /// </summary>
        NeutralArmy = 1 << 14,
        /// <summary>
        /// 中立空军
        /// </summary>
        NeutralAirForce = 1 << 15,
        /// <summary>
        /// 中立海军
        /// </summary>
        NeutralNavy = 1 << 16,
        /// <summary>
        /// 中立
        /// </summary>
        Neutral = NeutralArmy | NeutralAirForce | NeutralNavy,
        /// <summary>
        /// 敌方陆军
        /// </summary>
        EnemyArmy = 1 << 17,
        /// <summary>
        /// 敌方空军
        /// </summary>
        EnemyAirForce = 1 << 18,
        /// <summary>
        /// 敌方海军
        /// </summary>
        EnemyNavy = 1 << 19,
        /// <summary>
        /// 敌方
        /// </summary>
        Enemy = EnemyArmy | EnemyAirForce | EnemyNavy,
        /// <summary>
        /// 玩家
        /// </summary>
        Player = 1 << 20,
        SolidVoxel = 1 << 21,
        NonSolidVoxel = 1 << 22,
        AllCreature = Static | Friendly | Neutral | Enemy | Player,
        /// <summary>
        /// 排除玩家自身
        /// </summary>
        ExcludePlayer = ImmortalStatic | Bedrock | Static | Friendly | Neutral | Enemy | SolidVoxel | NonSolidVoxel,
    }
    public struct DOTSLayer
    {
        /// <summary>
        /// 静态环境，无法破坏的
        /// </summary>
        public const uint ImmortalStatic = 1 << 6;
        /// <summary>
        /// 基岩，必须保留检测区块时需要用到
        /// </summary>
        public const uint Bedrock = 1 << 7;
        /// <summary>
        /// 静态陆军
        /// </summary>
        public const uint StaticArmy = 1 << 8;
        /// <summary>
        /// 静态空军
        /// </summary>
        public const uint StaticAirForce = 1 << 9;
        /// <summary>
        /// 静态海军
        /// </summary>
        public const uint StaticNavy = 1 << 10;
        /// <summary>
        /// 静态物
        /// </summary>
        public const uint Static = StaticArmy | StaticAirForce | StaticNavy;
        /// <summary>
        /// 友方陆军
        /// </summary>
        public const uint FriendlyArmy = 1 << 11;
        /// <summary>
        /// 友方空军
        /// </summary>
        public const uint FriendlyAirForce = 1 << 12;
        /// <summary>
        /// 友方海军
        /// </summary>
        public const uint FriendlyNavy = 1 << 13;
        /// <summary>
        /// 友军
        /// </summary>
        public const uint Friendly = FriendlyArmy | FriendlyAirForce | FriendlyNavy;
        /// <summary>
        /// 中立陆军
        /// </summary>
        public const uint NeutralArmy = 1 << 14;
        /// <summary>
        /// 中立空军
        /// </summary>
        public const uint NeutralAirForce = 1 << 15;
        /// <summary>
        /// 中立海军
        /// </summary>
        public const uint NeutralNavy = 1 << 16;
        /// <summary>
        /// 中立
        /// </summary>
        public const uint Neutral = NeutralArmy | NeutralAirForce | NeutralNavy;
        /// <summary>
        /// 敌方陆军
        /// </summary>
        public const uint EnemyArmy = 1 << 17;
        /// <summary>
        /// 敌方空军
        /// </summary>
        public const uint EnemyAirForce = 1 << 18;
        /// <summary>
        /// 敌方海军
        /// </summary>
        public const uint EnemyNavy = 1 << 19;
        /// <summary>
        /// 敌方
        /// </summary>
        public const uint Enemy = EnemyArmy | EnemyAirForce | EnemyNavy;
        /// <summary>
        /// 玩家
        /// </summary>
        public const uint Player = 1 << 20;
        public const uint SolidVoxel = 1 << 21;
        public const uint NonSolidVoxel = 1 << 22;
        public const uint AllCreature = Static | Friendly | Neutral | Enemy | Player;
        public const uint AllDynamic = Friendly | Neutral | Enemy | Player;
        /// <summary>
        /// 排除玩家自身
        /// </summary>
        public const uint ExcludePlayer = ImmortalStatic | Bedrock | Static | Friendly | Neutral | Enemy | SolidVoxel | NonSolidVoxel;
    }
}
