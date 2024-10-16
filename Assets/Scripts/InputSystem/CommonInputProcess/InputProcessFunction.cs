using CatFramework.CatMath;
using System;
using UnityEngine;

namespace CatFramework.InputMiao
{
    public static class InputProcessFunction
    {
        public static void TopView(ITopviewInputProvider provider, ITopViewSettingProvider setting, ITopViewTarget target)
        {
            if (provider != null && setting != null && target != null)
            {
                Vector2 move = provider.MoveDelta;
                Vector2 mousePos = provider.PointerCoord;
                float rotaThreshold = setting.RotationThreshold;
                float rotationSpeed = setting.RotationSpeed;
                float moveVelocity = provider.Sprint ? setting.RunSpeed : setting.MovementSpeed;
                float jumpSpeed = setting.JumpSpeed;
            }
        }
        public static void FirstPersonFreeLook(ISpectatorInputProvider provider, IFirstPersonSettingProvider setting, IFirstPersonTarget target, float deltaTime)
        {
            if (provider != null && setting != null && target != null)
            {
                Vector2 move = provider.MoveDelta;
                float lifting = provider.Lifting;
                Vector2 mouseDelta = provider.PointDelta;
                float rotaThreshold = setting.RotationThreshold;
                float rotationSpeed = setting.RotationSpeed;
                float bottomClamp = setting.ViewBottomLimit;
                float topClamp = setting.ViewTopLimit;
                float moveVelocity = provider.Sprint ? setting.RunSpeed : setting.MovementSpeed;
                float jumpSpeed = setting.JumpSpeed;
                if (MathC.AnyGreater(MathC.Abs(mouseDelta), rotaThreshold))
                {
                    float xAngle = provider.XAngle;
                    float yAngle = provider.YAngle;
                    xAngle += mouseDelta.y * rotationSpeed;
                    yAngle += mouseDelta.x * rotationSpeed;
                    xAngle = MathC.Clamp(xAngle, bottomClamp, topClamp);
                    yAngle = MathC.LoopAngleIn360(yAngle);
                    target.Rotation = Quaternion.Euler(xAngle, yAngle, 0f);
                    provider.XAngle = xAngle;
                    provider.YAngle = yAngle;
                }
                Vector3 targetPos = target.Position;
                if (move != Vector2.zero)
                {
                    Vector3 dir = Quaternion.Euler(0f, target.EulerAngles.y, 0f) * new Vector3(move.x, 0f, move.y);
                    targetPos += moveVelocity * deltaTime * dir;
                }
                targetPos.y += jumpSpeed * lifting * deltaTime;
                target.Position = targetPos;
            }
        }
    }
}
