using System;
using UnityEngine;

namespace CatFramework.VFX
{
    public abstract class VFXController : MonoBehaviour, IVFXController
    {
        [SerializeField] string baseColorId = "_BaseColor";
        [SerializeField] bool closeWhenStart = true;
        protected int baseColorID;
        public bool IsUsable
        {
            get => this != null && gameObject.activeSelf;
            set { if (gameObject != null) { gameObject.SetActive(value); } }
        }
        protected virtual void Start()
        {
            baseColorID = Shader.PropertyToID(baseColorId);
            IsUsable = (!closeWhenStart);
        }
        public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
        public virtual void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public virtual void SetRotation(Quaternion rot)
        {
            transform.rotation = rot;
        }
    }
}
