using CatFramework;
using CatFramework.DataMiao;
using CatFramework.InputMiao;
using CatFramework.SLMiao;
using CatFramework.UiMiao;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.UGUICTR
{
    public class DebuggerSettingInfoItem : IDebuggerInfoItem
    {
        public string Name => Setting?.Name;
        public ISetting Setting { get; set; }
        public void GetInfo(StringBuilder stringBuilder)
        {
            if (Setting != null)
                Serialization.ObjectFieldToString(Setting, stringBuilder);
        }
    }
    public class DOTSSystemInfoItem : IDebuggerInfoItem
    {
        public string Name => SystemMiao?.Name;
        public ISystemMiao SystemMiao { get; set; }
        public void GetInfo(StringBuilder stringBuilder)
        {
            SystemMiao?.GetInfo(stringBuilder);
        }
    }
}