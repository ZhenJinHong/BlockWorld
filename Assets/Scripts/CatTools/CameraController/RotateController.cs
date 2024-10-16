using CatFramework.CatMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public class RotateController
    {
        public interface IInputProvider
        {
            float RotateSpeed { get; }
            Vector2 MouseDelta { get; }
            Quaternion CameraRotation { set; }
            Quaternion PlayerRotation { set; }
        }
        public float rotaThreshold = 1f;
        public float TopClamp = 89.0f;
        public float BottomClamp = -89.0f;
        float xAngle;
        float yAngle;
        IInputProvider InputProvider { get; set; }
        public RotateController(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
        }
        public void Update(float deltaTime)
        {
            Vector2 mouseDelta = InputProvider.MouseDelta;
            if (MathC.AnyGreater(MathC.Abs(mouseDelta), rotaThreshold))
            {
                xAngle += mouseDelta.y * InputProvider.RotateSpeed;
                yAngle += mouseDelta.x * InputProvider.RotateSpeed;
                xAngle = MathC.Clamp(xAngle, BottomClamp, TopClamp);
                yAngle = MathC.LoopAngleIn360(yAngle);
                InputProvider.CameraRotation = (Quaternion.Euler(xAngle, yAngle, 0f));
            }
            Quaternion yRotation = Quaternion.Euler(0f, yAngle, 0f);
            InputProvider.PlayerRotation = (yRotation);
        }
    }
}
