using CatFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class VoxelWorldMeshManaged
    {
        public void GetInfo(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("世界中心区块索引 : " + WorldCenterChunkIndex);
            stringBuilder.AppendLine("最大预加载距离 : " + UnloadingDistance);
            stringBuilder.AppendLine("渲染距离 : " + RenderDistance);
            stringBuilder.AppendLine("ActiveBigChunkRender : " + ActiveMap.Count);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("小区块网格数据容器池 :");
            stringBuilder.AppendLine(NativeMeshDataContainerPool.ToString());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("大区块渲染体池 :");
            stringBuilder.AppendLine(BigChunkRenderPool.ToString());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("等待构建中的大区块 : " + WaitingRebuildChunks.Count);
            stringBuilder.AppendLine("工作ToAdd : " + ToAdd.Count);
            stringBuilder.AppendLine("工作ToComplete : " + ToComplete.Count);
        }

        VoxelWorldDataBaseManaged DataBaseManaged;
        VoxelWorldDataBaseManaged.IVoxelWorldSetting VoxelWorldSetting => DataBaseManaged.VoxelWorldSetting;

        int2 WorldCenterChunkIndex;
        int UnloadingDistance;
        int RenderDistance;

        BigChunkRenderPool BigChunkRenderPool;
        NativeMeshDataContainerPool NativeMeshDataContainerPool;

        Dictionary<int3, BigChunkRender> ActiveMap;
        HashSet<BigChunkRender> WaitingRebuildChunks;
        List<BigChunkRender> TempBigChunkRenderList;


        List<BuildSmallChunkMeshJobData> ToAdd;
        List<BuildSmallChunkMeshJobData> ToComplete;
        public void Clear()
        {
            WorldCenterChunkIndex = int.MaxValue;
            BigChunkRenderPool.ForcedRepaid();
            NativeMeshDataContainerPool.ForcedRepaid();

            ActiveMap.Clear();
            WaitingRebuildChunks.Clear();
            TempBigChunkRenderList.Clear();

            ToAdd.Clear();
            ToComplete.Clear();
        }
        public void Dispose()
        {
            BigChunkRenderPool.Dispose();
            NativeMeshDataContainerPool.Dispose();

            ActiveMap = null;
            WaitingRebuildChunks = null;
            TempBigChunkRenderList = null;

            ToAdd = null;
            ToComplete = null;
        }
        public VoxelWorldMeshManaged(VoxelWorldDataBaseManaged dataBaseManaged)
        {
            DataBaseManaged = dataBaseManaged;

            WorldCenterChunkIndex = int.MaxValue;
            RenderDistance = VoxelWorldSetting.RenderDistance;
            UnloadingDistance = VoxelWorldSetting.UnloadingDistance;
            int chunkRenderObjPoolCapacity = VoxelWorldSetting.ChunkRenderObjectPoolCapacity;
            int unloadingRange = VoxelWorldSetting.UnloadingRange;
            BigChunkRenderPool = new BigChunkRenderPool(unloadingRange * unloadingRange, new ChunkRenderObjectPool(dataBaseManaged.VoxelWorldRenderProvider, chunkRenderObjPoolCapacity, 4096));

            int nativeContainerPoolCapacity = VoxelWorldSetting.UpdateMeshPerFrame;
            NativeMeshDataContainerPool = new NativeMeshDataContainerPool(nativeContainerPoolCapacity, math.max(nativeContainerPoolCapacity, VoxelWorldSetting.LoadBigChunkPerFrame * Settings.WorldHeightInChunk) * 2);// 当前帧需要,上一帧也需要,所以是双倍

            ActiveMap = new Dictionary<int3, BigChunkRender>();
            WaitingRebuildChunks = new HashSet<BigChunkRender>();
            TempBigChunkRenderList = new List<BigChunkRender>();

            ToAdd = new List<BuildSmallChunkMeshJobData>();
            ToComplete = new List<BuildSmallChunkMeshJobData>();
        }
        public void Update()
        {
            if (TempBigChunkRenderList.Count != 0) ConsoleCat.LogWarning("未清理临时列表");
            var temp = ToAdd;
            ToAdd = ToComplete;
            ToComplete = temp;

            UnloadingDistance = VoxelWorldSetting.UnloadingDistance;
            RenderDistance = VoxelWorldSetting.RenderDistance;
        }
        BigChunkRender GetOrNew(int3 bigChunkIndex)
        {
            if (!ActiveMap.TryGetValue(bigChunkIndex, out BigChunkRender bigChunkRender))
            {
                bigChunkRender = BigChunkRenderPool.Get();
                bigChunkRender.BigChunkIndex = bigChunkIndex;
                ActiveMap.Add(bigChunkIndex, bigChunkRender);
            }
            return bigChunkRender;
        }
        #region 工作的获取与完成
        public bool HasWaiting => WaitingRebuildChunks.Count != 0;

        public JobHandle GetJob(VoxelWorldMap.ReadOnly voxelWorldMap, int maxCount, JobHandle dependOn)
        {
            if (ToAdd.Count != 0)
            {
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("未处理上上帧应重构网格"); return dependOn;
            }
            if (WaitingRebuildChunks.Count == 0) return dependOn;
            TempBigChunkRenderList.Clear();
            var toRemove = TempBigChunkRenderList;
            NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(8, Allocator.Temp);
            foreach (var bigChunkRender in WaitingRebuildChunks)
            {
                if (maxCount < Settings.WorldHeightInChunk)
                    break;
                if (bigChunkRender.IsDirty)
                {
                    int3 bigChunkIndex = bigChunkRender.BigChunkIndex;
                    if (voxelWorldMap.TryGetReadOnlySlice(bigChunkIndex, out var bigChunkSlice))
                    {
                        if (voxelWorldMap.HasNearBigChunk(bigChunkIndex))
                        {
                            for (int y = 0; y < Settings.WorldHeightInChunk; y++)
                            {
                                if (!bigChunkRender.IndexIsDirty(y)) continue;
                                maxCount--;
                                BuildSmallChunkMeshJobData jobData = new BuildSmallChunkMeshJobData()
                                {
                                    SmallChunkIndex = new int3(bigChunkIndex.x, y, bigChunkIndex.z),
                                    MeshDataContainer = NativeMeshDataContainerPool.Get(),
                                };
                                ToAdd.Add(jobData);
                                JobHandle jobHandle
                                    = ScheduleBuildMeshData(jobData, bigChunkSlice.Split(y), voxelWorldMap, dependOn);
                                jobHandles.Add(jobHandle);
                            }
                            bigChunkRender.CleanDirty();
                        }
                    }
                    else if (ConsoleCat.Enable)
                        ConsoleCat.LogWarning($"等待重建网格的集合中,出现了无法获取切片的情况 : {bigChunkIndex}");
                }
                else if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning("等待重建网格的集合中,出现非Dirty的区块");
                if (!bigChunkRender.IsDirty)
                    toRemove.Add(bigChunkRender);
            }
            for (int i = 0; i < toRemove.Count; i++)
            {
                if (!WaitingRebuildChunks.Remove(toRemove[i]))
                {
                    if (ConsoleCat.Enable) ConsoleCat.LogWarning("未知情况");
                }
            }
            toRemove.Clear();
            return jobHandles.Length > 0 ? JobHandle.CombineDependencies(jobHandles.AsArray()) : dependOn;
        }
        JobHandle ScheduleBuildMeshData(BuildSmallChunkMeshJobData jobData, VoxelWorldMap.SmallChunkSliceReadyOnly smallChunkSlice, VoxelWorldMap.ReadOnly VoxelWorldMap, JobHandle dependOn)
        {
            IVoxelDefinitionDataBase voxelDefinitionDataBase = DataBaseManaged.VoxelDefinitionDataBase;
            IShapeDefinitionDataBase shapeDefinitionDataBase = DataBaseManaged.ShapeDefinitionDataBase;
            MeshDataContainer container = jobData.MeshDataContainer;
            MeshDataContainer.Block block = container.block;

            // 构建网格
            JobHandle buildMeshJobHandle = new BuildVoxelMeshDataJob()
            {
                VoxelWorldMap = VoxelWorldMap,

                SmallChunkSlice = smallChunkSlice,

                VoxelShapeBlobAsset = shapeDefinitionDataBase.VoxelShapeBlobAsset,
                VoxelTypeDataBase = voxelDefinitionDataBase.VoxelTypeDataBase,

                vertices = block.vertices,
                uvs = block.uvs,
                indexs = block.triangles,
                normals = block.normals,
                subMeshRanges = block.subMeshRanges,
                opaqueFaces = block.opaqueFaces,
                transparentFaces = block.transparentFaces,
            }.Schedule(dependOn);

            MeshDataContainer.PointMeshData grass = container.grass;
            MeshDataContainer.PointMeshData fire = container.fire;
            JobHandle buildGrassDataJobHandle = new BuildVoxelGrassDataJob()
            {
                SmallChunkSlice = smallChunkSlice,
                VoxelTypeDataBase = voxelDefinitionDataBase.VoxelTypeDataBase,
                aabb = container.aabb,
                verts = grass.verts,
                indexs = grass.indexs,
                fireVerts = fire.verts,
                fireIndexs = fire.indexs,
            }.Schedule(dependOn);

            MeshDataContainer.PointMeshData water = container.water;
            JobHandle buildWaterDataJobHandle = new BuildVoxelWaterDataJob()
            {
                SmallChunkSlice = smallChunkSlice,
                VoxelChunkMap = VoxelWorldMap,
                VoxelTypeDataBase = voxelDefinitionDataBase.VoxelTypeDataBase,
                VoxelShapeBlobAsset = shapeDefinitionDataBase.VoxelShapeBlobAsset,
                waterVerts = water.verts,
                waterIndexs = water.indexs,
            }.Schedule(dependOn);

            //JobHandle calculateBoundsJobHandle = new CalculateBoundsJob()
            //{
            //    aabb = container.aabb,
            //    SmallChunkSlice = smallChunkSlice,
            //}.Schedule(dependOn);// 复原,记得设置合并句柄
            var jobHandle = JobHandle.CombineDependencies(buildMeshJobHandle, buildGrassDataJobHandle, buildWaterDataJobHandle);
            return jobHandle;
        }
        public void CompleteJob()
        {
            if (ToComplete.Count == 0) return;
            for (int i = 0; i < ToComplete.Count; i++)
            {
                BuildSmallChunkMeshJobData smallChunkMeshJobData = ToComplete[i];
                int3 smallChunkIndex = smallChunkMeshJobData.SmallChunkIndex;
                MeshDataContainer meshDataContainer = smallChunkMeshJobData.MeshDataContainer;
                if (ActiveMap.TryGetValue(new int3(smallChunkIndex.x, 0, smallChunkIndex.z), out BigChunkRender bigChunkRender))
                {
                    bigChunkRender.SetMeshData(meshDataContainer, smallChunkIndex.y);
                }
                NativeMeshDataContainerPool.Repaid(meshDataContainer);
            }
            ToComplete.Clear();
        }
        #endregion
        /// <summary>
        /// 应当先添加,再获取是否需要重构的区块
        /// </summary>
        /// <param name="waitingRebuildSmallChunkEnumerator"></param>
        /// <param name="worldCenterChunkIndex"></param>
        public void AddRender(NativeArray<int3>.Enumerator waitingRebuildSmallChunkEnumerator, int2 worldCenterChunkIndex)
        {
            if (math.any(this.WorldCenterChunkIndex != worldCenterChunkIndex))
            {
                WaitingRebuildChunks.Clear();// 清理,重新按照范围计算
                this.WorldCenterChunkIndex = worldCenterChunkIndex;
                SetDirty(waitingRebuildSmallChunkEnumerator, false);

                int renderDistance = VoxelWorldSetting.RenderDistance;
                int unloadingDistance = VoxelWorldSetting.UnloadingDistance;
                var waitingUnload = TempBigChunkRenderList;
                foreach (var bigChunkRender in ActiveMap.Values)// 
                {
                    int2 distance = math.abs(bigChunkRender.BigChunkIndex.As2D() - worldCenterChunkIndex);
                    if (math.any(distance > unloadingDistance))// 超出卸载距离则 彻底清理掉网格数据
                    {
                        waitingUnload.Add(bigChunkRender);
                    }
                    else
                    {
                        bool inRenderRange = math.all(distance <= renderDistance);
                        if (inRenderRange && bigChunkRender.IsDirty)
                            WaitingRebuildChunks.Add(bigChunkRender);
                        bigChunkRender.Active = inRenderRange;
                    }
                }
                if (waitingUnload.Count != 0)
                {
                    for (int i = 0; i < waitingUnload.Count; i++)
                    {
                        BigChunkRender item = waitingUnload[i];
                        ActiveMap.Remove(item.BigChunkIndex);
                        BigChunkRenderPool.Repaid(item);
                    }
                    waitingUnload.Clear();// 全部从集合移除后，这里临时列表要清空
                }
            }
            else// 范围位置未变动的情况下
            {
                SetDirty(waitingRebuildSmallChunkEnumerator, true);
            }
        }
        void SetDirty(NativeArray<int3>.Enumerator waitingRebuildSmallChunkEnumerator, bool addRebuild)
        {
            while (waitingRebuildSmallChunkEnumerator.MoveNext())
            {
                int3 smallChunkIndex = waitingRebuildSmallChunkEnumerator.Current;
                BigChunkRender bigChunkRender = GetOrNew(new int3(smallChunkIndex.x, 0, smallChunkIndex.z));
                bigChunkRender.SetDirty(smallChunkIndex.y);
                if (!addRebuild) continue;
                if (VoxelMath.WithInDistance(bigChunkRender.BigChunkIndex.As2D(), WorldCenterChunkIndex, RenderDistance))
                {
                    WaitingRebuildChunks.Add(bigChunkRender);
                }
            }
        }
    }
    public struct BuildSmallChunkMeshJobData
    {
        public int3 SmallChunkIndex;
        public MeshDataContainer MeshDataContainer;
    }
}
