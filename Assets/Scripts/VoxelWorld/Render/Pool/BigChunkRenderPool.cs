using CatFramework.Tools;

namespace CatDOTS.VoxelWorld
{
    public class BigChunkRenderPool : ForcedPool<BigChunkRender>
    {
        public override string ToString()
        {
            return base.ToString() + "\n" + "小区块渲染体池:\n " + chunkRenderObjectPool;
        }
        ChunkRenderObjectPool chunkRenderObjectPool;
        public BigChunkRenderPool(int initCapacity, ChunkRenderObjectPool renderObjectPool) : base(initCapacity, initCapacity)
        {
            chunkRenderObjectPool = renderObjectPool;
        }
        public override void ForcedRepaid()
        {
            base.ForcedRepaid();
            chunkRenderObjectPool.ForcedRepaid();
        }
        public override void Dispose()
        {
            base.Dispose();
            chunkRenderObjectPool.Dispose();
        }
        protected override BigChunkRender CreateElement()
        {
            return new BigChunkRender(chunkRenderObjectPool, Settings.WorldHeightInChunk);
        }
    }
    //public struct BuildBigChunkMeshJobData
    //{
    //    public int2 BigChunkIndex;
    //    public List<BuildSmallChunkMeshJobData> jobDatas;
    //    public bool IsComplete => jobDatas.Count == 0;
    //    //public void Complete(Action<MeshDataContainer> repaidContainer)
    //    //{
    //    //    for (int i = 0; i < jobDatas.Count; i++)
    //    //    {
    //    //        var jobData = jobDatas[i];
    //    //        BigChunkRender.SetMeshData(jobData.MeshDataContainer, jobData.yIndex);
    //    //        repaidContainer(jobData.MeshDataContainer);
    //    //    }
    //    //    jobDatas.Clear();
    //    //}
    //}
}
