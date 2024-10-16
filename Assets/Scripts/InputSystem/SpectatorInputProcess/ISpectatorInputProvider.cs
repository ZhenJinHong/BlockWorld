using UnityEngine;

namespace CatFramework.InputMiao
{
    public interface ISpectatorInputProvider
    {
        Vector2 MoveDelta { get; }
        Vector2 PointDelta { get; }
        float Lifting { get; }
        float XAngle { get; set; }
        float YAngle { get; set; }
        bool Sprint { get; }
    }
}
