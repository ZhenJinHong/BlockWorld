//using CatFramework;
//using CatFramework.SLMiao;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.Collections;
//using Unity.Entities;
//using Random = Unity.Mathematics.Random;

//namespace CatDOTS.VoxelWorld
//{
//    public struct SeedData
//    {
//        public float rangeScale;
//        public float deepWaterScale;
//        public float waterScale;
//        public float groundScale;
//        public float mountainScale;

//        public float rangeOffset;
//        public float deepWaterOffset;
//        public float waterOffset;
//        public float groundOffset;
//        public float mountainOffset;
//        public float BeachWidth;

//        public float D3AirScale;
//        public float D3AirOffset;
//        public float D3AirThreshold;

//        public float SolidGroundHeight;
//        public float TerrainHeight;
//        public float deepWaterPoint;
//        public float waterPoint;
//        public float groundPoint;
//        public float mountainPoint;

//        public float noiseOffset;
//        public float grassScale;
//        public float grassThreshold;
//        public float noiseScale;
//        public int octave;
//        public float persistance;
//        public float redistributionModifier;
//        public float exponent;

//        public int octaves;
//        public uint octaveOffsetSeed;
//        public float nPersistance;
//        public float lacunarity;

//        public uint Seed;
//        public SeedData(uint seed)
//        {
//            Seed = seed;
//            Random random = new Random(seed);
//            rangeScale = random.NextFloat(4f, 5f) * 0.0003f;

//            deepWaterScale = random.NextFloat(0.003f, 0.008f);
//            waterScale = random.NextFloat(0.003f, 0.008f);
//            mountainScale = random.NextFloat(0.01f, 0.012f);
//            groundScale = random.NextFloat(0.01f, 0.012f);

//            rangeOffset = random.NextFloat(131f, 439457f);
//            deepWaterOffset = random.NextFloat(33f, 45631f);
//            waterOffset = random.NextFloat(214f, 235325f);
//            groundOffset = random.NextFloat(213f, 563546f);
//            mountainOffset = random.NextFloat(233f, 774897f);

//            BeachWidth = random.NextFloat(2f, 4f);

//            D3AirOffset = random.NextFloat(32f, 312f);
//            D3AirScale = random.NextFloat(0.05f, 0.1f);
//            D3AirThreshold = random.NextFloat(0.2f, 0.3f);


//            // 至少的高度,与地形高度加减
//            SolidGroundHeight = random.NextFloat(100f, 140f);
//            // 乘以噪声值获得高低
//            TerrainHeight = random.NextFloat(40f, 60f);
//            deepWaterPoint = random.NextFloat(-1f, -0.5f);
//            waterPoint = random.NextFloat(-0.5f, -0.1f);
//            groundPoint = random.NextFloat(0f, 0.3f);
//            mountainPoint = random.NextFloat(0.4f, 0.7f);


//            octave = random.NextInt(2, 4);
//            persistance = random.NextFloat(1f, 1.3f);

//            noiseScale = random.NextFloat(0.04f, 0.06f);
//            grassScale = random.NextFloat(1f, 2f);
//            grassThreshold = random.NextFloat(-0.5f, -0.3f);
//            redistributionModifier = random.NextFloat(1.1f, 1.2f);
//            exponent = random.NextFloat(2f, 3f);

//            octaves = 3;
//            octaveOffsetSeed = random.NextUInt();
//            nPersistance = random.NextFloat(0.4f, 0.6f);// 多梯度下高差影响度
//            lacunarity = 1f / random.NextFloat(1.5f, 2f);// 范围宽,因为噪声是乘缩放的方式,所以这个应该逆过来

//            noiseOffset = BeachWidth + TerrainHeight + noiseScale + redistributionModifier + exponent + SolidGroundHeight;
//        }
//        public static float WaterHeight(ref SeedData blobSeedData)
//        {
//            return blobSeedData.SolidGroundHeight - 2f;
//        }
//        public static BlobAssetReference<SeedData> Create(uint seed)
//        {
//            return BlobAssetReference<SeedData>.Create(new SeedData(seed));
//        }
//    }
//}
