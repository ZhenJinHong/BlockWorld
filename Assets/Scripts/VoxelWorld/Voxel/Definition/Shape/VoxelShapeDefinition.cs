using CatFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Collider = Unity.Physics.Collider;
using Material = Unity.Physics.Material;

namespace CatDOTS.VoxelWorld
{
    public interface IVoxelShapeDefinition
    {
        string ClassifyName { get; }
        string Name { get; }
        Texture2D Icon { get; }
        List<VoxelShapeElement> VoxelShapes { get; }
        ShapePhysicsInfo[] PhysicsInfos { get; }
    }
    [CreateAssetMenu(fileName = "New VoxelShapeDefinition", menuName = "ECSVoxelWorld/VoxelShapeDefinition")]
    public class VoxelShapeDefinition : UnityEngine.ScriptableObject, IVoxelShapeDefinition
    {
        public string Name => name;
        [SerializeField] Texture2D icon;
        public Texture2D Icon => icon;
        [SerializeField] string classifyName = "方块";
        public string ClassifyName => classifyName;
        [SerializeField] List<VoxelShapeElement> elements;
        public List<VoxelShapeElement> VoxelShapes => elements;
        [SerializeField] ShapePhysicsInfo[] shapePhysicsInfos;
        public ShapePhysicsInfo[] PhysicsInfos => shapePhysicsInfos;
        // 需要其他形状的可以把需要的各种形状的字段全部都列出来,
        public static BlobAssetReference<Collider> Create(IPhysicsInfo physicsInfo, bool solid = true)
        {
            switch (physicsInfo.ColliderType)
            {
                case ColliderType.Box:
                    return CreateBox(physicsInfo, solid);
                default:
                    return CreateBox(physicsInfo, solid);
            }
        }
        public static BlobAssetReference<Collider> CreateBox(IPhysicsInfo physicsInfo, bool solid = true)
        {
            BoxGeometry boxGeometry = new BoxGeometry()
            {
                BevelRadius = physicsInfo.BevelRadius,
                Center = physicsInfo.Center,
                Size = physicsInfo.Size,
                Orientation = Quaternion.Euler(physicsInfo.Angle),
            };
            CollisionFilter collisionFilter = new CollisionFilter()
            {
                BelongsTo = solid ? DOTSLayer.SolidVoxel : DOTSLayer.NonSolidVoxel,
                CollidesWith = DOTSLayer.AllDynamic,
                GroupIndex = 0,
            };
            Material material = new Material()
            {
                Friction = 0.05f,// 摩擦力
                Restitution = 0f,// 弹力
                CollisionResponse = CollisionResponsePolicy.Collide,// 碰撞反应,是否碰撞,或是触发器等
                CustomTags = 0,// 可以用来标记这个碰撞体是什么,体素不需要这样的标记?
                EnableSurfaceVelocity = false,   // 就是物体移动,上面的物体也移动?
                EnableMassFactors = false,// 重力系数组件是否可以应用?
                FrictionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
                RestitutionCombinePolicy = Material.CombinePolicy.ArithmeticMean,
            };
            BlobAssetReference<Collider> cubeSolid = Unity.Physics.BoxCollider.Create(boxGeometry, collisionFilter, material);
            return cubeSolid;
        }
    }
    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule,
        Triangle,
        Quad,
        Cylinder,
        Mesh,
    }
}