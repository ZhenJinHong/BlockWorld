using CatFramework.InputMiao;
using System.Collections;
using UnityEngine;

namespace CatFramework
{
    public class SetCursorState : MonoBehaviour
    {
        public void SetCursorStateConfined()
        {
            InputManagerMiao.CursorState = CursorLockMode.Confined;
        }
    }
}