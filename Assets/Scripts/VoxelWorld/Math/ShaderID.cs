using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public static class ShaderID
    {
        public readonly static int BaseTex2DArray = Shader.PropertyToID("_BaseTex2DArray");
        public readonly static int Normal2DArray = Shader.PropertyToID("_Normal2DArray");
    }
}
