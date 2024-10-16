using System;
using Unity.Entities;

namespace CatDOTS
{
    [InternalBufferCapacity(4)]
    public struct DamageBufferElement : IBufferElementData
    {
        public float Value;
    }
}
