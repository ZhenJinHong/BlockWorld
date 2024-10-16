using UnityEngine;

namespace CatFramework.Magics
{
    public abstract class SkillDefinition : ScriptableObject
    {
        public Hash128 ID => Hash128.Compute(Name);
        public virtual string Name => name;
        [SerializeField] Sprite icon;
        public Sprite Icon => icon;
        public float cd;
        public abstract ISkill Create(IDataProvider dataProvider);
    }
}
