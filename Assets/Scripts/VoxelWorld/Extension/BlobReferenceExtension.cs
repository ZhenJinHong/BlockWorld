using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public static class NativeContainerExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RemoveLast<T>(ref this NativeList<T> list) where T : unmanaged
        {
            T v = list[^1];
            list.RemoveAt(list.Length - 1);
            return v;
        }
    }
    public static class BlobReferenceExtension
    {
        public static void GetVoxelToBlobArray<V>(BlobBuilder blobBuilder, ref BlobArray<Voxel> array, V[] defs, Func<V, Voxel> converter)
        {
            if (defs == null || defs.Length == 0)
            {
                defs = new V[1];
            }
            BlobBuilderArray<Voxel> builder = blobBuilder.Allocate(ref array, defs.Length);
            for (int i = 0; i < defs.Length; i++)
            {
                builder[i] = converter.Invoke(defs[i]);
            }
        }
        public static void VoxelArrayToBlobArray(BlobBuilder blobBuilder, ref BlobArray<Voxel> array, Voxel[] voxels)
        {
            if (voxels == null || voxels.Length == 0)
            {
                voxels = new Voxel[1] { Voxel.Empty };
            }
            BlobBuilderArray<Voxel> builder = blobBuilder.Allocate(ref array, voxels.Length);
            for (int i = 0; i < voxels.Length; i++)
            {
                builder[i] = voxels[i];
            }
        }
        public static void CopyToBlobArray<T>(ref this BlobBuilder builder, ref BlobArray<T> target, IList<T> original) where T : unmanaged
        {
            BlobBuilderArray<T> array = builder.Allocate<T>(ref target, original.Count);
            for (int i = 0; i < original.Count; i++)
            {
                array[i] = original[i];
            }
        }
        public static void CopyToBlobArray<T>(ref this BlobBuilder builder, ref BlobArray<T> target, ref NativeList<T> original) where T : unmanaged
        {
            BlobBuilderArray<T> array = builder.Allocate<T>(ref target, original.Length);
            for (int i = 0; i < original.Length; i++)
            {
                array[i] = original[i];
            }
        }
        public static void CopyToBlobArray<T>(ref this BlobBuilder builder, ref BlobArray<T> target, ref NativeArray<T> original) where T : unmanaged
        {
            BlobBuilderArray<T> array = builder.Allocate<T>(ref target, original.Length);
            for (int i = 0; i < original.Length; i++)
            {
                array[i] = original[i];
            }
        }
    }
}
