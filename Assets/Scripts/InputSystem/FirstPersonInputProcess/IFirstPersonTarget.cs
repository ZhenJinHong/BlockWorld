using UnityEngine;

namespace CatFramework.InputMiao
{
    public interface IFirstPersonTarget
    {
        Vector3 Position { get; set; }
        Vector3 LocalPosition { get; set; }
        Quaternion Rotation { get; set; }
        Quaternion LocalRotation { get; set; }
        Vector3 EulerAngles { get; set; }
        Vector3 LocalEulerAngles { get; set; }
    }
}
