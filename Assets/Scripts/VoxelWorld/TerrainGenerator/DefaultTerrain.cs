//using System.Collections;
//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Collections.LowLevel.Unsafe;
//using Unity.Entities;
//using Unity.Entities.UniversalDelegates;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;
//using Random = Unity.Mathematics.Random;

//namespace CatDOTS.VoxelWorld
//{
//    [CreateAssetMenu(fileName = "New DefaultTerrain", menuName = "ECSVoxelWorld/DefaultTerrain")]
//    public class DefaultTerrain : TerrainBaseClass
//    {
//        [SerializeField] VoxelDefinition grass;
//        [SerializeField] VoxelDefinition Top;
//        [SerializeField] VoxelDefinition TopTransition;
//        [SerializeField] VoxelDefinition Center;
//        [SerializeField] VoxelDefinition Shore;
//        [SerializeField] VoxelDefinition Bedrock;
//        [SerializeField] VoxelDefinition ChordGoldOre;
//        public Voxel GrassVoxel;
//        // 顶端一层
//        public Voxel TopVoxel;
//        // 第二层
//        public Voxel TopTransitionVoxel;
//        // 余下全部
//        public Voxel CenterVoxel;
//        // 岸边
//        public Voxel ShoreVoxel;
//        public Voxel BedrockVoxel;
//        public Voxel chordGoldOre;
//        public override void Initialize(VoxelWorldDataBaseManaged manager)
//        {
//            GrassVoxel = manager.GetVoxel(grass.VoxelName);
//            TopVoxel = manager.GetVoxel(Top.VoxelName);
//            TopTransitionVoxel = manager.GetVoxel(TopTransition.VoxelName);
//            CenterVoxel = manager.GetVoxel(Center.VoxelName);
//            ShoreVoxel = manager.GetVoxel(Shore.VoxelName);
//            BedrockVoxel = manager.GetVoxel(Bedrock.VoxelName);
//            chordGoldOre = manager.GetVoxel(ChordGoldOre.VoxelName);
//        }
//        public override JobHandle ScheduleGenerateHeightMapJob(BlobAssetReference<SeedData> seedData, BigChunkDataContainer bigChunkDataContainer)
//        {
//            int3 bigChunkPosInt = bigChunkDataContainer.BigChunkPosInt;
//            JobHandle buildHeightMapJobHandle = new TerrainHeightMapJob()
//            {
//                HeightMap = bigChunkDataContainer.HeightMap,
//                bigChunkPos = bigChunkPosInt.As2D(),
//                SeedDataAsset = seedData,
//            }.Schedule();
//            return buildHeightMapJobHandle;
//        }
//        public override JobHandle ScheduleGenerateVoxelJob(BlobAssetReference<SeedData> seedData, NativeArray<byte> HeightMap, SmallChunkDataContainer smallChunkDataContainer, JobHandle dependOn)
//        {
//            JobHandle smallChunkGenerateJobHandle = new SmallChunkGenerateJob()
//            {
//                GrassVoxel = GrassVoxel,
//                TopVoxel = TopVoxel,
//                TopTransitionVoxel = TopTransitionVoxel,
//                CenterVoxel = CenterVoxel,
//                ShoreVoxel = ShoreVoxel,
//                BedrockVoxel = BedrockVoxel,
//                chordGoldOre = chordGoldOre,
//                HeightMap = HeightMap,
//                SmallChunkVoxelMap = smallChunkDataContainer.Voxels,
//                SmallChunkPos = smallChunkDataContainer.SmallChunkPosInt,
//                SeedDataAsset = seedData,
//            }.Schedule(dependOn);
//            return smallChunkGenerateJobHandle;
//        }
//    }
//    [BurstCompile]
//    public struct TerrainHeightMapJob : IJob
//    {
//        public NativeArray<byte> HeightMap;
//        public float2 bigChunkPos;
//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public void Execute()
//        {
//            ref SeedData blobSeedData = ref SeedDataAsset.Value;
//            //float maxNoiseHeight = float.MinValue;
//            //float minNosieHeight = float.MaxValue;

//            //NativeArray<float2> octvesOffsets = new NativeArray<float2>(blobSeedData.octaves, Allocator.Temp);
//            //Random random = new Random(blobSeedData.octaveOffsetSeed);
//            //for (int i = 0; i < octvesOffsets.Length; i++)
//            //{
//            //    octvesOffsets[i] = random.NextFloat2(-100000, 100000);
//            //}
//            for (int z = 0; z < Settings.SmallChunkSize; z++)
//            {
//                for (int x = 0; x < Settings.SmallChunkSize; x++)
//                {
//                    int index = VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize);
//                    HeightMap[index]
//                        = (byte)LevelPerlin(new float2(x, z) + bigChunkPos, ref blobSeedData);

//                    //float noiseV = OctaveNoise(new float2(x, z) + bigChunkPos, ref blobSeedData);

