namespace CatDOTS.VoxelWorld
{
    public interface IChunkRenderer
    {
        bool Active { set; }

        void ClearMeshData();
        void Initialized(VoxelWorldDataBaseManaged.IRenderProvider renderProvider);
        void SetMeshData(MeshDataContainer container);
    }
}