using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    [BurstCompile]
    public struct BuildVoxelWaterDataJob : IJob
    {
        [ReadOnly] public VoxelWorldMap.ReadOnly VoxelChunkMap;
        [ReadOnly] public VoxelWorldMap.SmallChunkSliceReadyOnly SmallChunkSlice;
        public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
        public BlobAssetReference<VoxelShapeBlobAsset> VoxelShapeBlobAsset;
        public NativeList<float3> waterVerts;
        public NativeList<ushort> waterIndexs;
        //public NativeList<float3> fireVerts;
        //public NativeList<ushort> fireIndexs;
        public void Execute()
        {
            ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;

            ref VoxelShapeBlobAsset voxelShapeBlobAsset = ref VoxelShapeBlobAsset.Value;
            ref BlobArray<VoxelFaceData> voxelFaceDatas = ref voxelShapeBlobAsset.FaceDatas;
            ref BlobArray<int3> FaceForwardFindMap = ref voxelShapeBlobAsset.FaceForwardFindMap;
            ref BlobArray<float> waterFaceCubeAngle = ref voxelShapeBlobAsset.CompressWateFaceCubeAngle;

            int3 smallChunkPos = SmallChunkSlice.ChunkPosInt;
            // 首先这个是索引
            int x = 0, y = 0, z = 0;// 需要从指定的小区块位置开始

            for (int voxelArrayIndex = 0; voxelArrayIndex < Settings.VoxelCapacityInSmallChunk; voxelArrayIndex++)
            {
                Voxel voxel = SmallChunkSlice[voxelArrayIndex];
                if (Voxel.Water(voxel.VoxelMaterial)) // 首先判断需不需要画这个方块
                {
                    int3 voxelPosInSmallChunk = new int3(x, y, z);
                    int faceDataStartIndex = VoxelShapeHeader.FaceDataStartIndex(voxel.ShapeIndex, voxel.ShapeDirection);
                    // 自身是否是空或透明
                    bool transparentSelf = VoxelIsTransparent(in voxel, ref voxelTypes);
                    for (int f = 0; f < 6; f++)
                    {
                        FaceRect faceRect = voxelFaceDatas[f + faceDataStartIndex].FaceRect;
                        if (transparentSelf || faceRect != FaceRect.Full)// 该面透明,则检查四周是有水或者是密闭的矩形
                        {
                            int3 faceForwardVoxelPosInBigChunk = voxelPosInSmallChunk + FaceForwardFindMap[f];
                            Voxel faceForwardVoxel = GetVoxel(in faceForwardVoxelPosInBigChunk, in smallChunkPos);
                            // 面向水?
                            bool renderFace = (!Voxel.Water(faceForwardVoxel.VoxelMaterial))
                                     &&
                                     (VoxelIsTransparent(in faceForwardVoxel, ref voxelTypes)
                                     || voxelFaceDatas[VoxelShapeHeader.FaceDataStartIndex(faceForwardVoxel.ShapeIndex, faceForwardVoxel.ShapeDirection) + (f ^ 1)].FaceRect != FaceRect.Full);
                            if (renderFace)
                            {
                                waterIndexs.Add((ushort)(waterVerts.Length & ushort.MaxValue));
                                waterVerts.Add(new float3(x + (y << 8) + (z << 16), waterFaceCubeAngle[f], 0));
                            }
                        }
                    }

                }
                //else if (Voxel.Fire(voxel.VoxelMaterial))
                //{
                //    int faceDataStartIndex = VoxelShapeHeader.FaceDataStartIndex(voxel.ShapeIndex, voxel.ShapeDirection);
                //    bool transparentSelf = VoxelIsTransparent(in voxel, ref voxelTypes);
                //    // 若四周都是火,只考虑左面和背面
                //    float pointPos = x + (y << 8) + (z << 16);
                //    if (transparentSelf) // 自身透明,则左面和背面必生成
                //    {
                //        if (!Voxel.Fire(GetVoxel(new int3(x, y, z + 1), in smallChunkPos).VoxelMaterial))// 前面
                //        {
                //            fireIndexs.Add((ushort)fireVerts.Length);
                //            fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[0], 0.0f));
                //        }

                //        if (!Voxel.Fire(GetVoxel(new int3(x + 1, y, z), in smallChunkPos).VoxelMaterial))// 右面
                //        {
                //            fireIndexs.Add((ushort)fireVerts.Length);
                //            fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[4], 0.0f));
                //        }

                //        fireIndexs.Add((ushort)fireVerts.Length);
                //        fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[1], 0.0f));// 背面

                //        fireIndexs.Add((ushort)fireVerts.Length);
                //        fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[5], 0.0f));// 左面
                //    }
                //    else // 前面0, 右面4, 后面1 左面5
                //    {
                //        Voxel faceVoxel = GetVoxel(new int3(x, y, z + 1), in smallChunkPos);
                //        int faceIndex = 0;
                //        if (!Voxel.Fire(faceVoxel.VoxelMaterial)) // 前面
                //        {
                //            FaceRect selfRect = voxelFaceDatas[faceIndex + faceDataStartIndex].FaceRect;
                //            if (Check(in selfRect, in faceVoxel, ref voxelTypes, ref voxelFaceDatas))
                //            {
                //                fireIndexs.Add((ushort)fireVerts.Length);
                //                fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[faceIndex], 0.0f));
                //            }
                //        }
                //        faceVoxel = GetVoxel(new int3(x + 1, y, z), in smallChunkPos);
                //        faceIndex = 4;
                //        if (!Voxel.Fire(faceVoxel.VoxelMaterial)) // 右面
                //        {
                //            FaceRect selfRect = voxelFaceDatas[faceIndex + faceDataStartIndex].FaceRect;
                //            if (Check(in selfRect, in faceVoxel, ref voxelTypes, ref voxelFaceDatas))
                //            {
                //                fireIndexs.Add((ushort)fireVerts.Length);
                //                fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[faceIndex], 0.0f));
                //            }
                //        }

                //        faceVoxel = GetVoxel(new int3(x, y, z - 1), in smallChunkPos);
                //        faceIndex = 1;
                //        FaceRect rect = voxelFaceDatas[faceIndex + faceDataStartIndex].FaceRect;
                //        if (Check(in rect, in faceVoxel, ref voxelTypes, ref voxelFaceDatas))// 背面
                //        {
                //            fireIndexs.Add((ushort)fireVerts.Length);
                //            fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[faceIndex], 0.0f));
                //        }

                //        faceVoxel = GetVoxel(new int3(x - 1, y, z), in smallChunkPos);
                //        faceIndex = 5;
                //        rect = voxelFaceDatas[faceIndex + faceDataStartIndex].FaceRect;
                //        if (Check(in rect, in faceVoxel, ref voxelTypes, ref voxelFaceDatas))// 左面
                //        {
                //            fireIndexs.Add((ushort)fireVerts.Length);
                //            fireVerts.Add(new float3(pointPos, waterFaceCubeAngle[faceIndex], 0.0f));
                //        }
                //    }
                //}
                // 若在 continue 之前执行++，这也意味着xyz在接下来不可以再继续使用！！！！！！
                z++;
                if (z == Settings.SmallChunkSize)
                {
                    z = 0;
                    x++;
                    if (x == Settings.SmallChunkSize)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }
        static bool VoxelIsTransparent(in Voxel voxel, ref BlobArray<VoxelType> voxelTypes)
        {
            return Voxel.IsAir(voxel.VoxelTypeIndex) || Voxel.Transparent(voxelTypes[voxel.VoxelTypeIndex].VoxelRenderType);
        }
        //static bool Check(in FaceRect selfRect, in Voxel faceVoxel, ref BlobArray<VoxelType> voxelTypes, ref BlobArray<VoxelFaceData> voxelFaceDatas)
        //{
        //    return selfRect != FaceRect.Full
        //        || VoxelIsTransparent(in faceVoxel, ref voxelTypes)
        //        || voxelFaceDatas[1 + VoxelShapeHeader.FaceDataStartIndex(faceVoxel.ShapeIndex, faceVoxel.ShapeDirection)].FaceRect != FaceRect.Full;
        //}
        //void ForFachFront()// 从计算包围盒看的遍历的消耗每次0.15ms 6次的消耗不低
        //{
        //    ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;

        //    ref VoxelShapeBlobAsset voxelShapeBlobAsset = ref VoxelShapeBlobAsset.Value;
        //    ref BlobArray<VoxelFaceData> voxelFaceDatas = ref voxelShapeBlobAsset.FaceDatas;
        //    ref BlobArray<int3> FaceForwardFindMap = ref voxelShapeBlobAsset.FaceForwardFindMap;
        //    ref BlobArray<float> waterFaceCubeAngle = ref voxelShapeBlobAsset.CompressWateFaceCubeAngle;

        //    int3 smallChunkPos = SmallChunkSlice.ChunkPosInt;
        //    for (int y = 0; y < Settings.SmallChunkSize; y++)
        //    {
        //        for (int x = 0; x < Settings.SmallChunkSize; x++)
        //        {
        //            Voxel self = SmallChunkSlice[VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, 0)];
        //            for (int z = 0; z < Settings.SmallChunkSize - 1; z++)
        //            {

        //            }
        //        }
        //    }
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly Voxel GetVoxel(in int3 voxelIndexInChunk, in int3 smallChunkPos)
        {
            return VoxelMath.VoxelInSamllChunkRange(voxelIndexInChunk)
                    ? SmallChunkSlice[VoxelMath.LocalVoxelArrayIndexInSmallChunk(voxelIndexInChunk)]
                    : VoxelChunkMap.GetVoxelOrBorder(voxelIndexInChunk + smallChunkPos);
        }
    }
}
