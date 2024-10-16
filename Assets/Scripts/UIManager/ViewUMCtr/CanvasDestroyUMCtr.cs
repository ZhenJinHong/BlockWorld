using System.Collections;
using UnityEngine;

namespace CatFramework.UiMiao
{
    public class CanvasDestroyUMCtr : MonoBehaviour
    {
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}