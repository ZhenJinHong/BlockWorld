using CatFramework;
using CatFramework.Tools;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class BigChunkRender : IPoolItem
    {
        int dirty;
        public bool IsDirty => dirty != 0;
        public bool Active
        {
            set
            {
                for (int i = 0; i < chunkRenderObjects.Length; i++)
                {
                    if (chunkRenderObjects[i] != null)
                        chunkRenderObjects[i].Active = value;
                }
            }
        }
        int3 bigChunkIndex;
        public int3 BigChunkIndex
        {
            get => bigChunkIndex;
            set => bigChunkIndex = value;
        }

        ChunkRenderObject[] chunkRenderObjects;
        ChunkRenderObjectPool chunkRenderObjectPool;
        public void Dispose()
        {
            Reset();
            chunkRenderObjects = null;
            chunkRenderObjectPool = null;
        }

        public void Reset()
        {
            chunkRenderObjectPool.Repaid(chunkRenderObjects);
            dirty = 0;
        }
        public BigChunkRender(ChunkRenderObjectPool renderObjectPool, int size)
        {
            chunkRenderObjectPool = renderObjectPool;
            chunkRenderObjects = new ChunkRenderObject[size];
        }
        public void SetDirty(int yIndex)
        {
            dirty |= (1 << yIndex);
        }
        public bool IndexIsDirty(int yIndex)
        {
            return (dirty & (1 << yIndex)) != 0;
        }
        public void CleanDirty()
            => dirty = 0;
        public void SetMeshData(MeshDataContainer meshDataContainer, int yIndex)
        {
            if (dirty != 0)
            {
                dirty = 0;
                if (ConsoleCat.Enable) ConsoleCat.LogWarning("未清空Dirty状态");
            }
            if (meshDataContainer.NonEmpty)
            {
                if (chunkRenderObjects[yIndex] == null)
                {
                    chunkRenderObjects[yIndex] = chunkRenderObjectPool.Get();
                }
                var renderObj = chunkRenderObjects[yIndex];
                renderObj.SetMeshData(meshDataContainer);
                renderObj.SetChunkIndex(new int3(bigChunkIndex.x, yIndex, bigChunkIndex.z));
                renderObj.Active = true;
            }
            else
            {
                chunkRenderObjectPool.Repaid(chunkRenderObjects[yIndex]);
                chunkRenderObjects[yIndex] = null;
            }
        }
    }
}
