using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS
{
    public interface ISyncECBCreate
    {
        EntityCommandBuffer CreateECB();
    }
    public class SyncECBCreator<T> : ISyncECBCreate
        where T : EntityCommandBufferSystem
    {
        T ecbSystem;
        public EntityCommandBuffer CreateECB()
        {
            ecbSystem ??= World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<T>();
            return ecbSystem.CreateCommandBuffer();
        }
    }
}
