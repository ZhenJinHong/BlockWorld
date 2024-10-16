//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Collections.LowLevel.Unsafe;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;

//namespace CatDOTS.VoxelWorld
//{
//    [Obsolete]
//    public struct VoxelCommand
//    {
//        public int3 VoxelIndexInBigChunk;
//        public Voxel Voxel;
//    }
//    [Obsolete]
//    public struct SingleVoxelCommand
//    {
//        public int3 VoxelIndexInWorld;
//        public Voxel Voxel;
//    }
//    [Obsolete]
//    public struct VoxelCommandBuffer
//    {
//        public readonly int3 TargetBigChunkIndex;
//        internal readonly NativeList<VoxelCommand> commands;
//        public VoxelCommandBuffer(int3 targetBigChunkIndex, NativeList<VoxelCommand> commands)
//        {
//            this.TargetBigChunkIndex = targetBigChunkIndex;
//            this.commands = commands;
//        }
//        public void Add(int3 voxelIndexInBigChunk, Voxel voxel)
//        {
//            commands.Add(new VoxelCommand()
//            {
//                VoxelIndexInBigChunk = voxelIndexInBigChunk,
//                Voxel = voxel
//            });
//        }
//    }
//    //// 按照名称更改体素区块图，并设置dirty
//    //[UpdateInGroup(typeof(AlterVoxelSystemGroup))]
//    //[BurstCompile]
//    [Obsolete]
//    public struct AlterVoxelSystem /*: ISystem, ISystemStartStop*/
//    {
//        public struct VoxelWorldAlterCommandBuffer : IComponentData
//        {
//            internal NativeList<SingleVoxelCommand> singleCommands;
//            internal NativeList<VoxelCommandBuffer> commandBufferList;
//            internal NativeList<AlterCommandBuffer> alterCommandBufferList;// 以非引用方式获取组件后，这个列表的长度能正确增加的前提，在于这个列表里的是使用的不安全列表的指针，关键在于指针
//            public readonly bool IsCreated => commandBufferList.IsCreated;
//            public readonly bool NonEmpty => singleCommands.Length != 0 || commandBufferList.Length != 0;
//            internal void Clear()
//            {
//                singleCommands.Clear();
//                commandBufferList.Clear();
//                alterCommandBufferList.Clear();
//            }
//            internal void Dispose()
//            {
//                singleCommands.Dispose();
//                commandBufferList.Dispose();
//                alterCommandBufferList.Dispose();
//            }
//            public void AddSingleCommand(int3 voxelIndexInWorld, Voxel voxel)
//            {
//                singleCommands.Add(new SingleVoxelCommand()
//                {
//                    VoxelIndexInWorld = voxelIndexInWorld,
//                    Voxel = voxel
//                });
//            }
//            public VoxelCommandBuffer CreateCommoanBuffer(int3 targetBigChunkIndex, WorldUnmanaged world)
//            {
//                return CreateCommandBuffer(targetBigChunkIndex, world.UpdateAllocator.ToAllocator);
//            }
//            public VoxelCommandBuffer CreateCommandBuffer(int3 targetBigChunkIndex, Allocator allocator)
//            {
//#if UNITY_EDITOR
//                if (targetBigChunkIndex.y != 0)
//                {
//                    Debug.LogError($"大区块索引越界：{targetBigChunkIndex}");
//                }
//#endif
//                NativeList<VoxelCommand> commandBuffer = new NativeList<VoxelCommand>(32, allocator);
//                var voxelCommandBuffer = new VoxelCommandBuffer(targetBigChunkIndex, commandBuffer);
//                commandBufferList.Add(voxelCommandBuffer);
//                return voxelCommandBuffer;
//            }
//            public AlterCommandBuffer CreateAlterCommandBuffer(int3 targetBigChunkIndex, Allocator allocator)
//            {
//                AlterCommandBuffer alterCommandBuffer = new AlterCommandBuffer(targetBigChunkIndex, allocator);
//                alterCommandBufferList.Add(alterCommandBuffer);
//                return alterCommandBuffer;
//            }
//        }
//        EntityQuery voxelWorldQuery;
//        //EntityQuery alterVoxelQuery;
//        [BurstCompile]
//        public void OnCreate(ref SystemState state)
//        {
//            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