//                    ////if (noiseV > maxNoiseHeight)
//                    ////    maxNoiseHeight = noiseV;
//                    ////else if (noiseV < minNosieHeight)
//                    ////    minNosieHeight = noiseV;
//                    //HeightMap[index] = (byte)(noiseV * blobSeedData.TerrainHeight + blobSeedData.SolidGroundHeight);
//                }
//            }
//            //maxNoiseHeight *= blobSeedData.TerrainHeight;
//            //minNosieHeight *= blobSeedData.TerrainHeight;
//            //for (int z = 0; z < Settings.SmallChunkSize; z++)
//            //{
//            //    for (int x = 0; x < Settings.SmallChunkSize; x++)
//            //    {
//            //        int index = VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize);
//            //        HeightMap[index]=math.le
//            //    }
//            //}
//        }
//        public static float GetSuffaceHeightNoise(float2 position, ref SeedData blobSeedData)
//        {
//            float terrainHeight = LevelPerlin(position, ref blobSeedData);
//            return terrainHeight;
//        }
//        public static float LevelPerlin(float2 position, ref SeedData blobSeedData)
//        {
//            float finallyValue;
//            finallyValue = OctavePerlin(position, ref blobSeedData);

//            finallyValue = Pow(finallyValue * blobSeedData.persistance);
//            float rangeValue = Noise.GetPerlinNoise(position, blobSeedData.rangeOffset, blobSeedData.rangeScale);
//            finallyValue += rangeValue;
//            static float Pow(float v)
//            {
//                return v * v * (v < 0f ? -1f : 1f);
//            }
//            return finallyValue * blobSeedData.TerrainHeight + blobSeedData.SolidGroundHeight;
//        }
//        public static float OctavePerlin(float2 position, ref SeedData blobSeedData)
//        {
//            float offset = blobSeedData.noiseOffset;
//            float persistance = blobSeedData.persistance;
//            float total = 0f;
//            float frequency = blobSeedData.noiseScale;
//            float amplitude = 1f;
//            float amplitudeSum = 0;
//            for (int i = 0; i < 3; i++)
//            {
//                total += Noise.GetPerlinNoise(position, offset, frequency) * amplitude;
//                amplitudeSum += amplitude;
//                amplitude *= persistance;
//                frequency *= 0.5f;
//            }
//            return total / amplitudeSum;
//        }
//        public static float LevelNoise(float2 position, ref SeedData blobSeedData)
//        {
//            float finallyValue;
//            finallyValue = OctaveNoise(position, ref blobSeedData);

//            //finallyValue = Pow(finallyValue * blobSeedData.persistance);
//            float rangeValue = Noise.GetPerlinNoise(position, blobSeedData.rangeOffset, blobSeedData.rangeScale);
//            finallyValue += rangeValue;
//            //static float Pow(float v)
//            //{
//            //    return v * v * (v < 0f ? -1f : 1f);
//            //}
//            return finallyValue * blobSeedData.TerrainHeight + blobSeedData.SolidGroundHeight;
//        }
//        public static float OctaveNoise(float2 position, ref SeedData blobSeedData)
//        {
//            float offset = blobSeedData.noiseOffset;
//            float octaves = blobSeedData.octaves;
//            float persistance = blobSeedData.nPersistance;
//            float laucunarity = blobSeedData.lacunarity;

//            float frequency = 1f;
//            float amplitude = 1f;
//            float amplitudeSum = 0f;
//            float total = 0f;
//            position *= blobSeedData.noiseScale;
//            for (int i = 0; i < octaves; i++)
//            {
//                total += Noise.GetPerlinNoise(position, offset, frequency) * amplitude;
//                amplitudeSum += amplitude;
//                amplitude *= persistance;
//                frequency *= laucunarity;
//            }
//            return total / amplitudeSum;
//        }
//    }
//    public struct DefaultTerrainHeightMapJob : IJob
//    {
//        public NativeArray<byte> HeightMap;
//        public float2 bigChunkPos;
//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public void Execute()
//        {
//            ref SeedData blobSeedData = ref SeedDataAsset.Value;
//            for (int z = 0; z < Settings.SmallChunkSize; z++)
//            {
//                for (int x = 0; x < Settings.SmallChunkSize; x++)
//                {
//                    int index = VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize);
//                    HeightMap[index]
//                        = (byte)(GetSuffaceHeightNoise(new float2(x, z) + bigChunkPos, ref blobSeedData)/* + SolidGroundHeight*/);
//                }
//            }
//            // 计算一个最大最小值,然后可以归一
//            // 噪声值除以最大值
//        }
//        float GetSuffaceHeightNoise(float2 position, ref SeedData blobSeedData)
//        {
//            //float noiseOffset = blobSeedData.noiseOffset;
//            //float noiseScale = blobSeedData.noiseScale;
//            //float persistance = blobSeedData.persistance;
//            //float redistributionModifier = blobSeedData.redistributionModifier;
//            //float exponent = blobSeedData.exponent;
//            float terrainHeight /*= Noise.OctavePerlin(position, noiseOffset, noiseScale, persistance); */;
//            //terrainHeight = MultiPerlin(position, ref blobSeedData);
//            terrainHeight = LevelPerlin(position, ref blobSeedData);
//            //terrainHeight = VoxelMath.Redistribution(terrainHeight, redistributionModifier, exponent);
//            //float smooth = math.pow(Noise.GetPerlinNoise(position, noiseOffset, noiseScale), octave);
//            //terrainHeight = Noise.Sample(terrainHeight, 0.2f, 0.3f, 0.6f, 0.7f);
//            return terrainHeight;
//        }
//        //public static float MultiPerlin(float2 position, ref BlobSeedData blobSeedData)
//        //{
//        //    float offset = blobSeedData.noiseOffset;
//        //    float total;
//        //    //total = OctavePerlin(position, ref blobSeedData);
//        //    //total = math.pow(total, 3f);
//        //    float rangeNoise = Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale * blobSeedData.groundScale);
//        //    //total = Noise.GetPerlinNoise(position, offset, math.abs(rangeNoise * blobSeedData.noiseScale));
//        //    //total = math.lerp(rangeNoise, total, math.abs(rangeNoise));
//        //    //rangeNoise = math.log(rangeNoise);
//        //    //rangeNoise = Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale) + math.pow((rangeNoise < 0 ? rangeNoise + 1 : rangeNoise) * blobSeedData.persistance, blobSeedData.exponent);

