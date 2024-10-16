namespace CatDOTS.VoxelWorld
{
    [System.Serializable]
    public class D3ChessboardFillerBuilder : IWorldFillerDefinition
    {
        public string Name { get; set; }
        public Voxel Voxel1;
        public Voxel Voxel2;
        public int FloorCount = 32;

        public IWorldFiller Create(uint seed, float baseHeight, VoxelWorldDataBaseManaged dataBase)
        {
            return new D3ChessboardFiller()
            {
                Name = Name,
                FloorCount = FloorCount,
                Voxel1 = Voxel1,
                Voxel2 = Voxel2,
            };
        }
    }
}