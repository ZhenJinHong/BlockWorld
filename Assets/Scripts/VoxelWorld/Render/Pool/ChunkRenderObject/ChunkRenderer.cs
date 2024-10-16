using CatFramework.Tools;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public class ChunkRendererProvider : IPoolItemProvider<ChunkRenderer>
    {
        public IPool<ChunkRenderer> Pool { get; set; }
        public GameObject prefab;
        public Transform parent;
        public ChunkRendererProvider(GameObject prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }
        public ChunkRenderer Create()
        {
            ChunkRenderer chunkRenderer = Object.Instantiate(prefab, parent).GetComponent<ChunkRenderer>();
            chunkRenderer.Initialized();
            return chunkRenderer;
        }

        public void Destroy(ChunkRenderer item)
        {
            Object.Destroy(item.gameObject);
        }

        public void OnGet(ChunkRenderer item)
        {
        }

        public void Reset(ChunkRenderer item)
        {
            item.Mesh.Clear();
        }
    }
    public abstract class ChunkRendererPoolUnion
    {
        public string RendererName;
        ChunkRenderer chunkRenderer;
        protected VoxelWorldDataBaseManaged.IRenderProvider renderProvider;
        IPool<ChunkRenderer> Pool => renderProvider.ChunkRenderPool;
        public bool Active
        {
            get => chunkRenderer != null && chunkRenderer.Active;
            set { if (chunkRenderer != null) chunkRenderer.Active = value; }
        }
        public void SetPosition(Vector3 position)
        {
            if (chunkRenderer != null) chunkRenderer.transform.position = position;
        }
        public ChunkRendererPoolUnion(VoxelWorldDataBaseManaged.IRenderProvider renderProvider)
        {
            this.renderProvider = renderProvider;
        }
        public void ClearMesh()
        {
            if (chunkRenderer != null)
                chunkRenderer.Mesh.Clear();
        }
        public ChunkRenderer GetOrClear()
        {
            if (chunkRenderer == null)
            {
                chunkRenderer = Pool.Get();
                if (RendererName != null)
                    chunkRenderer.name = RendererName;
                SetChunkRendererSetting(chunkRenderer);
            }
            else
            {
                chunkRenderer.Mesh.Clear();
            }
            return chunkRenderer;
        }
        protected abstract void SetChunkRendererSetting(ChunkRenderer chunkRenderer);
        public void Repaid()
        {
            if (chunkRenderer != null)
            {
                Pool.Repaid(chunkRenderer);
                chunkRenderer = null;
            }
        }
    }
    public class ChunkRenderer : MonoBehaviour
    {
        protected Mesh mesh;
        public Mesh Mesh => mesh;
        public MeshRenderer MeshRenderer => meshRenderer;
        public MeshFilter MeshFilter => filter;
        protected MeshRenderer meshRenderer;
        protected MeshFilter filter;
        public bool Active
        {
            get => meshRenderer.enabled;
            set => meshRenderer.enabled = mesh.vertexCount != 0 && value;
        }
        private void OnDestroy()
        {
            Destroy(mesh);
        }
        public void Initialized()
        {
            if (mesh != null) { return; }
            meshRenderer = GetComponent<MeshRenderer>();
            filter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            filter.mesh = mesh;
        }
    }
}