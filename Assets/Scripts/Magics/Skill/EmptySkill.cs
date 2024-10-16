namespace CatFramework.Magics
{
    public class EmptySkill : ISkill
    {
        public float CD { get; set; }
        public float CoolingPercent { get; set; }
        public ISkillReleaser SkillReleaser { get; set; }

        public void Fire(ISkillReleaser skillReleaser)
        {
            if (ConsoleCat.Enable) ConsoleCat.LogWarning("这是一个空技能!!!");
        }
    }
}
