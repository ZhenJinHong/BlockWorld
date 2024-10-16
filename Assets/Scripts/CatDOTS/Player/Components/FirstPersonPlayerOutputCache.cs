using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS
{
    public struct FirstPersonPlayerOutputCache : IComponentData
    {
        public float CameraXAngle;
        public float CameraYAngle;
        //public float3 Position;
        public quaternion CameraRotation;
        public quaternion PlayerRotation;
    }
}