//        //    //total = rangeNoise;
//        //    //total = (total * (1f + rangeNoise));

//        //    //total = (total * 64f + rangeNoise * 128f) * 0.0052f;
//        //    //float groundHeight = blobSeedData.SolidGroudHeight;
//        //    //float waterPoint1 = blobSeedData.WaterPoint1;
//        //    //float waterPoint2 = blobSeedData.WaterPoint2;
//        //    //float groundPoint1 = blobSeedData.GroundPoint1;
//        //    //float groundPoint2 = blobSeedData.GroundPoint2;
//        //    //float mountainPoint1 = blobSeedData.MountainPoint1;
//        //    //float center;
//        //    //float range;

//        //    if (rangeNoise < 0f)
//        //    {
//        //        float p = rangeNoise + 1f;// 0到1
//        //        p *= p;
//        //        //rangeNoise = math.lerp(-1f, rangeNoise, p);
//        //        rangeNoise = p - 1f;
//        //    }
//        //    else
//        //    {
//        //        float p = rangeNoise;// 0到1
//        //        p *= p;
//        //        rangeNoise = p;
//        //        //rangeNoise = math.lerp(0f, rangeNoise, p);
//        //    }
//        //    //rangeNoise = -rangeNoise;
//        //    float absRangeNoise = /*(rangeNoise + 1f) * 0.5f;*/math.abs(rangeNoise) + 0.05f;
//        //    //float mountainNoise = math.pow(Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale) + 1f, 3f);
//        //    //rangeNoise = mountainNoise;
//        //    //total = OctavePerlin(position, ref blobSeedData);
//        //    total = Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale * blobSeedData.r1) * blobSeedData.r3;
//        //    total = math.lerp(0f, math.abs(total), absRangeNoise) * (total > 0f ? 1f : -1f);
//        //    total += Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale * blobSeedData.r2) * blobSeedData.r2;
//        //    total = math.lerp(0f, math.abs(total), absRangeNoise) * (total > 0f ? 1f : -1f);
//        //    total += Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale * blobSeedData.r3) * blobSeedData.r1;
//        //    total = math.lerp(0f, math.abs(total), absRangeNoise) * (total > 0f ? 1f : -1f);
//        //    //total = ;
//        //    rangeNoise += total + 0.05f;
//        //    //if (rangeNoise < 0f)
//        //    //{
//        //    //    float p = rangeNoise + 1f;
//        //    //    p *= p * p;
//        //    //    rangeNoise = math.lerp(-1f, total, p);
//        //    //}
//        //    //else
//        //    //{
//        //    //    float p = rangeNoise;
//        //    //    p *= p * p;
//        //    //    rangeNoise = math.lerp(0f, total, p);
//        //    //}
//        //    //if (/*rangeNoise > waterPoint1 && */rangeNoise < 0f)
//        //    //{
//        //    //    float p = rangeNoise + 1f;
//        //    //    p *= p * p;
//        //    //    rangeNoise = math.lerp(-1f, rangeNoise, p);
//        //    //    //center = (waterPoint1 + waterPoint2) * 0.5f;
//        //    //    //range = waterPoint2;
//        //    //    //total = math.lerp(rangeNoise, total, math.abs(rangeNoise - center) / (range - center));
//        //    //    //total = (waterPoint1 + waterPoint2) * 0.5f;
//        //    //}
//        //    //else if (rangeNoise > 0f/* && rangeNoise < 1f*/)
//        //    //{
//        //    //    float p = rangeNoise * rangeNoise * rangeNoise;
//        //    //    rangeNoise = math.lerp(0f, rangeNoise,/* math.pow(math.remap(0f, 1f, 0, 1, rangeNoise), 3f)*/p);
//        //    //    //center = (groundPoint1 + groundPoint2) * 0.5f;
//        //    //    //range = groundPoint2;
//        //    //    //total = math.lerp(rangeNoise, total, math.abs(rangeNoise - center) / (range - center));
//        //    //    //total = (groundPoint1 + groundPoint2) * 0.5f;
//        //    //}
//        //    //else
//        //    //{
//        //    //    //total = OctavePerlin(position, ref blobSeedData);
//        //    //}
//        //    total = rangeNoise;
//        //    //total = math.pow(total, 3);
//        //    //float absRange = rangeNoise * 0.5f + 0.5f;
//        //    //total = math.lerp(rangeNoise, total, absRange);
//        //    //total = rangeNoise > total ? rangeNoise : total;
//        //    float terrainHeight = total * blobSeedData.TerrainHeight + blobSeedData.SolidGroundHeight;
//        //    return terrainHeight;
//        //}
//        public static float LevelPerlin(float2 position, ref SeedData blobSeedData)
//        {
//            float finallyValue = 0;
//            //if (MountainLevel(position, ref blobSeedData))
//            //{
//            //    finallyValue = blobSeedData.mountainPoint;
//            //}
//            //if (DeepWaterLevel(position, ref blobSeedData))
//            //{
//            //    finallyValue = blobSeedData.deepWaterPoint;
//            //}
//            //else if (WaterLevel(position, ref blobSeedData))
//            //{
//            //    finallyValue = blobSeedData.waterPoint;
//            //}
//            //else if (GroundLevel(position, ref blobSeedData))
//            //{
//            //    finallyValue = blobSeedData.groundPoint;
//            //}
//            finallyValue = OctavePerlin(position, ref blobSeedData);

