using CatFramework.CatMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public class FirstPersonController
    {
        public interface IInputProvider
        {
            Vector2 Move { get; }
            bool Jump { get; }
            Vector3 PlayerPosition { get; }
            Vector2 MouseDelta { get; }
            void SetFace(Quaternion rotation);
            void SetForward(Quaternion rotation);
            void SetVelocity(Vector3 velocity);
        }
        public Vector3 Offset = new Vector3(0f, 1.6f, 0f);
        public float TopClamp = 80.0f;
        public float BottomClamp = -88.0f;

        public float moveVeloctity = 10f;
        public float JumpHeight = 2f;
        public float rotationSpeed = 0.2f;
        public IInputProvider inputProvider;
        public float rotaThreshold = 0.01f;

        public float xAngle;
        public float yAngle;
        Transform target;
        public FirstPersonController(IInputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
            target = Camera.main.transform;
        }
        public void Update(float deltaTime)
        {
            if (inputProvider != null && target != null)
            {
                Vector2 mouseDelta = inputProvider.MouseDelta;
                //target.position = inputProvider.PlayerPosition + Offset;
                if (MathC.AnyGreater(MathC.Abs(mouseDelta), rotaThreshold))
                {
                    //Vector3 angle = target.eulerAngles;
                    //float x = mouseDelta.y * rotationSpeed + MathC.ClampAngleCompareBy180(angle.x);// angleX在比0小的时候会变成360度，导致超出最大值
                    //float y = mouseDelta.x * rotationSpeed + angle.y;
                    //x = MathC.Clamp(x, BottomClamp, TopClamp);
                    //y = MathC.ClampAngle(y);
                    //target.localRotation = Quaternion.Euler(x, y, 0f);
                    //Quaternion rotation = target.rotation;
                    //float x = mouseDelta.y * rotationSpeed;
                    //float y = mouseDelta.x * rotationSpeed;
                    //rotation *= Quaternion.Euler(x, y, 0f);
                    //target.rotation = rotation;
                    xAngle += mouseDelta.y * rotationSpeed;
                    yAngle += mouseDelta.x * rotationSpeed;
                    xAngle = MathC.Clamp(xAngle, BottomClamp, TopClamp);
                    yAngle = MathC.LoopAngleIn360(yAngle);
                    //target.localRotation = Quaternion.Euler(xAngle, yAngle, 0f);
                    inputProvider.SetFace(Quaternion.Euler(xAngle, yAngle, 0f));
                }
                Quaternion yRotation = Quaternion.Euler(0f, yAngle, 0f);
                inputProvider.SetForward(yRotation);
                Vector2 move = inputProvider.Move;
                Vector3 dir = new Vector3(0f, 0f, 0f);
                if (move != Vector2.zero)
                {
                    dir = yRotation * new Vector3(move.x, dir.y, move.y) * moveVeloctity;
                    inputProvider.SetVelocity(dir);
                }
            }
        }
    }
}
