using CatFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Magics
{
    public interface ISkillReleaser : IModule
    {
        IUser Owner { get; }
        IDataProvider DataProvider { get; }
        Vector3 FirePoint { get; }
    }
    public interface ISkill
    {
        ISkillReleaser SkillReleaser { get; set; }
        float CD { get; set; }
        float CoolingPercent { get; }

        // 激发的时候检查计时器,如果
        void Fire(ISkillReleaser skillReleaser);
    }
    public abstract class Skill : ISkill
    {
        public Timer Timer;
        private float cd;

        public abstract ISkillReleaser SkillReleaser { get; set; }
        public float CoolingPercent => Timer.Percent(cd);
        public float CD
        {
            get => cd;
            set
            {
                if (cd != value)
                {
                    cd = value;
                    Timer.Reset(cd);
                }
            }
        }
        public abstract void Fire(ISkillReleaser skillReleaser);
    }
}
