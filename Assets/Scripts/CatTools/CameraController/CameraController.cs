using CatFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using CatFramework.CatMath;
namespace CatFramework.Tools
{
    public class CameraController
    {
        public interface IInputProvider
        {
            Vector2 Move { get; }
            float Lifting { get; }
            Vector2 MouseDelta { get; }
        }
        public float TopClamp = 80.0f;
        public float BottomClamp = -90.0f;

        public bool clampRange;
        public float moveVeloctity = 20f;
        /// <summary>
        /// 高度对移动速度影响
        /// </summary>
        public float heightAffectMove = 0.5f;
        public float rotationSpeed = 0.05f;
        public Vector3 rangeMin = Vector3.zero;
        public Vector3 rangeMax = new Vector3(256f, 64f, 128f);
        public IInputProvider inputProvider;
        public float rotaThreshold = 0.01f;

        float xAngle;
        float yAngle;

        Transform target;
        public CameraController(IInputProvider inputProvider)
        {
            this.target = Camera.main.transform;
            this.inputProvider = inputProvider;
        }
        public void Update(float deltaTime)
        {
            if (inputProvider != null && target != null)
            {
                Vector2 move = inputProvider.Move;
                float lifting = inputProvider.Lifting;
                Vector2 mouseDelta = inputProvider.MouseDelta;
                //Debug.Log("mouseDelta" + mouseDelta+";"+"RotationSpeed:"+rotationSpeed);
                if (MathC.AnyGreater(MathC.Abs(mouseDelta), rotaThreshold))
                {
                    xAngle += mouseDelta.y * rotationSpeed;
                    yAngle += mouseDelta.x * rotationSpeed;
                    xAngle = MathC.Clamp(xAngle, BottomClamp, TopClamp);
                    yAngle = MathC.LoopAngleIn360(yAngle);
                    target.localRotation = Quaternion.Euler(xAngle, yAngle, 0f);
                }
                Vector3 targetPos = target.position;
                if (move != Vector2.zero)
                {
                    Vector3 dir = Quaternion.Euler(0f, target.eulerAngles.y, 0f) * new Vector3(move.x, 0f, move.y);
                    targetPos += (moveVeloctity + (targetPos.y * heightAffectMove)) * deltaTime * dir;
                }
                targetPos.y += moveVeloctity * lifting * deltaTime;
                target.position = clampRange ? MathC.Clamp(targetPos, rangeMin, rangeMax) : targetPos;
            }
        }
    }
}
