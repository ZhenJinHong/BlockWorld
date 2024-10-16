using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace CatDOTS.VoxelWorld
{
    public interface IBixelDataBase
    {
        Entity BixelPrefab { get; }
        float PutDelay { get; }
    }
    public class BixelDataBaseManaged : IBixelDataBase
    {
        //class BixelPrefab : IBixelPrefab
        //{
        //    public string Name { get; private set; }
        //    public Entity Prefab { get; private set; }
        //    public static BixelPrefab Create(IBixelDefinition bixelDefinition, Entity prefab)
        //    {
        //        return new BixelPrefab()
        //        {
        //            Name = bixelDefinition.Name,
        //            Prefab = prefab,
        //        };
        //    }
        //}
        //BixelPrefab[] bixelPrefabs;
        //Dictionary<UnityEngine.Hash128, BixelPrefab> prefabMap;
        Entity bixelPrefab;
        public Entity BixelPrefab => bixelPrefab;
        public float PutDelay => 0.15f;
        public BixelDataBaseManaged(BixelDataBaseDefinition bixeDataBaseDefinition, IVoxelDefinitionDataBase voxelDefinitionDataBase)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            //builder.WithAll<Bixel, Prefab>();
            //var query = builder.Build(entityManager);
            //NativeArray<ComponentType> componentTypes = entityManager.GetComponentTypes(query.GetSingletonEntity(), Allocator.Temp);
            ComponentType[] componentTypes = new ComponentType[]
            {
                ComponentType.ReadWrite<Prefab>(),
                ComponentType.ReadWrite<Bixel>(),
                //ComponentType.ReadWrite<BixelTimer>(),
                ComponentType.ReadWrite<LocalTransform>(),
                ComponentType.ReadWrite<LocalToWorld>(),
            };
            bixelPrefab = entityManager.CreateEntity(componentTypes);
            RenderMeshDescription renderMeshDescription = new RenderMeshDescription(UnityEngine.Rendering.ShadowCastingMode.Off, false);

            bixeDataBaseDefinition.OpaueVoxelMaterial.SetTexture(ShaderID.BaseTex2DArray, voxelDefinitionDataBase.Opaque2DArray);
            bixeDataBaseDefinition.GrassVoxelMaterial.SetTexture(ShaderID.BaseTex2DArray, voxelDefinitionDataBase.Grass2DArray);
            bixeDataBaseDefinition.TransparentMaterial.SetTexture(ShaderID.BaseTex2DArray, voxelDefinitionDataBase.Transparent2DArray);

            Material[] materials = new Material[]
            {
                bixeDataBaseDefinition.OpaueVoxelMaterial,
                bixeDataBaseDefinition.TransparentMaterial,
                bixeDataBaseDefinition.GrassVoxelMaterial,
            };

            RenderMeshArray renderMeshArray = new RenderMeshArray(materials, new Mesh[] { bixeDataBaseDefinition.BixelMesh });
            RenderMeshUtility.AddComponents(bixelPrefab, entityManager, in renderMeshDescription, renderMeshArray, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
            //EntitiesGraphicsSystem entitiesGraphicsSystem;
            //bixelPrefabs = new BixelPrefab[bixelDefinitions.Count];
            //int i = 0;
            //foreach (var bixeDef in bixelDefinitions)
            //{
            //    var hash128 = AuthoringExtension.Hash128(bixeDef.Name);
            //    BixelPrefab bixelPrefab = BixelPrefab.Create(bixeDef, entityDataBase.GetPrefab(hash128));
            //    bixelPrefabs[i] = bixelPrefab;
            //    prefabMap.Add(hash128, bixelPrefab);
            //    i++;
            //}
            //EntityArchetype entityArchetype;World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(,);
            //World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            //World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData()
            //World.DefaultGameObjectInjectionWorld.EntityManager.CreateArchetype()
            //ComponentType componentType;
            //World.DefaultGameObjectInjectionWorld.EntityManager.GetChunk().

        }
    }
}