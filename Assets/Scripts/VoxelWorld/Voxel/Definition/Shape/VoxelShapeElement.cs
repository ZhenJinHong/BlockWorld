using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class VoxelShapeElement
    {
        public Mesh Mesh;
        public ulong frontRect;
        public ulong backRect;
        public ulong topRect;
        public ulong bottomRect;
        public ulong rightRect;
        public ulong leftRect;
        public FaceRect FrontRect { get => (FaceRect)frontRect; }
        public FaceRect BackRect { get => (FaceRect)backRect; }
        public FaceRect TopRect { get => (FaceRect)topRect; }
        public FaceRect BottomRect { get => (FaceRect)bottomRect; }
        public FaceRect RightRect { get => (FaceRect)rightRect; }
        public FaceRect LeftRect { get => (FaceRect)leftRect; }
    }
}