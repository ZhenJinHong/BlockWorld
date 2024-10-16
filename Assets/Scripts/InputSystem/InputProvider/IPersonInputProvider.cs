using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.InputMiao
{
    public interface IPersonInputProvider
    {
        Vector2 MoveDelta { get; }
        Vector2 PointerDelta { get; }
        Vector2 PointerCoord { get; }
        bool LeftPress { get; }
        bool RightPress { get; }
        bool Sprint { get; }
        bool Fly { get; }
        bool Jump { get; }
        bool Crouch { get; }
    }
}
