using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.VFX
{
    public interface IVFXController : IModule
    {
        void SetPosition(Vector3 pos);
    }
}
