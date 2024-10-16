using CatDOTS.VoxelWorld;
using CatFramework.DataMiao;
using CatFramework.EventsMiao;
using System;
using UnityEngine;

namespace VoxelWorld.PlayerHelper
{
    public class FogShpereController : MonoBehaviour
    {
        [SerializeField] float distanceOffset;
        Material fogMat;
        private void Start()
        {
            fogMat = GetComponent<MeshRenderer>().sharedMaterial;
            var settting = DataManagerMiao.ListenSettingChange<QualitySettingsData>(SettingChanged);
            SettingChanged(settting);
        }
        private void OnDestroy()
        {
            DataManagerMiao.RemoveListenSettingChange<QualitySettingsData>(SettingChanged);
        }
        private void SettingChanged(QualitySettingsData data)
        {
            if (data.Fog)
            {
                gameObject.SetActive(true);
                fogMat.SetFloat("_FogDistance", (data.RenderDistance - distanceOffset) * Settings.SmallChunkSize);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
