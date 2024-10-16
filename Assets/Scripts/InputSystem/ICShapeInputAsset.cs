using CatFramework.InputMiao;
using System;
using UnityEngine.InputSystem;

namespace CatFramework.InputMiao
{
    public interface ICShapeInputAsset : IDisposable
    {
        InputActionAsset asset { get; }
    }
    partial class FirstPersonInput : ICShapeInputAsset { } 
}