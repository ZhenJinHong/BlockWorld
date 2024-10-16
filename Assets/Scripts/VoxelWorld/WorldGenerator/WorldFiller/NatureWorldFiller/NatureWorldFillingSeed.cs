using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    //public struct NatureWorldFillingSeed
    //{
    //    public float FlowerRangeOffset;
    //    public float FlowerRangeScale;
    //    public float FlowerThreshold;
    //    public float WaterHeight;
    //}
    //[BurstCompile]
    //public struct NatureWorldHeightMapJob : IJob
    //{
    //    public float2 BigChunkPos;
    //    public BlobAssetReference<NatureWorldSeed> Seed;
    //    public NativeArray<byte> HeightMap;
    //    public void Execute()
    //    {
    //        ref NatureWorldSeed seed = ref Seed.Value;
    //        BigChunkPos += seed.Offset;
    //        for (int z = 0; z < Settings.SmallChunkSize; z++)
    //        {
    //            for (int x = 0; x < Settings.SmallChunkSize; x++)
    //            {
    //                float2 pos = new float2(x, z) + BigChunkPos;
    //                float noise = Noise.GetPerlinNoise(pos, seed.Scale * 4f) * 128f;
    //                noise += Noise.GetPerlinNoise(pos, seed.Scale * 8f) * 64f;
    //                noise += Noise.GetPerlinNoise(pos, seed.Scale * 16f) * 32f;
    //                noise += 128f + 64f + 32f;
    //                noise *= 0.5f;
    //                int index = VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize);
    //                HeightMap[index] = (byte)noise;
    //            }
    //        }
    //    }
    //}
    //[BurstCompile]
    //public struct NatureWorldHumidityMapJob : IJob
    //{
    //    public void Execute()
    //    {

    //    }
    //}
}