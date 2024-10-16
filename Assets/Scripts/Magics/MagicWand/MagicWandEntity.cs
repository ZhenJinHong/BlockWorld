using System;
using System.Collections;
using UnityEngine;

namespace CatFramework.Magics
{
    [Obsolete]
    public class MagicWandEntity : MonoBehaviour
    {
        [SerializeField] float firePoint = 0.6f;
        public Vector3 FirePoint => firePoint * transform.forward + transform.position;
    }
}