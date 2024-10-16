using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorld
{
    public class VoxelShapeStorage : IUlatticeItemStorage
    {
        public Texture2D ItemImage => voxelShapeInfo?.Icon;
        public Texture2D CornerImage => null;
        public string Label => voxelShapeInfo?.Name ?? string.Empty;
        public string Name => voxelShapeInfo?.Name ?? string.Empty;
        public IVoxelShapeInfo voxelShapeInfo;
    }
    public class VoxelShapeInventory : IReadonlyItemStorageCollection<IUlatticeItemStorage>
    {
        class Classify
        {
            public VoxelShapeClassify VoxelShapeClassify { get; private set; }
            readonly VoxelShapeStorage[] voxelShapeStorages;
            public int Length => voxelShapeStorages.Length;
            public VoxelShapeStorage this[int index] => voxelShapeStorages[index];
            public Classify(VoxelShapeClassify voxelShapeClassify)
            {
                VoxelShapeClassify = voxelShapeClassify;
                var shapeDefinitions = voxelShapeClassify.ShapeDefinitions;
                voxelShapeStorages = new VoxelShapeStorage[shapeDefinitions.Count];
                for (int i = 0; i < shapeDefinitions.Count; i++)
                {
                    VoxelShapeStorage voxelShapeStorage = new VoxelShapeStorage
                    {
                        voxelShapeInfo = shapeDefinitions[i]
                    };
                    voxelShapeStorages[i] = voxelShapeStorage;
                }
            }
        }
        readonly Classify[] classifies;
        Classify Current { get; set; }
        public VoxelShapeInventory(IReadOnlyList<VoxelShapeClassify> voxelShapeClassifies)
        {
            classifies = new Classify[voxelShapeClassifies.Count];
            for (int i = 0; i < classifies.Length; i++)
            {
                classifies[i] = new Classify(voxelShapeClassifies[i]);
            }
            Current = classifies[0];
        }
        public int CurrentSelectedIndex { get; set; }

        public int Count 
            => Current.Length;
        public IUlatticeItemStorage GetItem(int index) 
            => Current[index];
        public bool IndexInRange(int itemIndex) 
            => itemIndex > -1 && itemIndex < Current.Length;
        public bool IndexIsEmpty(int itemIndex) 
            => Current[itemIndex] == null;
    }
}
