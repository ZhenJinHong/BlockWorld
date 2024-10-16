using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.DataMiao;
using CatFramework.EventsMiao;
using CatFramework.UiMiao;
using System.Collections.Generic;
using System.Text;
using Unity.Entities;
using UnityEngine;

namespace VoxelWorld.UGUICTR
{
    [RequireComponent(typeof(DebuggerView))]
    public class PopulateDebuggerView : MonoBehaviour
    {
        GameCassette gameCassette;
        private void Start()
        {
            gameCassette = EventManagerMiao.GetLazyObject<VoxelWorldManager>(nameof(VoxelWorldManager)).VoxelWorldGameCassette;
            DebuggerView debuggerView = GetComponent<DebuggerView>();
            List<IDebuggerInfoItem> debuggerInfoItems = new List<IDebuggerInfoItem>
            {
                new DebuggerInfoOption("基本信息", GetSystemInfo),
                new DebuggerInfoOption("数据库", GetDataBaseInfo),
            };

            foreach (var v in DataManagerMiao.GetReadonly().Values)
            {
                if (v.Value is ISetting setting)
                {
                    debuggerInfoItems.Add(new DebuggerSettingInfoItem()
                    {
                        Setting = setting,
                    });
                }
            };
            World world = World.DefaultGameObjectInjectionWorld;

            AddSystem(world.GetExistingSystemManaged<VoxelWorldChunkSystem>());
            AddSystem(world.GetExistingSystemManaged<BuildChunkMeshSystem>());

            debuggerView.Options = debuggerInfoItems.ToArray();
            void AddSystem(ISystemMiao systemMiao)
            {
                if (systemMiao != null)
                {
                    debuggerInfoItems.Add(new DOTSSystemInfoItem()
                    {
                        SystemMiao = systemMiao,
                    });
                }
            }
        }
        public void GetSystemInfo(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("帧率:" + 1 / Time.deltaTime + " FPS");
        }
        public void GetDataBaseInfo(StringBuilder stringBuilder)
        {
            gameCassette?.VoxelWorldDataBaseManaged.GetInfo(stringBuilder);
        }
    }
}