//            builder.WithAll<VoxelWorldTag, VoxelWorldDataBase, VoxelWorldChunkSystem.RegisterDirtyBigChunk>().WithAllRW<VoxelWorldMap>();
//            voxelWorldQuery = builder.Build(ref state);

//            builder.Reset();
//            builder.WithAllRW<VoxelWorldAlterCommandBuffer>();
//            //alterVoxelQuery = builder.Build(ref state);

//            state.RequireForUpdate(voxelWorldQuery);
//        }
//        VoxelWorldAlterCommandBuffer worldAlterCommandBuffer;
//        [BurstCompile]
//        public void OnStartRunning(ref SystemState state)
//        {
//            if (!worldAlterCommandBuffer.IsCreated)
//            {
//                worldAlterCommandBuffer = new VoxelWorldAlterCommandBuffer()
//                {
//                    singleCommands = new NativeList<SingleVoxelCommand>(16, Allocator.Persistent),
//                    commandBufferList = new NativeList<VoxelCommandBuffer>(16, Allocator.Persistent),
//                    alterCommandBufferList = new NativeList<AlterCommandBuffer>(16, Allocator.Persistent),
//                };
//                state.EntityManager.AddComponentData<VoxelWorldAlterCommandBuffer>(voxelWorldQuery.GetSingletonEntity(), worldAlterCommandBuffer);
//            }
//        }
//        [BurstCompile]
//        public void OnStopRunning(ref SystemState state)
//        {
//            worldAlterCommandBuffer.Clear();
//        }
//        [BurstCompile]
//        public void OnDestroy(ref SystemState state)
//        {
//            if (worldAlterCommandBuffer.IsCreated)
//                worldAlterCommandBuffer.Dispose();
//        }
//        [BurstCompile]
//        public void OnUpdate(ref SystemState state)
//        {
//            //if (worldAlterCommandBuffer.NonEmpty)
//            //{
//            //    state.Dependency.Complete();
//            //    Entity voxelWorld = voxelWorldQuery.GetSingletonEntity();
//            //    VoxelWorldMap voxelWorldMap = state.EntityManager.GetComponentData<VoxelWorldMap>(voxelWorld);
//            //    VoxelWorldChunkSystem.RegisterDirtyBigChunk registerDirtyBigChunk
//            //        = state.EntityManager.GetComponentData<VoxelWorldChunkSystem.RegisterDirtyBigChunk>(voxelWorld);

//            //    CompleteSingleCommand(ref voxelWorldMap, ref registerDirtyBigChunk);
//            //    CompleteCommandBuffer(ref voxelWorldMap, ref registerDirtyBigChunk);
//            //    state.Dependency = ScheduleCompleteACB(ref voxelWorldMap, ref registerDirtyBigChunk, state.Dependency);

//            //    worldAlterCommandBuffer.Clear();
//            //}
//        }
//        //void CompleteSingleCommand(ref VoxelWorldMap voxelWorldMap, ref VoxelWorldChunkSystem.RegisterDirtyBigChunk registerDirtyBigChunk)
//        //{
//        //    NativeList<SingleVoxelCommand> commands = worldAlterCommandBuffer.singleCommands;
//        //    if (commands.Length != 0)
//        //    {
//        //        for (int i = 0; i < commands.Length; i++)
//        //        {
//        //            SingleVoxelCommand command = commands[i];
//        //            int3 voxelIndexInWorld = command.VoxelIndexInWorld;
//        //            int3 bigChunkIndex = VoxelMath.BigChunkIndexByWorldCoord(voxelIndexInWorld);
//        //            int3 voxelIndexInBigChunk = VoxelMath.VoxelIndexInBigChunk(bigChunkIndex, voxelIndexInWorld);

