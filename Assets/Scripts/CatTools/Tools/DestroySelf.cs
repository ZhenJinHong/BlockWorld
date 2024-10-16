using System.Collections;
using UnityEngine;

namespace CatFramework.Tools
{
    public class DestroySelf : MonoBehaviour
    {
        [SerializeField] bool destroy;
        private void Awake()
        {
            if (destroy)
                Destroy(gameObject);
        }
    }
}