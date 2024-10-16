using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct DropGenerate
    {
        public float3 Coord;
        public ushort ID;
        public ushort ShapeIndex;
    }
    public struct DropsGenerateBuffer
    {
        NativeList<DropGenerate> buffer;
        public DropsGenerateBuffer(Allocator allocator)
        {
            buffer = new NativeList<DropGenerate>(allocator);
        }
        public void Add(float3 coord, Voxel voxel)
        {
            DropGenerate dropGenerate = new DropGenerate()
            {
                Coord = coord,
                ID = voxel.VoxelTypeIndex,
                ShapeIndex = voxel.ShapeIndex,
            };
            buffer.Add(dropGenerate);
        }
    }
    public struct DropsSystemBufferPool : IComponentData
    {
        
    }
    [DisableAutoCreation]
    [BurstCompile]
    public partial struct DropsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}
