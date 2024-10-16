using CatDOTS.VoxelWorld;
using CatFramework.SLMiao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace VoxelWorld
{
    public class VoxelTypeVersion
    {
        public int Version;
        // VoxelType里加个版本号,当这个体素索引为新的体素类型占用时版本号增加,
        // 另外每个大区块保存的时候保存一个当前最大版本号,
        // 当重新加载的时候,如果这个最大版本号小于某个VoxelType里的版本号则说明这个体素类型在上次保存之后修改过,
        // 这个VoxelType的对应该区块的体素都不应该加载了
        public void Save()
        {
            List<VoxelType> voxelTypes = new List<VoxelType>();// 还需要为每个体素类型分配一个名称ID
            Save(voxelTypes);// 假定此时体素类型里有版本号

        }
        void Save(object w) { }
        public void Load()
        {
            List<VoxelType> voxelTypes = new List<VoxelType>();// 如果加载上来
            Dictionary<string, int> typeMap = new Dictionary<string, int>();
            for (int i = 0; i < voxelTypes.Count; i++)
            {
                // 需要一个已经新计算好的Voxeltype列表内容
                // 然后与以往的typeMap里查找,是否存在同样ID的,如果存在则VoxelType的版本不用增加,如果不存在,则需要增加版本,通过字典找到所有的不需要增加的类型后
                // 并移除已经被判断过的类型,则字典里剩下的都是已经不存在于游戏中的旧类型,则其空位可以被使用
            }
        }
    }
    public class VoxelSaveAndLoad
    {
        public VoxelTypeVersion voxelTypeVersion;
        void Save(NativeArray<Voxel> voxels)
        {
            var bytes = voxels.Reinterpret<byte>(8);
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(""));
            writer.Write(voxelTypeVersion.Version);
            writer.Write(bytes);
        }
    }
}
