using Unity.Mathematics;

namespace CatDOTS.VoxelWorld.Player
{
    public interface IPlayerInWorldActionData
    {
        public bool EnableInput { get; }
        public float3 EyeOffset { get; }
        public float3 Position { get; }
        public quaternion CameraRotation { get; }
        public quaternion PlayerRotation { get; }
    }
}
