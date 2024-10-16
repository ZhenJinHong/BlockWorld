using Unity.Entities;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public struct VoxelShapeBlobAsset
    {
        public BlobArray<VoxelFaceData> FaceDatas;
        public BlobArray<float3> Verts;
        public BlobArray<ushort> Indexs;
        public BlobArray<float2> UVs;
        public BlobArray<float3> Normals;
        public BlobArray<int3> FaceForwardFindMap;
        public BlobArray<float4x4> DirectionMatrix;
        public BlobArray<float> CompressWateFaceCubeAngle;
        //public static float3 WaterFaceOffset(int f)
        //{
        //    const float offset = 0.001f;
        //    return f switch
        //    {
        //        0 => new float3(0f, 0f, -offset),
        //        1 => new float3(0f, 0f, offset),
        //        2 => new float3(0f, -offset, 0f),
        //        3 => new float3(0f, offset, 0f),
        //        4 => new float3(-offset, 0f, 0f),
        //        5 => new float3(offset, 0f, 0f),
        //        _ => new float3()
        //    };
        //}
        public static void BuildFaceForwardFindMap(ref VoxelShapeBlobAsset voxelShapeBlobAsset, ref BlobBuilder builder)
        {
            BlobBuilderArray<int3> blobBuilderArray = builder.Allocate<int3>(ref voxelShapeBlobAsset.FaceForwardFindMap, 7);
            blobBuilderArray[0] = new int3(0, 0, 1);  // 前面
            blobBuilderArray[1] = new int3(0, 0, -1); // 背面
            blobBuilderArray[2] = new int3(0, 1, 0);  // 上面
            blobBuilderArray[3] = new int3(0, -1, 0); // 下面
            blobBuilderArray[4] = new int3(1, 0, 0);  // 右面
            blobBuilderArray[5] = new int3(-1, 0, 0); // 左面
            blobBuilderArray[6] = new int3();
        }
        public static void BuildRotateMatrix(ref VoxelShapeBlobAsset voxelShapeBlobAsset, ref BlobBuilder builder)
        {
            BlobBuilderArray<float4x4> dirs = builder.Allocate<float4x4>(ref voxelShapeBlobAsset.DirectionMatrix, 8);
            dirs[0] = float4x4.RotateY(math.radians(0f));
            dirs[1] = float4x4.RotateY(math.radians(90f));
            dirs[2] = float4x4.RotateY(math.radians(180f));
            dirs[3] = float4x4.RotateY(math.radians(270f));
            float4x4 filp = float4x4.RotateZ(math.radians(180f));
            dirs[4] = math.mul(filp, float4x4.RotateY(math.radians(0f)));
            dirs[5] = math.mul(filp, float4x4.RotateY(math.radians(90f)));
            dirs[6] = math.mul(filp, float4x4.RotateY(math.radians(180f)));
            dirs[7] = math.mul(filp, float4x4.RotateY(math.radians(270f)));

        }
        public static void BuildCompressWaterFaceCubeAngle(ref VoxelShapeBlobAsset voxelShapeBlobAsset, ref BlobBuilder builder)
        {
            // 针对float3X3 yzx 旋转矩阵
            //float3 s, c;
            //sincos(xyz, s, c);
            //return float3x3(
            //            c.y * c.z, -c.y * s.z, s.y,
            //            c.z * s.x * s.y + c.x * s.z, c.x * c.z - s.x * s.y * s.z, -c.y * s.x,
            //            s.x * s.z - c.x * c.z * s.y, c.z * s.x + c.x * s.y * s.z, c.x * c.y
            //            );
            BlobBuilderArray<float> angles = builder.Allocate<float>(ref voxelShapeBlobAsset.CompressWateFaceCubeAngle, 6);
            //angles[0] = 1 + 0 + 0;// 前面
            //angles[1] = 3 + 0 + 0;// 背面
            //angles[2] = 0 + 0 + 0;// 上面 为默认
            //angles[3] = 2 + 0 + 0;// 下面
            //angles[4] = 0 + 0 + (3 << 16);// 右面
            //angles[5] = 0 + 0 + (1 << 16);// 左面
            // xyz
            angles[0] = 1 + 0 + 0;// 前面
            angles[1] = 1 + (2 << 8) + 0;// 背面
            angles[2] = 0 + 0 + 0;// 上面 为默认
            angles[3] = 2 + 0 + 0;// 下面
            angles[4] = 1 + (1 << 8) + 0;// 右面
            angles[5] = 1 + (3 << 8) + 0;// 左面
            //angles[0] = 0 + 0 + 0;// 前面
            //angles[1] = 0 + (2 << 8) + 0;// 背面
            //angles[2] = 3 + 0 + 0;// 上面
            //angles[3] = 1 + 0 + 0;// 下面
            //angles[4] = 0 + (1 << 8) + 0;// 右面
            //angles[5] = 0 + (3 << 8) + 0;// 左面
            // 说的沿着旋转轴向原点看去,顺时针的
            //BlobBuilderArray<float> angles = builder.Allocate<float>(ref voxelShapeBlobAsset.CompressWateFaceCubeAngle, 6);
            //angles[0] = 1 + (1 << 5) + 0 + 0;// 前面
            //angles[1] = 3 + (1 << 5) + 0 + 0;// 背面
            //angles[2] = 0 + (1 << 5) + 0 + 0;// 上面 为默认
            //angles[3] = 2 + (1 << 5) + 0 + 0;// 下面
            //angles[4] = 3 + 0 + 0 + (1 << 15);// 右面
            //angles[5] = 1 + 0 + 0 + (1 << 15);// 左面
        }
    }
}
