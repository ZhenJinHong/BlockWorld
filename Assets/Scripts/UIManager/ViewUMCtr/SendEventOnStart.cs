using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CatFramework.UiMiao
{
    [DisallowMultipleComponent]
    public class SendEventOnStart : MonoBehaviour
    {
        [SerializeField] ClickedEvent m_OnStart = new ClickedEvent();
        public event UnityAction OnStart
        {
            add => m_OnStart.AddListener(value);
            remove => m_OnStart.RemoveListener(value);
        }
        private void Start()
        {
            m_OnStart?.Invoke();
        }
    }
}