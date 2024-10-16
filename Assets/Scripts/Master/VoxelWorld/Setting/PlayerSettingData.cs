using CatFramework;
using CatFramework.DataMiao;
using CatFramework.InputMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorld
{
    public class PlayerFreeViewSetting : Setting, IFirstPersonSettingProvider
    {
        public override string Name => "自由视图";
        float viewTopLimit;
        float viewBottomLimit;
        public float ViewTopLimit => viewTopLimit;
        public float ViewBottomLimit => viewBottomLimit;

        [SerializeField] float movementSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float movementChangeRate;
        [SerializeField] float jumpSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float rotationThreshold;
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float RunSpeed { get => runSpeed; set => runSpeed = value; }
        public float MovementChangeRate { get => movementChangeRate; set => movementChangeRate = value; }
        public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
        public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
        public float RotationThreshold { get => rotationThreshold; set => rotationThreshold = value; }

        public override void ResetSetting()
        {
            viewTopLimit = 80.0f;
            viewBottomLimit = -90.0f;

            MovementSpeed = 2f;
            RunSpeed = 10f;
            MovementChangeRate = 6f;
            JumpSpeed = 5f;
            RotationSpeed = 0.1f;
            RotationThreshold = 0.01f;
        }
    }
    public class PlayerSettingData : Setting, IFirstPersonSettingProvider
    {
        public override string Name => "视角";
        float viewTopLimit;
        float viewBottomLimit;
        float movementSpeed;
        float runSpeed;
        float jumpSpeed;
        public float ViewTopLimit => viewTopLimit;
        public float ViewBottomLimit => viewBottomLimit;
        public float MovementSpeed => movementSpeed;
        public float RunSpeed => runSpeed;
        public float JumpSpeed => jumpSpeed;

        [SerializeField] float movementChangeRate;
        [SerializeField] float rotationSpeed;
        [SerializeField] float rotationThreshold;
       
        public float MovementChangeRate { get => movementChangeRate; set => movementChangeRate = value; }
        public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
        public float RotationThreshold { get => rotationThreshold; set => rotationThreshold = value; }

        public override void ResetSetting()
        {
            viewTopLimit = 80.0f;
            viewBottomLimit = -90.0f;
            movementSpeed = 4f;
            runSpeed = 7f;
            jumpSpeed = 6f;

            MovementChangeRate = 6f;
            RotationSpeed = 0.1f;
            RotationThreshold = 0.00f;
        }
    }
}
