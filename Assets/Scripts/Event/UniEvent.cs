using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CatFramework.EventsMiao
{
    [Serializable]
    public class TriggerEvent : UnityEvent { }
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    [Serializable]
    public class Float2Event : UnityEvent<Vector2> { }
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    [Serializable]
    public class IntEvent : UnityEvent<int> { }
    [Serializable]
    public class UintEvent : UnityEvent<uint> { }
    [SerializeField]
    public class ColorEvent : UnityEvent<Color> { }
}