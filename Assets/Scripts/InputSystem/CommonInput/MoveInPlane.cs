using CatFramework.CatMath;
using CatFramework.InputMiao;
using System.Collections;
using UnityEngine;

namespace CatFramework.InputMiao
{
    public class MoveInPlane : MonoBehaviour
    {
        public float Height;
        public float Speed;
        public Vector2 min;
        public Vector2 max;
        [HideInInspector] public Vector2 move;

        public bool IsUsable => this != null;

        public void Move(Vector2 move)
        {
            this.move = move;
        }

        public void Update()
        {
            Vector2 pos = transform.position.As2D() + Speed * Time.deltaTime * move;
            pos = MathC.Clamp(pos, min, max);
            transform.position = new Vector3(pos.x, Height, pos.y);
        }
    }
}