//            //finallyValue = math.pow(finallyValue * blobSeedData.redistributionModifier, blobSeedData.exponent);

//            finallyValue = Pow(finallyValue * blobSeedData.persistance);
//            //finallyValue = finallyValue * blobSeedData.persistance * math.abs(rangeValue);
//            float rangeValue = Noise.GetPerlinNoise(position, blobSeedData.rangeOffset, blobSeedData.rangeScale);
//            ////finallyValue += math.lerp(0f, finallyValue, math.abs(rangeValue));
//            finallyValue += rangeValue;
//            //finallyValue *= 0.5f;
//            //rangeValue = Noise.GetPerlinNoise(position, blobSeedData.rangeOffset, blobSeedData.rangeScale * 2f);
//            //finallyValue *= Pow(rangeValue);
//            static float Pow(float v)
//            {
//                return v * v * (v < 0f ? -1f : 1f);
//            }
//            //rangeValue = Noise.GetPerlinNoise(position, blobSeedData.rangeOffset, blobSeedData.rangeScale * 8f);
//            //finallyValue = finallyValue * blobSeedData.persistance * math.abs(rangeValue);
//            //finallyValue = Lerp(finallyValue, blobSeedData.deepWaterPoint, rangeValue);
//            //finallyValue = Lerp(finallyValue, blobSeedData.waterPoint, rangeValue);
//            //finallyValue = Lerp(finallyValue, blobSeedData.groundPoint, rangeValue);
//            //finallyValue = Lerp(finallyValue, blobSeedData.mountainPoint, rangeValue);
//            //static float Lerp(float v, float p, float r)
//            //{
//            //    return math.lerp(math.abs(v), math.abs(p), math.abs(r - p)) * (v < 0f ? -1f : 1f);
//            //}
//            //if (rangeValue < 0f)
//            //{
//            //    float p = rangeValue + 1f;
//            //    //p *= p;
//            //    finallyValue = math.lerp(-1f, finallyValue, p);
//            //}
//            //else
//            //{
//            //    float p = rangeValue;
//            //    //p *= p;
//            //    finallyValue = math.lerp(0f, finallyValue, p);
//            //}
//            ////finallyValue = (rangeValue + finallyValue) * 0.5f;
//            ////finallyValue = math.lerp(math.abs(finallyValue), math.abs(rangeValue), math.abs(finallyValue - rangeValue)) * (finallyValue + rangeValue);

