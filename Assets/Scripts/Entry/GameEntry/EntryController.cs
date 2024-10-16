using System.Collections;
using UnityEngine;

namespace CatFramework
{
    public class EntryController : MonoBehaviour
    {
        private void Awake()
        {
            Entry.IsActive = true;
        }
        private void Update()
        {
            Entry.BeginLoop();
            Entry.Update();
        }
        private void LateUpdate()
        {
            Entry.LateUpdate();
            Entry.CompleteLoop();
        }
        private void OnDestroy()
        {
            Entry.IsActive = false;
        }
    }
}