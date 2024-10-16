using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    /// <summary>
    /// 指向面数组的开始
    /// </summary>
    public struct VoxelShapeHeader
    {
        public const byte MaxDirection = 7;
        public const byte 正向 = 0;
        public const byte 右向 = 1;
        public const byte 后向 = 2;
        public const byte 左向 = 3;

        public const byte 颠倒正向 = 4;
        public const byte 颠倒右向 = 5;
        public const byte 颠倒后向 = 6;
        public const byte 颠倒左向 = 7;

        /// <summary>
        /// 在资产中，8个形状共8*7个面的起始索引
        /// </summary>
        public int StartIndexInTotalFaceDataArray
        {
            get
            {
                return FirstDirectionShapeFaceDataStartIndex(ShapeIndex);
            }
        }
        public static quaternion GetRotation(int direction)
        {
            return (direction) switch
            {
                0 => quaternion.identity,
                1 => quaternion.RotateY(math.radians(90f)),
                2 => quaternion.RotateY(math.radians(180f)),
                3 => quaternion.RotateY(math.radians(270f)),
                4 => quaternion.EulerZYX(math.radians(new float3(0f, 0f, 180f))),
                5 => quaternion.EulerZYX(math.radians(new float3(0f, 90f, 180f))),
                6 => quaternion.EulerZYX(math.radians(new float3(0f, 180f, 180f))),
                7 => quaternion.EulerZYX(math.radians(new float3(0f, 270f, 180f))),
                _ => quaternion.identity,
            };
        }
        /// <summary>
        /// 不包含方向变体的形状索引
        /// </summary>
        public ushort ShapeIndex;
        /// <summary>
        /// 每个形状有八个方向变体,每个变体七个面,所以形状句柄在面数据数组的每个开始都是当前形状索引*八个形状乘以7个面
        /// </summary>
        /// <param name="shapeIndex">八个形状里的首个形状索引</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FirstDirectionShapeFaceDataStartIndex(ushort shapeIndex)
        {
            return shapeIndex * ShapeDirectionCount * VoxelFaceData.FaceCountInSingleShape;
        }
        // 每个方向变体七个面,而每个方向等于一个方向变体,则要获得在面数据数组里的索引,需要首个方向的再加上当前哪个方向的每个方向7个面
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FaceDataStartIndex(ushort shapeIndex, byte putDirection)
        {
            return (FirstDirectionShapeFaceDataStartIndex(shapeIndex) + putDirection * VoxelFaceData.FaceCountInSingleShape);
        }
        /// <summary>
        /// 每个形状8个方向变体
        /// </summary>
        public const int ShapeDirectionCount = 8;

        public static readonly VoxelShapeHeader Default = new VoxelShapeHeader();
    }
}
