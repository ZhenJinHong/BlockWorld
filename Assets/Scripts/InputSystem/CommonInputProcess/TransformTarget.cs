using UnityEngine;

namespace CatFramework.InputMiao
{
    public class TransformTarget : IFirstPersonTarget
    {
        Transform target;
        public TransformTarget(Transform target)
        {
            this.target = target;
        }
        public Vector3 Position
        { get => target.position; set => target.position = value; }
        public Vector3 LocalPosition
        { get => target.localPosition; set => target.localPosition = value; }
        public Quaternion Rotation
        { get => target.rotation; set => target.rotation = value; }
        public Quaternion LocalRotation
        { get => target.localRotation; set => target.localRotation = value; }
        public Vector3 EulerAngles
        { get => target.eulerAngles; set => target.eulerAngles = value; }
        public Vector3 LocalEulerAngles
        { get => target.localEulerAngles; set => target.localEulerAngles = value; }
    }
}