//        //            if (voxelWorldMap.RefSliceWithoutDirty(bigChunkIndex, out NativeSlice<Voxel> bigChunkSlice))
//        //            {
//        //                DirtyBigChunkInfo dirtyBigChunkInfo = new DirtyBigChunkInfo(bigChunkIndex, true);// 实例一个区块变更信息
//        //                int voxelArrayIndex = VoxelMath.LocalVoxelArrayIndexInBigChunk(voxelIndexInBigChunk);
//        //                //command.Voxel.VoxelMaterial|=
//        //                bigChunkSlice[voxelArrayIndex] = command.Voxel;
//        //                dirtyBigChunkInfo.DirtyMesh(voxelIndexInBigChunk.y / Settings.SmallChunkSize);
//        //                registerDirtyBigChunk.Add(dirtyBigChunkInfo);

//        //                //  voxelIndexInBigChunk里的x和z在0到Settings.SmallChunkSize之间,求余后是不变的;而y会因为求余,最终剩余0到Settings.SmallChunkSize
//        //                int3 voxelIndexInSmallChunk = voxelIndexInBigChunk % Settings.SmallChunkSize;

//        //                int3 smallChunkIndex = VoxelMath.SmallChunkIndexByWorldCoord(voxelIndexInWorld);
//        //                //#if UNITY_EDITOR
//        //                //                        Debug.Log($"体素：{voxelIndexInWorld}在大区块{bigChunkIndex}中的小区块：{smallChunkIndex}中的索引：{voxelIndexInSmallChunk}");
//        //                //#endif
//        //                // 不要删除
//        //                // 不能使用math.any(voxelIndexInSmallChunk == 0)
//        //                // 和math.any(voxelIndexInSmallChunk == (Settings.SmallChunkSize - 1))
//        //                // 因为可能有处于背面（+Z）又同时处于左面的（-X）的
//        //                if (voxelIndexInSmallChunk.x == 0)
//        //                {
//        //                    int3 left = new int3(smallChunkIndex.x - 1, bigChunkIndex.y, smallChunkIndex.z);
//        //                    if (voxelWorldMap.ContainsKey(left))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(left, smallChunkIndex.y));
//        //                }
//        //                if (voxelIndexInSmallChunk.y == 0 && smallChunkIndex.y - 1 > -1)
//        //                {
//        //                    int3 bottom = new int3(smallChunkIndex.x, bigChunkIndex.y, smallChunkIndex.z);
//        //                    if (voxelWorldMap.ContainsKey(bottom))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(bottom, smallChunkIndex.y - 1));// 自身区块底下的区块索引是自身Y减一
//        //                }
//        //                if (voxelIndexInSmallChunk.z == 0)
//        //                {
//        //                    int3 front = new int3(smallChunkIndex.x, bigChunkIndex.y, smallChunkIndex.z - 1);
//        //                    if (voxelWorldMap.ContainsKey(front))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(front, smallChunkIndex.y));
//        //                }
//        //                int limitIndex = Settings.SmallChunkSize - 1;
//        //                if (voxelIndexInSmallChunk.x == limitIndex)
//        //                {
//        //                    int3 right = new int3(smallChunkIndex.x + 1, bigChunkIndex.y, smallChunkIndex.z);
//        //                    if (voxelWorldMap.ContainsKey(right))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(right, smallChunkIndex.y));
//        //                }
//        //                if (voxelIndexInSmallChunk.y == limitIndex && smallChunkIndex.y + 1 < Settings.WorldHeightInChunk)
//        //                {
//        //                    int3 top = new int3(smallChunkIndex.x, bigChunkIndex.y, smallChunkIndex.z);
//        //                    if (voxelWorldMap.ContainsKey(top))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(top, smallChunkIndex.y + 1));
//        //                }
//        //                if (voxelIndexInSmallChunk.z == limitIndex)
//        //                {
//        //                    int3 back = new int3(smallChunkIndex.x, bigChunkIndex.y, smallChunkIndex.z + 1);
//        //                    if (voxelWorldMap.ContainsKey(back))
//        //                        registerDirtyBigChunk.Add(new DirtyBigChunkInfo(back, smallChunkIndex.y));
//        //                }
//        //            }
//        //        }
//        //    }
//        //}
//        //void CompleteCommandBuffer(ref VoxelWorldMap voxelWorldMap, ref VoxelWorldChunkSystem.RegisterDirtyBigChunk registerDirtyBigChunk)
//        //{
//        //    var commandBufferList = worldAlterCommandBuffer.commandBufferList;
//        //    if (commandBufferList.Length != 0)
//        //    {
//        //        for (int i = 0; i < commandBufferList.Length; i++)
//        //        {
//        //            VoxelCommandBuffer voxelCommandBuffer = commandBufferList[i];
//        //            NativeList<VoxelCommand> voxelCommands = voxelCommandBuffer.commands;
//        //            if (voxelCommands.Length != 0)
//        //            {
//        //                int3 bigChunkIndex = voxelCommandBuffer.TargetBigChunkIndex;// 需要改的大区块
//        //                if (voxelWorldMap.RefSliceWithoutDirty(bigChunkIndex, out NativeSlice<Voxel> bigChunkSlice))// 是否存在该区块
//        //                {
//        //                    DirtyBigChunkInfo dirtyBigChunkInfo = new DirtyBigChunkInfo(bigChunkIndex, true);// 实例一个区块变更信息
//        //                    foreach (VoxelCommand command in voxelCommands)
//        //                    {
//        //                        int3 voxelIndexInBigChunk = command.VoxelIndexInBigChunk;
//        //                        bigChunkSlice[VoxelMath.LocalVoxelArrayIndexInBigChunk(voxelIndexInBigChunk)] = command.Voxel;
//        //                        // 不存在负数y的时候才能用整数以及不用floor
//        //                        dirtyBigChunkInfo.DirtyMesh(voxelIndexInBigChunk.y / Settings.SmallChunkSize);// 指定的区块网格需要重构
//        //                    }
//        //                    registerDirtyBigChunk.Add(dirtyBigChunkInfo);// 登记信息
//        //                }
//        //                // 还需要告知生成区块系统，该区块做过更改
//        //            }
//        //        }
//        //    }
//        //}
//        //JobHandle ScheduleCompleteACB(ref VoxelWorldMap voxelWorldMap, ref VoxelWorldChunkSystem.RegisterDirtyBigChunk registerDirtyBigChunk, JobHandle dependOn)
//        //{
//        //    NativeList<AlterCommandBuffer> alterCommandBuffers = worldAlterCommandBuffer.alterCommandBufferList;
//        //    JobHandle handle = dependOn;
//        //    if (alterCommandBuffers.Length != 0)
//        //    {
//        //        for (int i = 0; i < alterCommandBuffers.Length; i++)
//        //        {
//        //            AlterCommandBuffer command = alterCommandBuffers[i];
//        //            if (voxelWorldMap.RefSliceWithoutDirty(command.BigChunkIndex, out NativeSlice<Voxel> bigChunkSlice))
//        //            {
//        //                handle = new AlterCommandJob()
//        //                {
//        //                    AlterCommandBuffer = command,
//        //                    VoxelWorldSlice = bigChunkSlice,
//        //                }.Schedule(handle);
//        //                //JobHandle.CombineDependencies(handle, alterJobHandle);
//        //                DirtyBigChunkInfo dirtyBigChunkInfo = new DirtyBigChunkInfo(command.BigChunkIndex, true);
//        //                registerDirtyBigChunk.Add(dirtyBigChunkInfo);
//        //            }
//        //        }
//        //    }

//        //    return handle;
//        //}
//    }
//}
