using System.Collections;
using UnityEngine;

namespace CatFramework.VFX
{
    public class GlobalVFXFollow : MonoBehaviour
    {
        Transform cameraTrans;
        // Use this for initialization
        void Start()
        {
            cameraTrans = Camera.main.transform;
        }
        // Update is called once per frame
        void Update()
        {
            transform.position = cameraTrans.position;
        }
    }
}