namespace CatDOTS.VoxelWorld
{
    public enum VoxelMaskTag : byte
    {
        ID = 1 << 0,
        ShapeIndex = 1 << 1,
        Material = 1 << 2,
        ShapeDiretion = 1 << 3,
        Energy = 1 << 4,
    }
}
