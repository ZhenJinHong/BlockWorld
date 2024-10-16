

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
        /// ��ʼ����ʱ��Y�����С
        /// </summary>
        public const float PerishCoord = -500f;
    }
    public enum PhysicsLayer : uint
    {
        /// <summary>
        /// ��̬�������޷��ƻ���
        /// </summary>
        ImmortalStatic = 1 << 6,
        /// <summary>
        /// ���ң����뱣���������ʱ��Ҫ�õ�
        /// </summary>
        Bedrock = 1 << 7,
        /// <summary>
        /// ��̬½��
        /// </summary>
        StaticArmy = 1 << 8,
        /// <summary>
        /// ��̬�վ�
        /// </summary>
        StaticAirForce = 1 << 9,
        /// <summary>
        /// ��̬����
        /// </summary>
        StaticNavy = 1 << 10,
        /// <summary>
        /// ��̬��
        /// </summary>
        Static = StaticArmy | StaticAirForce | StaticNavy,
        /// <summary>
        /// �ѷ�½��
        /// </summary>
        FriendlyArmy = 1 << 11,
        /// <summary>
        /// �ѷ��վ�
        /// </summary>
        FriendlyAirForce = 1 << 12,
        /// <summary>
        /// �ѷ�����
        /// </summary>
        FriendlyNavy = 1 << 13,
        /// <summary>
        /// �Ѿ�
        /// </summary>
        Friendly = FriendlyArmy | FriendlyAirForce | FriendlyNavy,
        /// <summary>
        /// ����½��
        /// </summary>
        NeutralArmy = 1 << 14,
        /// <summary>
        /// �����վ�
        /// </summary>
        NeutralAirForce = 1 << 15,
        /// <summary>
        /// ��������
        /// </summary>
        NeutralNavy = 1 << 16,
        /// <summary>
        /// ����
        /// </summary>
        Neutral = NeutralArmy | NeutralAirForce | NeutralNavy,
        /// <summary>
        /// �з�½��
        /// </summary>
        EnemyArmy = 1 << 17,
        /// <summary>
        /// �з��վ�
        /// </summary>
        EnemyAirForce = 1 << 18,
        /// <summary>
        /// �з�����
        /// </summary>
        EnemyNavy = 1 << 19,
        /// <summary>
        /// �з�
        /// </summary>
        Enemy = EnemyArmy | EnemyAirForce | EnemyNavy,
        /// <summary>
        /// ���
        /// </summary>
        Player = 1 << 20,
        SolidVoxel = 1 << 21,
        NonSolidVoxel = 1 << 22,
        AllCreature = Static | Friendly | Neutral | Enemy | Player,
        /// <summary>
        /// �ų��������
        /// </summary>
        ExcludePlayer = ImmortalStatic | Bedrock | Static | Friendly | Neutral | Enemy | SolidVoxel | NonSolidVoxel,
    }
    public struct DOTSLayer
    {
        /// <summary>
        /// ��̬�������޷��ƻ���
        /// </summary>
        public const uint ImmortalStatic = 1 << 6;
        /// <summary>
        /// ���ң����뱣���������ʱ��Ҫ�õ�
        /// </summary>
        public const uint Bedrock = 1 << 7;
        /// <summary>
        /// ��̬½��
        /// </summary>
        public const uint StaticArmy = 1 << 8;
        /// <summary>
        /// ��̬�վ�
        /// </summary>
        public const uint StaticAirForce = 1 << 9;
        /// <summary>
        /// ��̬����
        /// </summary>
        public const uint StaticNavy = 1 << 10;
        /// <summary>
        /// ��̬��
        /// </summary>
        public const uint Static = StaticArmy | StaticAirForce | StaticNavy;
        /// <summary>
        /// �ѷ�½��
        /// </summary>
        public const uint FriendlyArmy = 1 << 11;
        /// <summary>
        /// �ѷ��վ�
        /// </summary>
        public const uint FriendlyAirForce = 1 << 12;
        /// <summary>
        /// �ѷ�����
        /// </summary>
        public const uint FriendlyNavy = 1 << 13;
        /// <summary>
        /// �Ѿ�
        /// </summary>
        public const uint Friendly = FriendlyArmy | FriendlyAirForce | FriendlyNavy;
        /// <summary>
        /// ����½��
        /// </summary>
        public const uint NeutralArmy = 1 << 14;
        /// <summary>
        /// �����վ�
        /// </summary>
        public const uint NeutralAirForce = 1 << 15;
        /// <summary>
        /// ��������
        /// </summary>
        public const uint NeutralNavy = 1 << 16;
        /// <summary>
        /// ����
        /// </summary>
        public const uint Neutral = NeutralArmy | NeutralAirForce | NeutralNavy;
        /// <summary>
        /// �з�½��
        /// </summary>
        public const uint EnemyArmy = 1 << 17;
        /// <summary>
        /// �з��վ�
        /// </summary>
        public const uint EnemyAirForce = 1 << 18;
        /// <summary>
        /// �з�����
        /// </summary>
        public const uint EnemyNavy = 1 << 19;
        /// <summary>
        /// �з�
        /// </summary>
        public const uint Enemy = EnemyArmy | EnemyAirForce | EnemyNavy;
        /// <summary>
        /// ���
        /// </summary>
        public const uint Player = 1 << 20;
        public const uint SolidVoxel = 1 << 21;
        public const uint NonSolidVoxel = 1 << 22;
        public const uint AllCreature = Static | Friendly | Neutral | Enemy | Player;
        public const uint AllDynamic = Friendly | Neutral | Enemy | Player;
        /// <summary>
        /// �ų��������
        /// </summary>
        public const uint ExcludePlayer = ImmortalStatic | Bedrock | Static | Friendly | Neutral | Enemy | SolidVoxel | NonSolidVoxel;
    }
}
