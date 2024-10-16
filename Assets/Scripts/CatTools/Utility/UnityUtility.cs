using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public static class UnityUtility
    {
        public static void Destroy(UnityEngine.Object obj)
        {
            UnityEngine.Object.Destroy(obj);
        }
        public static void DestroyAllChild(Transform parent)
        {
            for (int i = parent.childCount - 1; i > -1; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
        public static void DestroyGameObjects(UnityEngine.Component[] objs)
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                    Destroy(objs[i].gameObject);
            }
        }
    }
}
