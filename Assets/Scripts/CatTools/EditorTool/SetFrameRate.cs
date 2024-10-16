using System.Collections;
using UnityEngine;

namespace CatFramework.Tools
{
    public class SetFrameRate : MonoBehaviour
    {
        [SerializeField] int frameRate = 30;
        void Start()
        {
            Application.targetFrameRate = frameRate;
        }
        private void OnValidate()
        {
            if (Application.isPlaying)
                Application.targetFrameRate = frameRate;
        }
    }
}