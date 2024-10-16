using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace CatDOTS.VoxelWorld
{
    // 对于体素行为,如果不好认方向,可以通过形状去判断
    // 使用偏移对齐的最小单位是byte 不是bit,所以还是得手动写属性
    [StructLayout(LayoutKind.Explicit)]
    public struct Voxel
    {
        public const ulong Null = 0ul;
        // 偏移是从右边开始的
        [FieldOffset(0)] public ulong Value;
        [FieldOffset(0)] public ushort VoxelTypeIndex;      //                    0b 1111 1111
        [FieldOffset(2)] public ushort ShapeIndex;          //          0b 1111 1111 0000 0000
        // 材质必须有的,不是同一种体素都同样有水或无水
        [FieldOffset(4)] public VoxelMaterial VoxelMaterial;// 0b1111 1111 0000 0000 0000 0000
        [FieldOffset(5)] public byte Data1;
        [FieldOffset(6)] public byte Data2;
        [FieldOffset(7)] public byte Data3;
        //public ushort VoxelTypeIndex;
        //public ushort ShapeIndex;
        //// 材质必须有的,不是同一种体素都同样有水或无水
        //public VoxelMaterial VoxelMaterial;
        //public byte Data1;
        //public byte Data2;
        //public byte Data3;
        public byte ShapeDirection
        {
            readonly get => (byte)(Data1 & 0b0111);
            set => Data1 |= (byte)(value & 0b0111);// 0b111 为7
        }
        public readonly bool IsDirty => (Data1 & 0b1000) == 0b1000;
        public void CleanDirty() => Data1 &= 0b11110111;
        public void SetDirty() => Data1 |= 0b00001000;
        public byte Energy
        {
            readonly get => (byte)((Data1 & 0b11110000) >> 4);
            set => Data1 |= (byte)((value << 4) & 0b11110000);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateWater() { VoxelMaterial ^= VoxelMaterial.Water; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetWater() { VoxelMaterial |= VoxelMaterial.Water; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveWater() { VoxelMaterial &= ~VoxelMaterial.Water; }
        public unsafe static readonly uint TypeSize = (uint)sizeof(Voxel);

        public static readonly Voxel Empty = VoxelType.EmptyVoxelType.Voxel;
        /// <summary>
        /// 障碍为构建网格使用，为不透明
        /// </summary>
        public static readonly Voxel Border = VoxelType.BorderVoxelType.Voxel;

        public Voxel(ushort voxelTypeIndex, VoxelMaterial voxelMaterial, ushort shapeIndex = 0, byte shapeDirection = 0)
        {
            Value = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            VoxelTypeIndex = voxelTypeIndex;
            ShapeIndex = shapeIndex;
            VoxelMaterial = voxelMaterial;

            ShapeDirection = shapeDirection;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool FieldEquals()
        //{

        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapData(ref Voxel x, ref Voxel y, Voxel mask)
        {
            x ^= y & mask;
            y ^= x & mask;
            x ^= y & mask;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Block(VoxelRenderType renderType)
        {
            return renderType == VoxelRenderType.OpaqueBlock || renderType == VoxelRenderType.TransparentBlock;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Solid(VoxelMaterial material)
        {
            return (material & VoxelMaterial.Solid) == VoxelMaterial.Solid;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Water(VoxelMaterial material)
        {
            return (material & VoxelMaterial.Water) == VoxelMaterial.Water;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Fire(VoxelMaterial material)
        {
            return (material & VoxelMaterial.Water) != VoxelMaterial.Water && (material & VoxelMaterial.Blaze) == VoxelMaterial.Blaze;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NonAir(ushort voxelTypeIndex)
        {
            return voxelTypeIndex != VoxelType.EmptyVoxelID;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAir(ushort voxelTypeIndex)
        {
            return voxelTypeIndex == VoxelType.EmptyVoxelID;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transparent(VoxelRenderType voxelRenderType)
        {
            //return voxelRenderType == VoxelRenderType.TransparentBlock || voxelRenderType == VoxelRenderType.Grass;
            return voxelRenderType != VoxelRenderType.OpaqueBlock;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Grass(VoxelRenderType voxelRenderType)
        {
            return voxelRenderType == VoxelRenderType.Grass;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(Voxel voxel)
        {
            return voxel.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Voxel(ulong v)
        {
            return new Voxel() { Value = v };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Voxel x, Voxel y)
        {
            return x.Value == y.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Voxel x, Voxel y)
        {
            return x.Value != y.Value;
        }
        #region 与ulong
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong x, Voxel y)
        {
            return x == y.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong x, Voxel y)
        {
            return x != y.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Voxel x, ulong y)
        {
            return x.Value == y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Voxel x, ulong y)
        {
            return x.Value != y;
        }
        #endregion
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel operator &(Voxel x, Voxel y)
        {
            return new Voxel() { Value = x.Value & y.Value };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Voxel operator |(Voxel x, Voxel y)
        {
            return new Voxel() { Value = x.Value | y.Value };
        }
        public override bool Equals(object obj)
        {
            return ((Voxel)obj).Value.Equals(this.Value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
    [StructLayout(LayoutKind.Explicit)]
    struct VoxelUlongUnion
    {
        [FieldOffset(0)] public ulong Value;
        [FieldOffset(0)] public Voxel Voxel;
    }
}
