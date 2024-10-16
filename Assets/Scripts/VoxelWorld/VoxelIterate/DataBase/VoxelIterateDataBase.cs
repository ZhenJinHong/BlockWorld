using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public struct PlantGrowthChainAsset
    {
        public BlobArray<ushort> Current;// 索引即为体素类型索引,其内包含的ushort指向Next
        public BlobArray<PlantVoxelGrowthChain> Next;// 以类型索引找到next后,查看是否符合生长为next
    }
    public class VoxelIterateDataBase : IDisposable
    {
        BlobAssetReference<PlantGrowthChainAsset> PlantGrowthChainAsset;
        public void Dispose()
        {
            PlantGrowthChainAsset.Dispose();
        }
        public VoxelIterateDataBase(IList<PlantVoxelGrowthChainDefinition> plantVoxelGrowthChains, IVoxelDefinitionDataBase voxelDefinitionDataBase)
        {
            ushort[] current = new ushort[voxelDefinitionDataBase.VoxelTypeCount];
            List<PlantVoxelGrowthChain> next = new List<PlantVoxelGrowthChain>();
            foreach (var chain in plantVoxelGrowthChains)
            {
                var stages = chain.Stages;
                if (stages != null && stages.Length > 1)
                {
                    int i = 0;
                    VoxelDefinition nextStage = stages[i];
                    Voxel nextVoxel = voxelDefinitionDataBase.GetVoxel(nextStage);
                    do
                    {
                        Voxel currentVoxel = nextVoxel;
                        current[currentVoxel.VoxelTypeIndex] = (ushort)next.Count;

                        nextStage = stages[i + 1];
                        nextVoxel = voxelDefinitionDataBase.GetVoxel(nextStage);
                        next.Add(new PlantVoxelGrowthChain()
                        {
                            Next = nextVoxel.VoxelTypeIndex,
                            LightingRequirements = chain.LightingRequirements,
                        });
                        i++;
                    }
                    while (i < stages.Length - 1);
                }
            }

            BlobBuilder builder = new BlobBuilder(Allocator.Temp);
            ref PlantGrowthChainAsset plantGrowthChainAsset = ref builder.ConstructRoot<PlantGrowthChainAsset>();
            builder.CopyToBlobArray(ref plantGrowthChainAsset.Current, current);
            builder.CopyToBlobArray(ref plantGrowthChainAsset.Next, next);

            PlantGrowthChainAsset = builder.CreateBlobAssetReference<PlantGrowthChainAsset>(Allocator.Persistent);
            builder.Dispose();
        }
    }
}