//            //finallyValue = math.lerp(rangeValue, OctavePerlin(position, ref blobSeedData), math.abs(rangeValue));
//            //finallyValue = math.lerp(finallyValue, OctavePerlin(position, ref blobSeedData), math.abs(rangeValue));
//            //finallyValue = (rangeValue + finallyValue) * 0.5f;
//            return finallyValue * blobSeedData.TerrainHeight + blobSeedData.SolidGroundHeight;
//        }
//        static float ToZ(float v)
//        {
//            return (v + 1f) * 0.5f;
//        }
//        public static bool DeepWaterLevel(float2 position, ref SeedData blobSeedData)
//        {
//            float deepWaterValue = Noise.GetPerlinNoise(position, blobSeedData.deepWaterOffset, blobSeedData.deepWaterScale);
//            return (deepWaterValue > 0.8f);
//        }
//        public static bool WaterLevel(float2 position, ref SeedData blobSeedData)
//        {
//            float waterValue = Noise.GetPerlinNoise(position, blobSeedData.waterOffset, blobSeedData.waterScale);
//            return waterValue > -0.1f && waterValue < 0.1f;
//        }
//        public static bool GroundLevel(float2 position, ref SeedData blobSeedData)
//        {
//            float value = Noise.GetPerlinNoise(position, blobSeedData.groundOffset, blobSeedData.groundScale);
//            return value < 0.5f;
//        }
//        public static bool MountainLevel(float2 position, ref SeedData blobSeedData)
//        {
//            float value = Noise.GetPerlinNoise(position, blobSeedData.mountainOffset, blobSeedData.mountainScale);
//            return value > -0.2f && value < 0.2f;
//        }
//        public static float OctavePerlin(float2 position, ref SeedData blobSeedData)
//        {
//            //float scale = blobSeedData.noiseScale;
//            //float offset = blobSeedData.noiseOffset;
//            //float total = 0;
//            //float amplitudeSum;
//            //amplitudeSum = blobSeedData.top;
//            //total += Noise.GetPerlinNoise(position, offset, blobSeedData.r1 * scale) * blobSeedData.top;
//            //amplitudeSum += blobSeedData.p * blobSeedData.top;
//            //total += Noise.GetPerlinNoise(position, offset, blobSeedData.r2 * scale) * blobSeedData.top * blobSeedData.p;
//            //amplitudeSum += blobSeedData.p * blobSeedData.p * blobSeedData.top;
//            //total += Noise.GetPerlinNoise(position, offset, blobSeedData.r3 * scale) * blobSeedData.top * blobSeedData.p * blobSeedData.p;
//            //total /= amplitudeSum;
//            //total = (math.pow(math.abs(total) * blobSeedData.persistance, blobSeedData.exponent)) * (total > 0f ? 1f : -1f);
//            //return total;
//            //float scale = blobSeedData.noiseScale;
//            float offset = blobSeedData.noiseOffset;
//            float persistance = blobSeedData.persistance;
//            //int octave = blobSeedData.octave;
//            //position.x *= scale;
//            //position.y *= scale;
//            //position.x += scale;
//            //position.y += scale;
//            float total = 0f;
//            float frequency = blobSeedData.noiseScale;
//            float amplitude = 1f;
//            float amplitudeSum = 0;
//            for (int i = 0; i < 3; i++)
//            {
//                total += Noise.GetPerlinNoise(position, offset, frequency) * amplitude;
//                amplitudeSum += amplitude;
//                amplitude *= persistance;
//                frequency *= 0.5f;
//            }
//            //for (int i = 0; i < 3; i++)
//            //{
//            //    frequency *= blobSeedData.p;
//            //    total *= Noise.GetPerlinNoise(position, offset, frequency) * amplitude;
//            //    //amplitudeSum += amplitude;
//            //    amplitude *= persistance;
//            //}
//            return total / amplitudeSum;
//        }
//        static float Remap(float v)
//        {
//            return math.remap(-1f, 1f, 0f, 0.2f, v);
//        }
//        // 噪声圈,一圈又一圈
//        //public static float MultiPerlin(float2 position, ref BlobSeedData blobSeedData)
//        //{
//        //    float offset = blobSeedData.noiseOffset;
//        //    float total = 0f;
//        //    float rangeNoise = Noise.GetPerlinNoise(position, offset, blobSeedData.noiseScale);
//        //    total = Noise.GetPerlinNoise(position, offset, math.abs(rangeNoise * blobSeedData.noiseScale));
//        //    float terrainHeight = total * blobSeedData.TerrainHeight + blobSeedData.SolidGroudHeight;
//        //    return terrainHeight;
//        //}
//        // 分级山柱
//        //public static float MultiPerlin(float2 position, ref BlobSeedData blobSeedData)
//        //{
//        //    float scale = blobSeedData.noiseScale;
//        //    float offset = blobSeedData.noiseOffset;
//        //    float total = Noise.OctavePerlin(position, offset, scale, blobSeedData.persistance);
//        //    total = math.pow(total, 3f);
//        //    float rangeNoise = Noise.GetPerlinNoise(position, offset, scale * 4f);
//        //    float groundHeight = blobSeedData.SolidGroudHeight;
//        //    float waterPoint1 = blobSeedData.WaterPoint1;
//        //    float waterPoint2 = blobSeedData.WaterPoint2;
//        //    float groundPoint1 = blobSeedData.GroundPoint1;
//        //    float groundPoint2 = blobSeedData.GroundPoint2;
//        //    float mountainPoint1 = blobSeedData.MountainPoint1;
//        //    float center;
//        //    float range;
//        //    if (rangeNoise > waterPoint1 && rangeNoise < waterPoint2)
//        //    {
//        //        center = (waterPoint1 + waterPoint2) * 0.5f;
//        //        range = waterPoint2;
//        //    }
//        //    else if (rangeNoise > groundPoint1 && rangeNoise < groundPoint2)
//        //    {
//        //        center = (groundPoint1 + groundPoint2) * 0.5f;
//        //        range = groundPoint2;
//        //    }
//        //    else
//        //    {
//        //        range = mountainPoint1;
//        //        center = mountainPoint1 - 0.3f;
//        //    }
//        //    total = math.lerp(rangeNoise, (total + rangeNoise) * 0.5f, math.abs(rangeNoise - center) / (range - center));
//        //    //total = math.pow(total, 3);
//        //    float terrainHeight = total * blobSeedData.TerrainHeight + blobSeedData.SolidGroudHeight;
//        //    return terrainHeight;
//        //}
//        // 升片山脉
//        //public static float MultiPerlin(float2 position, ref BlobSeedData blobSeedData)
//        //{
//        //    float scale = blobSeedData.noiseScale;
//        //    float offset = blobSeedData.noiseOffset;
//        //    //position.x *= scale;
//        //    //position.y *= scale;
//        //    //position.x += scale;
//        //    //position.y += scale;
//        //    //float total = 0;
//        //    //float frequency = 2;
//        //    //for (int i = 0; i < 3; i++)
//        //    //{
//        //    //    total += Noise.GetPerlinNoise(position, offset, frequency);
//        //    //    frequency *= 2;
//        //    //}
//        //    //// 范围-3到3,乘以0.333归到-1到1
//        //    //total *= 0.333f;
//        //    float total = Noise.OctavePerlin(position, offset, scale, blobSeedData.persistance);
//        //    //total = math.pow(total, 3f);
//        //    float rangeNoise = Noise.GetPerlinNoise(position, offset, scale * 4f);
//        //    float groundHeight = blobSeedData.SolidGroudHeight;
//        //    float waterPoint1 = blobSeedData.WaterPoint1;
//        //    float waterPoint2 = blobSeedData.WaterPoint2;
//        //    float groundPoint1 = blobSeedData.GroundPoint1;
//        //    float groundPoint2 = blobSeedData.GroundPoint2;
//        //    float mountainPoint1 = blobSeedData.MountainPoint1;
//        //    float center;
//        //    float range;
//        //    if (rangeNoise > waterPoint1 && rangeNoise < waterPoint2)
//        //    {
//        //        center = (waterPoint1 + waterPoint2) * 0.5f;
//        //        range = waterPoint2;
//        //    }
//        //    else if (rangeNoise > groundPoint1 && rangeNoise < groundPoint2)
//        //    {
//        //        center = (groundPoint1 + groundPoint2) * 0.5f;
//        //        range = groundPoint2;
//        //    }
//        //    else
//        //    {
//        //        range = mountainPoint1;
//        //        center = mountainPoint1 - 0.3f;
//        //    }
//        //    total = math.lerp(rangeNoise, (total + rangeNoise) * 0.8f, math.abs(rangeNoise - center) / (range - center));
//        //    //total = math.pow(total, 3);
//        //    float terrainHeight = total * blobSeedData.TerrainHeight + blobSeedData.SolidGroudHeight;
//        //    return terrainHeight;
//        //}
//    }
//    public struct SmallChunkVoxelPlaceholdJob : IJobParallelForBatch
//    {
//        [ReadOnly] public NativeArray<byte> HeightMap;
//        public NativeSlice<bool> VoxelPlaceholder;
//        public float3 ChunkPos;
//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public static void Execute(NativeSlice<bool> voxelPlaceholder, float3 smallChunkPos, NativeArray<byte> HeightMap, ref SeedData blobSeedData)
//        {
//            int x = 0, y = 0, z = 0;
//            for (int i = 0; i < Settings.VoxelCapacityInSmallChunk; i++)
//            {
//                float3 voxelPosInWorld = new float3(x, y, z) + smallChunkPos;

