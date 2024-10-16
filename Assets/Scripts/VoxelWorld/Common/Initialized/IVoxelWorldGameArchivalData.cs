using CatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelWorldGameArchivalData
    {
        bool IsValid { get; }
        Archival Archival { get; }
        IWorldGenerator WorldGenerator { get; }
    }
    //public class ArchivalManaged : IDisposable
    //{
    //    public void CheckArchivalIsValid()
    //    {
    //        if (!archivalDataRef.IsCreated || !seedDataRef.IsCreated)
    //        {
    //            ConsoleCat.LogWarning("存档无效");
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (archivalDataRef.IsCreated) archivalDataRef.Dispose();
    //        if (seedDataRef.IsCreated) seedDataRef.Dispose();
    //    }

    //    BlobAssetReference<Archival.Data> archivalDataRef;
    //    public BlobAssetReference<Archival.Data> ArchivalDataRef
    //    {
    //        set
    //        {
    //            if (archivalDataRef.IsCreated) archivalDataRef.Dispose();
    //            archivalDataRef = value;
    //        }
    //    }
    //    BlobAssetReference<SeedData> seedDataRef;
    //    public BlobAssetReference<SeedData> SeedDataRef
    //    {
    //        set
    //        {
    //            if (seedDataRef.IsCreated) seedDataRef.Dispose();
    //            seedDataRef = value;
    //        }
    //    }
    //    public Archival Archival
    //    {
    //        get
    //        {
    //            return new Archival(archivalDataRef, seedDataRef);
    //        }
    //    }
    //}
}
