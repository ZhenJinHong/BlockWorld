using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class ShapePhysicsInfo : IPhysicsInfo
    {
        [SerializeField] ColliderType colliderType;
        [SerializeField] private float bevelRadius = 0.05f;
        [SerializeField] private Vector3 center = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] private Vector3 size = new Vector3(1, 1, 1);
        [SerializeField] private Vector3 angle = Vector3.zero;
        public ShapePhysicsInfo()
        {
            bevelRadius = 0.05f;
            center = new Vector3(0.5f, 0.5f, 0.5f);
            size = new Vector3(1, 1, 1);
            angle = Vector3.zero;
        }
        //[SerializeField] private bool solid = true;
        public static ShapePhysicsInfo Cube
        {
            get
            {
                return new ShapePhysicsInfo()
                {
                    center = new Vector3(),
                    size = new Vector3(1, 1, 1),
                    angle = Vector3.zero,
                    //solid = true,
                };
            }
        }
        public static bool IsValid(ShapePhysicsInfo info)
        {
            return info.size != Vector3.zero;
        }
        public ColliderType ColliderType => colliderType;
        public float BevelRadius { get => bevelRadius; set => bevelRadius = value; }
        public Vector3 Center { get => center; set => center = value; }
        public Vector3 Size { get => size; set => size = value; }
        public Vector3 Angle { get => angle; set => angle = value; }
        //public bool Solid { get => solid; set => solid = value; }
    }
}