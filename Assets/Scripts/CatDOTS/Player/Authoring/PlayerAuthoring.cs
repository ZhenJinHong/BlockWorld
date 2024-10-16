using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace CatDOTS
{
    internal sealed class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] float TopAngle = 89f;
        [SerializeField] float BottomAngle = -89f;
        class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FirstPersonPlayerOutputCache>(entity);
                AddComponent<Disabled>(entity);
                //AddComponent<PlayerTag>(entity);
                AddComponent<FirstPersonPlayer>(entity, new FirstPersonPlayer()
                {
                    TopAngle = authoring.TopAngle,
                    BottomAngle = authoring.BottomAngle,
                });
            }
        }
    }
}
