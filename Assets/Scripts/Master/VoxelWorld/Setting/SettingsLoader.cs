using CatFramework.DataMiao;
using System.Collections;
using UnityEngine;

namespace VoxelWorld
{
    public class SettingsLoader : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            DataManagerMiao.LoadOrCreateSetting<QualitySettingsData>();
            DataManagerMiao.LoadOrCreateSetting<PlayerSettingData>();
            DataManagerMiao.LoadOrCreateSetting<DebugSetting>();
            DataManagerMiao.LoadOrCreateSetting<PlayerFreeViewSetting>();
        }
    }
}