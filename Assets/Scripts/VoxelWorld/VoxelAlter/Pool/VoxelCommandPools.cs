using CatFramework.Tools;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelCommandPools
    {
        SingleVoxelCommand GetSingleVoxelCommand();
    }
    public class VoxelCommandPools : IVoxelCommandPools
    {
        public Pool<SingleVoxelCommand> SingleVoxelCommandPool { get; private set; }
        public VoxelCommandPools()
        {
            SingleVoxelCommandPool = new Pool<SingleVoxelCommand>(new VoxelCommandProvider<SingleVoxelCommand>(), 8, 16);
        }

        public SingleVoxelCommand GetSingleVoxelCommand()
        {
            return SingleVoxelCommandPool.Get();
        }
    }
}
