using CatFramework;
using CatFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    //public class AlterVoxelCommand<T> : SyncAlterVoxelSystem.ICommand<T> where T : AlterVoxelCommand<T>, new()
    //{
    //    public VoxelCommandPool<T> Pool { get ; set; }

    //    public void RepaidIfPoolItem()
    //    {
    //        Pool?.Repaid(this);// 无法执行
    //    }

    //    public JobHandle ScheduleAlterVoxel(VoxelWorldMap alterVoxelMap, JobHandle dependOn)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class VoxelCommandProvider<T> : IPoolItemProvider<T> where T : class, SyncAlterVoxelSystem.ICommand<T>, new()
    {
        public IPool<T> Pool { get; set; }
        public T Create()
        {
           return new T() { Pool = Pool };
        }
        public void Destroy(T item)
        {
        }
        public void OnGet(T item)
        {
        }
        public void Reset(T item)
        {
        }
    }
    //public class VoxelCommandPool<T> where T : class, SyncAlterVoxelSystem.ICommand<T>, new()
    //{
    //    List<T> pool;
    //    int maxCapacity;
    //    public VoxelCommandPool(int maxCapacity = 8)
    //    {
    //        this.maxCapacity = maxCapacity;
    //        pool = new List<T>();
    //    }
    //    public void Dispose()
    //    {
    //        pool.Clear();
    //    }

    //    public T Get()
    //    {
    //        if (pool.Count != 0)
    //        {
    //            int index = pool.Count - 1;
    //            T obj = pool[index];
    //            pool.RemoveAt(index);
    //            return obj;
    //        }
    //        else
    //        {
    //            T obj = new T() { Pool = this };
    //            return obj;
    //        }
    //    }

    //    public void Repaid(T value)
    //    {
    //        if (pool.Count < maxCapacity)
    //        {
    //            pool.Add(value);
    //        }
    //        else
    //        {
    //            if (ConsoleCat.IsDebug)
    //                ConsoleCat.DebugInfo($"体素命令池达到池上限{maxCapacity}");
    //        }
    //    }
    //}
    public class SingleVoxelCommand : SyncAlterVoxelSystem.ICommand<SingleVoxelCommand>
    {
        public IPool<SingleVoxelCommand> Pool { get; set; }
        public Voxel Voxel { get; set; }
        public int3 TargetVoxelIndexInWorld { get; set; }
        public SingleVoxelCommand()
        {

        }
        public void RepaidIfPoolItem()
        {
            Pool?.Repaid(this);
        }

        public JobHandle ScheduleAlterVoxel(VoxelWorldMap alterVoxelMap, JobHandle dependOn)
        {
            JobHandle jobHandle = new SetOneVoxelJob()
            {
                TargetVoxelIndexInWorld = TargetVoxelIndexInWorld,
                VoxelWorldMap = alterVoxelMap,
                Voxel = Voxel,
            }.Schedule(dependOn);
            return jobHandle;
        }
    }
    [BurstCompile]
    public struct SetOneVoxelJob : IJob
    {
        public int3 TargetVoxelIndexInWorld;
        public VoxelWorldMap VoxelWorldMap;
        public Voxel Voxel;
        public void Execute()
        {
            VoxelWorldMap.TrySetVoxel(Voxel, TargetVoxelIndexInWorld);
        }
    }
    //public class DestroyVoxelCommandPool : Pool<DestroyVoxelCommand>
    //{
    //    public DestroyVoxelCommandPool() : base(4, 64)
    //    {
    //    }

    //    protected override DestroyVoxelCommand CreateElement()
    //    {
    //        return new DestroyVoxelCommand();
    //    }
    //}
    public struct SingleAlterVoxelCommand
    {
        public int3 VoxelIndexInWorld;
        public Voxel Voxel;
    }
    /// <summary>
    /// 同步修改,限于那些需要立刻拿取到行为结果的
    /// </summary>
    [UpdateInGroup(typeof(AlterVoxelSystemGroup), OrderFirst = true)]
    public partial class SyncAlterVoxelSystem : SystemBase
    {
        public interface ICommand<T> : ICommand where T : class, ICommand<T>, new()
        {
            IPool<T> Pool { get; set; }
        }
        public interface ICommand
        {
            JobHandle ScheduleAlterVoxel(VoxelWorldMap alterVoxelMap, JobHandle dependOn);
            void RepaidIfPoolItem();
        }
        static List<ICommand> staticCommandList;
        //static NativeList<SingleAlterVoxelCommand> singleAlterVoxelCommandList;
        //static DestroyVoxelCommandPool destroyVoxelCommandPool;
        //public DestroyVoxelCommand GetDestroyVoxelCommand()
        //{
        //    return destroyVoxelCommandPool?.Get();
        //}
        public static void AddCommand(ICommand command)
        {
            if (command == null) return;
            if (staticCommandList == null)
                command.RepaidIfPoolItem();
            else
                staticCommandList.Add(command);
        }
        //public static void AddSingleAlter(int3 voxelIndexInWorld, Voxel voxel)
        //{
        //    if (singleAlterVoxelCommandList.IsCreated)
        //        singleAlterVoxelCommandList.Add(new SingleAlterVoxelCommand()
        //        {
        //            VoxelIndexInWorld = voxelIndexInWorld,
        //            Voxel = voxel,
        //        });
        //}
        EntityQuery voxelWorldQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);

            builder.WithAll<VoxelWorldTag>().WithAllRW<VoxelWorldMap>();

            voxelWorldQuery = builder.Build(this);

            RequireForUpdate(voxelWorldQuery);
        }
        class CommandData
        {
            public List<ICommand> commandList;
            public CommandData()
            {
                commandList = new List<ICommand>();
            }
            public void Clear()
            {
                commandList.Clear();
            }
            public void Dispose()
            {

            }
        }
        CommandData commandData;
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            commandData ??= new CommandData();
            staticCommandList = commandData.commandList;
            if (staticCommandList.Count != 0)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning("同步体素修改系统命令列表非空");
                staticCommandList.Clear();
            }
        }
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            commandData.Clear();
            staticCommandList = null;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            commandData?.Dispose();
        }
        protected override void OnUpdate()
        {
            var commands = commandData.commandList;
            if (commands.Count != 0)
            {
                //JobHandle dependOn = Dependency;
                //Dependency.Complete();
                VoxelWorldMap alterVoxelMap = voxelWorldQuery.GetSingleton<VoxelWorldMap>();
                JobHandle dependOn = Dependency;
                for (int i = 0; i < commands.Count; i++)
                {
                    ICommand command = commands[i];
                    dependOn = command.ScheduleAlterVoxel(alterVoxelMap, dependOn);
                    command.RepaidIfPoolItem();
                }
                commands.Clear();
                Dependency = dependOn;
            }
        }
    }
}
