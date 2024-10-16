using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct FaceDataCommand
    {
        public ushort VoxelIndexInSmallChunk;
        public ushort VoxelTypeIndex;
        public int FaceDataIndex;

    }
    public struct SubMeshRangeHelper
    {
        NativeList<float3> vertices;
        NativeList<ushort> triangles;
        // 第一个网格也有，即0
        NativeList<SubMeshRange> subMeshRanges;
        readonly VoxelRenderType renderType;
        int joined, lastTriangleLength, baseVertexIndex;
        public ushort FirstVertexIndex => (ushort)(vertices.Length - baseVertexIndex);
        public SubMeshRangeHelper(NativeList<float3> vertices, NativeList<ushort> triangles, NativeList<SubMeshRange> subMeshRanges, VoxelRenderType renderType)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.subMeshRanges = subMeshRanges;
            this.renderType = renderType;
            joined = 0;
            lastTriangleLength = triangles.Length;
            baseVertexIndex = vertices.Length;

            AddNewSubMeshRange();
        }
        public void Update(int vertexCount)
        {
            joined += vertexCount;
            if (joined > ushort.MaxValue)
            {
                joined = 0;
                EndSubMeshRange();
                lastTriangleLength = triangles.Length;
                baseVertexIndex = vertices.Length;

                AddNewSubMeshRange();
            }
        }
        public void Finish()
        {
            EndSubMeshRange();
        }
        void AddNewSubMeshRange()
        {
            subMeshRanges.Add(new SubMeshRange()
            {
                RenderType = renderType,
                TriangleStartIndex = lastTriangleLength,
                TriangleRangeLength = 0,// 长度不可知，等待下批对其修改
                BaseVertexIndex = baseVertexIndex,
            });
        }

        void EndSubMeshRange()
        {
            // 结束上次范围
            // 范围长度为当前三角形数组长度减去上次结束是的三角形数组长度
            SubMeshRange lastRange = subMeshRanges[^1];
            lastRange.TriangleRangeLength = triangles.Length - lastTriangleLength;
            subMeshRanges[^1] = lastRange;
        }
    }
    [BurstCompile]
    public struct BuildVoxelMeshDataJob : IJob// 取消追加命令的方式,直接加入顶点数据,并未有速度提升
    {
        [ReadOnly] public VoxelWorldMap.ReadOnly VoxelWorldMap;

        //[ReadOnly] public VoxelWorldMap.BigChunkSliceReadOnly VoxelChunkMapSlice;
        [ReadOnly] public VoxelWorldMap.SmallChunkSliceReadyOnly SmallChunkSlice;
        public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase;
        public BlobAssetReference<VoxelShapeBlobAsset> VoxelShapeBlobAsset;
        //public int SmallChunkIndex;

        public NativeList<float3> vertices;
        public NativeList<float4> uvs;
        public NativeList<ushort> indexs;
        public NativeList<float3> normals;
        // 第一个网格也有，即0
        public NativeList<SubMeshRange> subMeshRanges;
        public NativeList<FaceDataCommand> opaqueFaces;
        public NativeList<FaceDataCommand> transparentFaces;
        public void Execute()
        {
            BuildMeshData();
            CompleteCommand();
            // 不要删除，用以提示，在for循环里执行多个相同类型的工作会报错上次写入ECB（并行写入器也一样），需要先complete才能写入这次的ECB
            // 这个DOTS的BUG
            //LocalTransform localTransform = new LocalTransform()
            //{
            //    Position = VoxelMath.GetPosition3D(in VoxelChunk),
            //    Rotation = quaternion.identity,
            //    Scale = 1f,
            //};
            //RenderBounds renderBounds = new RenderBounds()
            //{
            //    Value = aabb,
            //};
            //ECB.SetComponent<LocalTransform>(ChunkEntity, localTransform);
            //ECB.SetComponent<RenderBounds>(ChunkEntity, renderBounds);
        }
        void BuildMeshData()
        {
            ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;

            ref VoxelShapeBlobAsset voxelShapeBlobAsset = ref VoxelShapeBlobAsset.Value;
            ref BlobArray<VoxelFaceData> voxelFaceDatas = ref voxelShapeBlobAsset.FaceDatas;
            ref BlobArray<int3> FaceForwardFindMap = ref voxelShapeBlobAsset.FaceForwardFindMap;

            int3 smallChunkPos = SmallChunkSlice.ChunkPosInt;
            //int3 smallChunkIndex = SmallChunkSlice.ChunkIndex;
            //NearBigChunkHelper nearBigChunkHelper = new NearBigChunkHelper(new int3(smallChunkIndex.x, 0, smallChunkIndex.z), VoxelWorldMap);
            int x = 0, y = 0, z = 0;

            for (int voxelArrayIndex = 0; voxelArrayIndex < Settings.VoxelCapacityInSmallChunk; voxelArrayIndex++)
            {
                int3 voxelPosInSmallChunk = new int3(x, y, z);// 在当前大区块内的相对位置

                Voxel voxel = SmallChunkSlice[voxelArrayIndex];
                VoxelRenderType renderType = voxelTypes[voxel.VoxelTypeIndex].VoxelRenderType;

                if (Voxel.NonAir(voxel.VoxelTypeIndex) && Voxel.Block(renderType)) // 首先判断需不需要画这个方块
                {
                    int faceDataStartIndex = VoxelShapeHeader.FaceDataStartIndex(voxel.ShapeIndex, voxel.ShapeDirection);
                    for (int f = 0; f < 6; f++)
                    {
                        VoxelFaceData voxelFaceData = voxelFaceDatas[f + faceDataStartIndex];// 在这里先判断这个面有没有数据
                        if (voxelFaceData.IndexStartIndex != voxelFaceData.IndexEnd)
                        {
                            #region 获取朝向的体素的数据
                            int3 faceForwardVoxelPosInSmallChunk = voxelPosInSmallChunk + FaceForwardFindMap[f];

                            Voxel faceForwardVoxel = GetVoxel(faceForwardVoxelPosInSmallChunk, smallChunkPos);
                            // 不用判断是否空体素,如果空会获取默认(正方体)的矩形
                            FaceRect faceForwardRect = voxelFaceDatas[VoxelShapeHeader.FaceDataStartIndex(faceForwardVoxel.ShapeIndex, faceForwardVoxel.ShapeDirection) + (f ^ 1)].FaceRect;

                            #endregion
                            bool notContain = (voxelFaceData.FaceRect & faceForwardRect) != voxelFaceData.FaceRect || (voxelFaceData.FaceRect == FaceRect.None && faceForwardRect != FaceRect.Full);

                            bool renderedFace = (renderType == VoxelRenderType.OpaqueBlock)
                                 ? (notContain || (voxelTypes[faceForwardVoxel.VoxelTypeIndex].VoxelRenderType != VoxelRenderType.OpaqueBlock))
                                 : (renderType == VoxelRenderType.TransparentBlock && (notContain || Voxel.IsAir(faceForwardVoxel.VoxelTypeIndex)));
                            if (renderedFace)
                            {
                                AddFaceCommand(renderType, new FaceDataCommand()
                                {
                                    VoxelIndexInSmallChunk = (ushort)voxelArrayIndex,
                                    FaceDataIndex = faceDataStartIndex + f,
                                    VoxelTypeIndex = voxel.VoxelTypeIndex,
                                });
                            }
                        }
                    }

                    int notFitFaceIndex = 6;
                    VoxelFaceData notfitFacedata = voxelFaceDatas[notFitFaceIndex + faceDataStartIndex];
                    if (notfitFacedata.VertexStartIndex != notfitFacedata.VertexEnd)
                    {
                        AddFaceCommand(renderType, new FaceDataCommand()
                        {
                            VoxelIndexInSmallChunk = (ushort)voxelArrayIndex,
                            FaceDataIndex = notFitFaceIndex + faceDataStartIndex,
                            VoxelTypeIndex = voxel.VoxelTypeIndex,
                        });
                    }
                }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AddFaceCommand(VoxelRenderType voxelRenderType, FaceDataCommand faceDataCommand)
        {
            if (voxelRenderType == VoxelRenderType.TransparentBlock)
            {
                transparentFaces.Add(faceDataCommand);
            }
            else
            {
                opaqueFaces.Add(faceDataCommand);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly Voxel GetVoxel(int3 voxelIndexInChunk, int3 smallChunkPos)
        {
            if (VoxelMath.VoxelInSamllChunkRange(voxelIndexInChunk))
            {
                return SmallChunkSlice[VoxelMath.LocalVoxelArrayIndexInSmallChunk(voxelIndexInChunk)];
            }
            else
            {
                return VoxelWorldMap.GetVoxelOrBorder(voxelIndexInChunk + smallChunkPos);
            }
            //return VoxelWorldMap.GetVoxelOrBorder(voxelIndexInChunk + smallChunkPos);
        }
        private void CompleteCommand()
        {
            if (opaqueFaces.Length > 0)
                CompleteCommand(VoxelRenderType.OpaqueBlock, opaqueFaces);
            if (transparentFaces.Length > 0)
                CompleteCommand(VoxelRenderType.TransparentBlock, transparentFaces);
        }
        private void CompleteCommand(VoxelRenderType renderType, NativeList<FaceDataCommand> commands)
        {
            ref BlobArray<VoxelType> voxelTypes = ref VoxelTypeDataBase.Value.VoxelTypes;
            ref VoxelShapeBlobAsset voxelShapeBlobAsset = ref VoxelShapeBlobAsset.Value;

            ref BlobArray<VoxelFaceData> voxelFaceDatas = ref voxelShapeBlobAsset.FaceDatas;
            ref BlobArray<float3> Verts = ref voxelShapeBlobAsset.Verts;
            ref BlobArray<ushort> Indexs = ref voxelShapeBlobAsset.Indexs;
            ref BlobArray<float2> UVs = ref voxelShapeBlobAsset.UVs;
            ref BlobArray<float3> Normals = ref voxelShapeBlobAsset.Normals;

            SubMeshRangeHelper subMeshRangeHelp = new SubMeshRangeHelper(vertices, indexs, subMeshRanges, renderType);

            for (int i = 0; i < commands.Length; i++)
            {
                FaceDataCommand faceDataCommand = commands[i];
                VoxelFaceData voxelFaceData = voxelFaceDatas[faceDataCommand.FaceDataIndex];

                int indexStartindex = voxelFaceData.IndexStartIndex;
                int indexEnd = voxelFaceData.IndexEnd;
                int vertexStartIndex = voxelFaceData.VertexStartIndex;
                int vertexEnd = voxelFaceData.VertexEnd;

                subMeshRangeHelp.Update(vertexEnd - vertexStartIndex);

                ushort firstVertexIndex = subMeshRangeHelp.FirstVertexIndex;
                
                for (; indexStartindex < indexEnd; indexStartindex++)
                {
                    indexs.Add((ushort)(firstVertexIndex + Indexs[indexStartindex]));
                }
                int3 voxelPosInSmallChunk = VoxelMath.InverseLocalVoxelArrayIndexInSmallChunk(faceDataCommand.VoxelIndexInSmallChunk);
                ref VoxelType voxelType = ref voxelTypes[faceDataCommand.VoxelTypeIndex];
                for (; vertexStartIndex < vertexEnd; vertexStartIndex++)
                {
                    vertices.Add(Verts[vertexStartIndex] + voxelPosInSmallChunk);
                    float2 uv = UVs[vertexStartIndex];
                    uvs.Add(new float4(uv.x, uv.y, voxelType.TextureIndex, voxelType.Energy));
                    normals.Add(Normals[vertexStartIndex]);
                }
            }
            subMeshRangeHelp.Finish();
        }
    }
}
