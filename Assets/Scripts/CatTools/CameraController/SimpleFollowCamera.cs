using UnityEngine;

namespace CatFramework.Tools
{
    public class FollowTargetCamera : MonoBehaviour
    {
        public Transform Target;
        public float PositionFolowForce = 5f;
        public float RotationFolowForce = 5f;
        void FixedUpdate()
        {
            var direction = Target.rotation * Vector3.forward;
            direction.y = 0f;

            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, Target.position, PositionFolowForce * Time.deltaTime),
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationFolowForce * Time.deltaTime));
        }
    }

}
