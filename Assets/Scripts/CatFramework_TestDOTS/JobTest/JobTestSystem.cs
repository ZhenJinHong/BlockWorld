using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;

namespace CatFramework_TestDOTS.Assets.Scripts.CatFramework_TestDOTS.JobTest
{
    [DisableAutoCreation]
    public partial class JobTestSystem : SystemBase
    {
        public JobTest JobTest;
        protected override void OnUpdate()
        {
            if (JobTest != null)
            {
                Dependency = JobTest.ScrJob(Dependency);
            }
        }
    }
    public abstract class JobTest
    {
        public abstract JobHandle ScrJob(JobHandle dependOn);
    }
}
