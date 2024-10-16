using System.Collections;
using UnityEngine;

namespace CatFramework.Tools
{
    public class DonDestroySelf : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this);
            Destroy(this);
        }
    }
}