//                voxelPlaceholder[VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, z)] =
//                         voxelPosInWorld.y <= HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)]
//                         /*&& (math.abs(Noise.Get3DPerlinNoise(voxelPosInWorld, blobSeedData.D3AirOffset, blobSeedData.D3AirScale)) < blobSeedData.D3AirThreshold)*/;
//                z++;
//                if (z == Settings.SmallChunkSize)
//                {
//                    z = 0;
//                    x++;
//                    if (x == Settings.SmallChunkSize)
//                    {
//                        x = 0;
//                        y++;
//                    }
//                }
//            }
//        }

//        public void Execute(int startIndex, int count)
//        {
//            int chunkYPos = startIndex / Settings.VoxelCapacityInSmallChunk * Settings.SmallChunkSize;
//            Execute(VoxelPlaceholder.Slice(startIndex, count), new float3(ChunkPos.x, chunkYPos, ChunkPos.z), HeightMap, ref SeedDataAsset.Value);
//        }
//    }
//    [BurstCompile]
//    public struct SmallChunkGenerateJob : IJob
//    {
//        public Voxel GrassVoxel;
//        // 顶端一层
//        public Voxel TopVoxel;
//        // 第二层
//        public Voxel TopTransitionVoxel;
//        // 余下全部
//        public Voxel CenterVoxel;
//        // 岸边
//        public Voxel ShoreVoxel;
//        public Voxel BedrockVoxel;

//        public Voxel chordGoldOre;

//        [ReadOnly] public NativeArray<byte> HeightMap;
//        public NativeArray<Voxel> SmallChunkVoxelMap;
//        public int3 SmallChunkPos;

//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public void Execute()
//        {
//            ref SeedData blobSeedData = ref SeedDataAsset.Value;
//            float waterHeight = SeedData.WaterHeight(ref blobSeedData);

//            for (int y = 0; y < Settings.SmallChunkSize; y++)
//            {
//                for (int x = 0; x < Settings.SmallChunkSize; x++)
//                {
//                    for (int z = 0; z < Settings.SmallChunkSize; z++)
//                    {
//                        SmallChunkVoxelMap[VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, z)] = GetVoxelID(new int3(x, y, z) + SmallChunkPos, HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)], waterHeight, ref blobSeedData);
//                    }
//                }
//            }
//            // 与下面的另一种循环方式没有看出速度差异


//        }
//        readonly Voxel GetVoxelID(int3 voxelIndexInWorld, byte terrainHeight, float waterHeight, ref SeedData blobSeedData)
//        {
//            float noiseOffset = blobSeedData.noiseOffset;
//            float noiseScale = blobSeedData.noiseScale;
//            int y = voxelIndexInWorld.y;
//            if (y == 0)
//                return BedrockVoxel;

