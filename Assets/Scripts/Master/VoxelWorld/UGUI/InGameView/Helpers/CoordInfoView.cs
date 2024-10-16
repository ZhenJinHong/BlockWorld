using CatFramework.EventsMiao;
using CatFramework.PlayerManager;
using CatFramework.UiMiao;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using VoxelWorld.UGUICTR;

namespace VoxelWorld
{
    public class CoordInfoView : MonoBehaviour
    {
        [SerializeField] TextMiao coordText;
        private void Update()
        {
            var pos = math.round(PlayerManagerMiao.PlayerPosition);
            coordText.TextValue = $"x: {pos.x} y: {pos.y} z: {pos.z}";
        }
    }
}