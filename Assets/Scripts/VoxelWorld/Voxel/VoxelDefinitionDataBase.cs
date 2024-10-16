using CatDOTS.VoxelWorld.Magics;
using CatFramework;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using CatFramework.Tools;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelDefinitionDataBase
    {
        int VoxelTypeCount { get; }
        IReadOnlyList<IVoxelItemInfo> VoxelItemInfos { get; }
        BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase { get; }
        Texture2DArray Opaque2DArray { get; }
        Texture2DArray Grass2DArray { get; }
        Texture2DArray Transparent2DArray { get; }

        IReadOnlyList<IVoxelItemInfo> GetClassifyVoxelItemInfos(string name);
        Voxel GetVoxel(string name);
        Voxel GetVoxel(IVoxelDefinition voxelDefinition);
        Voxel GetVoxel(IVoxelVariantDefinition voxelVariant);
        IVoxelItemInfo GetVoxelItemInfo(ushort id);
        bool TryGetVoxelType(string name, out VoxelType voxelType);
        Voxel[] VoxelDefToVoxel(IVoxelDefinition[] voxelDefinitions);
        bool VoxelIDIsValid(ushort id);
    }
    public class VoxelClassify
    {
        public readonly string ClassifyName;
        readonly IVoxelItemInfo[] voxelItemInfos;
        public IReadOnlyList<IVoxelItemInfo> VoxelItemInfos => voxelItemInfos;
        public VoxelClassify(string classifyName, IVoxelItemInfo[] voxelItemInfos)
        {
            ClassifyName = classifyName;
            this.voxelItemInfos = voxelItemInfos;
        }
    }
    public interface IVoxelItemInfo
    {
        public Texture2D Icon { get; }
        public string VoxelName { get; }
        VoxelType VoxelType { get; }
        Voxel Voxel { get; }
        ushort ID { get; }
    }
    public class VoxelDefinitionDataBase : IVoxelDefinitionDataBase
    {
        class InternalVoxelDefinition : IVoxelItemInfo
        {
            public Texture2D Icon { get; private set; }
            public string VoxelName { get; private set; }
            public string VoxelClassify { get; private set; }
            public VoxelType VoxelType { get; private set; }
            public Voxel Voxel => VoxelType.Voxel;
            public ushort ID => VoxelType.IndexInTypeArray;
            public static InternalVoxelDefinition Create(IVoxelDefinition voxelDefinition, VoxelType voxelType)
            {
                InternalVoxelDefinition internalVoxelDefinition = new InternalVoxelDefinition()
                {
                    Icon = voxelDefinition.Icon,
                    VoxelName = voxelDefinition.VoxelName,
                    VoxelClassify = voxelDefinition.ClassifyName,
                    VoxelType = voxelType,
                };
                return internalVoxelDefinition;
            }
            public static InternalVoxelDefinition Create(Texture2D icon, string voxelName, VoxelType voxelType)
            {
                InternalVoxelDefinition internalVoxelDefinition = new InternalVoxelDefinition()
                {
                    Icon = icon,
                    VoxelName = voxelName,
                    VoxelClassify = "默认",
                    VoxelType = voxelType,
                };
                return internalVoxelDefinition;
            }
        }
        InternalVoxelDefinition[] Defs;
        Dictionary<string, InternalVoxelDefinition> DefMap;
        VoxelClassify[] DefClassifies;
        BlobAssetReference<VoxelTypeAsset> VoxelTypeAsset;
        public BlobAssetReference<VoxelTypeAsset> VoxelTypeDataBase => VoxelTypeAsset;
        public Texture2DArray Opaque2DArray { get; private set; }
        public Texture2DArray Grass2DArray { get; private set; }
        public Texture2DArray Transparent2DArray { get; private set; }
        class Builder
        {
            List<InternalVoxelDefinition> internalVoxelDefinitionList;
            Dictionary<string, InternalVoxelDefinition> voxelDefinitionMap;
            Dictionary<string, List<InternalVoxelDefinition>> classifyVoxelDefMap;
            public Texture2DArray Opaque2DArray { get; private set; }
            public Texture2DArray Grass2DArray { get; private set; }
            public Texture2DArray Transparent2DArray { get; private set; }
            public Builder(DataBaseDefinition dataBase, IList<IVoxelDefinition> voxelDefinitions)
            {
                InternalVoxelDefinition empty = InternalVoxelDefinition.Create(null, "空", VoxelType.EmptyVoxelType);
                InternalVoxelDefinition border = InternalVoxelDefinition.Create(null, "边界", VoxelType.BorderVoxelType);

                internalVoxelDefinitionList = new List<InternalVoxelDefinition>() { empty, border, };

                voxelDefinitionMap = new Dictionary<string, InternalVoxelDefinition>()
                {
                    [empty.VoxelName] = empty,
                    [border.VoxelName] = border,
                };
                classifyVoxelDefMap = new Dictionary<string, List<InternalVoxelDefinition>>()
                {
                    ["默认"] = new List<InternalVoxelDefinition>() { empty, border, },
                };
                Texture2DArrayBuilder opaqueArray = new Texture2DArrayBuilder();
                Texture2DArrayBuilder transparentArray = new Texture2DArrayBuilder();
                Texture2DArrayBuilder grassArray = new Texture2DArrayBuilder();
                foreach (IVoxelDefinition definition in voxelDefinitions)
                {
                    if (definition != null && !voxelDefinitionMap.ContainsKey(definition.VoxelName))
                    {
                        int textureIndex;
                        switch (definition.VoxelRenderType)
                        {
                            case VoxelRenderType.OpaqueBlock:
                                opaqueArray.Add(definition.Texture, out textureIndex);
                                break;
                            case VoxelRenderType.TransparentBlock:
                                transparentArray.Add(definition.Texture, out textureIndex);
                                break;
                            case VoxelRenderType.Grass:
                                grassArray.Add(definition.Texture, out textureIndex);
                                break;
                            default: textureIndex = 0; break;
                        }
                        VoxelType voxelType = new VoxelType((ushort)internalVoxelDefinitionList.Count, (ushort)textureIndex, definition);
                        InternalVoxelDefinition voxelDef = InternalVoxelDefinition.Create(definition, voxelType);
                        // 按名查找图
                        voxelDefinitionMap.Add(definition.VoxelName, voxelDef);
                        // 分类
                        if (!classifyVoxelDefMap.TryGetValue(voxelDef.VoxelClassify, out List<InternalVoxelDefinition> voxelDefs))
                        {
                            voxelDefs = new List<InternalVoxelDefinition>();
                            classifyVoxelDefMap[voxelDef.VoxelClassify] = voxelDefs;
                        }
                        voxelDefs.Add(voxelDef);

                        // 最后再加入类型列表，否则在构造VoxelType索引的时候应-1
                        internalVoxelDefinitionList.Add(voxelDef);
                    }
                }

                Opaque2DArray = opaqueArray.Create(false);
                Transparent2DArray = transparentArray.Create(false);
                Grass2DArray = grassArray.Create(false);

                SetTexture2DArray(dataBase.opaqueMaterial, Opaque2DArray);
                SetTexture2DArray(dataBase.transparentMaterial, Transparent2DArray);
                SetTexture2DArray(dataBase.grassMaterial, Grass2DArray);
                static void SetTexture2DArray(Material material, Texture2DArray texture2DArray)
                {
                    if (texture2DArray == null) return;
                    material.SetTexture(ShaderID.BaseTex2DArray, texture2DArray);
                }
            }
            public void Finish(out InternalVoxelDefinition[] defs, out Dictionary<string, InternalVoxelDefinition> m_VoxelDefinitionMap, out VoxelClassify[] voxelClassifies, out BlobAssetReference<VoxelTypeAsset> m_VoxelTypeDataBase)
            {
                defs = internalVoxelDefinitionList.ToArray();
                m_VoxelDefinitionMap = voxelDefinitionMap;
                voxelClassifies = new VoxelClassify[classifyVoxelDefMap.Count];
                int i = 0;
                foreach (var kv in classifyVoxelDefMap)
                {
                    voxelClassifies[i] = new VoxelClassify(kv.Key, kv.Value.ToArray());
                    i++;
                }
                m_VoxelTypeDataBase = CreateVoxelTypeAsset(defs);
            }
            BlobAssetReference<VoxelTypeAsset> CreateVoxelTypeAsset(InternalVoxelDefinition[] internalVoxelDefinitions)
            {
                BlobBuilder builder = new BlobBuilder(Allocator.Temp);
                ref VoxelTypeAsset voxelTypeDataBase = ref builder.ConstructRoot<VoxelTypeAsset>();

                BlobBuilderArray<VoxelType> voxelTypes = builder.Allocate<VoxelType>(ref voxelTypeDataBase.VoxelTypes, internalVoxelDefinitions.Length);

                for (int i = 0; i < internalVoxelDefinitions.Length; i++)
                {
                    voxelTypes[i] = internalVoxelDefinitions[i].VoxelType;
                }

                var result = builder.CreateBlobAssetReference<VoxelTypeAsset>(Allocator.Persistent);
                builder.Dispose();
                return result;
            }
        }
        public VoxelDefinitionDataBase(DataBaseDefinition dataBase, IList<IVoxelDefinition> voxelDefinitions)
        {
            Builder builder = new Builder(dataBase, voxelDefinitions);
            builder.Finish(out Defs, out DefMap, out DefClassifies, out VoxelTypeAsset);
            Opaque2DArray = builder.Opaque2DArray;
            Grass2DArray = builder.Grass2DArray;
            Transparent2DArray = builder.Opaque2DArray;
        }

        public void Dispose()
        {
            VoxelTypeAsset.Dispose();
        }
        #region 外界调用
        public Voxel[] VoxelDefToVoxel(IVoxelDefinition[] voxelDefinitions)
        {
            if (voxelDefinitions == null || voxelDefinitions.Length == 0) return new Voxel[0];
            Voxel[] voxels = new Voxel[voxelDefinitions.Length];

            for (int i = 0; i < voxelDefinitions.Length; i++)
            {
                voxels[i] = GetVoxel(voxelDefinitions[i]);
            }

            return voxels;
        }
        public IReadOnlyList<IVoxelItemInfo> VoxelItemInfos => Defs;
        public IReadOnlyList<IVoxelItemInfo> GetClassifyVoxelItemInfos(string name)
        {
            for (int i = 0; i < DefClassifies.Length; i++)
            {
                if (DefClassifies[i].ClassifyName == name) return DefClassifies[i].VoxelItemInfos;
            }
            return null;
        }
        public IVoxelItemInfo GetVoxelItemInfo(ushort id)
        {
            return Defs[id];
        }
        public bool VoxelIDIsValid(ushort id)
        {
            return id < Defs.Length;
        }
        public int VoxelTypeCount => DefMap.Count;
        public Voxel GetVoxel(string name)
        {
            if (name == null || (!DefMap.TryGetValue(name, out InternalVoxelDefinition internalVoxelDefinition))) return Voxel.Empty;
            return internalVoxelDefinition.Voxel;
        }
        public Voxel GetVoxel(IVoxelDefinition voxelDefinition)
        {
            return voxelDefinition == null ? Voxel.Empty : GetVoxel(voxelDefinition.VoxelName);
        }
        public Voxel GetVoxel(IVoxelVariantDefinition voxelVariant)
        {
            if (voxelVariant == null) return Voxel.Empty;
            Voxel voxel = GetVoxel(voxelVariant.VoxelDef);
            voxel.VoxelMaterial = voxelVariant.VoxelMaterial;
            voxel.Energy = voxelVariant.Energy;
            return voxel;
        }
        public bool TryGetVoxelType(string name, out VoxelType voxelType)
        {
            if (DefMap.TryGetValue(name, out InternalVoxelDefinition def))
            {
                voxelType = def.VoxelType;
                return true;
            }
            voxelType = default;
            return false;
        }
        #endregion
    }
}
