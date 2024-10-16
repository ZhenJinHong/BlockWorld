using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatDOTS.VoxelWorld
{
    public struct SubMeshRange
    {
        public VoxelRenderType RenderType;
        public int TriangleStartIndex;
        public int TriangleRangeLength;
        public int BaseVertexIndex;
    }
}
