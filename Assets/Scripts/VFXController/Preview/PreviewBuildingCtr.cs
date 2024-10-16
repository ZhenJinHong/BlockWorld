using System;
using System.Collections;
using UnityEngine;

namespace CatFramework.VFX
{
    public class PreviewBuildingCtr : VFXController
    {
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Material material;
        public Color passColor = new Color(0f, 0.5f, 1.2f, 1f);
        public Color noPassColor = new Color(1.2f, 0f, 0.15f, 1f);
        protected override void Start()
        {
            base.Start();
            if (!TryGetComponent<MeshRenderer>(out meshRenderer))
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            if (!TryGetComponent<MeshFilter>(out meshFilter))
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            material = meshRenderer.sharedMaterial;
        }
        public void SetMesh(Mesh mesh)
        {
            meshFilter.sharedMesh = mesh;
        }
        public void SetPass(bool pass)
        {
            material.SetColor(baseColorID, pass ? passColor : noPassColor);
        }
    }
}