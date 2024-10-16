using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;

namespace CatDOTS
{
    public static partial class FileRW
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 ReadUint4(this BinaryReader br)
        {
            return new uint4(br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUint4(this BinaryWriter bw, uint4 value)
        {
            bw.Write(value.x);
            bw.Write(value.y);
            bw.Write(value.z);
            bw.Write(value.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ReadFloat3(this BinaryReader br) => new float3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloat3(this BinaryWriter bw, float3 value)
        {
            bw.Write(value.x);
            bw.Write(value.y);
            bw.Write(value.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ReadFloat4(this BinaryReader br) => new float4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloat4(this BinaryWriter bw, float4 value)
        {
            bw.Write(value.x);
            bw.Write(value.y);
            bw.Write(value.z);
            bw.Write(value.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion ReadQuaternion(BinaryReader br) => new float4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuaternion(BinaryWriter bw, quaternion value)
        {
            bw.WriteFloat4(value.value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Unity.Entities.Hash128 ReadHash128(BinaryReader br) => new Unity.Entities.Hash128(br.ReadUint4());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteHash128(BinaryWriter bw, Unity.Entities.Hash128 value)
        {
            bw.WriteUint4(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LocalTransform ReadLocalTransform(BinaryReader br)
        {
            return new LocalTransform()
            {
                Position = br.ReadFloat3(),
                Rotation = br.ReadFloat4(),
                Scale = br.ReadSingle(),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLocalTransform(BinaryWriter bw, LocalTransform value)
        {
            bw.WriteFloat3(value.Position);
            bw.WriteFloat4(value.Rotation.value);
            bw.Write(value.Scale);
        }
    }
}
