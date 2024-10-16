using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.InputMiao;
using CatFramework.PlayerManager;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.PlayerHelper
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChunkRangeShowController : MonoBehaviour
    {
        MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
            enabled = false;
        }
        private void FixedUpdate()
        {
            Vector3 value = PlayerManagerMiao.PlayerPosition;
            transform.position = math.floor(new float3(value.x, 0f, value.z) / Settings.SmallChunkSize) * Settings.SmallChunkSize;
        }
        public void Toggle(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                enabled = !enabled;
                meshRenderer.enabled = enabled;
            }
        }
    }
}