//            Voxel voxel = Voxel.Empty; //默认空气
//            if (y > terrainHeight)
//            {
//                if (y == terrainHeight + 1 && terrainHeight > waterHeight)
//                {
//                    if (Noise.GetSNoise(voxelIndexInWorld.xz, noiseOffset, blobSeedData.grassScale) > blobSeedData.grassThreshold)
//                    {
//                        voxel = GrassVoxel;
//                    }
//                }
//            }
//            else
//            {
//                if (y == terrainHeight)
//                {
//                    if (y < waterHeight)// 在水的高度
//                    {
//                        voxel = ShoreVoxel;// 放置岸边体素
//                    }
//                    else
//                    {
//                        voxel = TopVoxel;
//                    }
//                }
//                else if (y < terrainHeight - 4)
//                {
//                    if (Noise.Get3DPerlinNoise(voxelIndexInWorld, noiseOffset, noiseScale) < 0.4f)
//                    {
//                        voxel = chordGoldOre;
//                    }
//                    else voxel = CenterVoxel;
//                }
//                else if (y < terrainHeight)
//                {
//                    voxel = TopTransitionVoxel;
//                }
//            }
//            if (y <= waterHeight)
//            {
//                voxel.VoxelMaterial |= VoxelMaterial.Water;
//            }
//            return voxel;
//        }
//    }
//    public struct SmallChunkVoxelGenerateJob : IJobParallelForBatch
//    {
//        public Voxel GrassVoxel;
//        // 顶端一层
//        public Voxel TopVoxel;
//        // 第二层
//        public Voxel TopTransitionVoxel;
//        // 余下全部
//        public Voxel CenterVoxel;
//        // 岸边
//        public Voxel ShoreVoxel;
//        public Voxel BedrockVoxel;

//        public Voxel chordGoldOre;

//        [ReadOnly] public NativeArray<byte> HeightMap;
//        public NativeSlice<Voxel> ChunkVoxelMap;
//        public int3 bigChunkPosInt;

//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public void Execute(NativeSlice<Voxel> smallChunkVoxelMap, int3 smallChunkPos)
//        {
//            ref SeedData blobSeedData = ref SeedDataAsset.Value;
//            float waterHeight = SeedData.WaterHeight(ref blobSeedData);
//            // 与下面的另一种循环方式没有看出速度差异
//            for (int x = 0; x < Settings.SmallChunkSize; x++)
//            {
//                for (int z = 0; z < Settings.SmallChunkSize; z++)
//                {
//                    byte height = HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)];
//                    for (int y = 0; y < Settings.SmallChunkSize; y++)
//                    {
//                        smallChunkVoxelMap[VoxelMath.LocalVoxelArrayIndexInSmallChunk(x, y, z)] = GetVoxelID(new int3(x, y, z) + smallChunkPos, height, waterHeight, ref blobSeedData);
//                    }
//                }
//            }
//            //int x = 0, y = 0, z = 0;
//            //for (int i = 0; i < Settings.VoxelCapacityInSmallChunk; i++)
//            //{
//            //    smallChunkVoxelMap[i] =
//            //        GetVoxelID(new int3(x, y, z) + smallChunkPos, HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)], waterHeight, ref blobSeedData);
//            //    z++;
//            //    if (z == Settings.SmallChunkSize)
//            //    {
//            //        z = 0;
//            //        x++;
//            //        if (x == Settings.SmallChunkSize)
//            //        {
//            //            x = 0;
//            //            y++;
//            //        }
//            //    }
//            //}
//        }

//        public void Execute(int startIndex, int count)
//        {
//            int chunkYPos = startIndex / Settings.VoxelCapacityInSmallChunk * Settings.SmallChunkSize;

//            Execute(ChunkVoxelMap.Slice(startIndex, count), new int3(bigChunkPosInt.x, chunkYPos, bigChunkPosInt.z));
//        }

//        readonly Voxel GetVoxelID(int3 voxelIndexInWorld, byte terrainHeight, float waterHeight, ref SeedData blobSeedData)
//        {
//            float noiseOffset = blobSeedData.noiseOffset;
//            float noiseScale = blobSeedData.noiseScale;
//            int y = voxelIndexInWorld.y;
//            if (y == 0)
//                return BedrockVoxel;

//            Voxel voxel = Voxel.Empty; //默认空气
//            if (y > terrainHeight)
//            {
//                if (y == terrainHeight + 1 && terrainHeight > waterHeight)
//                {
//                    if (Noise.GetSNoise(voxelIndexInWorld.xz, noiseOffset, noiseScale) < 0.4f)
//                    {
//                        voxel = GrassVoxel;
//                    }
//                }
//            }
//            else
//            {
//                if (y == terrainHeight)
//                {
//                    if (y < waterHeight)// 在水的高度
//                    {
//                        voxel = ShoreVoxel;// 放置岸边体素
//                    }
//                    else
//                    {
//                        voxel = TopVoxel;
//                    }
//                }
//                else if (y < terrainHeight - 4)
//                {
//                    if (Noise.Get3DPerlinNoise(voxelIndexInWorld, noiseOffset, noiseScale) < 0.4f)
//                    {
//                        voxel = chordGoldOre;
//                    }
//                    else voxel = CenterVoxel;
//                }
//                else if (y < terrainHeight)
//                {
//                    voxel = TopTransitionVoxel;
//                }
//            }
//            if (y <= waterHeight)
//            {
//                voxel.VoxelMaterial |= VoxelMaterial.Water;
//            }
//            return voxel;
//        }
//    }
//    public struct DefaultTerrainGenerateJob : IJobParallelForBatch
//    {
//        public Voxel GrassVoxel;
//        // 顶端一层
//        public Voxel TopVoxel;
//        // 第二层
//        public Voxel TopTransitionVoxel;
//        // 余下全部
//        public Voxel CenterVoxel;
//        // 岸边
//        public Voxel ShoreVoxel;
//        public Voxel BedrockVoxel;

