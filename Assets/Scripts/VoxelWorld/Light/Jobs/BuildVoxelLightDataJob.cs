using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace CatDOTS.VoxelWorld
{
    //[BurstCompile]
    //public struct BuildVoxelLightDataJob : IJob
    //{
    //    [ReadOnly] public NativeSlice<Voxel> SmallChunkSlice;
    //    public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
    //    public NativeArray<byte> VoxelLightMatrix;
    //    public NativeArray<byte> OriginalMatrix;
    //    public NativeArray<byte> Front;
    //    public NativeArray<byte> Back;
    //    public NativeArray<byte> Top;
    //    public NativeArray<byte> Bottom;
    //    public NativeArray<byte> Right;
    //    public NativeArray<byte> Left;
    //    public void Execute()
    //    {
    //        ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;
    //        int x, y, z;
    //        // 朝后,朝右,朝上
    //        // 先从前面的读取边界光
    //        if (Front.IsCreated)
    //        {
    //            x = 0; y = 0; z = 0;
    //            for (; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (; y != Settings.SmallChunkSize; y++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, Settings.SmallChunkSize - 1);
    //                    int frontIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, 0);
    //                    VoxelLightMatrix[selfIndex] = Front[frontIndex];
    //                }
    //            }
    //        }
    //        if (Left.IsCreated)
    //        {
    //            x = 0; y = 0; z = 0;
    //            for (; y != Settings.SmallChunkSize; y++)
    //            {
    //                for (; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(Settings.SmallChunkSize - 1, y, z);
    //                    int frontIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(0, y, z);
    //                    VoxelLightMatrix[selfIndex] = Front[frontIndex];
    //                }
    //            }
    //        }
    //        int length = Settings.VoxelCapacityInSmallChunk;
    //        x = 0; y = 0; z = 0;
    //        for (int index = 0; index != length; index++)
    //        {

    //            z++;
    //            if (z == Settings.SmallChunkSize)
    //            {
    //                z = 0;
    //                x++;
    //                if (x == Settings.SmallChunkSize)
    //                {
    //                    x = 0;
    //                    y++;
    //                }
    //            }
    //        }
    //    }
    //}
    //[BurstCompile]
    //public struct BuildVoxelLightDataJob : IJob
    //{
    //    [ReadOnly] public NativeSlice<Voxel> SmallChunkSlice;
    //    public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
    //    public NativeArray<byte> VoxelLightMatrix;
    //    public NativeArray<byte> OriginalMatrix;
    //    public NativeArray<byte> Front;
    //    public NativeArray<byte> Back;
    //    public NativeArray<byte> Top;
    //    public NativeArray<byte> Bottom;
    //    public NativeArray<byte> Right;
    //    public NativeArray<byte> Left;
    //    public void Execute()
    //    {
    //        ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;
    //        int x, y, z;
    //        // 朝后,朝右,朝上
    //        // 先从前面的读取边界光
    //        if (Front.IsCreated)
    //        {
    //            x = 0; y = 0; z = 0;
    //            for (; x != Settings.SmallChunkSize; x++)
    //            {
    //                for (; y != Settings.SmallChunkSize; y++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, Settings.SmallChunkSize - 1);
    //                    int frontIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, 0);
    //                    VoxelLightMatrix[selfIndex] = Front[frontIndex];
    //                }
    //            }
    //        }
    //        if (Left.IsCreated)
    //        {
    //            x = 0; y = 0; z = 0;
    //            for (; y != Settings.SmallChunkSize; y++)
    //            {
    //                for (; z != Settings.SmallChunkSize; z++)
    //                {
    //                    int selfIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(Settings.SmallChunkSize - 1, y, z);
    //                    int frontIndex = VoxelMath.LocalVoxelArrayIndexInSmallChunk(0, y, z);
    //                    VoxelLightMatrix[selfIndex] = Front[frontIndex];
    //                }
    //            }
    //        }
    //        int length = Settings.VoxelCapacityInSmallChunk;
    //        x = 0; y = 0; z = 0;
    //        for (int index = 0; index != length; index++)
    //        {

    //            z++;
    //            if (z == Settings.SmallChunkSize)
    //            {
    //                z = 0;
    //                x++;
    //                if (x == Settings.SmallChunkSize)
    //                {
    //                    x = 0;
    //                    y++;
    //                }
    //            }
    //        }
    //    }
    //}
}
