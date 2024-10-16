using System.Collections;
using UnityEngine;

namespace CatFramework.Tools
{
    public class DestroyWhenBuild : MonoBehaviour
    {
#if !UNITY_EDITOR
        void Start()
        {
            Destroy(gameObject);
        }
#endif
    }
}