//        public Voxel chordGoldOre;

//        [ReadOnly] public NativeArray<byte> HeightMap;
//        public VoxelWorldMap.BigChunkSlice Slice;

//        public BlobAssetReference<SeedData> SeedDataAsset;
//        public void Execute(int startIndex, int count)
//        {
//            ref SeedData blobSeedData = ref SeedDataAsset.Value;
//            float waterHeight = SeedData.WaterHeight(ref blobSeedData);
//            int end = startIndex + count;
//            int3 bigChunkPos = Slice.BigChunkPosInt;
//            NativeSlice<Voxel> VoxelMap = Slice.VoxelMap;
//            // 比如开始索引为8192，数量为4096，则除后为2，乘以小区块大小，为最终高度
//            int x = 0, y = startIndex / count * Settings.SmallChunkSize, z = 0;// 高度已经偏移到目标小区块
//            for (int i = startIndex; i < end; i++)
//            {
//                VoxelMap[VoxelMath.LocalVoxelArrayIndexInBigChunk(x, y, z)]
//                    = GetVoxelID(bigChunkPos + new int3(x, y, z), HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)], waterHeight, ref blobSeedData);
//                z++;
//                if (z == Settings.SmallChunkSize)
//                {
//                    z = 0;
//                    x++;
//                    if (x == Settings.SmallChunkSize)
//                    {
//                        x = 0;
//                        y++;
//                    }
//                }
//            }
//            // 比如开始索引为8192，数量为4096，则除后为2，乘以小区块大小，为最终高度
//            // 高度已经偏移到目标小区块
//            //int y = startIndex / count * Settings.SmallChunkSize;
//            //int yEnd = y + Settings.SmallChunkSize;
//            //for (; y != yEnd; y++)
//            //{
//            //    for (int x = 0; x != Settings.SmallChunkSize; x++)
//            //    {
//            //        for ( int z = 0; z != Settings.SmallChunkSize; z++)
//            //        {
//            //            int3 voxelPos = new int3(x, y, z);
//            //            VoxelMap[VoxelMath.LocalVoxelArrayIndexInBigChunk(voxelPos)]
//            //                = GetVoxelID(bigChunkPos + new int3(x, y, z), HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)]);
//            //        }
//            //    }
//            //}

//            //for (int i = startIndex; i < end; i++)
//            //{
//            //    int3 voxelPos = new int3(x, y, z);
//            //    VoxelMap[VoxelMath.LocalVoxelArrayIndexInBigChunk(voxelPos)]
//            //        = GetVoxelID(bigChunkPos + new int3(x, y, z), HeightMap[VoxelMath.D2IndexToIndex(x, z, Settings.SmallChunkSize)]);
//            //    z++;
//            //    if (z == Settings.SmallChunkSize)
//            //    {
//            //        z = 0;
//            //        x++;
//            //        if (x == Settings.SmallChunkSize)
//            //        {
//            //            x = 0;
//            //            y++;
//            //        }
//            //    }
//            //}
//        }
//        // 完全的int3是用在需要3D噪声的时候，但这里没有
//        readonly Voxel GetVoxelID(int3 voxelIndexInWorld, byte terrainHeight, float waterHeight, ref SeedData blobSeedData)
//        {
//            float noiseOffset = blobSeedData.noiseOffset;
//            float noiseScale = blobSeedData.noiseScale;
//            int y = voxelIndexInWorld.y;
//            if (y == 0)
//                return BedrockVoxel;

//            Voxel voxel = Voxel.Empty; //默认空气
//            /*(y > terrainHeight - BeachWidth && y < terrainHeight + BeachWidth)*/
//            if (y > terrainHeight)
//            {
//                if (y == terrainHeight + 1 && terrainHeight > waterHeight)
//                {
//                    if (Noise.GetPerlinNoise(voxelIndexInWorld.xz, noiseOffset, noiseScale) < 0.4f)
//                    {
//                        voxel = GrassVoxel;
//                    }
//                }
//            }
//            else
//            {
//                if (y == terrainHeight)
//                {
//                    if (y < waterHeight)// 在水的高度
//                    {
//                        voxel = ShoreVoxel;// 放置岸边体素
//                    }
//                    else
//                    {
//                        voxel = TopVoxel;
//                    }
//                }
//                else if (y < terrainHeight - 4)
//                {
//                    if (Noise.Get3DPerlinNoise(voxelIndexInWorld, noiseOffset, noiseScale) < 0.4f)
//                    {
//                        voxel = chordGoldOre;
//                    }
//                    else voxel = CenterVoxel;
//                }
//                else if (y < terrainHeight)
//                {
//                    voxel = TopTransitionVoxel;
//                }
//            }

//            //if (voxelID == 2)// 修改石头区域生成矿物
//            //{
//            //    foreach (Lode lode in m_Lodes)
//            //    {
//            //        if (y > lode.MinHeight && y < lode.MaxHeight)
//            //        {
//            //            if (Noise.Get3DPerlinNoise(pos, lode.NoiseOffset, lode.Scale) < lode.Threshold)
//            //            {
//            //                voxelID = lode.BlockID;
//            //                break;
//            //            }
//            //        }
//            //    }
//            //}

//            return voxel;
//        }
//    }
//}