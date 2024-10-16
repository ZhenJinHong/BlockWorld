using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public readonly struct VoxelType
    {
        public readonly ushort IndexInTypeArray;
        public readonly ushort TextureIndex;// 如果需要压缩,可以预先压缩

        public readonly VoxelMaterial VoxelMaterial;
        public readonly VoxelRenderType VoxelRenderType;

        public readonly byte Energy;
        public readonly byte Resistance;


        //public readonly int IndexOffset;
        public readonly bool IsOpaque => VoxelRenderType == VoxelRenderType.OpaqueBlock;
        public readonly Voxel Voxel => new Voxel(IndexInTypeArray, VoxelMaterial);

        //// 定义的面的顺序是前面，背面，顶面，底面，右面，左面
        //public readonly int GetTextureIndex(int faceIndex)
        //{
        //    int offset = faceIndex * 4;// 4是位移的数
        //    return ((IndexOffset & (0b1111 << offset)) >> offset) + TextureIndex;
        //}
        //public static float EncodeTextureIndex(ushort textureIndex, ushort emissionIndex)
        //{
        //    return textureIndex * 4096 + emissionIndex;
        //}
        public VoxelType(ushort indexInTypeArray, VoxelMaterial voxelMaterial, VoxelRenderType voxelRenderType, ushort textureIndex)
        {
            IndexInTypeArray = indexInTypeArray;
            TextureIndex = textureIndex;
            VoxelMaterial = voxelMaterial;
            VoxelRenderType = voxelRenderType;
            Energy = 0;
            Resistance = byte.MaxValue;
        }
        public VoxelType(ushort indexInTypeArray, ushort textureIndex, IVoxelDefinition voxelDefinition)
        {
            IndexInTypeArray = indexInTypeArray;
            TextureIndex = textureIndex;
            VoxelMaterial = voxelDefinition.VoxelMaterial;
            VoxelRenderType = voxelDefinition.VoxelRenderType;
            Energy = voxelDefinition.Energy;
            Resistance = voxelDefinition.Resistance;
        }
        public override string ToString()
        {
            return $"索引为：{IndexInTypeArray}；是否固体：{VoxelMaterial}渲染类型：{VoxelRenderType}；贴图索引{TextureIndex}；";
        }


        //public static int CompressIndexOffset(List<int> indexOffsets)
        //{
        //    int offset = 0;
        //    for (int i = indexOffsets.Count - 1; i > -1; i--)
        //    {
        //        offset += indexOffsets[i];
        //        if (i == 0)// 最后一个不用偏移
        //        {
        //            break;
        //        }
        //        offset <<= 4;
        //    }
        //    return offset;
        //}
        ///// <summary>
        ///// 传入的是相对于首个贴图索引的偏移
        ///// </summary>
        //public static int GetCompressIndexOffset(int front, int back, int top, int bottom, int left, int right)
        //{
        //    // 排得越后的面塞的越深
        //    int offset = 0;
        //    offset += left;
        //    offset <<= 4;

        //    offset += right;
        //    offset <<= 4;

        //    offset += bottom;
        //    offset <<= 4;

        //    offset += top;
        //    offset <<= 4;

        //    offset += back;
        //    offset <<= 4;

        //    offset += front;// 最后排入的第一个面不用再位移
        //    return offset;
        //}

        public const ushort EmptyVoxelID = 0;
        public const ushort BorderVoxelID = 1;
        public static readonly VoxelType EmptyVoxelType = new VoxelType(EmptyVoxelID, VoxelMaterial.None, VoxelRenderType.Other, 0);
        public static readonly VoxelType BorderVoxelType = new VoxelType(BorderVoxelID, VoxelMaterial.Solid, VoxelRenderType.OpaqueBlock, 0);
    }
}
