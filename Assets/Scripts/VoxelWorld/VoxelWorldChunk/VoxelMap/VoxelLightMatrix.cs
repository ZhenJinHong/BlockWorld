using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelLightMatrix
    {
        public NativeArray<byte> LightMatrix;
        public VoxelLightMatrix(int matrixSize)
        {
            LightMatrix = new NativeArray<byte>(matrixSize * matrixSize * matrixSize, Allocator.Persistent);
        }
        public void Dispose()
        {
            LightMatrix.Dispose();
        }
    }
}
